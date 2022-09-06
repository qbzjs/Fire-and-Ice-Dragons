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

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;

    [Header("����")]
    float walkSpeed;//�樫�t��
    float flyAttackSpeed;//��������t��
    float flyAttackUpSpeed;//��������W�ɳt��

    [Header("����")]
    GameObject attackTarget;//������H
    int maxAttackNumber;//�֦��������ۦ�
    [SerializeField]int activeAttackNumber;//�ϥΪ������ۦ�
    float[] attackDelayTime;//��������ɶ�(�̤p��,�̤j��)
    [SerializeField] float attackTime;//�����ɶ�

    void Start()
    {
        animator = GetComponent<Animator>();

        allPlayer = GameObject.FindGameObjectsWithTag("Player");

        //�M��̪񪱮a
        OnFineClosestplayer();

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //����
        walkSpeed = 2;//�樫�t��
        flyAttackSpeed = 30;//��������t��
        flyAttackUpSpeed = 20;//��������W�ɳt��
        //����
        maxAttackNumber = 2;//�֦��������ۦ�
        attackDelayTime = new float[] { 0.5f, 5f};//��������ɶ�(�̤p��,�̤j��)
        attackTime = 3;//�����ɶ�
    }
     
    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnJudgeAnimation();//�P�_�ʵe
    }

    /// <summary>
    /// �M��̪񪱮a
    /// </summary>
    void OnFineClosestplayer()
    {
        float closest = 1000;
        int target = 0;
        for (int i = 0; i < allPlayer.Length; i++)
        {
            float dir = (transform.position - allPlayer[i].transform.position).magnitude;
            if(dir < closest)
            {
                closest = dir;
                target = i;
            }
        }

        attackTarget = allPlayer[target];
    }

    /// <summary>
    /// ��V�ܥؼ�
    /// </summary>
    void OnRotateToTarget()
    {
        //��V�ؼ�
        transform.forward = Vector3.RotateTowards(transform.forward, attackTarget.transform.position - transform.position, 0.065f, 0.065f);
        transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
    }

    /// <summary>
    /// �����ɶ�
    /// </summary>
    void OnAttackTime()
    {
        attackTime -= Time.deltaTime;//�����ɶ�
        if (attackTime <= 0)
        {
            attackTime = UnityEngine.Random.Range(attackDelayTime[0], attackDelayTime[1]);//�����ɶ�
            activeAttackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);//�ϥΪ������ۦ�

            //�樫���v
            if (UnityEngine.Random.Range(0, 100) < 50) OnChangeAnimation(animationName: "Walk", animationType: true);
            else OnChangeAnimation(animationName: "AttackNumber", animationType: activeAttackNumber);
        }
    }

    /// <summary>
    /// �P�_�ʵe
    /// </summary>
    void OnJudgeAnimation()
    {
        //�ݾ����A
        if(info.IsTag("Idle"))
        {
            OnAttackTime();//�����ɶ� 
            OnRotateToTarget();//��V�ܥؼ�
        }

        //�H�S���A
        if (info.IsName("Roar"))
        {
            //transform.position = transform.position + transform.forward * walkSpeed * Time.deltaTime;

            //�i�����
            if (info.normalizedTime >= 1)
            {
                OnChangeAnimation(animationName: "Walk", animationType: false);
                OnChangeAnimation(animationName: "AttackNumber", animationType: activeAttackNumber);
            }
        }

        //�������
        if(info.IsName("FlyAttack"))
        {            
            transform.position = transform.position + transform.forward * flyAttackSpeed / 2 * Time.deltaTime;
        }

        //��������
        if (info.IsTag("Attack") && info.normalizedTime >= 1) OnChangeAnimation(animationName: "AttackNumber", animationType: 0);
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
        Gizmos.DrawWireSphere(transform.position + boxCenter + transform.forward * 5, 3);
    }
}
