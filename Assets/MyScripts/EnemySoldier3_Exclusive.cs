using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ĤH�h�L3�M��
/// </summary>
public class EnemySoldier3_Exclusive : MonoBehaviourPunCallbacks
{
    GameData_NumericalValue NumericalValue;

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;
    }

    /// <summary>
    /// ����1_�ĤH�h�L3
    /// </summary>
    void OnAttack1_EnemySoldier3()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v
        float getDamage = NumericalValue.enemySoldier3_Attack_1_Damge * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.enemySoldier3_Attack_1_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.enemySoldier3_Attack_1_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.enemySoldier3_Attack_1_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.enemySoldier3_Attack_1_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.enemySoldier3_Attack_1_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.enemySoldier3_Attack_1_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                  
    }

    /// <summary>
    /// ����2_�ĤH�h�L3
    /// </summary>
    void OnAttack2_EnemySoldier3()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v
        float getDamage = NumericalValue.enemySoldier3_Attack_2_Damge * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.enemySoldier3_Attack_2_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.enemySoldier3_Attack_2_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.enemySoldier3_Attack_2_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.enemySoldier3_Attack_2_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.enemySoldier3_Attack_2_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.enemySoldier3_Attack_2_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                  
    }

    /// <summary>
    /// ����3_�ĤH�h�L1
    /// </summary>
    void OnAttack3_EnemySoldier3()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v
        float getDamage = NumericalValue.enemySoldier3_Attack_3_Damge * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.enemySoldier3_Attack_3_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.enemySoldier3_Attack_3_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.enemySoldier3_Attack_3_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.enemySoldier3_Attack_3_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.enemySoldier3_Attack_3_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.enemySoldier3_Attack_3_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                  
    }
}
