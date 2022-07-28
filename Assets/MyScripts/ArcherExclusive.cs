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
    
    [Header("�}�b����")]
    string[] normalAttackArrows;//�Ҧ����q�����}�b

    [Header("��L")]
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

        //�}�b����
        normalAttackArrows = new string[] { "archerNormalAttack_1_Arrow", "archerNormalAttack_2_Arrow", "archerNormalAttack_3_Arrow" };

        //��L
        shoopPosition = new float[] { 1.35f, 0.65f };//�g�X��m(0:�V�W�Z�� 1:�V�e�Z��)
    }
   
    void Update()
    {
        normalAttackNumber = playerControl.GetNormalAttackNumber;//���q�����s��
        OnArrowEnabledControl();
    }

    /// <summary>
    /// �ޯ�1�����欰_�}�b��
    /// </summary>
    void OnSkillAttackBehavior_1_Archer()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        Vector3[] diration = new Vector3[] { transform.forward - transform.right / 2, transform.forward - transform.right, transform.forward, transform.forward + transform.right / 2, transform.forward + transform.right };

        for (int i = 0; i < diration.Length; i++)
        {
            AttackMode attack = AttackMode.Instance;
            bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
            float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v

            attack.function = new Action(attack.OnSetShootFunction_Single);//�]�w����禡
            attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber("archerSkilllAttack_1_Arrow"), GameSceneManagement.Instance.loadPath.archerSkilllAttack_1_Arrow);//�������������(�ۨ�/�g�X����)
            attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
            attack.damage = NumericalValue.archerSkillAttackDamage[normalAttackNumber - 1] * rate;//�y���ˮ` 
            attack.animationName = NumericalValue.archerSkillAttackEffect[normalAttackNumber - 1];//�����ĪG(����ʵe�W��)
            attack.direction = NumericalValue.archerSkillAttackRepelDirection[normalAttackNumber - 1];//���h��V(0:���h, 1:����)
            attack.repel = NumericalValue.archerSkillAttackRepel[normalAttackNumber - 1];//���h/�����Z��
            attack.isCritical = isCritical;//�O�_�z��
            attack.speed = NumericalValue.arrowFloatSpeed;//����t��
            attack.lifeTime = NumericalValue.arrowLifeTime;//�ͦs�ɶ�
            attack.diration = diration[i];//�����V        
            attack.performObject.transform.position = transform.position + Vector3.up * shoopPosition[0] + transform.forward * shoopPosition[1];//�g�X��m
            GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)           
        }        
    }

    /// <summary>
    /// �ޯ�2�����欰_�}�b��
    /// </summary>
    void OnSkillAttackBehavior_2_Archer()
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
        attack.damage = NumericalValue.archerSkillAttackDamage[normalAttackNumber - 1] * rate;//�y���ˮ` 
        attack.animationName = NumericalValue.archerSkillAttackEffect[normalAttackNumber - 1];//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.archerSkillAttackRepelDirection[normalAttackNumber - 1];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerSkillAttackRepel[normalAttackNumber - 1];//���h/�����Z��
        attack.boxSize = NumericalValue.archerSkillAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//�񨭧�����Size
        attack.isCritical = isCritical;//�O�_�z��
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// �ޯ�2�����欰_�}�b��
    /// </summary>
    void OnSkillAttackBehavior_3_Archer()
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
        attack.damage = NumericalValue.archerSkillAttackDamage[normalAttackNumber - 1] * rate;//�y���ˮ` 
        attack.animationName = NumericalValue.archerSkillAttackEffect[normalAttackNumber - 1];//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.archerSkillAttackRepelDirection[normalAttackNumber - 1];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerSkillAttackRepel[normalAttackNumber - 1];//���h/�����Z��
        attack.boxSize = NumericalValue.archerSkillAttackBoxSize[normalAttackNumber - 1] * transform.lossyScale.x;//�񨭧�����Size
        attack.isCritical = isCritical;//�O�_�z��
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// ���D�����欰_�}�b��
    /// </summary>
    void OnJumpAttackBehavior_Archer()
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
        attack.damage = NumericalValue.archerJumpAttackDamage * rate;//�y���ˮ` 
        attack.animationName = NumericalValue.archerJumpAttackEffect;//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.archerJumpAttackRepelDirection;//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerJumpAttackRepelDistance;//���h�Z��
        attack.boxSize = NumericalValue.archerJumpAttackBoxSize * transform.lossyScale.x;//�񨭧�����Size
        attack.isCritical = isCritical;//�O�_�z��
        GameSceneManagement.Instance.AttackBehavior_List.Add(attack);//�[�JList(����)   
    }

    /// <summary>
    /// ���q�����欰_�}�b��
    /// </summary>
    void OnNormalAttackBehavior_Archer()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine) return;

        bool isCritical = UnityEngine.Random.Range(0, 100) < NumericalValue.playerCriticalRate ? true : false;//�O�_�z��
        float rate = isCritical ? NumericalValue.criticalBonus : 1;//�z���������ɭ��v
        
        //�]�wAttackBehavior Class�ƭ�
        AttackMode attack = AttackMode.Instance;
        attack.function = new Action(attack.OnSetShootFunction_Single);//�]�w����禡
        attack.performObject = GameSceneManagement.Instance.OnRequestOpenObject(GameSceneManagement.Instance.OnGetObjectNumber(normalAttackArrows[normalAttackNumber - 1]), GameSceneManagement.Instance.loadPath.archerNormalAttackArrows[normalAttackNumber - 1]);//�������������(�ۨ�/�g�X����)
        attack.layer = LayerMask.LayerToName(gameObject.layer);//������layer
        attack.damage = NumericalValue.archerNormalAttackDamge[normalAttackNumber - 1] * rate;//�y���ˮ` 
        attack.animationName = NumericalValue.archerNormalAttackEffect[normalAttackNumber - 1];//�����ĪG(����ʵe�W��)
        attack.direction = NumericalValue.archerNormalAttackRepelDirection[normalAttackNumber - 1];//���h��V(0:���h, 1:����)
        attack.repel = NumericalValue.archerNormalAttackRepelDistance[normalAttackNumber - 1];//���h/�����Z��
        attack.isCritical = isCritical;//�O�_�z��
        attack.speed = NumericalValue.arrowFloatSpeed;//����t��
        attack.lifeTime = NumericalValue.arrowLifeTime;//�ͦs�ɶ�
        attack.diration = transform.forward;//�����V        
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

    private void OnDrawGizmos()
    {
        
    }
}
