using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianExclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;
    PlayerControl playerControl;

    //�I����
    Vector3 boxCenter;
    Vector3 boxSize;

    //Buff
    [SerializeField]float addDamage;//�W�[�ˮ`��

    Transform body;//���骫��

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();

        //�I����
        boxCenter = GetComponent<BoxCollider>().center;
        boxSize = GetComponent<BoxCollider>().size;

        //Buff
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            if (GameDataManagement.Instance.equipBuff[i] == 1)
            {
                addDamage = GameDataManagement.Instance.numericalValue.buffAbleValue[1] / 100;//�W�[�ˮ`��
            }
        }

        body = ExtensionMethods.FindAnyChild<Transform>(transform, "Mesh");
    }

    void Update()
    {
        OnSkillAttack2_Magician();

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        //�קK��������bug
        if (!info.IsTag("SkillAttack") && !body.gameObject.activeSelf) GetComponent<CharactersCollision>().OnBodySetActive(active: 1);//(1:��� 0:�����)
    }    

    /// <summary>
    /// �ޯ����1_�k�v
    /// </summary>
    void OnSkillAttack1_Magician()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHealFunction);//�]�w����禡
        attack.damage = NumericalValue.magicianSkillAttack_1_HealValue * rate + addDamage;//�v���q(%)
        attack.forwardDistance = NumericalValue.magicianSkillAttack_1_ForwardDistance;//�v���d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.magicianSkillAttack_1_attackRange;//�v���b�|
        attack.isAttackBehind = NumericalValue.magicianSkillAttack_1_IsAttackBehind;//�k�v���q����1_�O�_�v���I�����

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// �ޯ����2_�k�v
    /// </summary>
    void OnSkillAttack2_Magician()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        LayerMask mask = LayerMask.GetMask("Enemy");

        if (info.IsName("SkillAttack_2") && info.normalizedTime < 1)
        {
            //����
            transform.position = transform.position + transform.forward * 20 * Time.deltaTime;

            //�I���ĤH
            if (Physics.CheckBox(transform.position + boxCenter, new Vector3(boxSize.x / 1.3f, boxSize.y, boxSize.z / 1.3f), transform.rotation, mask))
            {
                //Ĳ�o�ޯध2
                animator.SetBool("SkillAttack-2", true);
                if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendAniamtion_Boolean(photonView.ViewID, "SkillAttack-2", true);

                GetComponent<CharactersCollision>().OnBodySetActive(active: 1);//(1:��� 0:�����)
            }
        }
    }

    /// <summary>
    /// �ޯ����2�ĤG�q_�k�v
    /// </summary>
    /// <param name="number">�����q��</param>
    void OnOnSkillAttack2Second_Magician(int number)
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = (NumericalValue.magicianSkillAttack_2_Damge[number] + (NumericalValue.magicianSkillAttack_2_Damge[number] * addDamage)) * rate;//�y���ˮ` 
        attack.direction = NumericalValue.magicianSkillAttack_2_RepelDirection[number];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.magicianSkillAttack_2_RepelDistance[number];//���h�Z��
        attack.animationName = NumericalValue.magicianSkillAttack_2_Effect[number];//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.magicianSkillAttack_2_ForwardDistance[number];//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.magicianSkillAttack_2_attackRadius[number];//�����b�|
        attack.isAttackBehind = NumericalValue.magicianSkillAttack_2_IsAttackBehind[number];//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// �ޯ����3_�k�v
    /// </summary>
    void OnOnSkillAttack3_Magician()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = (NumericalValue.magicianSkillAttack_3_Damge + (NumericalValue.magicianSkillAttack_3_Damge * addDamage)) * rate;//�y���ˮ` 
        attack.direction = NumericalValue.magicianSkillAttack_3_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.magicianSkillAttack_3_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.magicianSkillAttack_32_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.magicianSkillAttack_3_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.magicianSkillAttack_3_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.magicianSkillAttack_3_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// ���D����_�k�v
    /// </summary>
    void OnJumpAttack_Magician()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = (NumericalValue.magicianJumpAttack_Damage + (NumericalValue.magicianJumpAttack_Damage * addDamage)) * rate;//�y���ˮ` 
        attack.direction = NumericalValue.magicianJumpAttack_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.magicianJumpAttack_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.magicianJumpAttack_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.magicianJumpAttack_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.magicianJumpAttack_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.magicianJumpAttack_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)

        playerControl.isJumpAttackDown = true;//���D�����U��
    }

    /// <summary>
    /// ���q����1_�k�v
    /// </summary>
    void OnNormalAttack1_Magician()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;
  
        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("magicianNormalAttack_1"), GameSceneManagement.Instance.loadPath.magicianNormalAttack_1);//�������������(�ۨ�/�g�X����)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetShootFunction_Group);//�]�w����禡       
        attack.damage = (NumericalValue.magicianNormalAttack_1_Damage + (NumericalValue.magicianNormalAttack_1_Damage * addDamage)) * rate;//�y���ˮ` 
        attack.direction = NumericalValue.magicianNormalAttack_1_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.magicianNormalAttack_1_Repel;//���h/�����Z��
        attack.animationName = NumericalValue.magicianNormalAttack_1_Effect;//�����ĪG(����ʵe�W��)        

        attack.flightSpeed = NumericalValue.magicianNormalAttack_1_FlightSpeed;//����t��
        attack.lifeTime = NumericalValue.magicianNormalAttack_1_LifeTime;//�ͦs�ɶ�
        attack.flightDiration = transform.forward;//�����V        
        attack.performObject.transform.position = transform.position + GetComponent<BoxCollider>().center + transform.forward * 1;//�g�X��m

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)                 
    }

    /// <summary>
    /// ���q����2_�k�v
    /// </summary>
    void OnNormalAttack2_Magician()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitBoxFunction);//�]�w����禡
        attack.damage = (NumericalValue.magicianNormalAttack_2_Damge + (NumericalValue.magicianNormalAttack_2_Damge * addDamage)) * rate;//�y���ˮ` 
        attack.direction = NumericalValue.magicianNormalAttack_2_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.magicianNormalAttack_2_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.magicianNormalAttack_2_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.magicianNormalAttack_2_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRange = NumericalValue.magicianNormalAttack_2_attackRange;//�����d��
        attack.isAttackBehind = NumericalValue.magicianNormalAttack_2_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// ���q����3_�k�v
    /// </summary>
    void OnNormalAttack3_Magician()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = (NumericalValue.magicianNormalAttack_3_Damge + (NumericalValue.magicianNormalAttack_3_Damge  * addDamage))* rate;//�y���ˮ` 
        attack.direction = NumericalValue.magicianNormalAttack_3_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.magicianNormalAttack_3_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.magicianNormalAttack_3_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.magicianNormalAttack_3_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.magicianNormalAttack_3_attackRadius;//�����d��
        attack.isAttackBehind = NumericalValue.magicianNormalAttack_3_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)           
    }
}
