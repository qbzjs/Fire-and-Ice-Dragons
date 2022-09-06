using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;

    GameObject[] allPlayer;//�Ҧ����a 

    [SerializeField] Dictionary<GameObject, float> playersDamage = new Dictionary<GameObject, float>();//�O���Ҧ����a�ˮ`

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;

    [Header("����")]
    [SerializeField] float attackRadius;//�����b�|
    [SerializeField] GameObject target;//�����ؼ�

    [Header("�l��")]
    float chaseSpeed;//�l���t��

    //�M��
    float fineTargetTime;//�M��ؼЮɶ�
    float findTime;//�M��ؼЮɶ�(�p�ɾ�)

    void Start()
    {
        animator = GetComponent<Animator>();

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //����
        attackRadius = 5;//�����b�|

        //�l��
        chaseSpeed = 6.3f;//�l���t��

        fineTargetTime = 5;//�M�䪱�a�ɶ�
    }
     
    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        Debug.LogError((transform.position - target.transform.position).magnitude);
        //OnJudgeAnimation();//�P�_�ʵe

        if(state == State.�ݾ����A)
        {
            OnStartDistance();
        }
        if(state == State.�l�����A)
        {
            OnFindTargetTime();//�M��ؼЮɶ�
            OnRotateToTarget();//��V�ܥؼ�
            OnChaseTarget();//�l���ؼ�
        }
    }

    public enum State
    {
        �ݾ����A,
        �l�����A,
        �������A
    }
    State state;

    /// <summary>
    /// �}�l�԰��d��
    /// </summary>
    void OnStartDistance()
    {
        if (Physics.CheckSphere(transform.position, 19, 1 << LayerMask.NameToLayer("Player")))
        {
            state = State.�l�����A;
            allPlayer = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < allPlayer.Length; i++)
            {
                playersDamage.Add(allPlayer[i], 0);
            }
            
            OnChangeAnimation(animationName: "Roar", animationType: true);
        }
    } 

    /// <summary>
    /// �M��ؼЮɶ�
    /// </summary>
    void OnFindTargetTime()
    {
        findTime -= Time.deltaTime;

        if(findTime <= 0)
        {
            findTime = fineTargetTime;
            OnFindTarget();//�M��ؼ�
        }
    }

    /// <summary>
    /// �M��ؼ�
    /// </summary>
    void OnFindTarget()
    {
        //�ˮ`�̰����ؼ�
        float number = -1;
        int i = 0;
        int bestDamage = 0;
        foreach (var player in playersDamage)
        {
            if (player.Value > number)
            {
                number = player.Value;
                bestDamage = i;
            }

            i++;
        }

        target = allPlayer[bestDamage];
    }

    /// <summary>
    /// ��V�ܥؼ�
    /// </summary>
    void OnRotateToTarget()
    {
        if (target != null || target.activeSelf)
        {
            //��V�ؼ�
            transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 0.03f, 0.03f);
            transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        }
    }

    /// <summary>
    /// �l���ؼ�
    /// </summary>
    void OnChaseTarget()
    {
        //�p�j������d��
        if((transform.position - target.transform.position).magnitude > attackRadius && info.IsTag("Run"))
        {
            transform.position = transform.position + transform.forward * chaseSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// �P�_�ʵe
    /// </summary>
    void OnJudgeAnimation()
    {
        if(info.IsTag("Roar") && info.normalizedTime >= 1)
        {

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
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
