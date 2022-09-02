using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;
    CharactersCollision charactersCollision;
    readonly AStart aStart = new AStart();

    LayerMask mask;//������HLayer

    [Header("�}��")]
    public Role role;
    public enum Role 
    {
        �P���h�L1,
        ���Y�H,
        �}�b��,
        ���Y�H,
        �pBoss
    }

    [Header("�d��")]
    [SerializeField] public float normalStateMoveRadius;//�@�몬�A���ʽd��
    [SerializeField] public float alertRadius;//ĵ�ٽd��
    [SerializeField] public float chaseRadius;//�l���d��
    [SerializeField] public float attackRadius;//�����d��

    [Header("�@�몬�A")]
    float[] normalStateMoveTime = new float[2];//�@�몬�A���ʶüƳ̤p�P�̤j��
    float normalStateMoveSpeed;//�@�몬�A���ʳt��
    bool isNormalMove;//�O�_�@�몬�A�w�g����
    Vector3 originalPosition;//��l��m
    Vector3 forwardVector;//���ʥؼЦV�q    
    float normalStateTime;//�@�몬�A���ʮɶ�(�p�ɾ�)
    float normalRandomMoveTime;//�@�몬�A�üƲ��ʮɶ�
    float normalRandomAngle;//�@�몬�A�üƿ��ਤ��

    [Header("ĵ�٪��A")]
    float alertToChaseTime;//ĵ�٨�l���ɶ�
    float leaveAlertRadiusAlertTime;//���}ĵ�ٽd��ĵ�ٮɶ�
    float leaveAlertTime;//���}ĵ�ٽd��ĵ�ٮɶ�(�p�ɾ�)
    float alertTime;//ĵ�٨�l���ɶ�(�p�ɾ�)
    float CheckPlayerDistanceTime;//�������a�Z���ɶ�
    float CheckTargetTime;//�������a�ɶ�(�p�ɾ�)        

    [Header("�H�S���A")]
    bool isRotateToPlayer;//�O�_��V�ܪ��a
    [SerializeField]bool isHowling;//�O�_�H�S

    [Header("�l�����A")]
    [SerializeField] float chaseSpeed;//�l���t��
    float maxRadiansDelta;//��V����
    float[] readyChaseRandomTime;//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)    
    float loseSpeed;//��֪��t�פ��
    float startChaseTime;//���}�԰���üƶ}�l�l���ɶ�(�p�ɾ�)
    int chaseNumber;//�l����H�s��
    bool isStartChase;//�O�_�}�l�l��
    bool isReadChase;//�O�_�ǳưl��
    float chaseTurnTime;//�l����V�ɶ�(�p�ɾ�)
    int chaseDiretion;//�l����V(0 = �e��, 1 = �k��, 2 = ����)       
    float delayAttackRandomTime;//��������üƮɶ�(�p�ɾ�)
    float[] delayAttackTime;//��������ɶ�(�üƳ̤p��, �̤j��)    

    [Header("�ˬd�P��")]
    float changeDiretionTime_Forward;//�󴫤�V�ɶ�_�e�谻��(�p�ɾ�)
    float changeDiretionTime_Near;//�󴫤�V�ɶ�_���k�谻��(�p�ɾ�)
    float changeDiretionTime;//�󴫤�V�ɶ� 
    float chaseSlowDownSpeed;//�l����t�t��
    bool isPauseChase;//�O�_�Ȱ��l��
    bool[] isCheckNearCompanion;//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)

    [Header("�������A")]
    [SerializeField] int maxAttackNumber;//�i�ϥΧ����ۦ�
    [SerializeField] float[] attackFrequency;//�����W�v(�üƳ̤p��, �̤j��)
    [SerializeField] float meleeAttackDistance;//��Z���ۦ������Z��    
    int attackNumber;//�����ۦ��s��(0 = ������)            
    float waitAttackTime;//���ݧ����ɶ�(�p�ɾ�)
    bool isAttackIdle;//�O�_�����ݾ�
    bool isAttacking;//�O�_������    
    bool isGetHit;//�O�_�Q����(�P�w"Pain"�ʵe�O�_Ĳ�o)

    [Header("�����ݾ�")]
    [SerializeField] float attackIdleMoveSpeed;//�����ݾ����ʳt��
    [SerializeField] float backMoveDistance;//�Z�����a�h��V�ᨫ
    bool isAttackIdleMove;//�O�_�����ݾ�����
    float attackIdleMoveRandomTime;//�����ݾ����ʶüƮɶ�(�p�ɾ�)     
    int attackIdleMoveDiretion;//�����ݾ����ʤ�V(0 = ������, 1 = �k, 2 = ��)

    [Header("�M��")]
    [SerializeField] bool isExecuteAStart;//�O�_����AStart
    List<Vector3> pathsList = new List<Vector3>();//���ʸ��|�`�I  
    int point = 0;//�M���`�I�s��
    int aStarCheckPointNumber;//AStar�ܤָg�L�h���I

    [Header("�Ҧ����a")]
    [SerializeField] GameObject[] allPlayers;//�Ҧ����a    
    [SerializeField] GameObject[] allPlayerAlliance;//�Ҧ����a�P���h�L
    [SerializeField] GameObject[] allEnemySoldier;//�Ҧ��ĤH�h�L
    [SerializeField] GameObject chaseObject;//�l����H

    [Header("�O�_�����")]
    [SerializeField] bool isMelee;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        //�s�u && ���O�ۤv��
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            //GameSceneManagement.Instance.OnSetMiniMapPoint(transform, GameSceneManagement.Instance.loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
            this.enabled = false;
            return;
        }
    }
    void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Enemy")) mask = LayerMask.GetMask("Player", "Alliance");//������HLayer
        if (gameObject.layer == LayerMask.NameToLayer("Alliance")) mask = LayerMask.GetMask("Enemy");//������HLayer

        aStart.initial();
        charactersCollision = GetComponent<CharactersCollision>();

        //�P�_����
         switch(role)
         {
             case Role.�P���h�L1://�P���h�L
                 isMelee = true;//��Ը}��

                 //�����d��
                 normalStateMoveRadius = 2.5f;//�@�몬�A���ʽd��
                 alertRadius = 12;//ĵ�ٽd��
                 chaseRadius = 8;//�l���d��
                 attackRadius = 3.0f;//�����d��

                 //�l�����A
                 chaseSpeed = 5.3f;//�l���t��
                 readyChaseRandomTime = new float[] { 0.5f, 3.3f };//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)

                 //�������A
                 attackFrequency = new float[2] { 0.5f, 3.5f };//�����W�v(�üƳ̤p��, �̤j��)  
                 maxAttackNumber = 3;//�i�ϥΧ����ۦ�
                 meleeAttackDistance = 2.0f;//��Z���ۦ������Z��

                 //�����ݾ�
                 attackIdleMoveSpeed = 1;//�����ݾ����ʳt��
                 backMoveDistance = 2.0f;//�Z�����a�h��V�ᨫ
                 break;
             case Role.���Y�H://���Y�H
                 isMelee = true;//��Ը}��

                 //�����d��
                 normalStateMoveRadius = 2.5f;//�@�몬�A���ʽd��
                 alertRadius = 12;//ĵ�ٽd��
                 chaseRadius = 8;//�l���d��
                 attackRadius = 3.0f;//�����d��

                 //�l�����A
                 chaseSpeed = 5.3f;//�l���t��
                 readyChaseRandomTime = new float[] { 0.5f, 3.3f };//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)

                 //�������A
                 attackFrequency = new float[2] { 0.5f, 3.5f };//�����W�v(�üƳ̤p��, �̤j��)  
                 maxAttackNumber = 3;//�i�ϥΧ����ۦ�

                 //�����ݾ�
                 attackIdleMoveSpeed = 1;//�����ݾ����ʳt��
                 backMoveDistance = 2.0f;//�Z�����a�h��V�ᨫ
                 meleeAttackDistance = 2.5f;//��Z���ۦ������Z��
                 break;
             case Role.�}�b��://�}�b��
                 isMelee = false;//�D��Ը}��

                 //�����d��
                 normalStateMoveRadius = 2.5f;//�@�몬�A���ʽd��
                 alertRadius = 14.0f;//ĵ�ٽd��
                 chaseRadius = 13.0f;//�l���d��
                 attackRadius = 11.0f;//�����d��

                 //�l�����A
                 chaseSpeed = 5.3f;//�l���t��
                 readyChaseRandomTime = new float[] { 1.0f, 4.0f };//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)

                 //�������A
                 attackFrequency = new float[2] { 1.0f, 5.0f };//�����W�v(�üƳ̤p��, �̤j��)  
                 maxAttackNumber = 3;//�i�ϥΧ����ۦ�

                 //�����ݾ�
                 attackIdleMoveSpeed = 2;//�����ݾ����ʳt��
                 backMoveDistance = 5.0f;//�Z�����a�h��V�ᨫ
                 meleeAttackDistance = 2.3f;//��Z���ۦ������Z��
                 break;
             case Role.���Y�H://���Y�H
                 isMelee = true;//��Ը}��

                 //�����d��
                 normalStateMoveRadius = 2.5f;//�@�몬�A���ʽd��
                 alertRadius = 12;//ĵ�ٽd��
                 chaseRadius = 8;//�l���d��
                 attackRadius = 3.0f;//�����d��

                 //�l�����A
                 chaseSpeed = 5.3f;//�l���t��
                 readyChaseRandomTime = new float[] { 0.5f, 2.3f };//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)

                 //�������A
                 attackFrequency = new float[2] { 0.5f, 2.75f };//�����W�v(�üƳ̤p��, �̤j��)  
                 maxAttackNumber = 3;//�i�ϥΧ����ۦ�

                 //�����ݾ�
                 attackIdleMoveSpeed = 1;//�����ݾ����ʳt��
                 backMoveDistance = 2.0f;//�Z�����a�h��V�ᨫ
                 meleeAttackDistance = 2.5f;//��Z���ۦ������Z��
                 break;
             case Role.�pBoss:
                 isMelee = true;//��Ը}��
                 //�����d��
                 normalStateMoveRadius = 2.5f;//�@�몬�A���ʽd��
                 alertRadius = 35;//ĵ�ٽd��
                 chaseRadius = 30;//�l���d��
                 attackRadius = 3.0f;//�����d��

                 //�l�����A
                 chaseSpeed = 6.3f;//�l���t��
                 readyChaseRandomTime = new float[] { 0.5f, 1.0f };//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)

                 //�������A
                 attackFrequency = new float[2] { 0.5f, 1.0f };//�����W�v(�üƳ̤p��, �̤j��)  
                 maxAttackNumber = 4;//�i�ϥΧ����ۦ�

                 //�����ݾ�
                 attackIdleMoveSpeed = 2;//�����ݾ����ʳt��
                 backMoveDistance = 2.3f;//�Z�����a�h��V�ᨫ
                 meleeAttackDistance = 2.7f;//��Z���ۦ������Z��
                 break;
         }    

        //�@�몬�A
        originalPosition = transform.position;//��l��m
        forwardVector = transform.forward;//�e��V�q
        normalStateMoveSpeed = 1;//�@�몬�A���ʳt��
        normalStateMoveTime = new float[2] { 1.5f, 3.5f };//�@�몬�A���ʶüƳ̤p�P�̤j��

        //ĵ�٪��A        
        if (GameDataManagement.Instance.isConnect) allPlayers = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];//�Ҧ����a
        else allPlayers = new GameObject[1];
        CheckPlayerDistanceTime = 2;//�������a�Z���ɶ�
        alertToChaseTime = 1;//ĵ�٨�l���ɶ�
        leaveAlertRadiusAlertTime = 3;//���}ĵ�ٽd��ĵ�ٮɶ�
        leaveAlertTime = leaveAlertRadiusAlertTime;//���}ĵ�ٽd��ĵ�ٮɶ�(�p�ɾ�)

        //�ˬd�P��
        isCheckNearCompanion = new bool[2] { false, false};//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)

        //�l�����A
        maxRadiansDelta = 0.065f;//��V����        
        changeDiretionTime = 0.5f;//�󴫤�V�ɶ�
        delayAttackTime = new float[] { 0.5f, 1};//��������ɶ�(�üƳ̤p��, �̤j��)
        loseSpeed = 0.45f;//��֪��t�פ��
        readyChaseRandomTime = new float[] { 0.5f, 2.3f };//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)

        //�M��
        aStarCheckPointNumber = 2;//AStar�ܤָg�L�h���I

        OnGetAllPlayers();//����Ҧ����a
        isHowling = true;
        OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
    }

    void Update()
    {
        if (!charactersCollision.isDie)
        {
            //OnCollision();//�I����

            OnStateBehavior();//���A�欰               
            OnCheckLefrAndRightCompanion();//�ˬd���k�P��

            OnGetAllPlayers();//����Ҧ����a
            OnCheckClosestPlayer();//�ˬd�̪񪱮a
        }
    }

    /// <summary>
    /// ��l��
    /// </summary>
    public void OnInitial()
    {
        isHowling = true;
        OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
        isExecuteAStart = false;//�O�_����AStart
    }

    /// <summary>
    /// AI���A
    /// </summary>
    enum AIState
    {
        �@�몬�A,
        ĵ�٪��A,
        �l�����A,
        �������A
    }
    [Header("AI���A")]
    [SerializeField] AIState aiState = AIState.�@�몬�A;


   /* /// <summary>
    /// �I����
    /// </summary>    
    /// <returns></returns>
    public void OnCollision()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsTag("Attack") && !isExecuteAStart)//����&&����AStart���I��
        {
            LayerMask mask = LayerMask.GetMask("Enemy");
            RaycastHit hit;

            //�g�u��V
            Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.forward + transform.right,
                                                -transform.right };

            float boxSize = 0.4f;//�I���ؤj�p
                                 //�}��I��    
            for (int i = 0; i < rayDiration.Length; i++)
            {
                if (Physics.BoxCast(transform.position + charactersCollision.boxCenter, new Vector3(charactersCollision.boxCollisionDistance * boxSize, charactersCollision.boxSize.y / 2, charactersCollision.boxCollisionDistance * boxSize), rayDiration[i], out hit, Quaternion.Euler(transform.localEulerAngles), charactersCollision.boxCollisionDistance * boxSize, mask))
                {
                    //�I��
                    transform.position = transform.position - rayDiration[i] * (Mathf.Abs((charactersCollision.boxCollisionDistance * boxSize) - hit.distance));
                }
            }
        }
    }*/
    
    /// <summary>
    /// ���A�欰
    /// </summary>
    void OnStateBehavior()
    {
        switch (aiState)
        {
            case AIState.�@�몬�A:
                OnNormalStateBehavior();
                OnRotateToTarget();//����ܪ��a��V
                break;
            case AIState.ĵ�٪��A:
                OnAlertStateBehavior();
                OnRotateToTarget();//����ܪ��a��V
                break;
            case AIState.�l�����A:
                OnChaseBehavior();
                break;
            case AIState.�������A:
                OnAttackBehavior();                
                break;
        }
    }

    /// <summary>
    /// �@�몬�A�欰
    /// </summary>
    void OnNormalStateBehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        normalStateTime -= Time.deltaTime;//�@�몬�A���ʮɶ�

        if (normalStateTime <= 0 && !isRotateToPlayer)
        {
            if (!isNormalMove)//�O�_�@�몬�A�w�g����
            {
                isNormalMove = true;

                normalRandomMoveTime = UnityEngine.Random.Range(2, 4.5f);//�@�몬�A�üƲ��ʮɶ�

                //����
                if ((transform.position - originalPosition).magnitude > normalStateMoveRadius)//���}�@�몬�A���ʽd��
                {                    
                    forwardVector = originalPosition - transform.position;//���ʥؼЦV�q
                }
                else//�@�몬�A���ʽd��
                {
                    normalRandomAngle = UnityEngine.Random.Range(0, 360);//�@�몬�A�üƿ��ਤ��                    
                    forwardVector = Quaternion.AngleAxis(normalRandomAngle, Vector3.up) * forwardVector;//���ʥؼЦV�q
                }

                //�󴫰ʵe
                OnChangeAnimation(animationName: "Walk", animationType: true);
            }

            if (normalRandomMoveTime > 0)//����
            {
                normalRandomMoveTime -= Time.deltaTime;
                float maxRadiansDelta = 0.03f;//��V����

                //�ˬd�e��P�� && �D����AStart
                if (OnCheckCompanionBox(diretion: transform.forward) && !isExecuteAStart)
                {                    
                    maxRadiansDelta = 0.065f;//��V����                    
                    forwardVector = -transform.forward - transform.right;//��V��V                    
                }
        
                //���� && ��V                
                transform.forward = Vector3.RotateTowards(transform.forward, forwardVector, maxRadiansDelta, maxRadiansDelta);
                transform.position = transform.position + transform.forward * normalStateMoveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
            }
            else//�ݾ�
            {                
                isNormalMove = false;
                normalStateTime = UnityEngine.Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//�@�몬�A�üƲ��ʮɶ�

                //�󴫰ʵe
                OnChangeAnimation(animationName: "Walk", animationType: false);
            }
        }
                
        OnAlertRangeCheck();//ĵ�ٽd�򰻴�
    }    

    /// <summary>
    /// ĵ�ٽd�򰻴�
    /// </summary>
    void OnAlertRangeCheck()
    {
        //�󴫪��A����
        if (OnDetectionRange(radius: alertRadius))
        {
            alertTime += Time.deltaTime;//ĵ�٨�l���ɶ�

            if (aiState != AIState.ĵ�٪��A)
            {
                OnChangeState(state: AIState.ĵ�٪��A, openAnimationName: "Alert", closeAnimationName: "Walk", animationType: true);
            }
            else//ĵ�٪��A��
            {
                //ĵ�٨�l���ɶ�
                if (alertTime >= alertToChaseTime)
                {
                    OnHowlingBehavior();//�H�S�欰
                    OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
                    OnChangeAnimation(animationName: "Idle", animationType: false);
                }
            }
        }
        else
        {
            if (aiState == AIState.ĵ�٪��A) leaveAlertTime -= Time.deltaTime;//���}ĵ�ٽd��ĵ�ٮɶ�(�p�ɾ�)

            if (leaveAlertTime <= 0)
            {
                if (aiState != AIState.�@�몬�A)
                {
                    leaveAlertTime = leaveAlertRadiusAlertTime;//���s���}ĵ�ٽd��ĵ�ٮɶ�
                    normalRandomMoveTime = 0;//�@�몬�A�üƲ��ʮɶ�
                    normalStateTime = UnityEngine.Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//�@�몬�A�üƲ��ʮɶ�

                    OnChangeState(state: AIState.�@�몬�A, openAnimationName: "Idle", closeAnimationName: "Alert", animationType: true);
                }
            }
        }
    }

    /// <summary>
    /// ĵ�٪��A�欰
    /// </summary>
    void OnAlertStateBehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnAlertRangeCheck();//ĵ�ٽd�򰻴�        
        OnChaseRangeCheck();//�l���d�򰻴�
        //OnCheckClosestPlayer();//�ˬd�̪񪱮a        
    }

    /// <summary>
    /// �������
    /// </summary>
    public void OnGetHit()
    {
        isGetHit = true;//�Q����(�P�w"Pain"�ʵe�O�_Ĳ�o)

        if (aiState == AIState.�l�����A || aiState == AIState.�������A) return;
        
        //�����ʵe
        if (aiState == AIState.ĵ�٪��A) OnChangeAnimation(animationName: "Alert", animationType: false);
        if (aiState == AIState.�@�몬�A)
        {
            OnChangeAnimation(animationName: "Walk", animationType: false);
            OnChangeAnimation(animationName: "Idle", animationType: false);
        }

        isHowling = true;//�O�_�H�S

        //OnGetAllPlayers();//����Ҧ����a
        OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);

        OnHowlingBehavior();//�H�S�欰
    }

    /// <summary>
    /// �H�S�欰
    /// </summary>
    void OnHowlingBehavior()
    {
        /*AI[] companions = GameObject.FindObjectsOfType<AI>();        
        foreach (var companion in companions)
        {
            float distance = (transform.position - companion.transform.position).magnitude;
            if(distance <= alertRadius)
            {
                //�q���P��
                companion.OnChangeStateToChase(allPlayers: allPlayers, chaseObject: chaseNumber);
            }
        }*/

        StartCoroutine(OnWaitChase());//���ݰl��
    }

    /*/// <summary>
    /// �󴫪��A��l�����A
    /// </summary>
    /// <param name="allPlayers">�Ҧ����a</param>
    /// <param name="chaseObject">�l�������a</param>
    void OnChangeStateToChase(GameObject[] allPlayers, int chaseObject)
    {
        this.allPlayers = allPlayers;//�Ҧ����a
        this.chaseNumber = chaseObject;//�l�������a
        StartCoroutine(OnWaitChase());//���ݰl��
    }*/

    /// <summary>
    /// ����ܥؼФ�V
    /// </summary>
    void OnRotateToTarget()
    {
        //����ܥؼФ�V
        if (isRotateToPlayer)
        {            
            Vector3 targetDiration = chaseObject.transform.position - transform.position;//allPlayers[chaseNumber].transform.position - transform.position;
            transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, maxRadiansDelta, maxRadiansDelta);
        }
    }

    /// <summary>
    /// ���ݰl��
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitChase()
    {
        isRotateToPlayer = true;//��V�ܪ��a                
        
        yield return new WaitForSeconds(0.55f);

        isRotateToPlayer = false;

        if (aiState != AIState.�l�����A || aiState != AIState.�������A)
        {
            isHowling = true;//�O�_�H�S

            OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
        }

        yield return 0;
    }

    /// <summary>
    /// �l���d�򰻴�
    /// </summary>
    void OnChaseRangeCheck()
    {
        //�󴫪��A����
        if (OnDetectionRange(radius: chaseRadius))
        {
            if (aiState != AIState.�l�����A)
            {
                isHowling = true;//�O�_�H�S
                OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);
            }
        }
    }   

    /// <summary>
    /// �l���欰
    /// </summary>
    void OnChaseBehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        if(isHowling && !info.IsName("Howling"))
        {
            OnChangeAnimation(animationName: "Howling", animationType: true);
        }

        //�H�S���}�l�l��
        if (info.IsName("Howling") && info.normalizedTime >= 1)
        {
            //�^�@�몬�A�ƫe�]�w
            isNormalMove = false;//�O�_�@�몬�A�w�g����
            normalStateTime = UnityEngine.Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//���s�@�몬�A�üƲ��ʮɶ�
            
            //�l��
            isStartChase = true;//�}�l�l��                        

            if (chaseObject != null)
            {
                Vector3 targetDiration = chaseObject.transform.position - transform.position; //allPlayers[chaseNumber].transform.position - transform.position;
                transform.forward = transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, maxRadiansDelta, maxRadiansDelta);
            }

            isHowling = false;//�O�_�H�S

            OnChangeAnimation(animationName: "Idle", animationType: false);
            OnChangeAnimation(animationName: "Howling", animationType: false);
            OnChangeAnimation(animationName: "Run", animationType: true);
        }

        //�}�l�l��
        if (isStartChase)
        {
            OnAttackRangeCheck();//�����d�򰻴�        
            OnCheckExecuteAStart();//�����O�_����AStart
            //OnCheckClosestPlayer();//�ˬd�̪񪱮a            
            OnCheckForwardCompanion();//�ˬd�e��P��
            

            //�ª��a����
            if (info.IsName("Run")) OnMoveBehavior();

            //�ˬd�̪񪱮a
            //if (chaseTurnTime <= 0) OnCheckClosestPlayer();

            /*//�l�������a���` && �l���d�򤺨S����L���a
            if (chaseObject.activeSelf == false && OnDetectionRange(radius: chaseRadius) == false)
            {
                if (aiState != AIState.�@�몬�A)
                {
                    OnChangeState(state: AIState.�@�몬�A, openAnimationName: "Idle", closeAnimationName: "Run", animationType: true);
                    return;
                }
            }*/
        }
    }

    /// <summary>
    /// �ˬd�P��I����
    /// </summary>
    /// <param name="diretion">��V</param>
    bool OnCheckCompanionBox(Vector3 diretion)
    {
        float chechSize = 0.6f;//�ˬd�ؤj�p
        LayerMask mask = LayerMask.GetMask("Enemy", "StageObject");

        //�I��P��
        if (Physics.CheckBox(transform.position + charactersCollision.boxCenter + diretion * (charactersCollision.boxSize.x + (chechSize / 2)), new Vector3(chechSize * 1.1f, 0.1f, chechSize), Quaternion.Euler(transform.localEulerAngles), mask))
        {            
            return true;            
        }

        return false;        
    }

    /// <summary>
    /// �ˬd�e��P��
    /// </summary>
    void OnCheckForwardCompanion()
    {
        //�ˬd�e��P�� && �D����AStart
        if (OnCheckCompanionBox(diretion: transform.forward) && !isExecuteAStart)
        {
            chaseSlowDownSpeed -= loseSpeed * Time.deltaTime;//�l����t�t��            
            if (chaseSlowDownSpeed <= 0f)
            {
                chaseSlowDownSpeed = 0f;
                
                if (!isPauseChase)
                {
                    isPauseChase = true;//�Ȱ��l��

                    OnChangeAnimation(animationName: "Run", animationType: false);
                    OnChangeAnimation(animationName: "AttackIdle", animationType: true);
                }
            }

            changeDiretionTime_Forward -= Time.deltaTime;//�󴫤�V�ɶ�
            if (changeDiretionTime_Forward <= 0)
            {
                changeDiretionTime_Forward = changeDiretionTime;
                if (!isCheckNearCompanion[1]) chaseDiretion = 2;//���S���P��
                if (!isCheckNearCompanion[0] || (!isCheckNearCompanion[0] && !isCheckNearCompanion[1])) chaseDiretion = 1;//�k�S���P�� || ���k���S���P��                
            }
        }
        else
        {
            //�O�_�Ȱ��l��
            if (isPauseChase)
            {
                isPauseChase = false;//�Ȱ��l��

                OnChangeAnimation(animationName: "Run", animationType: true);
                OnChangeAnimation(animationName: "AttackIdle", animationType: false);
            }

            if (chaseSlowDownSpeed < 1)//�l���t��
            {
                chaseSlowDownSpeed += Time.deltaTime;
                if (chaseSlowDownSpeed >= 1) chaseSlowDownSpeed = 1;
            }
        }
    }

    /// <summary>
    /// �ˬd���k�P��
    /// </summary>
    void OnCheckLefrAndRightCompanion()
    {
        changeDiretionTime_Near -= Time.deltaTime;

        if (changeDiretionTime_Near <= 0)
        {
            changeDiretionTime_Near = changeDiretionTime;

            //�ˬd�k��P�� && �D����AStart
            if (OnCheckCompanionBox(diretion: transform.right) && !isExecuteAStart)//�ˬd�k��P��
            {
                if (!isCheckNearCompanion[0])//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)
                {
                    isCheckNearCompanion[0] = true;
                    chaseDiretion = 2;//�l����V(0 = �e��, 1 = �k��, 2 = ����)
                }
            }
            else isCheckNearCompanion[0] = false;//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)

            //�ˬd����P�� && �D����AStart
            if (OnCheckCompanionBox(diretion: -transform.right) && !isExecuteAStart)
            {
                if (!isCheckNearCompanion[1])//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)
                {
                    isCheckNearCompanion[1] = true;
                    chaseDiretion = 1;//�l����V(0 = �e��, 1 = �k��, 2 = ����)
                }
            }
            else isCheckNearCompanion[1] = false;//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)    

            //���k���S���I���P��
            if (!isCheckNearCompanion[0] && !isCheckNearCompanion[1]) chaseDiretion = 0;//�l����V(0 = �e��, 1 = �k��, 2 = ����)
        }
    }

    /// <summary>
    /// ���ʦ欰
    /// </summary>
    void OnMoveBehavior()
    {        
        //�l����V(0 = �e��, 1 = �k��, 2 = ����)
        switch (chaseDiretion)
        {
            case 1://�V�k����
                transform.position = transform.position + transform.right * (chaseSpeed / 2) * Time.deltaTime;
                break;
            case 2://�V������
                transform.position = transform.position - transform.right * (chaseSpeed / 2) * Time.deltaTime;
                break;
        }

        //����(�P�_�O�_�����s)
        if (chaseDiretion == 0) transform.position = transform.position + transform.forward * chaseSpeed * chaseSlowDownSpeed * Time.deltaTime;
        else transform.position = transform.position + transform.forward * (chaseSpeed / 2) * chaseSlowDownSpeed * Time.deltaTime;
    }    

    /// <summary>
    /// �����d�򰻴�
    /// </summary>
    void OnAttackRangeCheck()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //�󴫪��A����
        if (OnDetectionRange(radius: attackRadius))
        {
            delayAttackRandomTime -= Time.deltaTime;//��������üƮɶ�(�p�ɾ�)

            if (isReadChase) isReadChase = false;//�O�_�ǳưl��
            
            if (aiState != AIState.�������A && delayAttackRandomTime <= 0)
            {
                delayAttackRandomTime = UnityEngine.Random.Range(delayAttackTime[0], delayAttackTime[1]);//��������ɶ�
                
                //������V�h����
                StartCoroutine(OnWaitTurnToAttack());             
            }
        } 
        else
        {
            isReadChase = true;//�ǳưl��
            startChaseTime -= Time.deltaTime;//���}�԰���üƶ}�l�l���ɶ�(�p�ɾ�)

            if (chaseObject != null)
            {
                //�l�������a���` && �l���d�򤺨S����L���a
                if (chaseObject.activeSelf == false && OnDetectionRange(radius: chaseRadius) == false)
                {
                    if (aiState != AIState.�l�����A)
                    {
                        OnChangeState(state: AIState.�l�����A, openAnimationName: "Run", closeAnimationName: "AttackIdle", animationType: true);

                        if (isAttackIdleMove)
                        {
                            isAttackIdleMove = false;
                            OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                        }
                        if (isAttacking)
                        {
                            isAttacking = false; ;
                            OnChangeAnimation(animationName: "AttackNumber", animationType: 0);
                        }
                        /* if(isStartChase)
                         {
                             isStartChase = false;
                             OnChangeAnimation(animationName: "Run", animationType: false);
                         }*/

                        return;
                    }
                }
            }

            if (startChaseTime <= 0)
            {
                //"�ݾ����ʵe"���S����"������"���A
                if (info.IsName("AttackIdle") && isAttacking) isAttacking = false;

                //�b�����ݾ��� && ���A������
                if (isAttackIdle && !isAttacking)
                {
                    if (aiState != AIState.�l�����A)
                    {                           
                        OnChangeAnimation(animationName: "AttackNumber", animationType: 0);
                        OnChangeState(state: AIState.�l�����A, openAnimationName: "Run", closeAnimationName: "AttackIdle", animationType: true);
                    }
                }
            }
            else
            {
                if (isReadChase)//�ǳưl��
                {
                    if (info.IsTag("Attack") && info.normalizedTime >= 1)
                    {
                        if (isAttacking)//������
                        {
                            OnChangeAnimation(animationName: "AttackNumber", animationType: 0);                            
                            OnChangeAnimation(animationName: "AttackIdle", animationType: true);
                        }
                        
                    }
                }
            }

            //�����ݾ�����
            if (isAttackIdleMove)
            {
                isAttackIdleMove = false;
                OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                OnChangeAnimation(animationName: "AttackIdle", animationType: true);
            }
        }
    }

    /// <summary>
    /// ������V�h����
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitTurnToAttack()
    {
        isRotateToPlayer = true;//��V�ܪ��a
        isAttacking = true;//������
        isAttackIdle = false;//�D�����ݾ�

        //�H�S���A���Ѱ�
        if (isHowling)
        {
            isHowling = false;
            OnChangeAnimation(animationName: "Howling", animationType: false);
        }

        /* //�����ۦ�
         if ((transform.position - chaseObject.transform.position).magnitude < meleeAttackDistance)//�񨭧���
         {
             attackNumber = maxAttackNumber;
         }
         else if ((transform.position - chaseObject.transform.position).magnitude >= meleeAttackDistance)//�Ĩ����
         {
             if(gameObject.tag == "GuardBoss") attackNumber = UnityEngine.Random.Range(1, 3);
             else attackNumber = 1;
         }
         else//�@�����
         {
             if (gameObject.tag == "EnemySoldier_2") attackNumber = UnityEngine.Random.Range(1, 3);
             else attackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);
         }*/
        attackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);
        yield return new WaitForSeconds(0.3f);               

        OnChangeState(state: AIState.�������A, openAnimationName: "AttackNumber", closeAnimationName: "Run", animationType: attackNumber);

        //���}�԰���üƶ}�l�l���ɶ�(�p�ɾ�)
        startChaseTime = UnityEngine.Random.Range(readyChaseRandomTime[0], readyChaseRandomTime[1]);
    }

    /// <summary>
    /// �������A�欰
    /// </summary>
    void OnAttackBehavior()
    {        
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnAttackRangeCheck();//�����d�򰻴�
                             //if (!isAttacking) OnCheckClosestPlayer();//�ˬd�̪񪱮a       

        if (!isHowling && info.IsName("Howling"))
        {
            OnChangeAnimation(animationName: "AttackIdle", animationType: true);
        }

        //�����M��
        if (pathsList.Count > 0)
        {            
            isExecuteAStart = false;//�D����AStart
            point = 0;//�M���`�I�s��
            pathsList.Clear();
        }

        //��������
        if (info.IsTag("Attack") && info.normalizedTime >= 1)
        {
            if (!isAttackIdle)//�O�_�����ݾ�
            {                
                isAttackIdle = true;
                isAttacking = false;
             
                OnChangeAnimation(animationName: "AttackNumber", animationType: 0);

                if (OnDetectionRange(radius: attackRadius))//�����d��
                {
                    OnChangeAnimation(animationName: "AttackIdle", animationType: true);
                }               
                
                attackIdleMoveDiretion = UnityEngine.Random.Range(1, 3);//�����ݾ����ʤ�V(0 = ������, 1 = �k, 2 = ��)
                waitAttackTime = UnityEngine.Random.Range(attackFrequency[0], attackFrequency[1]);//�üƧ����ݾ��ɶ�
                attackIdleMoveRandomTime = UnityEngine.Random.Range(1, waitAttackTime); ;//�����ݾ����ʶüƮɶ�(�p�ɾ�)

                waitAttackTime = waitAttackTime + attackIdleMoveRandomTime;//�����ݾ��ɶ� + �����ݾ����ʮɶ�
            } 
        }

        //���������Q�����L����"����"�ʵe
        if (info.IsTag("Attack") && isGetHit)
        {            
            OnChangeAnimation(animationName: "Pain", animationType: false);
            isGetHit = false;
        }

        if (isHowling)//�O�_�H�S
        {
            isHowling = false;                
            OnChangeAnimation(animationName: "Howling", animationType: false);
        }

        if (!isReadChase) OnWaitAttackBehavior();//���ݧ����欰
    }    

    /// <summary>
    /// ���ݧ����欰
    /// </summary>
    void OnWaitAttackBehavior()
    {
        //���ݧ����ɶ� && �D������
        if (waitAttackTime > 0 && !isAttacking)
        {
            //�D����AStart
            if (!isExecuteAStart)
            {
                waitAttackTime -= Time.deltaTime;//�üƧ����ݾ��ɶ�
                attackIdleMoveRandomTime -= Time.deltaTime;//�����ݾ����ʶüƮɶ�(�p�ɾ�)
            }
            
            OnAttackIdleMove();//�����ݾ�����            

            if (waitAttackTime <= 0)//�����ݾ��ɶ�
            {
                isAttackIdle = false;//�����ݾ�
                isAttacking = true;//������

                //�����ݾ�����
                if (isAttackIdleMove)
                {
                    isAttackIdleMove = false;//�O�_�����ݾ�����
                                  
                    if(!isMelee) OnChangeAnimation(animationName: "Backflip", animationType: false);
                    OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                }

                OnChangeAnimation(animationName: "AttackIdle", animationType: false);

                if ((transform.position - chaseObject.transform.position).magnitude < meleeAttackDistance)//�񨭧���
                {
                    attackNumber = maxAttackNumber;
                }
                else attackNumber = UnityEngine.Random.Range(1, maxAttackNumber);
                OnChangeAnimation(animationName: "AttackNumber", animationType: attackNumber);                
            }
        }  
    }   

    /// <summary>
    /// �����ݾ�����
    /// </summary>
    void OnAttackIdleMove()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //�����ݾ����ʮɶ� && �D�����ݾ����� && �����ݾ����ʤ�V!=�e��
        if (attackIdleMoveRandomTime <= 0 && !isAttackIdleMove && attackIdleMoveDiretion != 0)
        {            
            OnAttackIdleMoveExecution();//����������ʰʵe            
        }
        
        //�����ݾ�����
        if (isAttackIdleMove)
        {
            if (info.IsName("AttackIdleMove"))
            {
                int dir = 1;//�����ݾ����ʤ�V
                if (attackIdleMoveDiretion == 2) dir = -1;

                changeDiretionTime_Forward -= Time.deltaTime;//�󴫤�V�ɶ�
                if (changeDiretionTime_Forward <= 0)
                {
                    changeDiretionTime_Forward = changeDiretionTime;//�󴫤�V�ɶ�

                    //�k�観�P��
                    if (isCheckNearCompanion[0] && dir == 1)
                    {
                        attackIdleMoveDiretion = 2;
                        dir = -1;
                        OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: true);
                    }

                    //���観�P��
                    if (isCheckNearCompanion[1] && dir == -1)
                    {
                        attackIdleMoveDiretion = 1;
                        dir = 1;
                        OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: false);
                    }
                }

                if (isCheckNearCompanion[0] && isCheckNearCompanion[1])
                {
                    attackIdleMoveDiretion = 0;
                    /*OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                    OnChangeAnimation(animationName: "AttackIdle", animationType: true);*/
                }

                OnRotateToTarget();//����ܪ��a��V

                //���k����
                transform.position = transform.position + (transform.right * dir) * attackIdleMoveSpeed * Time.deltaTime;

                //�P�_�P���a�Z�� || ���k�����P��
                if ((transform.position - chaseObject.transform.position).magnitude < backMoveDistance
                    || attackIdleMoveDiretion == 0)
                {
                    //�V�ᨫ
                    transform.position = transform.position - transform.forward * attackIdleMoveSpeed * Time.deltaTime;
                }
            }            
        }

        if (info.IsName("Backflip"))//���Z���k�]
        {
            //�V���
            if (info.normalizedTime > 0.25f && info.normalizedTime < 0.6f) transform.position = transform.position - transform.forward * chaseSpeed * Time.deltaTime;

            if (info.normalizedTime >= 1)
            {
                OnChangeAnimation(animationName: "Backflip", animationType: false);
                OnChangeAnimation(animationName: "AttackIdleMove", animationType: true);
            }
        }
    }

    /// <summary>
    /// ����������ʰʵe
    /// </summary>
    void OnAttackIdleMoveExecution()
    {        
        isAttackIdleMove = true;//�O�_�����ݾ�����

        //�蹳�ʵeBoolen
        if (attackIdleMoveDiretion == 2) OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: true);
        else OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: false);

        //�D��Ԩ��� && �P���a�Z�� < backMoveDistance
        if (!isMelee && (transform.position - chaseObject.transform.position).magnitude < backMoveDistance)
        {
            OnChangeAnimation(animationName: "AttackIdle", animationType: false);
            OnChangeAnimation(animationName: "Backflip", animationType: true);                     

            return;
        }

        OnChangeAnimation(animationName: "AttackIdle", animationType: false);
        OnChangeAnimation(animationName: "AttackIdleMove", animationType: true);
    }
  
    /// <summary>
    /// �����d��
    /// </summary>
    /// <param name="radius">�����b�|</param>  
    bool OnDetectionRange(float radius)
    {
        if (Physics.CheckSphere(transform.position, radius, mask))
        {
            //OnGetAllPlayers();//����Ҧ����a

           /* if (allPlayerAlliance != null)
            {
                for (int i = 0; i < allPlayerAlliance.Length; i++)
                {
                    if(allPlayerAlliance[i])
                }
            }*/

            return true;
        }

        return false;
    }

    /// <summary>
    /// ����Ҧ����a
    /// </summary>
    void OnGetAllPlayers()
    {
        //�M��Ҧ����a
        if (allPlayers[allPlayers.Length - 1] == null) allPlayers = GameObject.FindGameObjectsWithTag("Player");        
    }

    /// <summary>
    /// �����O�_����AStart
    /// </summary>
    void OnCheckExecuteAStart()
    {
        //�I������_��
        if (charactersCollision.OnCollision_Wall())
        {
            if(isExecuteAStart)//���b����AStar
            {
                isExecuteAStart = false;
            }
        }

        //�I������_�ؼ�
        if (OnCollision_Target())
        {
            //�M��`�I
            if (!isExecuteAStart)
            {
                if (pathsList.Count > 0) pathsList.Clear();

                isExecuteAStart = true;
                Vector3 startPosition = transform.position;//�_�l��m(AI��m)
                Vector3 targetPosition = chaseObject.transform.position;
                pathsList = aStart.OnGetBestPoint(startPosition, targetPosition);//�M����|
                point = 0;//�M���`�I�s��
            }
        }      

        if(isExecuteAStart && pathsList.Count > 0) OnWayPoint();//�M����V
    }

    /// <summary>
    /// �I������_�ؼ�
    /// </summary>
    /// <returns></returns>
    bool OnCollision_Target()
    {
        //���a����s�b && �D����AStar
        if (chaseObject != null && chaseObject.activeSelf != false)
        {
            CharactersCollision charactersCollision = chaseObject.GetComponent<CharactersCollision>();

            if (charactersCollision != null)
            {
                //������ê��
                LayerMask mask = LayerMask.GetMask("StageObject");
                if (Physics.Linecast(transform.position + (this.charactersCollision.boxCenter / 2), chaseObject.transform.position + (charactersCollision.boxCenter / 2), mask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// �M����V
    /// </summary>
    void OnWayPoint()
    {
        //AI��m
        Vector3 AIPosistion = transform.position;
        AIPosistion.y = 0;

        //���a��m
        Vector3 playerPosition = pathsList[point];
        playerPosition.y = 0;

        //�P�`�I�Z��
        float distance = (AIPosistion - playerPosition).magnitude;
        
        if (distance < 1f)
        {
            point++;//�ثe�M���`�I�s��

            //��F�ؼ�
            if (point >= pathsList.Count)
            {                
                point = pathsList.Count;//�M���`�I�s��
                isExecuteAStart = false;//�D����AStart
                pathsList.Clear();                
            }

            //��F�j���`�I�ƶq
            if (point >= aStarCheckPointNumber)
            {                
                //�P�ؼФ����S����ê��
                if (!OnCollision_Target())
                {
                    point = pathsList.Count - 1;//�M���`�I�s��
                    isExecuteAStart = false;//�D����AStart
                    pathsList.Clear();
                }
            }
        }
    }

    /// <summary>
    /// �ˬd�̪񪱮a
    /// </summary>
    void OnCheckClosestPlayer()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        CheckTargetTime -= Time.deltaTime;//�����ؼЮɶ�

        if (CheckTargetTime < 0)
        {
            //�ĤH�h�LAI
            if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                allPlayerAlliance = GameObject.FindGameObjectsWithTag("Alliance");//�Ҧ����a�P���h�L                              

                if (allPlayerAlliance != null)
                {
                    chaseNumber = 0;//�l����H�s��
                    float closestPlayerDistance = 100000;//�̪�Z��
                    float distance;//��L���a�Z��

                    for (int i = 0; i < allPlayers.Length; i++)
                    {
                        if (allPlayers[i].activeSelf != false)
                        {
                            distance = (allPlayers[i].transform.position - transform.position).magnitude;
                            if (distance < closestPlayerDistance)
                            {
                                closestPlayerDistance = distance;
                                chaseNumber = i;
                            }
                        }
                    }
                    
                    //�ˬd���a�P���h�L
                    for (int i = 0; i < allPlayerAlliance.Length; i++)
                    {
                        if (allPlayerAlliance[i].activeSelf != false)
                        {
                            if ((allPlayerAlliance[i].transform.position - transform.position).magnitude < (allPlayers[chaseNumber].transform.position - transform.position).magnitude &&
                                allPlayerAlliance[i].GetComponent<CharactersCollision>() != null)
                            {
                                CheckTargetTime = CheckPlayerDistanceTime;
                                chaseObject = allPlayerAlliance[i];//�l���ؼ�
                                return;
                            }
                        }
                    }
                }

                chaseObject = allPlayers[chaseNumber];//�l���ؼ�(���a)
            }

            //�ڤ�P���h�LAI
            if(gameObject.layer == LayerMask.NameToLayer("Alliance"))
            {
                allEnemySoldier = GameObject.FindGameObjectsWithTag("Enemy");//�Ҧ��ĤH�h�L

                if (allEnemySoldier != null)
                {
                    //�ˬd���a�P���h�L
                    chaseNumber = 0;//�l����H�s��
                    float closestPlayerDistance = 100000;//�̪�Z��
                    float distance;//��L���a�Z��
                    for (int i = 0; i < allEnemySoldier.Length; i++)
                    {
                        if (allEnemySoldier[i].activeSelf != false &&
                            allEnemySoldier[i].GetComponent<CharactersCollision>() != null)
                        {
                            distance = (allEnemySoldier[i].transform.position - transform.position).magnitude;
                            if (distance < closestPlayerDistance)
                            {
                                closestPlayerDistance = distance;
                                chaseNumber = i;
                            }
                        }
                    }

                    if (allEnemySoldier != null)
                    {
                        chaseObject = allEnemySoldier[chaseNumber];//�l���ؼ�
                    }
                    else chaseObject = null;
                }                
            }

            CheckTargetTime = CheckPlayerDistanceTime;
        }
        
        //�[�ݳ̪񪱮a 
        Vector3 targetDiration = Vector3.one;//�ؼЦV�q        
        if (pathsList.Count > 0)//����AStart
        {
            targetDiration = pathsList[point] - transform.position;       
        }

        else//�@�뱡�p
        {
            if(chaseObject != null) targetDiration = chaseObject.transform.position - transform.position;
        }
        
        //�P�_�ؼЦb��/�k��               
        transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, maxRadiansDelta, maxRadiansDelta);        
        transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);  
    }

    /// <summary>
    /// �󴫪��A
    /// </summary>
    /// <param name="state">�󴫪��A</param>
    /// <param name="openAnimationName">�}�Ұʵe�W��</param>
    /// <param name="closeAnimationName">�}�����W��</param>
    /// <param name="animationType">����ʵeType</param>
    void OnChangeState<T>(AIState state, string openAnimationName, string closeAnimationName, T animationType)
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //�󴫪��A
        if (aiState != state)
        {
            aiState = state;//�󴫪��A
            OnChangeAnimation(animationName: openAnimationName, animationType: animationType);
            OnChangeAnimation(animationName: closeAnimationName, animationType: false);            
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
        
        switch(animationType.GetType().Name)
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
        //�@�몬�A���ʽd��
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(originalPosition, normalStateMoveRadius);

        //ĵ�ٽd��
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);

        //�l���d��
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);        

        //�����d��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        //�M��
        for (int i = 0; i < pathsList.Count - 1; i++)
        {
            Vector3 s = pathsList[i];
            //s.y = 3;
            Vector3 n = pathsList[i + 1];
            //n.y = 3;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(s, n);
        }

        //�e��P�񰻴��d��
       /* float chechSize = 1;
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position + charactersCollision.boxCenter + transform.forward * (charactersCollision.boxSize.x + chechSize / 2), new Vector3(chechSize, 1, chechSize));*/

        /*//�����d�����
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 1.4f, 1.3f);*/
    }
}
