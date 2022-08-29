using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ԥh�M��
/// </summary>
public class WarriorExclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;
    PlayerControl playerControl;

    //Buff
    float addDamage;//�W�[�ˮ`��      

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();

        //Buff
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            //�W�[�ˮ`��
            if (GameDataManagement.Instance.equipBuff[i] == 1 && GetComponent<PlayerControl>()) addDamage = GameDataManagement.Instance.numericalValue.buffAbleValue[1] / 100;                      
        }        
    }

    void Update()
    {
        OnAttackMove_Warrior();//��������
    }

    /// <summary>
    /// �ޯ����1_�Ԥh
    /// </summary>
    void OnSkillAttack1_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v
        float getDamage = (NumericalValue.warriorSkillAttack_1_Damge + (NumericalValue.warriorSkillAttack_1_Damge * addDamage)) * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.warriorSkillAttack_1_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorSkillAttack_1_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.warriorSkillAttack_1_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.warriorSkillAttack_1_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.warriorSkillAttack_1_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.warriorSkillAttack_1_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                  
    }

    /// <summary>
    /// �ޯ����2_�Ԥh
    /// </summary>
    void OnSkillAttack2_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v
        float getDamage = (NumericalValue.warriorSkillAttack_2_Damge + (NumericalValue.warriorSkillAttack_2_Damge * addDamage)) * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.warriorSkillAttack_2_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorSkillAttack_2_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.warriorSkillAttack_2_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.warriorSkillAttack_2_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.warriorSkillAttack_2_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.warriorSkillAttack_2_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                  
    }

    /// <summary>
    /// �ޯ����3_�Ԥh
    /// </summary>
    /// <param name="count">�ĴX�q����</param>
    void OnSkillAttack3_Warrior(int count)
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v
        float getDamage = (NumericalValue.warriorSkillAttack_3_Damge[count] + (NumericalValue.warriorSkillAttack_3_Damge[count] * addDamage)) * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;        
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.warriorSkillAttack_3_RepelDirection[count];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorSkillAttack_3_RepelDistance[count];//���h�Z��
        attack.animationName = NumericalValue.warriorSkillAttack_3_Effect[count];//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.warriorSkillAttack_3_ForwardDistance[count];//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.warriorSkillAttack_3_attackRadius[count];//�����b�|
        attack.isAttackBehind = NumericalValue.warriorSkillAttack_3_IsAttackBehind[count];//�O�_�����I��ĤH
        
        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                                 
    }

    /// <summary>
    /// ���D����_�Ԥh
    /// </summary>
    void OnJumpAttack_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v
        float getDamage = (NumericalValue.warriorJumpAttack_Damage + (NumericalValue.warriorJumpAttack_Damage * addDamage)) * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.warriorJumpAttack_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorJumpAttac_kRepelDistance;//���h�Z��
        attack.animationName = NumericalValue.warriorJumpAttack_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.warriorJumpAttack_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.warriorJumpAttack_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.warriorJumpAttack_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)

        playerControl.isJumpAttackMove = true;//���D�����U��
    }

    /// <summary>
    /// ���q����1_�Ԥh
    /// </summary>
    void OnNormalAttack1_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v
        float getDamage = (NumericalValue.warriorNormalAttack_1_Damge + (NumericalValue.warriorNormalAttack_1_Damge * addDamage)) * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.warriorNormalAttack_1_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorNormalAttack_1_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.warriorNormalAttack_1_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.warriorNormalAttack_1_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.warriorNormalAttack_1_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.warriorNormalAttack_1_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// ���q����2_�Ԥh
    /// </summary>
    void OnNormalAttack2_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v
        float getDamage = (NumericalValue.warriorNormalAttack_2_Damge + (NumericalValue.warriorNormalAttack_2_Damge * addDamage)) * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.warriorNormalAttack_2_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorNormalAttack_2_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.warriorNormalAttack_2_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.warriorNormalAttack_2_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.warriorNormalAttack_2_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.warriorNormalAttack_2_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// ���q����3_�Ԥh
    /// </summary>
    void OnNormalAttack3_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;
        
        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v
        float getDamage = (NumericalValue.warriorNormalAttack_3_Damge + (NumericalValue.warriorNormalAttack_3_Damge * addDamage)) * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.warriorNormalAttack_3_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorNormalAttack_3_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.warriorNormalAttack_3_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.warriorNormalAttack_3_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.warriorNormalAttack_3_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.warriorNormalAttack_3_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// ��������_�Ԥh
    /// </summary>
    void OnAttackMove_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        float move = 3.5f;

        if (GameDataManagement.Instance.selectRoleNumber == 0)
        {
            if (animationInfo.IsName("NormalAttack_1") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.6f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("NormalAttack_2") && animationInfo.normalizedTime > 0.4f && animationInfo.normalizedTime < 0.5f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("NormalAttack_3") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_1") && animationInfo.normalizedTime > 0.55f && animationInfo.normalizedTime < 0.65f) transform.position = transform.position + transform.forward * (-move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_2") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_2") && animationInfo.normalizedTime > 0.6f && animationInfo.normalizedTime < 0.7f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.2f && animationInfo.normalizedTime < 0.3f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.58f && animationInfo.normalizedTime < 0.68f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
        }
    }
}
