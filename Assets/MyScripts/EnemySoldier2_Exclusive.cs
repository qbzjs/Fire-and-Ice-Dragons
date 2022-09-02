using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ĤH�h�L2�M��
/// </summary>
public class EnemySoldier2_Exclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;

    MeshRenderer arrowMeshRenderer;//�}�b����ֽ�

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //�}�b����ֽ�
        arrowMeshRenderer = ExtensionMethods.FindAnyChild<MeshRenderer>(transform, "Arrow");
        arrowMeshRenderer.enabled = false;
    }

    void Update()
    {
        OnArrowEnabledControl();
    }

    /// <summary>
    /// ����1_�ĤH�h�L2
    /// </summary>
    void OnAttack1_EnemySoldier2()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("enemySoldier2Attack_Arrow"), GameSceneManagement.Instance.loadPath.enemySoldier2Attack_Arrow);//�������������(�ۨ�/�g�X����)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetShootFunction_Single);//�]�w����禡       
        attack.damage = NumericalValue.enemySoldier2_Attack1_Damge * rate;//�y���ˮ` 
        attack.direction = NumericalValue.enemySoldier2_Attack1_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.enemySoldier2_Attack1_RepelDistance;//���h/�����Z��
        attack.animationName = NumericalValue.enemySoldier2_Attack1_Effect;//�����ĪG(����ʵe�W��)        

        attack.flightSpeed = NumericalValue.enemySoldier2_Attack1_FloatSpeed;//����t��
        attack.lifeTime = NumericalValue.enemySoldier2_Attack1_LifeTime;//�ͦs�ɶ�
        attack.flightDiration = transform.forward;//�����V        
        attack.performObject.transform.position = arrowMeshRenderer.transform.position;//�g�X��m

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)             
    }

    /// <summary>
    /// ����2_�ĤH�h�L2
    /// </summary>
    void OnAttack2_EnemySoldier2()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("enemySoldier2Attack_Arrow"), GameSceneManagement.Instance.loadPath.enemySoldier2Attack_Arrow);//�������������(�ۨ�/�g�X����)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetShootFunction_Single);//�]�w����禡       
        attack.damage = NumericalValue.enemySoldier2_Attack2_Damge * rate;//�y���ˮ` 
        attack.direction = NumericalValue.enemySoldier2_Attack2_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.enemySoldier2_Attack2_RepelDistance;//���h/�����Z��
        attack.animationName = NumericalValue.enemySoldier2_Attack2_Effect;//�����ĪG(����ʵe�W��)        

        attack.flightSpeed = NumericalValue.enemySoldier2_Attack2_FloatSpeed;//����t��
        attack.lifeTime = NumericalValue.enemySoldier2_Attack2_LifeTime;//�ͦs�ɶ�
        attack.flightDiration = transform.forward;//�����V        
        attack.performObject.transform.position = arrowMeshRenderer.transform.position;//�g�X��m

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                
    }

    /// <summary>
    /// ����3_�ĤH�h�L2
    /// </summary>
    void OnAttack3_EnemySoldier2()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v
        float getDamage = NumericalValue.enemySoldier2_Attack_3_Damge * rate;//�y���ˮ`

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = getDamage;//�y���ˮ` 
        attack.direction = NumericalValue.enemySoldier2_Attack_3_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.enemySoldier2_Attack_3_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.enemySoldier2_Attack_3_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.enemySoldier2_Attack_3_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.enemySoldier2_Attack_3_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.enemySoldier2_Attack_3_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)                  
    }

    /// <summary>
    /// �}�b��ܱ���
    /// </summary>
    void OnArrowEnabledControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Attack.Attack1") && info.normalizedTime > 0.2f && info.normalizedTime < 0.63f)
        {
            if (!arrowMeshRenderer.enabled)
            {                
                //��m
                arrowMeshRenderer.transform.localPosition = new Vector3(1.99999995e-05f, 0.00486999983f, 0.00153999997f);
                arrowMeshRenderer.transform.localRotation = Quaternion.Euler(286.910248f, 1.72138309f, 257.902863f);

                arrowMeshRenderer.enabled = true;
            }
        }
        else if (info.IsName("Attack.Attack2") && info.normalizedTime > 0.2f && info.normalizedTime < 0.63f)
        {
            //��m
            arrowMeshRenderer.transform.localPosition = new Vector3(1.99999995e-05f, 0.00486999983f, 0.00153999997f);
            arrowMeshRenderer.transform.localRotation = Quaternion.Euler(286.910248f, 1.72138309f, 257.902863f);

            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }       
        else
        {
            if (arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = false;
        }
    }
}
