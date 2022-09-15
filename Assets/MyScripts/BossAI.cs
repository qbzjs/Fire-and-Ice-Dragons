using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;

    [SerializeField] GameObject[] allPlayer;//�Ҧ����a 

    [SerializeField] Dictionary<int, float> allPlayerDamage = new Dictionary<int, float>();//�����Ҧ����a�ˮ`

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;

    [Header("����")]
    float longAttackRadius;//�����b�|(���Z��)
    float closeAttackRadius;//�����b�|(��Z��)
    [SerializeField] GameObject target;//�����ؼ�
    float[] attackRandomTime;//�����üƮɶ�(�̤p,�̤j)
    float attackTime;//�����ɶ�(�p�ɾ�)
    int maxAttackNumber;//�֦������ۦ�
    int attackNumber;//�ϥΧ����ۦ�

    [Header("�����ݾ�")]
    bool isStart;//�O�_�}�l
    [SerializeField] float attackIdleTime;//�����ݾ��ɶ�(�p�ɾ�)
    float maxAttackIdleTime;//�̤j�����ݾ��ɶ�    

    [Header("�l��")]
    float chaseSpeed;//�l���t��
    //�M��
    float fineTargetTime;//�M��ؼЮɶ�
    float findTime;//�M��ؼЮɶ�(�p�ɾ�)

    [Header("����3")]    
    [SerializeField]float attack3Time;
    [SerializeField] bool isAttacked;
    public GameObject boomPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        //�s�u && ���O�ۤv��
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            GameSceneManagement.Instance.OnSetMiniMapPoint(transform, GameSceneManagement.Instance.loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
            this.enabled = false;
            return;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;
        GetComponent<BoxCollider>().enabled = false;

        //����
        longAttackRadius = 30;//�����b�|(���Z��)
        closeAttackRadius = 10;//�����b�|(��Z��)
        attackRandomTime = new float[] { 0.5f, 2.0f };//�����üƮɶ�(�̤p,�̤j)
        maxAttackNumber = 3;//�֦������ۦ�

        //�����ݾ�
        maxAttackIdleTime = 1.5f;//�̤j�����ݾ��ɶ�

        //�l��
        chaseSpeed = 5.3f;//�l���t��

        fineTargetTime = 3;//�M�䪱�a�ɶ�        

        state = State.�ݾ����A;
    }

    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnJudgeAnimation();//�P�_�ʵe

        if (state == State.�l�����A)
        {
            OnFindTargetTime();//�M��ؼЮɶ�
            OnRotateToTarget();//��V�ܥؼ�
            if (isStart) OnChaseTarget();//�l���ؼ�
        }

        if (state == State.�������A)
        {
            OnFindTargetTime();//�M��ؼЮɶ�
            OnAttaclIdleTime();//�����ݾ��ɶ�
        }

        OnAttack3JudgeTime();
    }


    public enum State
    {
        �ݾ����A,
        �l�����A,
        �������A
    }
    [Header("���A")]
    public State state;

    /// <summary>
    /// �E�����
    /// </summary>
    public void OnActive()
    {
        if (state != State.�l�����A)
        {
            state = State.�l�����A;
            allPlayer = GameObject.FindGameObjectsWithTag("Player");

            if (GameDataManagement.Instance.isConnect)
            {
                for (int i = 0; i < allPlayer.Length; i++)
                {
                    allPlayerDamage.Add(allPlayer[i].GetComponent<PhotonView>().ViewID, 0);
                }
            }

            OnChangeAnimation(animationName: "Roar", animationType: true);
        }
    }

    /// <summary>
    /// ����3�����ɶ��P�_
    /// </summary>
    void OnAttack3JudgeTime()
    {
        if(boomPos.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
        {
            attack3Time += Time.deltaTime;

            //�����ɶ�
            if(attack3Time >= 2f && !isAttacked)
            {
                GetComponent<Boss_Exclusive>().OnAttack3_Boss();
                isAttacked = true;
                StartCoroutine(OnWaitAttack3());
            }            
        }
    }
    
    /// <summary>
    /// ���ݧ���3
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitAttack3()
    {
        yield return new WaitForSeconds(0.67f);

        isAttacked = false;
        attack3Time = 0;
    }

    /// <summary>
    /// �M��ؼЮɶ�
    /// </summary>
    void OnFindTargetTime()
    {
        findTime -= Time.deltaTime;

        if (findTime <= 0)
        {
            findTime = fineTargetTime;
            if (!GameDataManagement.Instance.isConnect) OnFindTarget();//�M��ؼ�
            else OnFindTarget_Connect();//�M��ؼ�_�s�u
        }
    }

    /// <summary>
    /// �����ˮ`
    /// </summary>
    /// <param name="id">���aID</param>
    /// <param name="damage">�ˮ`</param>
    public void OnSetRecordDamage(int id, float damage)
    {
        allPlayerDamage[id] += damage;
    }

    /// <summary>
    /// �M��ؼ�_�s�u
    /// </summary>
    void OnFindTarget_Connect()
    {
        float bestDamage = 0;
        int number = 0;
        int targetNumber = 0;
        foreach (var player in allPlayerDamage)
        {
            if (player.Value > bestDamage)
            {
                if (allPlayer[targetNumber].activeSelf)
                {
                    bestDamage = player.Value;
                    targetNumber = number;
                }

            }
            number++;
        }

        if (bestDamage != 0 && allPlayer[targetNumber].GetComponent<CharactersCollision>().Hp > 0) target = allPlayer[targetNumber];
        else OnFindTarget();
    }

    /// <summary>
    /// �M��ؼ�
    /// </summary>
    void OnFindTarget()
    {
        //�̪񬰥ؼ�
        float closestPlayerDistance = 100000;//�̪�Z��
        float distance;//��L���a�Z��
        int chaseNumber = 0;//�l���s��
        for (int i = 0; i < allPlayer.Length; i++)
        {
            if (allPlayer[i].GetComponent<CharactersCollision>().Hp > 0)
            {
                distance = (allPlayer[i].transform.position - transform.position).magnitude;
                if (distance < closestPlayerDistance)
                {
                    closestPlayerDistance = distance;
                    chaseNumber = i;
                }
            }
        }

        if (allPlayer[chaseNumber].GetComponent<CharactersCollision>().Hp > 0) target = allPlayer[chaseNumber];
        else
        {
            for (int i = 0; i < allPlayer.Length; i++)
            {
                if (allPlayer[i].GetComponent<CharactersCollision>().Hp > 0)
                {
                    target = allPlayer[i];
                    return;
                }
            }
        }
    }

    /// <summary>
    /// ��V�ܥؼ�
    /// </summary>
    /// <param name="speed">��V�t��</param>
 public   void OnRotateToTarget()
    {
        float speed = 2.0f;
        if (info.IsTag("Attack")) speed = 0.5f;

        if (target != null || target.activeSelf)
        {
            if (!info.IsTag("Die"))
            {
                //��V�ؼ�
                transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, speed * Time.deltaTime, speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
            }
        }
    }

    /// <summary>
    /// �l���ؼ�
    /// </summary>
    void OnChaseTarget()
    {
        //�p�j������d��
        if ((transform.position - target.transform.position).magnitude > longAttackRadius)
        {
            if (info.IsName("Fly"))
            {
                transform.position = transform.position + transform.forward * chaseSpeed * Time.deltaTime;
            }
        }
        else
        {
            //�l����Ĥ@������
            if (state != State.�������A)
            {
                state = State.�������A;

                OnAttackNumber();//�����ۦ��P�_

                OnChangeAnimation(animationName: "AttackNumber", animationType: attackNumber);
                OnChangeAnimation(animationName: "Fly", animationType: false);
            }
        }
    }

    /// <summary>
    /// �����ۦ��P�_
    /// </summary>
    void OnAttackNumber()
    {
        if ((transform.position - target.transform.position).magnitude <= closeAttackRadius)//�ϥΧ����ۦ�(��)
        {
            //UnityEngine.Random.Range(3, maxAttackNumber + 1);
            attackNumber = 3;
        }
        else attackNumber = UnityEngine.Random.Range(1, 3);//�ϥΧ����ۦ�(��)
    }

    /// <summary>
    /// �����ݾ��ɶ�
    /// </summary>
    void OnAttaclIdleTime()
    {
        if (attackIdleTime > 0)
        {
            if (!info.IsTag("Attack"))
            {
                OnRotateToTarget();
                attackIdleTime -= Time.deltaTime;
            }

            if (attackIdleTime <= 0)
            {
                //�j������d��
                if ((transform.position - target.transform.position).magnitude > longAttackRadius)
                {
                    state = State.�l�����A;
                    OnChangeAnimation(animationName: "Fly", animationType: true);
                    return;
                }
            }
        }

        if (attackIdleTime <= 0)
        {
            float dir = Vector3.Dot(transform.forward, Vector3.Cross(Vector3.up, target.transform.position - transform.position));
            //�w��V�ܥؼ�
            if (Vector3.Dot(transform.forward, target.transform.position - transform.position) > 0 && dir > -1 && dir < 1)
            {
                if (!info.IsTag("Attack"))
                {
                    OnAttackNumber();//�����ۦ��P�_

                    OnChangeAnimation(animationName: "AttackNumber", animationType: attackNumber);
                    OnChangeAnimation(animationName: "Fly", animationType: false);
                    OnChangeAnimation(animationName: "Pain", animationType: false);
                }
            }
            else
            {
                if (!info.IsTag("Attack")) OnRotateToTarget();//��V�ܥؼ�
            }

            attackIdleTime = UnityEngine.Random.Range(1, maxAttackIdleTime);//�����ݾ��ɶ�(�p�ɾ�)
        }
    }

    /// <summary>
    /// �P�_�ʵe
    /// </summary>
    void OnJudgeAnimation()
    {
        //�H�S����
        if (info.IsTag("Roar") && info.normalizedTime >= 1)
        {
            isStart = true;//�O�_�}�l
            OnChangeAnimation(animationName: "Roar", animationType: false);
            OnChangeAnimation(animationName: "Fly", animationType: true);
        }

        /* //����1(�� �Q��)
         if(info.IsName("Attack1") && info.normalizedTime < 0.95f)
         {            
             transform.position = transform.position + Vector3.up * 11 * Time.deltaTime;
         } */

        //��������
        if (info.IsTag("Attack") && info.normalizedTime >= 1)
        {
            OnChangeAnimation(animationName: "AttackNumber", animationType: 0);
        }

        //�ݾ����A
        if (info.IsTag("Idle") && state != State.�ݾ����A)
        {
            if (target != null && target.activeSelf)
            {

                //OnRotateToTarget();//��V�ܥؼ�
            }
        }
    }

    /// <summary>
    /// �󴫰ʵe
    /// </summary>
    /// <param name="animationName">����ʵe�W��</param>
    /// <param name="animationType">�ʵeType</param>
    void OnChangeAnimation<T>(string animationName, T animationType)
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        switch (animationType.GetType().Name)
        {
            case "Boolean":
                animator.SetBool(animationName, Convert.ToBoolean(animationType));
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, animationName, Convert.ToBoolean(animationType));
                break;
            case "Single":
                break;
            case "Int32":
                animator.SetInteger(animationName, Convert.ToInt32(animationType));
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, animationName, Convert.ToInt32(animationType));
                break;
            case "String":
                break;
        }
    }

    private void OnDrawGizmos()
    {
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(transform.position, 10);
    }

    public Transform GetTarget()
    {
        return target.transform;
    }    
}
