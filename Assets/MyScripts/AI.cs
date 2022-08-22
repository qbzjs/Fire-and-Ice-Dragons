using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;
    CharactersCollision charactersCollision;
    AStart aStart = new AStart();

    LayerMask mask;//������HLayer

    [Header("�d��")]
    [Tooltip("�@�몬�A���ʽd��")] public float normalStateMoveRadius;
    [Tooltip("ĵ�ٽd��")] public float alertRadius;
    [Tooltip("�l���d��")] public float chaseRadius;
    [Tooltip("�����d��")] public float attackRadius;

    [Header("�@�몬�A")]
    [SerializeField] float[] normalStateMoveTime = new float[2];//�@�몬�A���ʶüƳ̤p�P�̤j��
    [SerializeField] float normalStateMoveSpeed;//�@�몬�A���ʳt��
    bool isNormalMove;//�O�_�@�몬�A�w�g����
    Vector3 originalPosition;//��l��m
    Vector3 forwardVector;//���ʥؼЦV�q    
    float normalStateTime;//�@�몬�A���ʮɶ�(�p�ɾ�)
    float normalRandomMoveTime;//�@�몬�A�üƲ��ʮɶ�
    float normalRandomAngle;//�@�몬�A�üƿ��ਤ��

    [Header("ĵ�٪��A")]
    [Tooltip("ĵ�٨�l���ɶ�")] public float alertToChaseTime;//ĵ�٨�l���ɶ�
    float alertTime;//ĵ�٨�l���ɶ�(�p�ɾ�)
    float CheckPlayerDistanceTime;//�������a�Z���ɶ�
    float CheckPlayerTime;//�������a�ɶ�(�p�ɾ�)    

    [Header("�l�����A")]
    [SerializeField] float chaseSpeed;//�l���t��
    [SerializeField] float maxRadiansDelta;//��V����
    [SerializeField] float[] readyChaseRandomTime;//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)
    float startChaseTime;//���}�԰���üƶ}�l�l���ɶ�(�p�ɾ�)
    int chaseObject;//�l����H�s��
    bool isStartChase;//�O�_�}�l�l��
    bool isReadChase;//�O�_�ǳưl��
    [SerializeField] public float chaseTurnTime;//�l����V�ɶ�

    [Header("�������A")]
    [SerializeField] float[] attackFrequency;//�����W�v(�üƳ̤p��, �̤j��)
    float attackTime;//�����ɶ�(�p�ɾ�)
    [SerializeField]bool isAttackIdle;//�O�_�����ݾ�
    [SerializeField]bool isAttacking;//�O�_������

    [SerializeField] GameObject[] players;//�Ҧ����a

    [Header("�M��")]
    [SerializeField] List<Vector3> pathsList = new List<Vector3>();//���ʸ��|�`�I  
    [SerializeField] int point = 0;//�M���`�I�s��    
    bool isExecuteAStart;//�O�_����AStart

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
        attackRadius = 2.0f;//�����d��

        //�@�몬�A
        originalPosition = transform.position;//��l��m
        forwardVector = transform.forward;//�e��V�q
        normalStateMoveSpeed = 1;//�@�몬�A���ʳt��
        normalStateMoveTime = new float[2] { 1.5f, 3.5f };//�@�몬�A���ʶüƳ̤p�P�̤j��

        //ĵ�٪��A        
        if (GameDataManagement.Instance.isConnect) players = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];//�Ҧ����a
        else players = new GameObject[1];
        CheckPlayerDistanceTime = 3;//�������a�Z���ɶ�
        alertToChaseTime = 2;//ĵ�٨�l���ɶ�

        //�l�����A
        chaseSpeed = 5.3f;//�l���t��
        maxRadiansDelta = 0.1405f;//��V����
        readyChaseRandomTime = new float[] { 1.5f, 3.5f};//���}�԰���üƷǳưl���ɶ�(�üƳ̤p��, �̤j��)

        //�������A
        attackFrequency = new float[2] { 0.5f, 1.75f};//�����W�v(�üƳ̤p��, �̤j��)  
    }

    void Update()
    {
        OnStateBehavior();
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

                normalRandomMoveTime = Random.Range(2, 4.5f);//�@�몬�A�üƲ��ʮɶ�

                //����
                if ((transform.position - originalPosition).magnitude > normalStateMoveRadius)//���}�@�몬�A���ʽd��
                {
                    forwardVector = originalPosition - transform.position;//���ʥؼЦV�q
                }
                else//�@�몬�A���ʽd��
                {
                    normalRandomAngle = Random.Range(0, 360);//�@�몬�A�üƿ��ਤ��
                    forwardVector = Quaternion.AngleAxis(normalRandomAngle, Vector3.up) * forwardVector;//���ʥؼЦV�q
                }

                //�󴫰ʵe
                OnChangeAnimation(animationName: "Walk", isAnimationActive: true);
            }

            if (normalRandomMoveTime > 0)//����
            {
                normalRandomMoveTime -= Time.deltaTime;

                //�����}��I��
                RaycastHit hit;
                if (charactersCollision.OnCollision_Characters(out hit)) forwardVector = transform.position - hit.transform.position;//��V
        
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
                normalStateTime = Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//�@�몬�A�üƲ��ʮɶ�                

                //�󴫰ʵe
                OnChangeAnimation(animationName: "Walk", isAnimationActive: false);
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
            if (aiState != AIState.ĵ�٪��A)
            {
                OnChangeState(state: AIState.ĵ�٪��A, openAnimationName: "Alert", closeAnimationName: "Walk");
            }
            else//ĵ�٪��A��
            {
                alertTime += Time.deltaTime;//ĵ�٨�l���ɶ�

                if (alertTime >= alertToChaseTime)
                {
                    OnHowlingBehavior();//�H�S�欰
                    OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert");
                }
            }
        }
        else
        {
            if (aiState != AIState.�@�몬�A)
            {
                normalRandomMoveTime = 0;//�@�몬�A�üƲ��ʮɶ�
                alertTime = 0;
                normalStateTime = Random.Range(normalStateMoveTime[0], normalStateMoveTime[1]);//�@�몬�A�üƲ��ʮɶ�
                                                                                               //
                OnChangeState(state: AIState.�@�몬�A, openAnimationName: "Idle", closeAnimationName: "Alert");
            }
        }
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
                companion.OnChangeStateToChase(players);
            }

        }

       /* RaycastHit hit;
        if(Physics.SphereCast(transform.position, alertRadius, transform.up, out hit, alertRadius, 1 << LayerMask.NameToLayer("Enemy")))
        {
            AI ai =  hit.transform.GetComponent<AI>();

            //�q���P��
            if (ai != null) ai.OnChangeStateToChase();
        }*/
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
    /// �l���d�򰻴�
    /// </summary>
    void OnChaseRangeCheck()
    {
        //�󴫪��A����
        if (OnDetectionRange(radius: chaseRadius))
        {
            if (aiState != AIState.�l�����A)
            {
                OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert");
            }
        }
    }

    /// <summary>
    /// �󴫪��A��l�����A
    /// </summary>
    /// <param name="chasePlaters">�l�������a</param>
    public void OnChangeStateToChase(GameObject[] chasePlaters)
    {
        if(aiState != AIState.�l�����A || aiState != AIState.�������A)
        {
            players = chasePlaters;//�l�������a
            OnChangeState(state: AIState.�l�����A, openAnimationName: "Howling", closeAnimationName: "Alert");
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
            isStartChase = true;//�}�l�l��

            OnChangeAnimation(animationName: "Howling", isAnimationActive: false);
            OnChangeAnimation(animationName: "Run", isAnimationActive: true);
        }

        //�}�l�l��
        if (isStartChase)
        {
            OnAttackRangeCheck();//�����d�򰻴�        
            OnCheckExecuteAStart();//�����I��

            //�ª��a����
            if(info.IsName("Run")) transform.position = transform.position + transform.forward * chaseSpeed * Time.deltaTime;

            chaseTurnTime -= Time.deltaTime;

            //�e�観�ۤv�H
            if (charactersCollision.OnCollision_Enemy())
            {
                chaseTurnTime = 1;

                //��V                
                transform.forward = Vector3.RotateTowards(transform.forward, transform.right, maxRadiansDelta, maxRadiansDelta);
                
            }
            else
            {
                if (chaseTurnTime <= 0) OnCheckClosestPlayer();//�ˬd�̪񪱮a
            }
        }

        /*//�I�𭫷s�p��M��
        if (pathsList.Count > 0)
        {
            for (int i = 0; i < charactersCollision.collisionObject.Length; i++)
            {
                aStart.OnGetBestPoint(transform.position, players[chaseObject].transform.position);
            }
        }*/
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

                OnChangeState(state: AIState.�������A, openAnimationName: "NormalAttack", closeAnimationName: "Run");
                startChaseTime = Random.Range(readyChaseRandomTime[0], readyChaseRandomTime[1]);//���}�԰���üƶ}�l�l���ɶ�(�p�ɾ�)
            }
        } 
        else
        {
            isReadChase = true;//�ǳưl��
            startChaseTime -= Time.deltaTime;//���}�԰���üƶ}�l�l���ɶ�(�p�ɾ�)

            if (startChaseTime <= 0)
            {
                //"�ݾ����ʵe"���S����"������"���A
                if (info.IsName("AttackIdle") && isAttacking) isAttacking = false;

                //�b�����ݾ��� && ���A������
                if (isAttackIdle && !isAttacking)
                {
                    if (aiState != AIState.�l�����A)
                    {
                        OnChangeAnimation("NormalAttack", false);
                        OnChangeState(state: AIState.�l�����A, openAnimationName: "Run", closeAnimationName: "AttackIdle");
                    }
                }
            }
            else
            {
                if (isReadChase)//�ǳưl��
                {
                    if (info.IsName("NormalAttack") && info.normalizedTime >= 1)
                    {
                        if (isAttacking)//������
                        {                            
                            OnChangeAnimation("NormalAttack", false);
                            OnChangeAnimation("AttackIdle", true);
                        }
                        
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// �������A�欰
    /// </summary>
    void OnAttackBehavior()
    {        
        info = animator.GetCurrentAnimatorStateInfo(0);

        OnAttackRangeCheck();
        if (!isAttacking) OnCheckClosestPlayer();//�ˬd�̪񪱮a       

        //�����M��
        if (pathsList.Count > 0)
        {            
            isExecuteAStart = false;//�D����AStart
            point = 0;//�M���`�I�s��
            pathsList.Clear();
        }

        if (info.IsName("NormalAttack") && info.normalizedTime >= 1)
        {
            if (!isAttackIdle)//�O�_�����ݾ�
            {                
                isAttackIdle = true;
                isAttacking = false;
               
                OnChangeAnimation("NormalAttack", false);

                if (OnDetectionRange(radius: attackRadius))//�����d��
                {
                    OnChangeAnimation("AttackIdle", true);
                }                

                //�üƧ����ݾ��ɶ�
                attackTime = Random.Range(attackFrequency[0], attackFrequency[1]);
            }
        }  
        
        if(!isReadChase) OnWaitAttack();//���ݧ���
    }

    /// <summary>
    /// ���ݧ���
    /// </summary>
    void OnWaitAttack()
    {
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;

            if(attackTime <= 0)
            {
                isAttackIdle = false;
                isAttacking = true;
                
                OnChangeAnimation(animationName: "AttackIdle", isAnimationActive: false);
                OnChangeAnimation(animationName: "NormalAttack", isAnimationActive: true);                
            }
        }
    }
  
    /// <summary>
    /// �����d��
    /// </summary>
    /// <param name="radius">�����b�|</param>  
    bool OnDetectionRange(float radius)
    {
        if (Physics.CheckSphere(transform.position, radius, mask))
        {
            //�M��Ҧ����a
            if (players[players.Length - 1] == null) players = GameObject.FindGameObjectsWithTag("Player");

            return true;
        }

        return false;
    }

    /// <summary>
    /// �����O�_����AStart
    /// </summary>
    void OnCheckExecuteAStart()
    {
        LayerMask mask = LayerMask.GetMask("StageObject");
        RaycastHit hit;

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
                Vector3 targetPosition = players[chaseObject].transform.position;
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
        if (players[chaseObject] != null)
        {
            CharactersCollision playerCharactersCollision = players[chaseObject].GetComponent<CharactersCollision>();

            //������ê��
            LayerMask mask = LayerMask.GetMask("StageObject");
            if (Physics.Linecast(transform.position + (charactersCollision.boxCenter / 2), players[chaseObject].transform.position + (playerCharactersCollision.boxCenter / 2), mask))
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

            for (int i = 0; i < players.Length; i++)
            {
                distance = (players[i].transform.position - transform.position).magnitude;
                if (distance < closestPlayerDistance)
                {
                    closestPlayerDistance = distance;
                    chaseObject = i;
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
            targetDiration = players[chaseObject].transform.position - transform.position;
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
    void OnChangeState(AIState state, string openAnimationName, string closeAnimationName)
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //�󴫪��A
        if (aiState != state)
        {
            aiState = state;
            OnChangeAnimation(animationName: openAnimationName, isAnimationActive: true);
            OnChangeAnimation(animationName: closeAnimationName, isAnimationActive: false);            
        }
    }

    /// <summary>
    /// �󴫰ʵe
    /// </summary>
    /// <param name="animationName">����ʵe�W��</param>
    /// <param name="isAnimationActive">�ʵe�O�_����</param>
    void OnChangeAnimation(string animationName, bool isAnimationActive)
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        
        animator.SetBool(animationName, isAnimationActive);
        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, animationName, isAnimationActive);
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
    }
}
