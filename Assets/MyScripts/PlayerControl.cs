using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    bool isSendRun;//�O�_�w�o�e���ʰʵe       

    //���D
    bool isJump;//�O�_���D
    Vector3 jumpForward;//���D�e��V�q
    bool isRunJump;//���D�e�O�_�V�e

    //����
    bool isNormalAttack;//�O�_���q����        
    bool isSkillAttack;//�O�_�ޯ����    
    int normalAttackNumber;//���q�����s��
    public int GetNormalAttackNumber => normalAttackNumber;
    bool isJumpAttack;//�O�_���D����
    public bool isJumpAttackDown;//���D�����U��


    private void Awake()
    {       
        gameObject.layer = LayerMask.NameToLayer("Player");//�]�wLayer                
        gameObject.tag = "Player";//�]�wTag

        animator = GetComponent<Animator>();
        
        if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();
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

        //�]�w��v���[���I
        CameraControl.SetLookPoint = ExtensionMethods.FindAnyChild<Transform>(transform, "CameraLookPoint");

        //�p�a����v��
        GameObject miniMap_Camera = GameObject.Find("MiniMap_Camera");
        miniMap_Camera.transform.SetParent(transform);
        miniMap_Camera.transform.localPosition = new Vector3(0, 10, 0);               

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //����
        forwardVector = transform.forward;

        //����
         Cursor.visible = false;//��������
         Cursor.lockState = CursorLockMode.Locked;//��w����
    }

    void Update()
    {
        //���O���`���A & �S���}�ҿﶵ����
        if (!charactersCollision.isDie && !GameSceneUI.Instance.isOptions && !info.IsName("Pain"))
        {            
            OnJumpControl();
            OnAttackControl();

            if (!isJumpAttack && !isNormalAttack && !isSkillAttack && !info.IsName("Pain"))
            {
                if(!info.IsName("Dodge")) OnMovementControl();
                else
                {
                    if (inputValue > 0)
                    {
                        inputValue = 0;
                        animator.SetFloat("Run", inputValue);
                    }
                }
                
                OnDodgeControl();
            } 
            
        }

        OnJumpHehavior();
        OnInput();

        if (isJumpAttackDown) OnJumpAttackMove();
    }

    
    public void OnJumpAttackMove()
    {
        transform.position = transform.position + Vector3.down * 20 * Time.deltaTime;//��t�U��
    }

    /// <summary>
    /// ��������
    /// </summary>
    void OnAttackControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        //���q����
        if (Input.GetMouseButton(0) && !info.IsTag("SkillAttack") && !info.IsTag("SkillAttack-2") && !info.IsName("Dodge"))
        {
            //���D����
            if (isJump && !isJumpAttack)
            {
                isJumpAttack = true;

                animator.SetBool("JumpAttack", isJumpAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJumpAttack);
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

            //���ݴ��q���������A����ޯ�
            if (isSkillAttack && info.IsTag("NormalAttack") && info.normalizedTime >= 1)
            {
                //��V               
                if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//����
                else if (inputX != 0) transform.forward = horizontalCross * inputX;//���k
                else if (inputZ != 0) transform.forward = forwardVector * inputZ;//�e��
                
                animator.SetBool("SkillAttack", isSkillAttack);
                if(GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack", isSkillAttack);
                return;
            }                           

            //���q����(�Ĥ@������)
            if (!isSkillAttack && !isNormalAttack && !isJumpAttack)
            {
                isNormalAttack = true;
                normalAttackNumber = 1;                

                animator.SetBool("NormalAttack", isNormalAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
            }

            //�������q�����ۦ�
            if (info.IsTag("NormalAttack") && info.normalizedTime >= 1)
            {                                
                normalAttackNumber++;//���q�����s��                
                if (normalAttackNumber > 3) normalAttackNumber = 0;
                
                //��V               
                if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//����
                else if (inputX != 0) transform.forward = horizontalCross * inputX;//���k
                else if (inputZ != 0) transform.forward = forwardVector * inputZ;//�e��

                animator.SetInteger("NormalAttackNumber", normalAttackNumber);
                if (GameDataManagement.Instance.isConnect)  PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);
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
                    if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttackNumber", normalAttackNumber);

                    if (info.IsTag("NormalAttack"))
                    {
                        animator.SetBool("NormalAttack", isNormalAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
                    }

                    if (info.IsTag("SkillAttack"))
                    {
                        animator.SetBool("SkillAttack", isSkillAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack", isSkillAttack);
                    }

                    if (info.IsTag("SkillAttack-2"))
                    {
                        animator.SetBool("SkillAttack", isSkillAttack);
                        animator.SetBool("SkillAttack-2", false);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack", isSkillAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack-2", false);
                    }

                    
                }              
            }
        }

        if (info.IsTag("JumpAttack") && info.normalizedTime >= 1)
        {
            isJump = false;
            isNormalAttack = false;
            isJumpAttack = false;

            animator.SetBool("JumpAttack", isJumpAttack);
            animator.SetBool("Jump", isJump);
            animator.SetBool("NormalAttack", isJump);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJumpAttack);
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isJump);
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
            }
        }

        //�ޯ�������������q����
        if (isNormalAttack && info.normalizedTime > 0.35f && info.IsTag("SkillAttack") || info.IsTag("SkillAttack-2"))
        {
            isNormalAttack = false;
            isNormalAttack = false;

            animator.SetBool("NormalAttack", isNormalAttack);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
        }           
    }      
    
    /// <summary>
    /// �{������
    /// </summary>
    void OnDodgeControl()
    {        
        //�{������
        if (info.IsName("Idle") || info.IsName("Run"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.SetBool("Dodge", true);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Dodge", true);
            }
        }

        //�{������
        if (info.IsName("Dodge") && info.normalizedTime < 1)
        {
            //transform.position = transform.position + transform.forward * GameDataManagement.Instance.numericalValue.playerDodgeSeppd * Time.deltaTime;
        }

            //�{������
        if (info.IsName("Dodge") && info.normalizedTime > 1)
        {
            animator.SetBool("Dodge", false);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Dodge", false);       
        }
    }

    /// <summary>
    /// ���D����
    /// </summary>
    void OnJumpControl()
    {        
        if (Input.GetKeyDown(KeyCode.Space) && !isJump && !isNormalAttack && !isSkillAttack && !info.IsName("Dodge"))
        {
            //���V�W�@�I
            transform.position = transform.position + Vector3.up * NumericalValue.playerJumpForce * Time.deltaTime;

            jumpForward = transform.forward;//���D�e��V�q
            if (inputValue != 0) isRunJump = true;
            
            isJump = true;
            isNormalAttack = false;

            if (charactersCollision.floating_List.Count > 0) charactersCollision.floating_List.Clear();
            charactersCollision.floating_List.Add(new CharactersFloating { target = transform, force = NumericalValue.playerJumpForce, gravity = NumericalValue.gravity });//�B��List
            
            animator.SetBool("NormalAttack", isNormalAttack);
            animator.SetBool("Jump", isJump);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isJump);
            }
        }
    }

    /// <summary>
    /// ���D�欰
    /// </summary>
    void OnJumpHehavior()
    {      
        if (info.IsTag("Jump") && info.normalizedTime > 0.5f || info.IsTag("JumpAttack"))
        {
            float boxCollisionDistance = boxSize.x < boxSize.z ? boxSize.x / 2 : boxSize.z / 2;
            LayerMask mask = LayerMask.GetMask("StageObject");
            RaycastHit hit;
            if (Physics.BoxCast(transform.position + Vector3.up * boxSize.y, new Vector3(boxCollisionDistance - 0.06f, 0.01f, boxCollisionDistance - 0.06f), -transform.up, out hit, Quaternion.Euler(transform.localEulerAngles), boxSize.y + 0.3f, mask))
            {
                if (isJump || isJumpAttack)
                {
                    
                    isRunJump = false;                
                    charactersCollision.floating_List.Clear();

                    if (isJump)
                    {
                        isJump = false;
                        animator.SetBool("Jump", isJump);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isJump);
                    }
                    if (isJumpAttack) isJumpAttackDown = false;                   
                }
            }
        }   
    }       
     
    /// <summary>
    /// ���ʱ���
    /// </summary>
    void OnMovementControl()
    {        
        if (!info.IsName("JumpAttack"))
        {
            //��V
            float maxRadiansDelta = 0.15f;//��V����
            if (inputX != 0 && inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, (horizontalCross * inputX) + (forwardVector * inputZ), maxRadiansDelta, maxRadiansDelta);//����
            else if (inputX != 0) transform.forward = Vector3.RotateTowards(transform.forward, horizontalCross * inputX, maxRadiansDelta, maxRadiansDelta);//���k
            else if (inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, forwardVector * inputZ, maxRadiansDelta, maxRadiansDelta);//�e��      
        }               

        inputValue = Mathf.Abs(inputX) + Mathf.Abs(inputZ);//��J��
        if (inputValue > 1) inputValue = 1;

        if (isJump)
        {
            if (isRunJump) inputValue = Mathf.Abs(inputX) + Mathf.Abs(inputZ);//��J��            
            if (inputValue > 1) inputValue = 1;
            if (inputValue < 0) inputValue = 0;

            transform.position = transform.position + jumpForward * inputValue * NumericalValue.playerMoveSpeed * Time.deltaTime;
            return;
        }

        //����
        transform.position = transform.position + transform.forward * inputValue * NumericalValue.playerMoveSpeed * Time.deltaTime;

        animator.SetFloat("Run", inputValue);
        if (GameDataManagement.Instance.isConnect)
        {
            if (inputValue > 0.1f && !isSendRun)
            {
                isSendRun = true;
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Run", inputValue);
            }
            if (inputValue < 0.1f && isSendRun)
            {
                isSendRun = false;
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Run", 0.0f);
            }
        }
    }           

    /// <summary>
    /// ��J��
    /// </summary>
    void OnInput()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);

        inputX = Input.GetAxis("Horizontal");//��JX��
        inputZ = Input.GetAxis("Vertical");//��JZ��

        forwardVector = Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up) * forwardVector;//�e��V�q
        horizontalCross = Vector3.Cross(Vector3.up, forwardVector);//�����b      

        //�ƹ�
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.visible = !Cursor.visible;//���� ���/����
            if (!Cursor.visible) Cursor.lockState = CursorLockMode.Locked;//��w����
            else Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnDrawGizmos()
    {
        /*BoxCollider box = GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + box.center + transform.forward * 0, 1.3f);        
        Gizmos.DrawWireCube(transform.position + box.center + transform.forward * 2.5f, new Vector3(1, 1, 4));*/
        
    }
}
