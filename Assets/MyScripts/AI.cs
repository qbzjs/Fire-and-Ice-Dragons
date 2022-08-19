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
    //NavMeshAgent navMeshAgent;

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
    float CheckPlayerDistanceTime;//�������a�Z���ɶ�
    float CheckPlayerTime;//�������a�ɶ�(�p�ɾ�)
    

    [Header("�l�����A")]
    [SerializeField] float chaseSpeed;//�l���t��
    int chaseObject;//�l����H�s��
    bool isStartChase;//�O�_�}�l�l��

    [Header("�������A")]
    [SerializeField] float[] attackFrequency;//�����W�v(�üƳ̤p��, �̤j��)
    float attackTime;//�����ɶ�(�p�ɾ�)
    [SerializeField]bool isAttackIdle;//�O�_�����ݾ�
    [SerializeField]bool isAttacking;//�O�_������

    [SerializeField] GameObject[] players;//�Ҧ����a

    [Header("�M��")]
    [SerializeField] List<Vector3> pathsList = new List<Vector3>();//���ʸ��|�`�I  
    int point = 0;//�M���`�I�s��
    [SerializeField]int numberOfSeach;//�j���`�I�ƶq
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
        
        //navMeshAgent = GetComponent<NavMeshAgent>();

        //�����d��
        normalStateMoveRadius = 3;//�@�몬�A���ʽd��
        alertRadius = 30;//ĵ�ٽd��
        chaseRadius = 18;//�l���d��
        attackRadius = 3.0f;//�����d��

        //�@�몬�A
        originalPosition = transform.position;//��l��m
        forwardVector = transform.forward;//�e��V�q
        normalStateMoveSpeed = 1;//�@�몬�A���ʳt��
        normalStateMoveTime = new float[2] { 1.5f, 3.5f };//�@�몬�A���ʶüƳ̤p�P�̤j��

        //ĵ�٪��A        
        if (GameDataManagement.Instance.isConnect) players = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];//�Ҧ����a
        else players = new GameObject[1];
        CheckPlayerDistanceTime = 3;//�������a�Z���ɶ�

        //�l�����A
        chaseSpeed = 5;//�l���t��

        //�������A
        attackFrequency = new float[2] { 0.5f, 3.0f};//�����W�v(�üƳ̤p��, �̤j��)  

        //�M��
        numberOfSeach = 3;//�j���`�I�ƶq
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

                float maxRadiansDelta = 0.03f;//��V����
                transform.forward = Vector3.RotateTowards(transform.forward, forwardVector, maxRadiansDelta, maxRadiansDelta);
                transform.position = transform.position + transform.forward * normalStateMoveSpeed * Time.deltaTime;                              
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
            OnChangeState(state: AIState.ĵ�٪��A, openAnimationName: "Alert", closeAnimationName: "Walk");
        }
        else
        {
            OnChangeState(state: AIState.�@�몬�A, openAnimationName: "Walk", closeAnimationName: "Alert");
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
    /// �l���d�򰻴�
    /// </summary>
    void OnChaseRangeCheck()
    {
        //�󴫪��A����
        if (OnDetectionRange(radius: chaseRadius))
        {            
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
            OnCheckClosestPlayer();//�ˬd�̪񪱮a
            OnAttackRangeCheck();//�����d�򰻴�        
            OnCheckCollision();//�����I��

            //�ª��a����
            transform.position = transform.position + transform.forward * chaseSpeed * Time.deltaTime;
            //navMeshAgent.SetDestination(players[alertObject].transform.position);
        }
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
            isAttacking = true;//�b������
            OnChangeState(state: AIState.�������A, openAnimationName: "NormalAttack", closeAnimationName: "Run");            
        } 
        else
        {
            //"�ݾ����ʵe"���S����"������"���A
            if (info.IsName("AttackIdle") && isAttacking) isAttacking = false;

            //�b�����ݾ��� && ���A������
            if (isAttackIdle && !isAttacking)
            {
                if (aiState != AIState.�l�����A) OnChangeAnimation("NormalAttack", false);
                OnChangeState(state: AIState.�l�����A, openAnimationName: "Run", closeAnimationName: "AttackIdle");                
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
        
        OnWaitAttack();//���ݧ���
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
    /// �����I��
    /// </summary>
    void OnCheckCollision()
    {        
        if (players[chaseObject] != null)
        {            
            CharactersCollision playerCharactersCollision = players[chaseObject].GetComponent<CharactersCollision>();

            //������ê��
            LayerMask mask = LayerMask.GetMask("StageObject");
            if (Physics.Linecast(transform.position + charactersCollision.boxCenter, players[chaseObject].transform.position + playerCharactersCollision.boxCenter, mask))
            {
                //�M��`�I
                if (!isExecuteAStart)
                {
                    isExecuteAStart = true;
                    Debug.LogError("s");
                    pathsList = aStart.OnGetBestPoint(transform.position, players[chaseObject].transform.position);
                    point = 0;//�M���`�I�s��
                }
            }         
        }

        if(isExecuteAStart && pathsList.Count > 0) OnWayPoint();//�M����V
    }

    /// <summary>
    /// �M����V
    /// </summary>
    void OnWayPoint()
    {
        //�P�`�I�Z��
        float distance = (transform.position - pathsList[point]).magnitude;       

        if (distance < 0.1f)
        {
            point++;//�ثe�M���`�I�s��

            //��F�ؼ� || ��F�j���`�I�ƶq
            if (point > pathsList.Count - 1 || point == numberOfSeach)
            {
                point = pathsList.Count - 1;

                isExecuteAStart = false;//�D����AStart
                pathsList.Clear();
            }          
        }
    }
    Vector3 horizontalCross;
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
        float maxRadiansDelta = 0.07f;//��V����
        Vector3 targetDiration;//�ؼЦV�q
        if (pathsList.Count > 0) targetDiration = pathsList[point] - transform.position;//����AStart
        else targetDiration = players[chaseObject].transform.position - transform.position;//�@�뱡�p        

        Vector3 v = Vector3.RotateTowards(transform.forward, targetDiration, maxRadiansDelta, maxRadiansDelta);//��V

        transform.rotation = Quaternion.LookRotation(v);
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
            s.y = 1;
            Vector3 n = pathsList[i + 1];
            n.y = 1;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(s, n);
        }
    }
}
