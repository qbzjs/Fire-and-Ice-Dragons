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

    int normalAttackNumber;//���q�����s��

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();
    }

    void Update()
    {
        normalAttackNumber = playerControl.GetNormalAttackNumber;//���q�����s��
        OnAttackMove_Warrior();
    }

    /// <summary>
    /// �ޯ�����欰_�Ԥh
    /// </summary>
    void OnSkillAttackBehavior_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        AttackMode attack = AttackMode.Instance;
        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.damage = NumericalValue.warriorSkillAttackDamage[normalAttackNumber - 1] * rate;//�y���ˮ` 
        attack.animationName = NumericalValue.warriorSkillAttackEffect[normalAttackNumber - 1];//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.warriorSkillAttackRepelDirection[normalAttackNumber - 1];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorSkillAttackRepel[normalAttackNumber - 1];//���h�Z��
        attack.boxSize = NumericalValue.warriorSkillAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//�񨭧�����Size
        attack.isCritical = isCritical;//�O�_�z��
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)              
    }

    /// <summary>
    /// ���D�����欰_�Ԥh
    /// </summary>
    void OnJumpAttackBehavior_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        //�]�wAttackBehavior Class�ƭ�
        AttackMode attack = AttackMode.Instance;
        attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.damage = NumericalValue.warriorJumpAttackDamage * rate;//�y���ˮ` 
        attack.animationName = NumericalValue.warriorJumpAttackEffect;//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.warriorJumpAttackRepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorJumpAttackRepelDistance;//���h�Z��
        attack.boxSize = NumericalValue.warriorJumpAttackBoxSize * transform.lossyScale.x;//�񨭧�����Size
        attack.isCritical = isCritical;//�O�_�z��
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// ���q�����欰_�Ԥh
    /// </summary>
    void OnNormalAttackBehavior_Warrior()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        //�]�wAttackBehavior Class�ƭ�
        AttackMode attack = AttackMode.Instance;
        attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.damage = NumericalValue.warriorNormalAttackDamge[normalAttackNumber - 1] * rate;//�y���ˮ` 
        attack.animationName = NumericalValue.warriorNormalAttackEffect[normalAttackNumber - 1];//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.warriorNormalAttackRepelDirection[normalAttackNumber - 1];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.warriorNormalAttackRepelDistance[normalAttackNumber - 1];//���h�Z��
        attack.boxSize = NumericalValue.warriorNormalAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//�񨭧�����Size
        attack.isCritical = isCritical;//�O�_�z��
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)           
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
            if (animationInfo.IsName("SkillAttack_1") && animationInfo.normalizedTime > 0.55f && animationInfo.normalizedTime < 0.65f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_2") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_2") && animationInfo.normalizedTime > 0.6f && animationInfo.normalizedTime < 0.7f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.2f && animationInfo.normalizedTime < 0.3f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.35f && animationInfo.normalizedTime < 0.45f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
            if (animationInfo.IsName("SkillAttack_3") && animationInfo.normalizedTime > 0.58f && animationInfo.normalizedTime < 0.68f) transform.position = transform.position + transform.forward * (move - Time.deltaTime) * Time.deltaTime;
        }
    }
}
