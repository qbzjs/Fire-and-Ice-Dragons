using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

/// <summary>
/// ���a����
/// </summary>
public class PlayerControl : MonoBehaviourPunCallbacks
{
    Animator animator;
    AnimatorStateInfo info;
    CharactersCollision charactersCollision;
    GameData_NumericalValue NumericalValue;

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;

    //����
    float inputX;//��JX��
    float inputZ;//��JZ��
    float inputValue;//�`��J��
    Vector3 forwardVector;//�e��V�q
    Vector3 horizontalCross;//�����b
    float addMoveSpeed;//�W�[���ʳt�׭�
    bool isSendRun;//�O�_�w�o�e���ʰʵe       

    //���D
    public bool isJump;//�O�_���D
    bool isJumpTimeCountdown;//�i������D�˼�(�˼Ʈɤ�����D)
    float doJumpTime;//������D�ɶ����j
    float JumpTime;//������D�ɶ����j(�p�ɾ�)   
    bool isSendClosePain;//�O�_�w�������˰ʵe

    //�{��
    bool isDodge;//�O�_�{��
    bool isDodgeCollision;//�O�_�{���I��

    //����
    bool isNormalAttack;//�O�_���q����        
    bool isSkillAttack;//�O�_�ޯ����    
    int normalAttackNumber;//���q�����s��
    public int GetNormalAttackNumber => normalAttackNumber;
    bool isJumpAttack;//�O�_���D����
    public bool isJumpAttackMove;//���D�����U��

