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

    string[] normalAttackArrows;//�}�b����
    MeshRenderer arrowMeshRenderer;//�}�b����ֽ�
    int normalAttackNumber;//���q�����s��
    float[] shoopPosition;//�g�X��m(0:�V�W�Z�� 1:�V�e�Z��)

    void Start()
    {
        animator = GetComponent<Animator>();
        NumericalValue = GameDataManagement.Instance.numericalValue;
        playerControl = GetComponent<PlayerControl>();
 
        arrowMeshRenderer = ExtensionMethods.FindAnyChild<MeshRenderer>(transform, "Arrow");
        arrowMeshRenderer.enabled = false;

        
        normalAttackArrows = new string[] { "archerNormalAttack_1", "archerNormalAttack_2", "archerNormalAttack_3" };//�}�b����(�M�䪫��s����)                
        shoopPosition = new float[] { 1.35f, 0.65f };//�g�X��m(0:�V�W�Z�� 1:�V�e�Z��)
    }
   
    void Update()
    {
        normalAttackNumber = playerControl.GetNormalAttackNumber;//���q�����s��
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
            float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

            AttackMode attack = AttackMode.Instance;
            attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("archerSkilllAttack_1"), GameSceneManagement.Instance.loadPath.archerSkilllAttack_1);//�������������(�ۨ�/�g�X����)
            attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
            attack.isCritical = isCritical;//�O�_�z��

            attack.function = new Action(attack.OnSetShootFunction_Single);//�]�w����禡       
            attack.damage = NumericalValue.archerSkillAttack_1_Damage * rate;//�y���ˮ` 
            attack.direction = NumericalValue.archerSkillAttack_1_RepelDirection;//���h��V(0:���h, 1:����)
            attack.repel = NumericalValue.archerSkillAttack_1_Repel;//���h/�����Z��
            attack.animationName = NumericalValue.archerSkillAttack_1_Effect;//�����ĪG(����ʵe�W��)        

            attack.flightSpeed = NumericalValue.archerSkillAttack_1_FlightSpeed;//����t��
            attack.lifeTime = NumericalValue.archerSkillAttack_1_LifeTime;//�ͦs�ɶ�
            attack.flightDiration = diration[i];//�����V        
            attack.performObject.transform.position = transform.position + GetComponent<BoxCollider>().center + transform.forward * 1;//�g�X��m

            GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)           
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
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
        attack.damage = NumericalValue.archerSkillAttack_2_Damge * rate;//�y���ˮ` 
        attack.direction = NumericalValue.archerSkillAttack_2_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerSkillAttack_2_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.archerSkillAttack_2_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.archerSkillAttack_2_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.archerSkillAttack_2_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.archerSkillAttack_2_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// �ޯ����3_�}�b��
    /// </summary>
    void OnSkillAttack3_Archer()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
        attack.damage = NumericalValue.archerSkillAttack_3_Damge * rate;//�y���ˮ` 
        attack.direction = NumericalValue.archerSkillAttack_3_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerSkillAttack_3_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.archerSkillAttack_3_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.archerSkillAttack_3_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.archerSkillAttack_3_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.archerSkillAttack_3_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// ���D����_�}�b��
    /// </summary>
    void OnJumpAttack_Archer()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;
        attack.performObject = gameObject;//�������������(�ۨ�/�g�X����)                                                                                            
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetHitFunction);//�]�w����禡
        attack.damage = NumericalValue.archerJumpAttack_Damage * rate;//�y���ˮ` 
        attack.direction = NumericalValue.archerJumpAttack_RepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerJumpAttack_RepelDistance;//���h�Z��
        attack.animationName = NumericalValue.archerJumpAttack_Effect;//�����ĪG(����ʵe�W��)
        attack.forwardDistance = NumericalValue.archerJumpAttack_ForwardDistance;//�����d�򤤤��I�Z������e��
        attack.attackRadius = NumericalValue.archerJumpAttack_attackRadius;//�����b�|
        attack.isAttackBehind = NumericalValue.archerJumpAttack_IsAttackBehind;//�O�_�����I��ĤH

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// ���q����_�}�b��
    /// </summary>
    void OnNormalAttacks_Archer()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

        AttackMode attack = AttackMode.Instance;        
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber(normalAttackArrows[normalAttackNumber - 1]), GameSceneManagement.Instance.loadPath.archerAllNormalAttack[normalAttackNumber - 1]);//�������������(�ۨ�/�g�X����)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.isCritical = isCritical;//�O�_�z��

        attack.function = new Action(attack.OnSetShootFunction_Single);//�]�w����禡
        attack.damage = NumericalValue.archerNormalAttack_Damge[normalAttackNumber - 1] * rate;//�y���ˮ` 
        attack.direction = NumericalValue.archerNormalAttack_RepelDirection[normalAttackNumber - 1];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerNormalAttack_RepelDistance[normalAttackNumber - 1];//���h/�����Z��
        attack.animationName = NumericalValue.archerNormalAttack_Effect[normalAttackNumber - 1];//�����ĪG(����ʵe�W��)
        

        attack.flightSpeed = NumericalValue.archerNormalAttack_FloatSpeed[normalAttackNumber - 1];//����t��
        attack.lifeTime = NumericalValue.archerNormalAttack_LifeTime[normalAttackNumber - 1];//�ͦs�ɶ�
        attack.flightDiration = transform.forward;//�����V        
        attack.performObject.transform.position = transform.position + Vector3.up * shoopPosition[0] + transform.forward * shoopPosition[1];//�g�X��m

        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)           
    }

    /// <summary>
    /// �}�b��ܱ���
    /// </summary>
    void OnArrowEnabledControl()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Attack.NormalAttack_1") && info.normalizedTime < 0.45f && !arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        else if (info.IsName("Attack.NormalAttack_2") && info.normalizedTime < 0.45f && !arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        else if (info.IsName("Attack.NormalAttack_3") && info.normalizedTime > 0.23f && info.normalizedTime < 0.7f && !arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = true;
        else
        {
            if(arrowMeshRenderer.enabled) arrowMeshRenderer.enabled = false;
        }
    }
}
