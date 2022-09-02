using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b��M��
/// </summary>
public class ArcherExclusive : MonoBehaviourPunCallbacks
{
    Animator animator;
    GameData_NumericalValue NumericalValue;
    PlayerControl playerControl;

    //Buff
    [SerializeField]float addDamage;//�W�[�ˮ`��

    MeshRenderer arrowMeshRenderer;//�}�b����ֽ�    
    string[] normalAttackArrowsPath;//���q�����}�b����

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();

        //Buff
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            if (GameDataManagement.Instance.equipBuff[i] == 1)
            {
                addDamage = GameDataManagement.Instance.numericalValue.buffAbleValue[1] / 100;//�W�[�ˮ`��
            }
        }

        //�}�b����ֽ�
        arrowMeshRenderer = ExtensionMethods.FindAnyChild<MeshRenderer>(transform, "Arrow");
        arrowMeshRenderer.enabled = false;

        normalAttackArrowsPath = new string[] { "archerNormalAttack_1", "archerNormalAttack_2", "archerNormalAttack_3" };//���q�����}�b����
    }
   
    void Update()
    {        
        OnArrowEnabledControl();
    }

    /// <summary>
    /// �ޯ����1_�}�b��
    /// </summary>
    void OnSkillAttack1_Archer()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        //�g����V
        Vector3[] diration = new Vector3[] { transform.forward - transform.right / 2,
                                             transform.forward - transform.right / 4,
                                             transform.forward,
                                             transform.forward + transform.right / 4,
                                             transform.forward + transform.right / 2};

        for (int i = 0; i < diration.Length; i++)
        {
            bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
            float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

            AttackMode attack = AttackMode.Instance;
            attack.performCharacters = gameObject;//��������}��
            attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("archerSkilllAttack_1"), GameSceneManagement.Instance.loadPath.archerSkilllAttack_1);//�������������(�ۨ�/�g�X����)
            attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
            attack.isCritical = isCritical;//�O�_�z��

            attack.function = new Action(attack.OnSetShootFunction_Single);//�]�w����禡       
            attack.damage = (NumericalValue.archerSkillAttack_1_Damage + (NumericalValue.archerSkillAttack_1_Damage * addDamage)) * rate;//�y���ˮ` 
            attack.direction = NumericalValue.archerSkillAttack_1_RepelDirection;//���h��V(0:���h, 1:����)
            attack.repel = NumericalValue.archerSkillAttack_1_Repel;//���h/�����Z��
            attack.animationName = NumericalValue.archerSkillAttack_1_Effect;//�����ĪG(����ʵe�W��)        

            attack.flightSpeed = NumericalValue.archerSkillAttack_1_FlightSpeed;//����t��
            attack.lifeTime = NumericalValue.archerSkillAttack_1_LifeTime;//�ͦs�ɶ�
            attack.flightDiration = diration[i];//�����V        
            attack.performObject.transform.position = arrowMeshRenderer.transform.position;//�g�X��m

            GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)           
        }        
    }

    /// <summary>
    /// �ޯ����2_�}�b��
    /// </summary>
    void OnSkillAttack2_Archer()
    {        
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = (NumericalValue.archerSkillAttack_2_Damge + (NumericalValue.archerSkillAttack_2_Damge * addDamage)) * rate;//�y���ˮ` 
        attack.direction = NumericalValue.archerSkillAttack_2_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerSkillAttack_2_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.archerSkillAttack_2_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.archerSkillAttack_2_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.archerSkillAttack_2_attackRadius;//�����d��
        attack.isAttackBehind = NumericalValue.archerSkillAttack_2_IsAttackBehind;//�O�_�����I��ĤH
        
        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// �ޯ����3_�}�b��
    /// </summary>
    void OnSkillAttack3_Archer()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = (NumericalValue.archerSkillAttack_3_Damge + (NumericalValue.archerSkillAttack_3_Damge * addDamage)) * rate;//�y���ˮ` 
        attack.direction = NumericalValue.archerSkillAttack_3_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerSkillAttack_3_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.archerSkillAttack_3_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.archerSkillAttack_3_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.archerSkillAttack_3_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.archerSkillAttack_3_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// ���D����_�}�b��
    /// </summary>
    void OnJumpAttack_Archer()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitSphereFunction);//�]�w����禡
        attack.damage = (NumericalValue.archerJumpAttack_Damage + (NumericalValue.archerJumpAttack_Damage * addDamage)) * rate;//�y���ˮ` 
        attack.direction = NumericalValue.archerJumpAttack_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerJumpAttack_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.archerJumpAttack_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.archerJumpAttack_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.archerJumpAttack_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.archerJumpAttack_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)   

        playerControl.isJumpAttackMove = true;//���D�����U��
    }

    /// <summary>
    /// ���q����_�}�b��
    /// </summary>
    /// <param name="number"></param>
    void OnNormalAttacks_Archer(int number)
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : UnityEngine.Random.Range(0.9f, 1.0f);//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performCharacters = gameObject;//��������}��
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber(normalAttackArrowsPath[number]), GameSceneManagement.Instance.loadPath.archerAllNormalAttack[number]);//�������������(�ۨ�/�g�X����)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetShootFunction_Single);//�]�w����禡       
        attack.damage = (NumericalValue.archerNormalAttack_Damge[number] + (NumericalValue.archerNormalAttack_Damge[number] * addDamage)) * rate;//�y���ˮ` 
        attack.direction = NumericalValue.archerNormalAttack_RepelDirection[number];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerNormalAttack_RepelDistance[number];//���h/�����Z��
        attack.animationName = NumericalValue.archerNormalAttack_Effect[number];//�����ĪG(����ʵe�W��)        

        attack.flightSpeed = NumericalValue.archerNormalAttack_FloatSpeed[number];//����t��
        attack.lifeTime = NumericalValue.archerNormalAttack_LifeTime[number];//�ͦs�ɶ�
        attack.flightDiration = transform.forward;//�����V        
        attack.performObject.transform.position = arrowMeshRenderer.transform.position;//�g�X��m

        GameSceneManagement.Instance.AttackMode_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// �}�b��ܱ���
    /// </summary>
    void OnArrowEnabledControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Attack.NormalAttack_1") && info.normalizedTime < 0.2f)
        {
            if (!arrowMeshRenderer.enabled)
            {
                //��m
                arrowMeshRenderer.transform.localPosition = new Vector3(-0.000209999998f, 0.00526000001f, 0.000190000006f);
                arrowMeshRenderer.transform.localRotation = Quaternion.Euler(272.496521f, 261.24234f, 8.01684952f);

                arrowMeshRenderer.enabled = true;
            }
        }
        else if (info.IsName("Attack.NormalAttack_2") && info.normalizedTime < 0.2f)
        {
            //��m
            arrowMeshRenderer.transform.localPosition = new Vector3(-0.000209999998f, 0.00526000001f, 0.000190000006f);
            arrowMeshRenderer.transform.localRotation = Quaternion.Euler(272.496521f, 261.24234f, 8.01684952f);

            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }
        else if (info.IsName("Attack.NormalAttack_3") && info.normalizedTime > 0.2f && info.normalizedTime < 0.63f)
        {
            //��m
            arrowMeshRenderer.transform.localPosition = new Vector3(1.99999995e-05f, 0.00486999983f, 0.00153999997f);
            arrowMeshRenderer.transform.localRotation = Quaternion.Euler(286.910248f, 1.72138309f, 257.902863f);

            if (!arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        }
        else if (info.IsName("Attack.SkillAttack_1") && info.normalizedTime > 0.2f && info.normalizedTime < 0.63f)
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
