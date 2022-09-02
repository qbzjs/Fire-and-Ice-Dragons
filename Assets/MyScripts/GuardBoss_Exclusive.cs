using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �u��Boss�M��
/// </summary>
public class GuardBoss_Exclusive : MonoBehaviourPunCallbacks
{
    GameData_NumericalValue NumericalValue;

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;
    }

    /// <summary>
    /// ����1_�u��Boss
    /// </summary>
    void OnAttack1_GuardBoss()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("guardBossAttack_1"), GameSceneManagement.Instance.loadPath.guardBossAttack_1);//�������������(�ۨ�/�g�X����)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetShootFunction_Group);//�]�w����禡       
        attack.damage = NumericalValue.guardBoss_Attack_1_Damge * rate;//�y���ˮ` 
        attack.direction = NumericalValue.guardBoss_Attack_1_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.guardBoss_Attack_1_RepelDistance;//���h/�����Z��
        attack.animationName = NumericalValue.guardBoss_Attack_1_Effect;//�����ĪG(����ʵe�W��)        

        attack.flightSpeed = NumericalValue.guardBoss_Attack_1_FlightSpeed;//����t��
        attack.lifeTime = NumericalValue.guardBoss_Attack_1_LifeTime;//�ͦs�ɶ�
        attack.flightDiration = transform.forward;//�����V        
        attack.performObject.transform.position = transform.position;//�g�X��m

        attack.performObject.transform.parent = transform;//�[�J�ܤl����

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// ����2_�u��Boss
    /// </summary>
    void OnAttack2_GuardBoss()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v
        float getDamage = NumericalValue.guardBoss_Attack_2_Damge * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.guardBoss_Attack_2_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.guardBoss_Attack_2_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.guardBoss_Attack_2_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.guardBoss_Attack_2_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.guardBoss_Attack_2_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.guardBoss_Attack_2_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                  
    }

    /// <summary>
    /// ����3_�u��Boss
    /// </summary>
    void OnAttack3_GuardBoss()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v
        float getDamage = NumericalValue.guardBoss_Attack_3_Damge * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.guardBoss_Attack_3_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.guardBoss_Attack_3_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.guardBoss_Attack_3_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.guardBoss_Attack_3_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.guardBoss_Attack_3_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.guardBoss_Attack_3_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                  
    }

    /// <summary>
    /// ����4_�u��Boss
    /// </summary>
    void OnAttack4_GuardBoss()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v
        float getDamage = NumericalValue.guardBoss_Attack_4_Damge * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.guardBoss_Attack_4_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.guardBoss_Attack_4_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.guardBoss_Attack_4_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.guardBoss_Attack_4_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.guardBoss_Attack_4_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.guardBoss_Attack_4_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                  
    }
}
