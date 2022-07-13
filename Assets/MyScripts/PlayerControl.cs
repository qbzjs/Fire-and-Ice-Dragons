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

    //�@�q
    float gravity;//���O

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;

    //����
    bool isLockMove;//�O�_�����
    float inputX;//��JX��
    float inputZ;//��JZ��
    float moveSpeed;//���ʳt��
    Vector3 forwardVector;//�e��V�q
    Vector3 horizontalCross;//�����b

    //���D
    bool isJump;//�O�_���D
    float jumpForce;//���D�O        

    //���q����
    bool isNormalAttack;//�O�_���q����
    bool isTrick;//�O�_�ϥε���
    int normalAttackNumber;//���q�����s��
    float[] playerNormalAttackDamge;//���a���q�����ˮ`
    float[] playerNormalAttackMoveDistance;//���a���q�������ʶZ��
    float[] playerNormalAttackRepelDistance;//���a���q���� ���h/�����Z��
    float[] playerNormalAttackRepelDirection;//���a���q������V(0:���h 1:����)
    string[] playerNormalAttackEffect;//���a���q�����ĪG(�����̼��񪺰ʵe�W��)
    Vector3[] playerNormalAttackBoxSize;//���a���q����������Size

    //���D����
    bool isJumpAttack;//�O�_���D����
    float playerJumpAttackDamage;//���a���D�����ˮ`
    string playerJumpAttackEffect;//���a���D�����ĪG(�����̼��񪺰ʵe�W��)
    float playerJumpAttackRepelDistance;//���a���D���� ���h�Z��
    Vector3 playerJumpAttackBoxSize;//���a���D����������Size

    //�ޯ����
    bool isSkillAttack;//�O�_�ޯ����
    //�ޯ����_1
    float playerSkillAttack_1_Damage;//�ޯ����_1_�����ˮ`
    string playerSkillAttack_1_Effect;//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
    float playerSkillAttack_1_FlyingSpeed;//�ޯ����_1_���󭸦�t��
    float playerSkillAttack_1_LifeTime;//�ޯ����_1_�ͦs�ɶ�
    float playerSkillAttack_1_Repel;//�ޯ����_1_���h�Z��

    //���������
    int playerSkill_1_Number;//���a�ޯ�1_����s��

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
        //����
        Cursor.visible = false;//��������
        Cursor.lockState = CursorLockMode.Locked;//��w����

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //�@�q
        gravity = GameData.Instance.OnGetFloatValue("gravity");//���O

        //����
        forwardVector = transform.forward;
        moveSpeed = GameData.Instance.OnGetFloatValue("playerMoveSpeed");//���ʳt��
        jumpForce = GameData.Instance.OnGetFloatValue("playerJumpForce");//���D�O

        //���q����
        playerNormalAttackDamge = GameData.Instance.OnGetFloatArrayValue("playerNormalAttackDamge");//���a���q�����ˮ`
        playerNormalAttackMoveDistance = GameData.Instance.OnGetFloatArrayValue("playerNormalAttackMoveDistance");//���a���q�������ʶZ��
        playerNormalAttackRepelDistance = GameData.Instance.OnGetFloatArrayValue("playerNormalAttackRepelDistance");//���a���q���� ���h/�����Z��
        playerNormalAttackRepelDirection = GameData.Instance.OnGetFloatArrayValue("playerNormalAttackRepelDirection");//���a���q���� ���h/�����Z��
        playerNormalAttackEffect = GameData.Instance.OnGetStringArrayValue("playerNormalAttackEffect");//���a���q�����ĪG(�����̼��񪺰ʵe�W��)
        playerNormalAttackBoxSize = GameData.Instance.OnGetVectorArrayValue("playerNormalAttackBoxSize");//���a���q����������Size

        //���D����
        playerJumpAttackDamage = GameData.Instance.OnGetFloatValue("playerJumpAttackDamage");//���a���D�����ˮ`
        playerJumpAttackEffect = GameData.Instance.OnGetStringValue("playerJumpAttackEffect");//���a���D�����ĪG(�����̼��񪺰ʵe�W��)
        playerJumpAttackRepelDistance = GameData.Instance.OnGetFloatValue("playerJumpAttackRepelDistance");//���a���D���� ���h�Z��
        playerJumpAttackBoxSize = GameData.Instance.OnGetVectorValue("playerJumpAttackBoxSize");//���a���D����������Size

        //�ޯ����_1
        playerSkillAttack_1_Damage = GameData.Instance.OnGetFloatValue("playerSkillAttack_1_Damage");//�ޯ����_1_�����ˮ`
        playerSkillAttack_1_Effect = GameData.Instance.OnGetStringValue("playerSkillAttack_1_Effect");//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
        playerSkillAttack_1_FlyingSpeed = GameData.Instance.OnGetFloatValue("playerSkillAttack_1_FlyingSpeed");//�ޯ����_1_���󭸦�t��
        playerSkillAttack_1_LifeTime = GameData.Instance.OnGetFloatValue("playerSkillAttack_1_LifeTime");//�ޯ����_1_�ͦs�ɶ�
        playerSkillAttack_1_Repel = GameData.Instance.OnGetFloatValue("playerSkillAttack_1_Repel");//�ޯ����_1_���h�Z��

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
        //�P�_�ثe���q�����s��
        switch(normalAttackNumber)
        {
            case 1://�ޯ�1
                GameObject obj = GameManagement.Instance.OnRequestOpenObject(playerSkill_1_Number);
                obj.transform.position = transform.position + boxCenter;
                GameManagement.Instance.flyingAttackObject_List.Add(new FlyingAttackObject
                {
                    flyingObject = obj,//���檫�� 
                    speed = playerSkillAttack_1_FlyingSpeed,//����t��
                    diration = transform.forward,//�����V
                    lifeTime = playerSkillAttack_1_LifeTime,//�ͦs�ɶ�                                                                                             
                    layer = gameObject.layer,//������layer
                    damage = playerSkillAttack_1_Damage,//�y���ˮ`
                    animationName = playerSkillAttack_1_Effect,//�����ĪG(�����̼��񪺰ʵe�W��)
                    repel = playerSkillAttack_1_Repel//���h�Z��
                });
                break;
        }
    }
 
    /// <summary>
    /// ���D�����欰
    /// </summary>
    void OnJumpAttackBehavior()
    {
        //������        
        Collider[] hits = Physics.OverlapBox(transform.position + boxCenter + transform.forward, playerJumpAttackBoxSize, Quaternion.identity, attackMask);
        foreach (var hit in hits)
        {
            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                collision.OnGetHit(attacker: gameObject,//�����̪���
                                   layer: gameObject.layer,//������layer
                                   damage: playerJumpAttackDamage,//�y���ˮ`
                                   animationName: playerJumpAttackEffect,//����ʵe�W��
                                   effect: 0,//�����ĪG(0:���h, 1:����)
                                   repel: playerJumpAttackRepelDistance);//���h�Z��
            }
        }
    }

    /// <summary>
    /// ���q�����欰
    /// </summary>
    void OnNormalAttackBehavior()
    {
        //��������
        transform.position = transform.position + transform.forward * playerNormalAttackMoveDistance[normalAttackNumber - 1] * Time.deltaTime;

        //������
        Collider[] hits = Physics.OverlapBox(transform.position + boxCenter + transform.forward, playerNormalAttackBoxSize[normalAttackNumber - 1], Quaternion.identity, attackMask);
        foreach(var hit in hits)
        {         
            CharactersCollision collision = hit.GetComponent<CharactersCollision>();
            if (collision != null)
            {
                collision.OnGetHit(attacker: gameObject,//�����̪���
                                   layer: gameObject.layer,//������layer
                                   damage: playerNormalAttackDamge[normalAttackNumber - 1],//�y���ˮ` 
                                   animationName: playerNormalAttackEffect[normalAttackNumber - 1],//����ʵe�W��
                                   effect: (int)playerNormalAttackRepelDirection[normalAttackNumber - 1],//�����ĪG(0:���h, 1:����)
                                   repel: playerNormalAttackRepelDistance[normalAttackNumber - 1]);//���h�Z��
            }
        }
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
            charactersCollision.floating_List.Add(new CharactersFloating { target = transform, force = jumpForce, gravity = gravity });//�B��List

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
        transform.position = transform.position + transform.forward * inputValue * moveSpeed * Time.deltaTime;

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

   /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + boxCenter, playerJumpAttackBoxSize);
    }*/
}
