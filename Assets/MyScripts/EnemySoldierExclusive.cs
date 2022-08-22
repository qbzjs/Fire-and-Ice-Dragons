using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ĤH�h�L�M��
/// </summary>
public class EnemySoldierExclusive : MonoBehaviourPunCallbacks
{
    GameData_NumericalValue NumericalValue;

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���q����1_�ĤH�h�L
    /// </summary>
    void OnNormalAttack1_EnemySoldier()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v
        float getDamage = (NumericalValue.enemySoldierNormalAttack_1_Damge + (NumericalValue.enemySoldierNormalAttack_1_Damge)) * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.enemySoldierNormalAttack_1_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.enemySoldierNormalAttack_1_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.enemySoldierNormalAttack_1_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.enemySoldierNormalAttack_1_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.enemySoldierNormalAttack_1_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.enemySoldierNormalAttack_1_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)                  
    }
}