    private void Awake()
    {       
        gameObject.layer = LayerMask.NameToLayer("Player");//�]�wLayer                
        gameObject.tag = "Player";//�]�wTag

        animator = GetComponent<Animator>();
        
        //if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();
        charactersCollision = GetComponent<CharactersCollision>();

        //�s�u && ���O�ۤv��
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            GameSceneManagement.Instance.OnSetMiniMapPoint(transform, GameSceneManagement.Instance.loadPath.miniMapMatirial_OtherPlayer);//�]�w�p�a���I�I
            this.enabled = false;
            return;
        }        
    }
    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //�ХD
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = false;//�����۰ʦP�B����
        }

        //�]�w��v���[���I
        CameraControl.SetLookPoint = ExtensionMethods.FindAnyChild<Transform>(transform, "CameraLookPoint");

        //�p�a����v��
        GameObject miniMap_Camera = GameObject.Find("MiniMap_Camera");
        miniMap_Camera.transform.SetParent(transform);
        miniMap_Camera.transform.localPosition = new Vector3(0, 10, 0);               

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //���D
        doJumpTime = 1.3f;//������D�ɶ����j
        JumpTime = doJumpTime;//������D�ɶ����j(�p�ɾ�)

        //����
        forwardVector = transform.forward;

        //����
         Cursor.visible = false;//��������
         Cursor.lockState = CursorLockMode.Locked;//��w����

        //Buff
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            switch(GameDataManagement.Instance.equipBuff[i])
            {               
                case 2://�W�[���m��
                    charactersCollision.addDefence = GameDataManagement.Instance.numericalValue.buffAbleValue[2];
                    break;
                case 3://�W�[�l��ĪG�W
                    charactersCollision.isSuckBlood = true;
                    break;
                case 4://�[���ʳt�׭�
                    addMoveSpeed = NumericalValue.playerMoveSpeed * (GameDataManagement.Instance.numericalValue.buffAbleValue[3] / 100);                    
                    break;
                case 5://�W�[�^�_�ĪG
                    charactersCollision.isSelfHeal = true;
                    break;
            }         
        }  
        
        /*//�P�_���d
        if(GameDataManagement.Instance.selectLevelNumber == 1)//���ե�1 else = 0
        {
            //�s��¶���a
            Dragon_Level1 dragon_Level1 = GameObject.Find("Dragon_Around").GetComponent<Dragon_Level1>();
            dragon_Level1.SetRotateAroundTarger = transform;
        }*/
    }

    void Update()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //���O���`���A & ���O�����ʵe
        if (!charactersCollision.isDie && !info.IsName("Pain"))
        {
            //�S���}�ҿﶵ����
            if (!GameSceneUI.Instance.isOptions)
            {
                OnJumpControl();
                OnAttackControl();

                if (!isJumpAttack && !isNormalAttack && !isSkillAttack && !info.IsName("Pain"))
                {
                    if (!info.IsName("Dodge")) OnMovementControl();
                    else
                    {
                        if (inputValue > 0)
                        {
                            inputValue = 0;
                            animator.SetFloat("Run", inputValue);
                        }
                    }

                   // OnDodgeControl();
                }

                if (!isJumpAttack && !isSkillAttack && !info.IsName("Pain"))
                {
                    OnDodgeControl();
                }
            }

            OnFallBehavior();
        }                

        OnJumpHehavior();
        OnInput();

        if (isJumpAttackMove) OnJumpAttackMove();

       /* if (Input.GetKeyDown(KeyCode.P))        
        {
            AI[] ai = GameObject.FindObjectsOfType<AI>();

            foreach (var a in ai)
            {
                Debug.LogError("s");
                a.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);//�洫�ХD
            }
            
        }*/
    }

    /// <summary>
    /// ���U�欰
    /// </summary>
    void OnFallBehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Fall"))
        {
            if(isJump)//��������
            {                
                isJump = false;
                animator.SetBool("Jump", isJump);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Jump", isJump);
            }
            
            if(isJumpAttack)
            {
                isJumpAttack = false;
                animator.SetBool("JumpAttack", isJumpAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "JumpAttack", isJumpAttack);
            }
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    void OnAttackControl()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //���q����
        if (Input.GetMouseButton(0) && !info.IsTag("SkillAttack") && !info.IsTag("SkillAttack-2") && !info.IsName("Fall") && !isDodge)
        {           
            //���D����
            if (isJump && !isJumpAttack)
            {
                isJumpAttack = true;

                animator.SetBool("JumpAttack", isJumpAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "JumpAttack", isJumpAttack);
                return;
            }

            //�ޯ����
            if (Input.GetMouseButtonDown(1))
            {
                //���q�����ɶ������U
                if (info.IsTag("NormalAttack") && info.normalizedTime < 1)
                {                                       
                    isSkillAttack = true;                    
                }
            }

            if(info.IsTag("NormalAttack") || info.IsTag("SkillAttack"))
            {
                if(info.normalizedTime < 0.05f)
                {
                    //��V               
                    if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//����
                    else if (inputX != 0) transform.forward = horizontalCross * inputX;//���k
                    else if (inputZ != 0) transform.forward = forwardVector * inputZ;//�e��
                    /*float maxRadiansDelta = 0.3f;//��V����
                    if (inputX != 0 && inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, (horizontalCross * inputX) + (forwardVector * inputZ), maxRadiansDelta, maxRadiansDelta);//����
                    else if (inputX != 0) transform.forward = Vector3.RotateTowards(transform.forward, horizontalCross * inputX, maxRadiansDelta, maxRadiansDelta);//���k
                    else if (inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, forwardVector * inputZ, maxRadiansDelta, maxRadiansDelta);//�e��*/
                }
            }

            //���ݴ��q���������A����ޯ�
            if (isSkillAttack && info.IsTag("NormalAttack") && info.normalizedTime >= 1)
            {
                /*//��V               
                if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//����
                else if (inputX != 0) transform.forward = horizontalCross * inputX;//���k
                else if (inputZ != 0) transform.forward = forwardVector * inputZ;//�e��*/
                
                animator.SetBool("SkillAttack", isSkillAttack);
                if(GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "SkillAttack", isSkillAttack);
                return;
            }                           

            //���q����(�Ĥ@������)
            if (!isSkillAttack && !isNormalAttack )//&& !isJumpAttack)
            {
                isNormalAttack = true;
                normalAttackNumber = 1;                

                animator.SetBool("NormalAttack", isNormalAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
            }                        

            //�������q�����ۦ�
            if (info.IsTag("NormalAttack") && info.normalizedTime >= 1)
            {                                
                normalAttackNumber++;//���q�����s��                
                if (normalAttackNumber > 3) normalAttackNumber = 0;
                
                /*//��V               
                if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//����
                else if (inputX != 0) transform.forward = horizontalCross * inputX;//���k
                else if (inputZ != 0) transform.forward = forwardVector * inputZ;//�e��*/

                animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                if (GameDataManagement.Instance.isConnect)  PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);
            }          
        }       
        else
        {
            //�ʵe/��������
            if (info.normalizedTime >= 1)
            {                                
                if (info.IsTag("NormalAttack") || info.IsTag("SkillAttack") || info.IsTag("SkillAttack-2") || info.IsTag("JumpAttack"))
                {                    
                    normalAttackNumber = 0;//���q�����s��
                    isNormalAttack = false;
                    isSkillAttack = false;                    

                    animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);

                    if (info.IsTag("NormalAttack"))
                    {                        
                        animator.SetBool("NormalAttack", isNormalAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
                    }

                    if (info.IsTag("SkillAttack"))
                    {
                        animator.SetBool("SkillAttack", isSkillAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "SkillAttack", isSkillAttack);
                    }

                    if (info.IsTag("SkillAttack-2"))
                    {
                        animator.SetBool("SkillAttack", isSkillAttack);
                        animator.SetBool("SkillAttack-2", false);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "SkillAttack", isSkillAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "SkillAttack-2", false);
                    }                    
                }              
            }

            if (info.IsTag("JumpAttack") && isNormalAttack)
            {
                isNormalAttack = false;

                animator.SetBool("NormalAttack", isNormalAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
            }
        }

        if (info.IsTag("JumpAttack") && info.normalizedTime >= 1)
        {
            isJump = false;            
            isJumpAttack = false;

            animator.SetBool("JumpAttack", isJumpAttack);
            animator.SetBool("Jump", isJump);
            
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "JumpAttack", isJumpAttack);
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Jump", isJump);                
            }
        }

        //�����������{��
        if((info.IsTag("NormalAttack") && info.normalizedTime < 0.5f) && isDodge)
        {
            isDodge = false;

            animator.SetBool("Dodge", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Dodge", false);
        }

        //�ޯ�������������q����
        if (isNormalAttack && info.normalizedTime > 0.35f && info.IsTag("SkillAttack") || info.IsTag("SkillAttack-2"))
        {
            isNormalAttack = false;
            isNormalAttack = false;

            animator.SetBool("NormalAttack", isNormalAttack);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
        }           
    }  

    /// <summary>
    /// �{������
    /// </summary>
    void OnDodgeControl()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        
        //�{������
        if (info.IsName("Idle") || info.IsName("Run") || (info.IsTag("NormalAttack") && info.normalizedTime > 0.6f))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isDodge = true;
                isDodgeCollision = false;

                for (int i = 0; i < charactersCollision.GetCollisionObject.Length; i++)
                {
                    //�P�_�O�_�w�g�I��
                    if (isDodgeCollision = charactersCollision.GetCollisionObject[i]) break;                    
                }                
       
                if(info.IsTag("NormalAttack"))
                {
                    normalAttackNumber = 0;//���q�����s��
                    isNormalAttack = false;

                    animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                    animator.SetBool("NormalAttack", isNormalAttack);
                    if (GameDataManagement.Instance.isConnect)
                    {
                        PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
                        PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);
                    }
                }

                animator.SetBool("Dodge", true);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Dodge", true);                 
            }
        }

        //�{������
        if (info.IsName("Dodge") && info.normalizedTime < 1)
        {
            LayerMask mask = LayerMask.GetMask("StageObject");

            //�P�_�O�_���I��
            if (Physics.Raycast(transform.position + boxCenter, transform.forward, boxSize.z * boxSize.z, mask)) isDodgeCollision = true;//�{���I��

            if (isDodgeCollision) transform.position = transform.position - transform.forward * 5 * Time.deltaTime;
            else transform.position = transform.position + transform.forward * GameDataManagement.Instance.numericalValue.playerDodgeSeppd * Time.deltaTime;

            //���q������
            if(isNormalAttack)
            {
                normalAttackNumber = 0;//���q�����s��
                isNormalAttack = false;

                animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                animator.SetBool("NormalAttack", isNormalAttack);
                if (GameDataManagement.Instance.isConnect)
                {
                    PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
                    PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);
                }
            }
        }

        //�{������
        if (info.IsName("Dodge") && info.normalizedTime > 1)
        {
            isDodge = false;

            animator.SetBool("Dodge", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Dodge", false);

            animator.SetBool("Pain", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Pain", false);
        }
    }

    /// <summary>
    /// ���D����
    /// </summary>
    void OnJumpControl()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumpTimeCountdown && !isJump && !isNormalAttack && !isSkillAttack && !info.IsName("Dodge") && !info.IsName("Fall") && !info.IsName("Pain"))
        {            
            isJump = true;
            isNormalAttack = false;
            isJumpTimeCountdown = true;//�i������D�˼�                        
            isSendClosePain = false;//�O�_�w�������˰ʵe

            charactersCollision.floating_List.Add(new CharactersFloating { target = transform, force = NumericalValue.playerJumpForce, gravity = NumericalValue.gravity});//�B��List

            animator.SetBool("NormalAttack", isNormalAttack);
            animator.SetBool("Jump", isJump);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "NormalAttack", isNormalAttack);
                PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Jump", isJump);
            }
        }
    }

    /// <summary>
    /// ���D�欰
    /// </summary>
    void OnJumpHehavior()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        //���D�p�ɾ��˼�
        if (isJumpTimeCountdown)
        {
            JumpTime -= Time.deltaTime;//���D�p�ɾ�

            if (JumpTime <= 0)
            {
                JumpTime = doJumpTime;//���������D�ɶ����j 
                isJumpTimeCountdown = false;
            }
        }

        //���������D�ɶ����j
        if (info.IsName("Jump"))
        {         
            if (doJumpTime != animator.GetCurrentAnimatorClipInfo(0).FirstOrDefault(x => x.clip.name == "Jump").clip.length + 0.4f) doJumpTime = animator.GetCurrentAnimatorClipInfo(0).FirstOrDefault(x => x.clip.name == "Jump").clip.length + 0.4f;            
        }

        if (info.IsTag("Jump") || info.IsTag("JumpAttack"))
        {           
            LayerMask mask = LayerMask.GetMask("StageObject");
            RaycastHit hit;
            if (charactersCollision.OnCollision_Floor(out hit))
            {
                if ((isJump || isJumpAttack) && info.normalizedTime > 0.9f)
                {
                    if (isJump)
                    {
                        isJump = false;
                        animator.SetBool("Jump", isJump);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Jump", isJump);
                    }
                    if (isJumpAttack) isJumpAttackMove = false;
                }
            }        
        }

        //���D�ɨ�����
        if(isJump && info.IsTag("Pain") && !isSendClosePain)
        {            
            isSendClosePain = true;//�O�_�w�������˰ʵe
            animator.SetBool("Pain", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Pain", false);
        }
    }

    /// <summary>
    /// ���D��������
    /// </summary>
    public void OnJumpAttackMove()
    {
        transform.position = transform.position + 10 * Time.deltaTime * Vector3.down;//��t�U��
    }

    /// <summary>
    /// ���ʱ���
    /// </summary>
    void OnMovementControl()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName("JumpAttack"))
        {
            //��V
            float maxRadiansDelta = 0.1405f;//��V����
            if (inputX != 0 && inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, (horizontalCross * inputX) + (forwardVector * inputZ), maxRadiansDelta, maxRadiansDelta);//����
            else if (inputX != 0) transform.forward = Vector3.RotateTowards(transform.forward, horizontalCross * inputX, maxRadiansDelta, maxRadiansDelta);//���k
            else if (inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, forwardVector * inputZ, maxRadiansDelta, maxRadiansDelta);//�e��      
        }               

        inputValue = Mathf.Abs(inputX) + Mathf.Abs(inputZ);//��J��
        if (inputValue > 1) inputValue = 1;

        animator.SetFloat("Run", inputValue);
        if (GameDataManagement.Instance.isConnect)
        {
            if (!info.IsName("Fall"))
            {
                if (inputValue > 0.1f && !isSendRun)
                {
                    isSendRun = true;
                    PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Run", inputValue);
                }
                if (inputValue < 0.1f && isSendRun)
                {
                    isSendRun = false;
                    PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Run", 0.0f);
                }
            }
        }

        RaycastHit hit;
        if (info.IsName("Jump"))
        {
            if(charactersCollision.OnCollision_Floor(out hit))
            {
                if (hit.transform.tag == "Stairs") inputValue = (Mathf.Abs(inputX) + Mathf.Abs(inputZ)) * 0.8f;//��J��
            }      
        }
  
        //����
        transform.position = transform.position + transform.forward * inputValue * (NumericalValue.playerMoveSpeed + addMoveSpeed) * Time.deltaTime;

        if (inputValue > 0) charactersCollision.OnCollision_Characters(out hit);//�I����_�}��
    }           

    /// <summary>
    /// ��J��
    /// </summary>
    void OnInput()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        inputX = Input.GetAxis("Horizontal");//��JX��
        inputZ = Input.GetAxis("Vertical");//��JZ��

        forwardVector = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * CameraControl.Instance.rotateSpeed, Vector3.up) * forwardVector;//�e��V�q
        horizontalCross = Vector3.Cross(Vector3.up, forwardVector);//�����b      

        //�}�Ҥ���
        if(GameSceneUI.Instance.isOptions && info.IsName("Run"))
        {
            animator.SetFloat("Run", 0);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion(photonView.ViewID, "Run", 0.0f);
        }

        //�ƹ�
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.visible = !Cursor.visible;//���� ���/����
            if (!Cursor.visible) Cursor.lockState = CursorLockMode.Locked;//��w����
            else Cursor.lockState = CursorLockMode.None;
        }
    }

    public float gizmosSpherCenter;
    public float  gizmosSpherRadius;
    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + box.center + transform.forward * gizmosSpherCenter, gizmosSpherRadius);        
        //Gizmos.DrawWireCube(transform.position + box.center + transform.forward * 5f, new Vector3(1, 1, 10));
        
    }
}
