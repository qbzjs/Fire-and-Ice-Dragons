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

    [Header("�d��")]
    [SerializeField] public float normalStateMoveRadius;//�@�몬�A���ʽd��
    [SerializeField] public float alertRadius;//ĵ�ٽd��
    [SerializeField] public float chaseRadius;//�l���d��
    [SerializeField] public float attackRadius;//�����d��

    [Header("�@�몬�A")]
    [SerializeField] float[] normalStateMoveTime = new float[2];//�@�몬�A���ʶüƳ̤p�P�̤j��
    [SerializeField] float normalStateMoveSpeed;//�@�몬�A���ʳt��
    bool isNormalMove;//�O�_�@�몬�A�w�g����
    Vector3 originalPosition;//��l��m
    Vector3 forwardVector;//���ʥؼЦV�q    
    [SerializeField]float normalStateTime;//�@�몬�A���ʮɶ�(�p�ɾ�)
    float normalRandomMoveTime;//�@�몬�A�üƲ��ʮɶ�
    float normalRandomAngle;//�@�몬�A�üƿ��ਤ��

    [Header("ĵ�٪��A")]
    [SerializeField] float alertToChaseTime;//ĵ�٨�l���ɶ�
    [SerializeField] float leaveAlertRadiusAlertTime;//���}ĵ�ٽd��ĵ�ٮɶ�
    float leaveAlertTime;//���}ĵ�ٽd��ĵ�ٮɶ�(�p�ɾ�)
    float alertTime;//ĵ�٨�l���ɶ�(�p�ɾ�)
    float CheckPlayerDistanceTime;//�������a�Z���ɶ�
    float CheckPlayerTime;//�������a�ɶ�(�p�ɾ�)        

    [Header("�H�S���A")]
    bool isRotateToPlayer;//�O�_��V�ܪ��a

    [Header("�l�����A")]
    [SerializeField] float chaseSpeed;//�l���t��
    [SerializeField] float maxRadiansDelta;//��V����
    [SerializeField] float[] readyChaseRandomTime;//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)
    float startChaseTime;//���}�԰���üƶ}�l�l���ɶ�(�p�ɾ�)
    int chaseObject;//�l����H�s��
    bool isStartChase;//�O�_�}�l�l��
    bool isReadChase;//�O�_�ǳưl��
    float chaseTurnTime;//�l����V�ɶ�(�p�ɾ�)
    [SerializeField]int chaseDiretion;//�l����V(0 = �e��, 1 = �k��, 2 = ����)

    [Header("�ˬd�P��")]
    float chaseSlowDownSpeed;//�l����t�t��
    bool isPauseChase;//�O�_�Ȱ��l��
    [SerializeField]bool[] isCheckNearCompanion;//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)

    [Header("�������A")]
    [SerializeField] float[] attackFrequency;//�����W�v(�üƳ̤p��, �̤j��)
    [SerializeField] int attackNumber;//�����ۦ��s��(0 = ������)    
    int maxAttackNumber;//�i�ϥΧ����ۦ�
    float waitAttackTime;//���ݧ����ɶ�(�p�ɾ�)
    bool isAttackIdle;//�O�_�����ݾ�
    bool isAttacking;//�O�_������    
    bool isGetHit;//�O�_�Q����(�P�w"Pain"�ʵe�O�_Ĳ�o)

    [Header("�����ݾ�")]
    [SerializeField] float attackIdleMoveSpeed;//�����ݾ����ʳt��
    [SerializeField] float backMoveDistance;//�Z�����a�h��V�ᨫ
    bool isAttackIdleMove;//�O�_�����ݾ�����
    float attackIdleMoveRandomTime;//�����ݾ����ʶüƮɶ�(�p�ɾ�)     
    int attackIdleMoveDiretion;//�����ݾ����ʤ�V

    [Header("�M��")]
    bool isExecuteAStart;//�O�_����AStart
    List<Vector3> pathsList = new List<Vector3>();//���ʸ��|�`�I  
    int point = 0;//�M���`�I�s��        

    [Header("�Ҧ����a")]
    [SerializeField] GameObject[] allPlayers;//�Ҧ����a

    private void Awake()
    {
        animator = GetComponent<Animator>();

        gameObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer        
        
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
        mask = LayerMask.GetMask("Player");//������HLayer

        aStart.initial();
        charactersCollision = GetComponent<CharactersCollision>();

        //�����d��
        normalStateMoveRadius = 2.5f;//�@�몬�A���ʽd��
        alertRadius = 12;//ĵ�ٽd��
        chaseRadius = 8;//�l���d��
        attackRadius = 2.3f;//�����d��

        //�@�몬�A
        originalPosition = transform.position;//��l��m
        forwardVector = transform.forward;//�e��V�q
        normalStateMoveSpeed = 1;//�@�몬�A���ʳt��
        normalStateMoveTime = new float[2] { 1.5f, 3.5f };//�@�몬�A���ʶüƳ̤p�P�̤j��

        //ĵ�٪��A        
        if (GameDataManagement.Instance.isConnect) allPlayers = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];//�Ҧ����a
        else allPlayers = new GameObject[1];
        CheckPlayerDistanceTime = 3;//�������a�Z���ɶ�
        alertToChaseTime = 2;//ĵ�٨�l���ɶ�
        leaveAlertRadiusAlertTime = 3;//���}ĵ�ٽd��ĵ�ٮɶ�
        leaveAlertTime = leaveAlertRadiusAlertTime;//���}ĵ�ٽd��ĵ�ٮɶ�(�p�ɾ�)

        //�ˬd�P��
        isCheckNearCompanion = new bool[2] { false, false};//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)

        //�l�����A
        chaseSpeed = 5.3f;//�l���t��
        maxRadiansDelta = 0.1405f;//��V����
        readyChaseRandomTime = new float[] { 1.5f, 3.5f};//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)

        //�������A
        attackFrequency = new float[2] { 0.5f, 3.75f};//�����W�v(�üƳ̤p��, �̤j��)  
        maxAttackNumber = 3;//�i�ϥΧ����ۦ�

        //�����ݾ�
        attackIdleMoveSpeed = 1;//�����ݾ����ʳt��
        backMoveDistance = 1.5f;//�Z�����a�h��V�ᨫ
    }

    void Update()
    {
        OnStateBehavior();//���A�欰
        OnRotateToPlayer();//����ܪ��a��V
        OnCollision();//�I����
    }

    /// <summary>
    /// �I����
    /// </summary>    
    /// <returns></returns>
    public void OnCollision()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        RaycastHit hit;

        //�g�u��V
        Vector3[] rayDiration = new Vector3[] { transform.forward,
                                                transform.forward - transform.right,
                                                transform.right,
                                                transform.forward + transform.right,
                                                -transform.right };

        float boxSize = 0.5f;//�I���ؤj�p
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

    /// <summary>
    /// ���A�欰
    /// </summary>
    void OnStateBehavior()
    {
        switch (aiState)
        {
            case AIState.�@�몬�A:
                OnNormalStateBehavior();
                break;
            case AIState.ĵ�٪��A:
                OnAlertStateBehavior();
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

        if (normalStateTime <= 0)
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

                //�����}��I��
              /*  RaycastHit hit;
                if (charactersCollision.OnCollision_Characters(out hit)) forwardVector = transform.position - hit.transform.position;//��V*/
        
                //���� && ��V
                float maxRadiansDelta = 0.03f;//��V����
                transform.forward = Vector3.RotateTowards(transform.forward, forwardVector, maxRadiansDelta, maxRadiansDelta);
                transform.position = transform.position + transform.forward * normalStateMoveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

                //charactersCollision.OnCollision_Character();//�I����_�}��
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
        OnCheckClosestPlayer();//�ˬd�̪񪱮a             
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
                
        OnGetAllPlayers();//����Ҧ����a
        OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert", animationType: true);

        OnHowlingBehavior();//�H�S�欰
    }

    /// <summary>
    /// �H�S�欰
    /// </summary>
    void OnHowlingBehavior()
    {
        AI[] companions = GameObject.FindObjectsOfType<AI>();        
        foreach (var companion in companions)
        {
            float distance = (transform.position - companion.transform.position).magnitude;
            if(distance <= alertRadius)
            {
                //�q���P��
                companion.OnChangeStateToChase(allPlayers: allPlayers, chaseObject: chaseObject);
            }
        }   
    }

    /// <summary>
    /// �󴫪��A��l�����A
    /// </summary>
    /// <param name="allPlayers">�Ҧ����a</param>
    /// <param name="chaseObject">�l�������a</param>
    void OnChangeStateToChase(GameObject[] allPlayers, int chaseObject)
    {
        this.allPlayers = allPlayers;//�Ҧ����a
        this.chaseObject = chaseObject;//�l�������a
        StartCoroutine(OnWautChase());//���ݰl��
    }

    /// <summary>
    /// ����ܪ��a��V
    /// </summary>
    void OnRotateToPlayer()
    {
        //��V�ܪ��a
        if (isRotateToPlayer)
        {
            Vector3 targetDiration = allPlayers[chaseObject].transform.position - transform.position;
            transform.forward = transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, maxRadiansDelta, maxRadiansDelta);
        }
    }

    /// <summary>
    /// ���ݰl��
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWautChase()
    {
        isRotateToPlayer = true;//��V�ܪ��a

        yield return new WaitForSeconds(1);

        isRotateToPlayer = false;

        if (aiState != AIState.�l�����A || aiState != AIState.�������A)
        {
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

        //�H�S���}�l�l��
        if (info.IsName("Howling") && info.normalizedTime >= 1)
        {
            //�^�@�몬�A�ƫe�]�w
            isNormalMove = false;//�O�_�@�몬�A�w�g����
            normalStateTime = UnityEngine.Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//���s�@�몬�A�üƲ��ʮɶ�

            //�l��
            isStartChase = true;//�}�l�l��                        

            Vector3 targetDiration = allPlayers[chaseObject].transform.position - transform.position;
            transform.forward = transform.forward = Vector3.RotateTowards(transform.forward, targetDiration, 1, 1);

            OnChangeAnimation(animationName: "Howling", animationType: false);
            OnChangeAnimation(animationName: "Run", animationType: true);
        }

        //�}�l�l��
        if (isStartChase)
        {
            OnAttackRangeCheck();//�����d�򰻴�        
            OnCheckExecuteAStart();//�����O�_����AStart
            OnCheckClosestPlayer();//�ˬd�̪񪱮a            
            OnCheckForwardCompanion();//�ˬd�e��P��
            OnCheckLefrAndRightCompanion();//�ˬd���k�P��

            //�ª��a����
            if (info.IsName("Run")) OnMoveBehavior();

            //�ˬd�̪񪱮a
            if (chaseTurnTime <= 0) OnCheckClosestPlayer();

            //�l�������a���` && �l���d�򤺨S����L���a
            if (allPlayers[chaseObject].activeSelf == false && OnDetectionRange(radius: chaseRadius) == false)
            {
                if (aiState != AIState.�@�몬�A)
                {
                    OnChangeState(state: AIState.�@�몬�A, openAnimationName: "Idle", closeAnimationName: "Run", animationType: true);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// �ˬd �P��/��ê�� �I����
    /// </summary>
    /// <param name="diretion">��V</param>
    bool OnCheckCompanionBox(Vector3 diretion)
    {
        float chechSize = 0.5f;//�ˬd�ؤj�p
        LayerMask mask = LayerMask.GetMask("Enemy");

        //�I��P��
        if (Physics.CheckBox(transform.position + charactersCollision.boxCenter + diretion * (charactersCollision.boxSize.x + (chechSize / 2)), new Vector3(chechSize, 0.1f, chechSize), Quaternion.Euler(transform.localEulerAngles), mask))
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
        //�ˬd�e��P��
        if (OnCheckCompanionBox(diretion: transform.forward))
        {
            chaseSlowDownSpeed -= 0.4f * Time.deltaTime;//�l����t�t��            
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

            chaseDiretion = UnityEngine.Random.Range(1, 3);//����V
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
        //�ˬd�k��P��
        if (OnCheckCompanionBox(diretion: transform.right))//�ˬd�k��P��
        {
            if (!isCheckNearCompanion[0])//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)
            {
                isCheckNearCompanion[0] = true;
                chaseDiretion = 2;//�l����V(0 = �e��, 1 = �k��, 2 = ����)
            }
        }
        else isCheckNearCompanion[0] = false;//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)
       
        //�ˬd����P��
        if (OnCheckCompanionBox(diretion: -transform.right))
        {
            if (!isCheckNearCompanion[1])//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)
            {
                isCheckNearCompanion[1] = true;
                chaseDiretion = 1;//�l����V(0 = �e��, 1 = �k��, 2 = ����)
            }
        }
        else isCheckNearCompanion[1] = false;//�ˬd����P��(0 = �k�観�I��, 1 = ���観�I��)    
        
        //���k���S���I���P��
        if(!isCheckNearCompanion[0] && !isCheckNearCompanion[1]) chaseDiretion = 0;//�l����V(0 = �e��, 1 = �k��, 2 = ����)
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
            if (isReadChase) isReadChase = false;//�O�_�ǳưl��

            if (aiState != AIState.�������A)
            {
                isAttacking = true;//������
                isAttackIdle = false;//�D�����ݾ�               
                                                
                attackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);//�����ۦ�
                OnChangeState(state: AIState.�������A, openAnimationName: "AttackNumber", closeAnimationName: "Run", animationType: attackNumber);

                startChaseTime = UnityEngine.Random.Range(readyChaseRandomTime[0], readyChaseRandomTime[1]);//���}�԰���üƶ}�l�l���ɶ�(�p�ɾ�)
            }
        } 
        else
        {
            isReadChase = true;//�ǳưl��
            startChaseTime -= Time.deltaTime;//���}�԰���üƶ}�l�l���ɶ�(�p�ɾ�)

            //�l�������a���` && �l���d�򤺨S����L���a
            if (allPlayers[chaseObject].activeSelf == false && OnDetectionRange(radius: chaseRadius) == false)
            {
                if (aiState != AIState.�@�몬�A)
                {
                    OnChangeState(state: AIState.�@�몬�A, openAnimationName: "Idle", closeAnimationName: "AttackIdle", animationType: true);
                    if(isAttackIdleMove) OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);

                    return;
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
    /// �������A�欰
    /// </summary>
    void OnAttackBehavior()
    {        
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnAttackRangeCheck();//�����d�򰻴�
        if (!isAttacking) OnCheckClosestPlayer();//�ˬd�̪񪱮a       

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

                attackIdleMoveDiretion = chaseDiretion;//�����ݾ����ʤ�V
                waitAttackTime = UnityEngine.Random.Range(attackFrequency[0], attackFrequency[1]);//�üƧ����ݾ��ɶ�
                attackIdleMoveRandomTime = UnityEngine.Random.Range(0.5f, waitAttackTime); ;//�����ݾ����ʶüƮɶ�(�p�ɾ�)

                waitAttackTime = waitAttackTime + attackIdleMoveRandomTime;//�����ݾ��ɶ� + �����ݾ����ʮɶ�
            } 
        }

        //���������Q�����L����"����"�ʵe
        if (info.IsTag("Attack") && isGetHit)
        {            
            OnChangeAnimation(animationName: "Pain", animationType: false);
            isGetHit = false;
        }
        
        if(!isReadChase) OnWaitAttackBehavior();//���ݧ����欰
    }    

    /// <summary>
    /// ���ݧ����欰
    /// </summary>
    void OnWaitAttackBehavior()
    {
        //���ݧ����ɶ� && �D������
        if (waitAttackTime > 0 && !isAttacking)
        {
            waitAttackTime -= Time.deltaTime;//�üƧ����ݾ��ɶ�
            attackIdleMoveRandomTime -= Time.deltaTime;//�����ݾ����ʶüƮɶ�(�p�ɾ�)

            OnCheckLefrAndRightCompanion();//�ˬd���k�P��
            OnAttackIdleMove();//�����ݾ�����            

            if (waitAttackTime <= 0)//�����ݾ��ɶ�
            {
                isAttackIdle = false;//�����ݾ�
                isAttacking = true;//������

                //�����ݾ�����
                if (isAttackIdleMove)
                {
                    isAttackIdleMove = false;//�O�_�����ݾ�����
                    OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                }

                OnChangeAnimation(animationName: "AttackIdle", animationType: false);

                attackNumber = UnityEngine.Random.Range(1, maxAttackNumber + 1);
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

        //�����ݾ����ʮɶ� && �D�����ݾ����� && �D������ && �l����V��=�e��
        if (attackIdleMoveRandomTime <= 0 && !isAttackIdleMove && chaseDiretion != 0)
        {
            OnCheckLefrAndRightCompanion();//�ˬd���k�P��
            OnAttackIdleMoveExecution();//����������ʰʵe            
        }
        
        //�����ݾ�����
        if (isAttackIdleMove && info.IsName("AttackIdleMove"))
        {            
            //�����ݾ����ʤ�V!=�e��
            if (attackIdleMoveDiretion != 0)
            {
                int dir = 1;//�����ݾ����ʤ�V
                if (attackIdleMoveDiretion == 2) dir = -1;
                else dir = 1;

                //���k����
                transform.position = transform.position + (transform.right * dir) * attackIdleMoveSpeed * Time.deltaTime;
            }
            else
            {
                if(isAttackIdleMove)//�O�_�����ݾ�����
                {                    
                    isAttackIdleMove = false;
                    OnChangeAnimation(animationName: "AttackIdleMove", animationType: false);
                    OnChangeAnimation(animationName: "AttackIdle", animationType: true);
                }                
            }

            //�P�_�P���a�Z��
            if ((transform.position - allPlayers[chaseObject].transform.position).magnitude < backMoveDistance)
            {
                //�V�ᨫ
                transform.position = transform.position - transform.forward * attackIdleMoveSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// ����������ʰʵe
    /// </summary>
    void OnAttackIdleMoveExecution()
    {
        attackIdleMoveDiretion = chaseDiretion;//�����ݾ����ʤ�V

        isAttackIdleMove = true;//�O�_�����ݾ�����
        OnChangeAnimation(animationName: "AttackIdle", animationType: false);
        OnChangeAnimation(animationName: "AttackIdleMove", animationType: true);

        //�蹳�ʵeBoolen
        if (attackIdleMoveDiretion == 2) OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: true);
        else OnChangeAnimation(animationName: "IsAttackIdleMoveMirror", animationType: false);
    }
  
    /// <summary>
    /// �����d��
    /// </summary>
    /// <param name="radius">�����b�|</param>  
    bool OnDetectionRange(float radius)
    {
        if (Physics.CheckSphere(transform.position, radius, mask))
        {
            OnGetAllPlayers();//����Ҧ����a
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

        //�I������_���a
        if (OnCollision_Player())
        {
            //�M��`�I
            if (!isExecuteAStart)
            {
                if (pathsList.Count > 0) pathsList.Clear();

                isExecuteAStart = true;
                Vector3 startPosition = transform.position;//�_�l��m(AI��m)
                Vector3 targetPosition = allPlayers[chaseObject].transform.position;
                pathsList = aStart.OnGetBestPoint(startPosition, targetPosition);//�M����|
                point = 0;//�M���`�I�s��
            }
        }      

        if(isExecuteAStart && pathsList.Count > 0) OnWayPoint();//�M����V
    }

    /// <summary>
    /// �I������_���a
    /// </summary>
    /// <returns></returns>
    bool OnCollision_Player()
    {
        //���a����s�b && �D����AStar
        if (allPlayers[chaseObject] != null)
        {
            CharactersCollision playerCharactersCollision = allPlayers[chaseObject].GetComponent<CharactersCollision>();

            //������ê��
            LayerMask mask = LayerMask.GetMask("StageObject");
            if (Physics.Linecast(transform.position + (charactersCollision.boxCenter / 2), allPlayers[chaseObject].transform.position + (playerCharactersCollision.boxCenter / 2), mask))
            {                
                return true;
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

            //��F�ؼ� || ��F�j���`�I�ƶq
            if (point >= pathsList.Count || !OnCollision_Player())
            {                
                point = pathsList.Count;//�M���`�I�s��
                isExecuteAStart = false;//�D����AStart
                pathsList.Clear();
            }            
        }
    }
    
    /// <summary>
    /// �ˬd�̪񪱮a
    /// </summary>
    void OnCheckClosestPlayer()
    {
        CheckPlayerTime -= Time.deltaTime;//�������a�ɶ�

        if (CheckPlayerTime < 0)
        {
            chaseObject = 0;//�l����H�s��
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
                        chaseObject = i;
                    }
                }
            }

            CheckPlayerTime = CheckPlayerDistanceTime;
        }

        //�[�ݳ̪񪱮a 
        Vector3 targetDiration;//�ؼЦV�q        
        if (pathsList.Count > 0)//����AStart
        {
            targetDiration = pathsList[point] - transform.position;       
        }

        else//�@�뱡�p
        {
            targetDiration = allPlayers[chaseObject].transform.position - transform.position;
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
        float chechSize = 1;
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position + charactersCollision.boxCenter + transform.forward * (charactersCollision.boxSize.x + chechSize / 2), new Vector3(chechSize, 1, chechSize));

        /*//�����d�����
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 1.4f, 1.3f);*/
    }
}
