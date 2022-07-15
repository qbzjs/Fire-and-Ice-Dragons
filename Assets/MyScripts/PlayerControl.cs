using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a����
/// </summary>
public class PlayerControl : MonoBehaviour
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
    bool isTrick;//�O�_�ϥε���
    int normalAttackNumber;//���q�����s��

    //���D����
    bool isJumpAttack;//�O�_���D����

    //�ޯ����
    bool isSkillAttack;//�O�_�ޯ����

    //���������
    [SerializeField]int playerSkill_1_Number;//���a�ޯ�1_����s��

    //��L
    LayerMask attackMask;//������H

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");//�]�wLayer                

        animator = GetComponent<Animator>();
        if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();
        charactersCollision = GetComponent<CharactersCollision>();        
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Insrance.numericalValue;

        //�]�w��v���[���I
        CameraControl.SetLookPoint = ExtensionMethods.FindAnyChild<Transform>(transform, "CameraLookPoint");

        //����
        Cursor.visible = false;//��������
        Cursor.lockState = CursorLockMode.Locked;//��w����

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //����
        forwardVector = transform.forward;               

        //���������
        playerSkill_1_Number = GameManagement.Instance.OnGetObjectNumber("playerSkill_1_Number");//���a�ޯ�1_����s��

        //��L
        attackMask = LayerMask.GetMask("Enemy");//������H
    }
   
    void Update()
    {        
        OnInput();
        OnJumpControl();
        OnAttackControl();
        OnJumpBehavior();

        if (!isNormalAttack && !isSkillAttack && !isTrick)
        {
            OnMovementControl();            
        }
    }

    /// <summary>
    /// �ޯ�����欰
    /// </summary>
    void OnSkillAttackBehavior()
    {
        AttackBehavior attack = AttackBehavior.Instance;
       
        //�P�_�ثe���q�����s��
        switch (normalAttackNumber)
        {            
            case 1://�ޯ�1
                GameObject obj = GameManagement.Instance.OnRequestOpenObject(playerSkill_1_Number);//�}��/���ͪ���
                obj.transform.position = transform.position + boxCenter;
                //�]�wAttackBehavior Class�ƭ�
                
                attack.function = new Action(attack.OnSetShootFunction);//�]�w����禡
                attack.performObject = obj;//�������������(�ۨ�/�g�X����) 
                attack.speed = NumericalValue.playerSkillAttack_1_FlyingSpeed;//����t��
                attack.diration = transform.forward;//�����V
                attack.lifeTime = NumericalValue.playerSkillAttack_1_LifeTime;//�ͦs�ɶ�                                                                                             
                attack.layer = gameObject.layer;//������layer
                attack.damage = NumericalValue.playerSkillAttack_1_Damage;//�y���ˮ`
                attack.animationName = NumericalValue.playerSkillAttack_1_Effect;//�����ĪG(�����̼��񪺰ʵe�W��)
                attack.direction = NumericalValue.playerSkillAttack_1_RepelDirection;//���h��V((0:���h 1:����))
                attack.repel = NumericalValue.playerSkillAttack_1_Repel;//���h�Z��
                GameManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)             
                break;
            case 2://�ޯ�2               
                attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
                attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
                attack.layer = gameObject.layer;//������layer
                attack.damage = NumericalValue.playerSkillAttack_2_Damage;//�y���ˮ` 
                attack.animationName = NumericalValue.playerSkillAttack_2_Effect;//�����ĪG(����ʵe�W��)
                attack.direction = NumericalValue.playerSkillAttack_2_RepelDirection;//���h��V(0:���h, 1:����)
                attack.repel = NumericalValue.playerSkillAttack_2_Repel;//���h�Z��
                attack.boxSize = NumericalValue.playerSkillAttack_2_BoxSize * transform.lossyScale.x;//�񨭧�����Size
                GameManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)                   
                break;
            case 3://�ޯ�3                
                attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
                attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
                attack.layer = gameObject.layer;//������layer
                attack.damage = NumericalValue.playerSkillAttack_3_Damage;//�y���ˮ` 
                attack.animationName = NumericalValue.playerSkillAttack_3_Effect;//�����ĪG(����ʵe�W��)
                attack.direction = NumericalValue.playerSkillAttack_3_RepelDirection;//���h��V(0:���h, 1:����)
                attack.repel = NumericalValue.playerSkillAttack_3_Repel;//���h�Z��
                attack.boxSize = NumericalValue.playerSkillAttack_3_BoxSize * transform.lossyScale.x;//�񨭧�����Size
                GameManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)     
                break;
        }
    }
 
    /// <summary>
    /// ���D�����欰
    /// </summary>
    void OnJumpAttackBehavior()
    {
        //�]�wAttackBehavior Class�ƭ�
        AttackBehavior attack = AttackBehavior.Instance;
        attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = gameObject.layer;//������layer
        attack.damage = NumericalValue.playerJumpAttackDamage;//�y���ˮ` 
        attack.animationName = NumericalValue.playerJumpAttackEffect;//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.playerJumpAttackRepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.playerJumpAttackRepelDistance;//���h�Z��
        attack.boxSize = NumericalValue.playerJumpAttackBoxSize * transform.lossyScale.x;//�񨭧�����Size
        GameManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// ���q�����欰
    /// </summary>
    void OnNormalAttackBehavior()
    {
        //��������
        transform.position = transform.position + transform.forward * NumericalValue.playerNormalAttackMoveDistance[normalAttackNumber - 1] * Time.deltaTime;

        //�]�wAttackBehavior Class�ƭ�
        AttackBehavior attack = AttackBehavior.Instance;
        attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = gameObject.layer;//������layer
        attack.damage = NumericalValue.playerNormalAttackDamge[normalAttackNumber - 1];//�y���ˮ` 
        attack.animationName = NumericalValue.playerNormalAttackEffect[normalAttackNumber - 1];//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.playerNormalAttackRepelDirection[normalAttackNumber - 1];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.playerNormalAttackRepelDistance[normalAttackNumber - 1];//���h�Z��
        attack.boxSize = NumericalValue.playerNormalAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//�񨭧�����Size
        GameManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)       
    }    

    /// <summary>
    /// ��������
    /// </summary>
    void OnAttackControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        //���q����
        if (Input.GetMouseButton(0) && !info.IsTag("SkillAttack") && !isTrick)
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
                return;
            }            

            //���D����
            if (isJump)
            {
                isJumpAttack = true;
                animator.SetBool("JumpAttack", isJumpAttack);
                return;
            }

            //���q����(�Ĥ@������)
            if (!isSkillAttack && !isNormalAttack)
            {
                isNormalAttack = true;
                normalAttackNumber = 1;
                animator.SetBool("NormalAttack", isNormalAttack);              
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
                    animator.SetInteger("NormalAttackNumber", normalAttackNumber);

                    isNormalAttack = false;
                    animator.SetBool("NormalAttack", isNormalAttack);

                    isSkillAttack = false;
                    animator.SetBool("SkillAttack", isSkillAttack);

                    isJumpAttack = false;
                    animator.SetBool("JumpAttack", isJumpAttack);
                }              
            }
        }     

        //�ޯ�������������q����
        if(info.IsTag("SkillAttack") && isNormalAttack)
        {
            isNormalAttack = false;
            animator.SetBool("NormalAttack", isNormalAttack);
        }
        
        //����
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (!isJump)
            {
                isNormalAttack = false;
                isTrick = true;
                animator.SetBool("Trick", isTrick);
                animator.SetBool("NormalAttack", isNormalAttack);
            }
        }

        if(isTrick)//��V
        {
            //��V               
            if (inputX != 0 && inputZ != 0) transform.forward = (horizontalCross * inputX) + (forwardVector * inputZ);//����
            else if (inputX != 0) transform.forward = horizontalCross * inputX;//���k
            else if (inputZ != 0) transform.forward = forwardVector * inputZ;//�e��
        }

        //���۵���
        if(info.IsTag("Trick") && info.normalizedTime > 1)
        {
            isTrick = false;
            animator.SetBool("Trick", isTrick);
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
        }        
    }

    /// <summary>
    /// ���ʱ���
    /// </summary>
    void OnMovementControl()
    {                
        //��V
        float maxRadiansDelta = 0.025f;
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
