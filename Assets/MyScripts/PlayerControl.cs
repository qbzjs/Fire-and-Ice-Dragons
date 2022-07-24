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
    CharactersCollision charactersCollision;
    GameData_NumericalValue NumericalValue;

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;

    //����
    bool isLockMove;//�O�_�����
    float inputX;//��JX��
    float inputZ;//��JZ��
    Vector3 forwardVector;//�e��V�q
    Vector3 horizontalCross;//�����b

    //���D
    bool isJump;//�O�_���D

    //���q����
    bool isNormalAttack;//�O�_���q����
    int normalAttackNumber;//���q�����s��

    //���D����
    bool isJumpAttack;//�O�_���D����

    //�ޯ����
    bool isSkillAttack;//�O�_�ޯ����

    private void Awake()
    {       
        gameObject.layer = LayerMask.NameToLayer("Player");//�]�wLayer                

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

        //����
       /* Cursor.visible = false;//��������
        Cursor.lockState = CursorLockMode.Locked;//��w����*/

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //����
        forwardVector = transform.forward;               
    }
   
    void Update()
    {        
        OnInput();
        OnJumpControl();
        OnAttackControl();
        OnJumpBehavior();

        if (!isNormalAttack && !isSkillAttack)
        {
            OnMovementControl();            
        }  
    }

    /// <summary>
    /// �ޯ�����欰
    /// </summary>
    void OnSkillAttackBehavior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        AttackBehavior attack = AttackBehavior.Instance;
        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        //�P�_�ثe���q�����s��
        switch (normalAttackNumber)
        {            
            case 1://�ޯ�1
                GameObject obj = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("playerSkill_1_Numbering"), GameSceneManagement.Instance.loadPath.playerCharactersSkill_1);//���ͪ���
                obj.transform.position = transform.position + boxCenter;
                
                //�]�wAttackBehavior Class�ƭ�                
                attack.function = new Action(attack.OnSetShootFunction);//�]�w����禡
                attack.performObject = obj;//�������������(�ۨ�/�g�X����) 
                attack.speed = NumericalValue.playerSkillAttack_1_FlyingSpeed;//����t��
                attack.diration = transform.forward;//�����V
                attack.lifeTime = NumericalValue.playerSkillAttack_1_LifeTime;//�ͦs�ɶ�                                                                                             
                attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
                attack.damage = NumericalValue.playerSkillAttack_1_Damage * rate;//�y���ˮ`
                attack.animationName = NumericalValue.playerSkillAttack_1_Effect;//�����ĪG(�����̼��񪺰ʵe�W��)
                attack.direction = NumericalValue.playerSkillAttack_1_RepelDirection;//���h��V((0:���h 1:����))
                attack.repel = NumericalValue.playerSkillAttack_1_Repel;//���h�Z��
                attack.isCritical = isCritical;//�O�_�z��
                GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)             
                break;
            case 2://�ޯ�2               
                attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
                attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
                attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
                attack.damage = NumericalValue.playerSkillAttack_2_Damage * rate;//�y���ˮ` 
                attack.animationName = NumericalValue.playerSkillAttack_2_Effect;//�����ĪG(����ʵe�W��)
                attack.direction = NumericalValue.playerSkillAttack_2_RepelDirection;//���h��V(0:���h, 1:����)
                attack.repel = NumericalValue.playerSkillAttack_2_Repel;//���h�Z��
                attack.boxSize = NumericalValue.playerSkillAttack_2_BoxSize * transform.lossyScale.x;//�񨭧�����Size
                attack.isCritical = isCritical;//�O�_�z��
                GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)                   
                break;
            case 3://�ޯ�3                
                attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
                attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
                attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
                attack.damage = NumericalValue.playerSkillAttack_3_Damage * rate;//�y���ˮ` 
                attack.animationName = NumericalValue.playerSkillAttack_3_Effect;//�����ĪG(����ʵe�W��)
                attack.direction = NumericalValue.playerSkillAttack_3_RepelDirection;//���h��V(0:���h, 1:����)
                attack.repel = NumericalValue.playerSkillAttack_3_Repel;//���h�Z��
                attack.boxSize = NumericalValue.playerSkillAttack_3_BoxSize * transform.lossyScale.x;//�񨭧�����Size
                attack.isCritical = isCritical;//�O�_�z��
                GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)     
                break;
        }
    }
 
    /// <summary>
    /// ���D�����欰
    /// </summary>
    void OnJumpAttackBehavior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        //�]�wAttackBehavior Class�ƭ�
        AttackBehavior attack = AttackBehavior.Instance;
        attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.damage = NumericalValue.playerJumpAttackDamage * rate;//�y���ˮ` 
        attack.animationName = NumericalValue.playerJumpAttackEffect;//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.playerJumpAttackRepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.playerJumpAttackRepelDistance;//���h�Z��
        attack.boxSize = NumericalValue.playerJumpAttackBoxSize * transform.lossyScale.x;//�񨭧�����Size
        attack.isCritical = isCritical;//�O�_�z��
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// ���q�����欰
    /// </summary>
    void OnNormalAttackBehavior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        //��������
         transform.position = transform.position + transform.forward * NumericalValue.playerNormalAttackMoveDistance[normalAttackNumber - 1] * Time.deltaTime;

         bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
         float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

         //�]�wAttackBehavior Class�ƭ�
         AttackBehavior attack = AttackBehavior.Instance;
         attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
         attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
         attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
         attack.damage = NumericalValue.playerNormalAttackDamge[normalAttackNumber - 1] * rate;//�y���ˮ` 
         attack.animationName = NumericalValue.playerNormalAttackEffect[normalAttackNumber - 1];//�����ĪG(����ʵe�W��)
         attack.direction = NumericalValue.playerNormalAttackRepelDirection[normalAttackNumber - 1];//���h��V(0:���h, 1:����)
         attack.repel = NumericalValue.playerNormalAttackRepelDistance[normalAttackNumber - 1];//���h�Z��
         attack.boxSize = NumericalValue.playerNormalAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//�񨭧�����Size
         attack.isCritical = isCritical;//�O�_�z��
         GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)           
    }    

    /// <summary>
    /// ��������
    /// </summary>
    void OnAttackControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        //���q����
        if (Input.GetMouseButton(0) && !info.IsTag("SkillAttack"))
        {
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

            //���D����
            if (isJump)
            {
                isJumpAttack = true;

                animator.SetBool("JumpAttack", isJumpAttack);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJumpAttack);
                return;
            }

            //���q����(�Ĥ@������)
            if (!isSkillAttack && !isNormalAttack)
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
                if (info.IsTag("NormalAttack") || info.IsTag("SkillAttack") || info.IsTag("JumpAttack"))
                {                    
                    normalAttackNumber = 0;//���q�����s��                    
                    isNormalAttack = false;
                    isSkillAttack = false;
                    isJumpAttack = false;

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

                    if (info.IsTag("JumpAttack"))
                    {                        
                        animator.SetBool("JumpAttack", isJumpAttack);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJumpAttack);
                    }  
                }              
            }
        }     

        //�ޯ�������������q����
        if(isNormalAttack && info.IsTag("SkillAttack") && info.normalizedTime > 0.35f)
        {
            isNormalAttack = false;

            animator.SetBool("NormalAttack", isNormalAttack);
            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
        }           
    }      
    
    /// <summary>
    /// ���D�欰
    /// </summary>
    void OnJumpBehavior()
    {       
        if (isJump) StartCoroutine(OnWaitJump());       
    }

    /// <summary>
    /// ���ݸ��D(�קK�L�kĲ�o�ʵe)
    /// </summary>
    /// <returns></returns>
    IEnumerator OnWaitJump()
    {
        yield return new WaitForSeconds(0.1f);

        //�I������
        LayerMask mask = LayerMask.GetMask("StageObject");
        if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 4, boxSize.y / 2, boxSize.z / 4), Quaternion.identity, mask))
        {
            isLockMove = false;
            isJump = false;

            animator.SetBool("Jump", isJump);
            animator.SetBool("JumpAttack", isJump);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isJump);
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "JumpAttack", isJump);
            }
        }
    }

    /// <summary>
    /// ���D����
    /// </summary>
    void OnJumpControl()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            charactersCollision.floating_List.Add(new CharactersFloating { target = transform, force = NumericalValue.playerJumpForce, gravity = NumericalValue.gravity });//�B��List

            isJump = true;         
            isNormalAttack = false;
            animator.SetBool("NormalAttack", isNormalAttack);
            animator.SetBool("Jump", isJump);
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "NormalAttack", isNormalAttack);
                PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "Jump", isJump);
            }
        }        
    }
    bool isSendRun;
    /// <summary>
    /// ���ʱ���
    /// </summary>
    void OnMovementControl()
    {                
        //��V
        float maxRadiansDelta = 0.055f;
        if (inputX != 0 && inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, (horizontalCross * inputX) + (forwardVector * inputZ), maxRadiansDelta, maxRadiansDelta);//����
        else if (inputX != 0) transform.forward = Vector3.RotateTowards(transform.forward, horizontalCross * inputX, maxRadiansDelta, maxRadiansDelta);//���k
        else if (inputZ != 0) transform.forward = Vector3.RotateTowards(transform.forward, forwardVector * inputZ, maxRadiansDelta, maxRadiansDelta);//�e��      
        
        float inputValue = Mathf.Abs(inputX) + Mathf.Abs(inputZ);//��J��

        //���D�����i�A�W�[���O
        if (isJump && inputValue == 0) isLockMove = true;       
        if (isLockMove) inputValue = 0;

        //����
        if (inputValue > 1) inputValue = 1;
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
}
