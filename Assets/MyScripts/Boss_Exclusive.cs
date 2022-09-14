using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Exclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue; 
    }

    /// <summary>
    /// ����1_Boss(�Q���y)
    /// </summary>
    void OnAttack1_Boss()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("bossAttack1"), GameSceneManagement.Instance.loadPath.bossAttack1);//�������������(�ۨ�/�g�X����)
        attack.layer = "Enemy";//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetShootFunction_Group);//�]�w����禡       
        attack.damage = NumericalValue.bossAttack1_Damge * rate;//�y���ˮ` 
        attack.direction = NumericalValue.bossAttack1_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.bossAttack1_RepelDistance;//���h/�����Z��
        attack.animationName = NumericalValue.bossAttack1_Effect;//�����ĪG(����ʵe�W��)        

        attack.flightSpeed = NumericalValue.bossAttack1_FloatSpeed;//����t��
        attack.lifeTime = NumericalValue.bossAttack1_LifeTime;//�ͦs�ɶ�
        attack.flightDiration = transform.forward;//�����V        

        attack.performObject.transform.SetParent(gameObject.transform);        
        attack.performObject.transform.localPosition = new Vector3(0, 0, 4);

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)       
    }

    /// <summary>
    /// ����2_Boss(�Y����)
    /// </summary>
    void OnAttack2_Boss()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("bossAttack1"), GameSceneManagement.Instance.loadPath.bossAttack2);//�������������(�ۨ�/�g�X����)
        attack.layer = "Enemy";//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetShootFunction_Group);//�]�w����禡       
        attack.damage = NumericalValue.bossAttack2_Damge * rate;//�y���ˮ` 
        attack.direction = NumericalValue.bossAttack2_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.bossAttack2_RepelDistance;//���h/�����Z��
        attack.animationName = NumericalValue.bossAttack2_Effect;//�����ĪG(����ʵe�W��)        

        attack.flightSpeed = NumericalValue.bossAttack2_FloatSpeed;//����t��
        attack.lifeTime = NumericalValue.bossAttack2_LifeTime;//�ͦs�ɶ�
        attack.flightDiration = transform.forward;//�����V        

        attack.performObject.transform.SetParent(gameObject.transform);
        attack.performObject.transform.localPosition = new Vector3(0, 0, 4);

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)   
    }
}
