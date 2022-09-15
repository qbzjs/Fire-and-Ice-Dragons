using UnityEngine;

/// <summary>
/// �C���ƭ�
/// </summary>
[System.Serializable]
public class GameData_NumericalValue
{
    [Header("�@�q")]
    public float gravity;//���O
    public float criticalBonus;//�����ˮ`�[��
    public string[] levelNames;//���d�W��

    [Header("Buff�W�[�ƭ�")]
    public string[] buffAbleString;//Buff�W�q��r
    public float[] buffAbleValue;//Buff�W�q�ƭ�

    [Header("��v��")]
    public float distance;//�P���a�Z��
    public float limitUpAngle;//����V�W����
    public float limitDownAngle;//����V�U����
    //public float cameraAngle;//��v������

    [Header("���a")]
    public float playerHp;//���a�ͩR��
    public float playerMoveSpeed;//���a���ʳt��
    public float playerJumpForce;//���a���D�O
    public float playerCriticalRate;//���a�����v
    public float playerDodgeSeppd;//���a�{���t��
    public float playerSelfHealTime;//���a�ۨ��^�_�ɶ�

    [Header("���I")]
    public float strongholdHp;//���IHP

    [Header("�P���h�LHP")]
    public float allianceSoldier1_Hp;//�P���h�L1_�ͩR��

    [Header("�ĤHHP")]
    public float boss_Hp;//Boss_�ͩR��
    public float enemySoldier1_Hp;//�ĤH�h�L1_�ͩR��
    public float enemySoldier2_Hp;//�ĤH�h�L2_�ͩR��
    public float enemySoldier3_Hp;//�ĤH�h�L3_�ͩR��
    public float guardBoss_Hp;//�����u��Boss_�ͩR��

    #region �Ԥh
    [Header("�Ԥh ���q����1")]
    public float warriorNormalAttack_1_Damge;//�Ԥh���q����1_�ˮ`
    public int warriorNormalAttack_1_RepelDirection;//�Ԥh���q����1_���h��V(0:���h 1:����)
    public float warriorNormalAttack_1_RepelDistance;//�Ԥh���q����1_���h/�����Z��    
    public string warriorNormalAttack_1_Effect;//�Ԥh���q����1_�ĪG(�����̼��񪺰ʵe�W��)        
    public float warriorNormalAttack_1_ForwardDistance;//�Ԥh���q����1_�����d�򤤤��I�Z������e��
    public float warriorNormalAttack_1_attackRadius;//�Ԥh���q����1_�����b�|
    public bool warriorNormalAttack_1_IsAttackBehind;//�Ԥh���q����1_�O�_�����I��ĤH

    [Header("�Ԥh ���q����2")]
    public float warriorNormalAttack_2_Damge;//�Ԥh���q����2_�ˮ`
    public int warriorNormalAttack_2_RepelDirection;//�Ԥh���q����2_���h��V(0:���h 1:����)
    public float warriorNormalAttack_2_RepelDistance;//�Ԥh���q����2_���h/�����Z��    
    public string warriorNormalAttack_2_Effect;//�Ԥh���q����2_�ĪG(�����̼��񪺰ʵe�W��)        
    public float warriorNormalAttack_2_ForwardDistance;//�Ԥh���q����2_�����d�򤤤��I�Z������e��
    public float warriorNormalAttack_2_attackRadius;//�Ԥh���q����2_�����b�|
    public bool warriorNormalAttack_2_IsAttackBehind;//�Ԥh���q����2_�O�_�����I��ĤH

    [Header("�Ԥh ���q����3")]
    public float warriorNormalAttack_3_Damge;//�Ԥh���q����3_�ˮ`
    public int warriorNormalAttack_3_RepelDirection;//�Ԥh���q����3_���h��V(0:���h 1:����)
    public float warriorNormalAttack_3_RepelDistance;//�Ԥh���q����3_���h/�����Z��    
    public string warriorNormalAttack_3_Effect;//�Ԥh���q����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float warriorNormalAttack_3_ForwardDistance;//�Ԥh���q����3_�����d�򤤤��I�Z������e��
    public float warriorNormalAttack_3_attackRadius;//�Ԥh���q����3_�����b�|
    public bool warriorNormalAttack_3_IsAttackBehind;//�Ԥh���q����3_�O�_�����I��ĤH

    [Header("�Ԥh ���D����")]
    public float warriorJumpAttack_Damage;//�Ԥh���D����_�ˮ`
    public int warriorJumpAttack_RepelDirection;//�Ԥh���D����_���h��V(0:���h 1:����)
    public float warriorJumpAttac_kRepelDistance;//�Ԥh���D����_���h�Z��
    public string warriorJumpAttack_Effect;//�Ԥh���D�����ĪG(�����̼��񪺰ʵe�W��)
    public float warriorJumpAttack_ForwardDistance;//�Ԥh���q����3_�����d�򤤤��I�Z������e��
    public float warriorJumpAttack_attackRadius;//�Ԥh���q����3_�����b�|
    public bool warriorJumpAttack_IsAttackBehind;//�Ԥh���q����3_�O�_�����I��ĤH

    [Header("�Ԥh �ޯ����1")]
    public float warriorSkillAttack_1_Damge;//�Ԥh�ޯ����1_�ˮ`
    public int warriorSkillAttack_1_RepelDirection;//�Ԥh�ޯ����1_���h��V(0:���h 1:����)
    public float warriorSkillAttack_1_RepelDistance;//�Ԥh�ޯ����1_���h/�����Z��    
    public string warriorSkillAttack_1_Effect;//�Ԥh���ޯ����1_�ĪG(�����̼��񪺰ʵe�W��)        
    public float warriorSkillAttack_1_ForwardDistance;//�Ԥh�ޯ����1_�����d�򤤤��I�Z������e��
    public float warriorSkillAttack_1_attackRadius;//�Ԥh�ޯ����1_�����b�|
    public bool warriorSkillAttack_1_IsAttackBehind;//�Ԥh�ޯ����1_�O�_�����I��ĤH

    [Header("�Ԥh �ޯ����2")]
    public float warriorSkillAttack_2_Damge;//�Ԥh�ޯ����2_�ˮ`
    public int warriorSkillAttack_2_RepelDirection;//�Ԥh�ޯ����2_���h��V(0:���h 1:����)
    public float warriorSkillAttack_2_RepelDistance;//�Ԥh�ޯ����2_���h/�����Z��    
    public string warriorSkillAttack_2_Effect;//�Ԥh���ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
    public float warriorSkillAttack_2_ForwardDistance;//�Ԥh�ޯ����2_�����d�򤤤��I�Z������e��
    public float warriorSkillAttack_2_attackRadius;//�Ԥh�ޯ����2_�����b�|
    public bool warriorSkillAttack_2_IsAttackBehind;//�Ԥh�ޯ����2_�O�_�����I��ĤH

    [Header("�Ԥh �ޯ����3")]
    public float[] warriorSkillAttack_3_Damge;//�Ԥh�ޯ����2_�ˮ`
    public int[] warriorSkillAttack_3_RepelDirection;//�Ԥh�ޯ����3_���h��V(0:���h 1:����)
    public float[] warriorSkillAttack_3_RepelDistance;//�Ԥh�ޯ����3_���h/�����Z��    
    public string[] warriorSkillAttack_3_Effect;//�Ԥh���ޯ����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float[] warriorSkillAttack_3_ForwardDistance;//�Ԥh�ޯ����3_�����d�򤤤��I�Z������e��
    public float[] warriorSkillAttack_3_attackRadius;//�Ԥh�ޯ����3_�����b�|
    public bool[] warriorSkillAttack_3_IsAttackBehind;//�Ԥh�ޯ����3_�O�_�����I��ĤH
    #endregion

    #region �}�b��   
    [Header("�}�b�� ���q����")]
    public float[] archerNormalAttack_Damge;//�}�b�ⴶ�q����_�ˮ`
    public int[] archerNormalAttack_RepelDirection;//�}�b�ⴶ�q����_���h��V(0:���h 1:����)
    public float[] archerNormalAttack_RepelDistance;//�}�b�ⴶ�q����_���h/�����Z��    
    public string[] archerNormalAttack_Effect;//�}�b�ⴶ�q����_�ĪG(�����̼��񪺰ʵe�W��)
    public float[] archerNormalAttack_FloatSpeed;//�}�b�ⴶ�q��������t��
    public float[] archerNormalAttack_LifeTime;//�}�b�ⴶ�q�����ͦs�ɶ�

    [Header("�}�b�� ���D����")]
    public float archerJumpAttack_Damage;//�}�b����D����_�ˮ`
    public int archerJumpAttack_RepelDirection;//�}�b����D����_���h��V(0:���h 1:����)  
    public float archerJumpAttack_RepelDistance;//�}�b����D����_���h�Z��
    public string archerJumpAttack_Effect;//�}�b����D����_�ĪG(�����̼��񪺰ʵe�W��)
    public float archerJumpAttack_ForwardDistance;//�}�b����D����_�����d�򤤤��I�Z������e��
    public float archerJumpAttack_attackRadius;//�}�b����D����_�����b�|
    public bool archerJumpAttack_IsAttackBehind;//�}�b����D����_�O�_�����I��ĤH

    [Header("�}�b�� �ޯ����1")]
    public float archerSkillAttack_1_Damage;//�}�b��ޯ����1_�ˮ`
    public int archerSkillAttack_1_RepelDirection;//�}�b��ޯ����1_���h��V(0:���h 1:����)
    public float archerSkillAttack_1_Repel;//�}�b��ޯ����1_���h�Z��        
    public string archerSkillAttack_1_Effect;//�}�b��ޯ����1_�ĪG(�����̼��񪺰ʵe�W��)
    public float archerSkillAttack_1_FlightSpeed;//�}�b��ޯ����1_����t��
    public float archerSkillAttack_1_LifeTime;//�}�b��ޯ����1_�ͦs�ɶ�

    [Header("�}�b�� �ޯ����2")]
    public float archerSkillAttack_2_Damge;//�}�b��ޯ����2_�ˮ`
    public int archerSkillAttack_2_RepelDirection;//�}�b��ޯ����2_���h��V(0:���h 1:����)
    public float archerSkillAttack_2_RepelDistance;//�}�b��ޯ����2_���h/�����Z��    
    public string archerSkillAttack_2_Effect;//�}�b��ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
    public float archerSkillAttack_2_ForwardDistance;//�}�b��ޯ����2_�����d�򤤤��I�Z������e��
    public float archerSkillAttack_2_attackRadius;//�}�b��ޯ����2_�����b�|
    public bool archerSkillAttack_2_IsAttackBehind;//�}�b��ޯ����2_�O�_�����I��ĤH

    [Header("�}�b�� �ޯ����3")]
    public float archerSkillAttack_3_Damge;//�}�b��ޯ����3_�ˮ`
    public int archerSkillAttack_3_RepelDirection;//�}�b��ޯ����3_���h��V(0:���h 1:����)
    public float archerSkillAttack_3_RepelDistance;//�}�b��ޯ����3_���h/�����Z��    
    public string archerSkillAttack_3_Effect;//�}�b��ޯ����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float archerSkillAttack_3_ForwardDistance;//�}�b��ޯ����3_�����d�򤤤��I�Z������e��
    public float archerSkillAttack_3_attackRadius;//�}�b��ޯ����3_�����b�|
    public bool archerSkillAttack_3_IsAttackBehind;//�}�b��ޯ����3_�O�_�����I��ĤH
    #endregion

    #region �k�v
    [Header("�k�v ���q����1")]
    public float magicianNormalAttack_1_Damage;//�k�v���q����1_�ˮ`
    public int magicianNormalAttack_1_RepelDirection;//�k�v���q����1_���h��V(0:���h 1:����)
    public float magicianNormalAttack_1_Repel;//�k�v���q����1_���h�Z��        
    public string magicianNormalAttack_1_Effect;//�k�v���q����1_�ĪG(�����̼��񪺰ʵe�W��)
    public float magicianNormalAttack_1_FlightSpeed;//�k�v���q����1_����t��
    public float magicianNormalAttack_1_LifeTime;//�k�v���q����1_�ͦs�ɶ�

    [Header("�k�v ���q����2")]
    public float magicianNormalAttack_2_Damge;//�k�v���q����2_�ˮ`
    public int magicianNormalAttack_2_RepelDirection;//�k�v���q����2_���h��V(0:���h 1:����)
    public float magicianNormalAttack_2_RepelDistance;//�k�v���q����2_���h/�����Z��    
    public string magicianNormalAttack_2_Effect;//�k�v���q����2_�ĪG(�����̼��񪺰ʵe�W��)        
    public float magicianNormalAttack_2_ForwardDistance;//�k�v���q����2_�����d�򤤤��I�Z������e��
    public Vector3 magicianNormalAttack_2_attackRange;//�k�v���q����2_�����d��
    public bool magicianNormalAttack_2_IsAttackBehind;//�k�v���q����2_�O�_�����I��ĤH

    [Header("�k�v ���q����3")]
    public float magicianNormalAttack_3_Damge;//�k�v���q����3_�ˮ`
    public int magicianNormalAttack_3_RepelDirection;//�k�v���q����3_���h��V(0:���h 1:����)
    public float magicianNormalAttack_3_RepelDistance;//�k�v���q����3_���h/�����Z��    
    public string magicianNormalAttack_3_Effect;//�k�v���q����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float magicianNormalAttack_3_ForwardDistance;//�k�v���q����3_�����d�򤤤��I�Z������e��
    public float magicianNormalAttack_3_attackRadius;//�k�v���q����3_�����b�|
    public bool magicianNormalAttack_3_IsAttackBehind;//�k�v���q����3_�O�_�����I��ĤH

    [Header("�k�v ���D����")]
    public float magicianJumpAttack_Damage;//�k�v���D����_�ˮ`
    public int magicianJumpAttack_RepelDirection;//�k�v���D����_���h��V(0:���h 1:����)  
    public float magicianJumpAttack_RepelDistance;//�k�v���D����_���h�Z��
    public string magicianJumpAttack_Effect;//�k�v���D����_�ĪG(�����̼��񪺰ʵe�W��)
    public float magicianJumpAttack_ForwardDistance;//�k�v���D����_�����d�򤤤��I�Z������e��
    public float magicianJumpAttack_attackRadius;//�k�v���D����_�����b�|
    public bool magicianJumpAttack_IsAttackBehind;//�k�v���D����_�O�_�����I��ĤH

    [Header("�k�v �ޯ����1")]
    public float magicianSkillAttack_1_HealValue;//�k�v���q����1_�v���q    
    public float magicianSkillAttack_1_ForwardDistance;//�k�v���q����1_�����d�򤤤��I�Z������e��
    public float magicianSkillAttack_1_attackRange;//�k�v���q����1_�v���d��
    public bool magicianSkillAttack_1_IsAttackBehind;//�k�v���q����1_�O�_�v���I�����

    /*[Header("�k�v �ޯ����2")]
    public float[] magicianSkillAttack_2_Damge;//�k�v�ޯ����2_�ˮ`
    public int[] magicianSkillAttack_2_RepelDirection;//�k�v�ޯ����2_���h��V(0:���h 1:����)
    public float[] magicianSkillAttack_2_RepelDistance;//�k�v�ޯ����2_���h/�����Z��    
    public string[] magicianSkillAttack_2_Effect;//�k�v�ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
    public float[] magicianSkillAttack_2_ForwardDistance;//�k�v�ޯ����2_�����d�򤤤��I�Z������e��
    public float[] magicianSkillAttack_2_attackRadius;//�k�v�ޯ����2_�����b�|
    public bool[] magicianSkillAttack_2_IsAttackBehind;//�k�v�ޯ����2_�O�_�����I��ĤH*/

    [Header("�k�v �ޯ��23")]
    public float magicianSkillAttack_2_Damge;//�k�v�ޯ����2_�ˮ`
    public int magicianSkillAttack_2_RepelDirection;//�k�v�ޯ����2_���h��V(0:���h 1:����)
    public float magicianSkillAttack_2_RepelDistance;//�k�v�ޯ����2_���h/�����Z��    
    public string magicianSkillAttack_2_Effect;//�k�v�ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
    public float magicianSkillAttack_2_ForwardDistance;//�k�v�ޯ����2_�����d�򤤤��I�Z������e��
    public float magicianSkillAttack_2_attackRadius;//�k�v�ޯ����2_�����b�|
    public bool magicianSkillAttack_2_IsAttackBehind;//�k�v�ޯ����2_�O�_�����I��ĤH

    [Header("�k�v �ޯ����3")]
    public float magicianSkillAttack_3_Damge;//�k�v�ޯ����3_�ˮ`
    public int magicianSkillAttack_3_RepelDirection;//�k�v�ޯ����3_���h��V(0:���h 1:����)
    public float magicianSkillAttack_3_RepelDistance;//�k�v�ޯ����3_���h/�����Z��    
    public string magicianSkillAttack_3_Effect;//�k�v�ޯ����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float magicianSkillAttack_3_ForwardDistance;//�k�v�ޯ����3_�����d�򤤤��I�Z������e��
    public float magicianSkillAttack_3_attackRadius;//�k�v�ޯ����3_�����b�|
    public bool magicianSkillAttack_3_IsAttackBehind;//�k�v�ޯ����3_�O�_�����I��ĤH
    #endregion

    #region �ĤH�h�L1(���Y�H)
    [Header("�ĤH�h�L1 ����1")]
    public float enemySoldier1_Attack_1_Damge;//�ĤH�h�L1_����1_�ˮ`
    public int enemySoldier1_Attack_1_RepelDirection;//�ĤH�h�L1_����1_���h��V(0:���h 1:����)
    public float enemySoldier1_Attack_1_RepelDistance;//�ĤH�h�L1_����1_���h/�����Z��    
    public string enemySoldier1_Attack_1_Effect;//�ĤH�h�L1_����1_�ĪG(�����̼��񪺰ʵe�W��)        
    public float enemySoldier1_Attack_1_ForwardDistance;//�ĤH�h�L1_����1_�����d�򤤤��I�Z������e��
    public float enemySoldier1_Attack_1_attackRadius;//�ĤH�h�L1_����1_�����b�|
    public bool enemySoldier1_Attack_1_IsAttackBehind;//�ĤH�h�L1_����1_�O�_�����I��ĤH

    [Header("�ĤH�h�L1 ����2")]
    public float enemySoldier1_Attack_2_Damge;//�ĤH�h�L1_����2_�ˮ`
    public int enemySoldier1_Attack_2_RepelDirection;//�ĤH�h�L1_����2_���h��V(0:���h 1:����)
    public float enemySoldier1_Attack_2_RepelDistance;//�ĤH�h�L1_����2_���h/�����Z��    
    public string enemySoldier1_Attack_2_Effect;//�ĤH�h�L1_����2_�ĪG(�����̼��񪺰ʵe�W��)        
    public float enemySoldier1_Attack_2_ForwardDistance;//�ĤH�h�L1_����2_�����d�򤤤��I�Z������e��
    public float enemySoldier1_Attack_2_attackRadius;//�ĤH�h�L1_����2_�����b�|
    public bool enemySoldier1_Attack_2_IsAttackBehind;//�ĤH�h�L1_����2_�O�_�����I��ĤH

    [Header("�ĤH�h�L1 ����3")]
    public float enemySoldier1_Attack_3_Damge;//�ĤH�h�L1_����3_�ˮ`
    public int enemySoldier1_Attack_3_RepelDirection;//�ĤH�h�L1_����3_���h��V(0:���h 1:����)
    public float enemySoldier1_Attack_3_RepelDistance;//�ĤH�h�L1_����3_���h/�����Z��    
    public string enemySoldier1_Attack_3_Effect;//�ĤH�h�L1_����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float enemySoldier1_Attack_3_ForwardDistance;//�ĤH�h�L1_����3_�����d�򤤤��I�Z������e��
    public float enemySoldier1_Attack_3_attackRadius;//�ĤH�h�L1_����3_�����b�|
    public bool enemySoldier1_Attack_3_IsAttackBehind;//�ĤH�h�L1_����3_�O�_�����I��ĤH
    #endregion

    #region �ĤH�h�L2(�}�b��)
    [Header("�ĤH�h�L2 ����1")]
    public float enemySoldier2_Attack1_Damge;//�ĤH�h�L2_����1_�ˮ`
    public int enemySoldier2_Attack1_RepelDirection;//�ĤH�h�L2_����1_���h��V(0:���h 1:����)
    public float enemySoldier2_Attack1_RepelDistance;//�ĤH�h�L2_����1_���h/�����Z��    
    public string enemySoldier2_Attack1_Effect;//�ĤH�h�L2_����1_�ĪG(�����̼��񪺰ʵe�W��)
    public float enemySoldier2_Attack1_FloatSpeed;//�ĤH�h�L2_����1_����t��
    public float enemySoldier2_Attack1_LifeTime;//�ĤH�h�L2_����1_�ͦs�ɶ�

    [Header("�ĤH�h�L2 ����2")]
    public float enemySoldier2_Attack2_Damge;//�ĤH�h�L2_����2_�ˮ`
    public int enemySoldier2_Attack2_RepelDirection;//�ĤH�h�L2_����2_���h��V(0:���h 1:����)
    public float enemySoldier2_Attack2_RepelDistance;//�ĤH�h�L2_����2_���h/�����Z��    
    public string enemySoldier2_Attack2_Effect;//�ĤH�h�L2_����2_�ĪG(�����̼��񪺰ʵe�W��)
    public float enemySoldier2_Attack2_FloatSpeed;//�ĤH�h�L2_����2_����t��
    public float enemySoldier2_Attack2_LifeTime;//�ĤH�h�L2_����2_�ͦs�ɶ�

    [Header("�ĤH�h�L2 ����3")]
    public float enemySoldier2_Attack_3_Damge;//�ĤH�h�L2_����3_�ˮ`
    public int enemySoldier2_Attack_3_RepelDirection;//�ĤH�h�L2_����3_���h��V(0:���h 1:����)
    public float enemySoldier2_Attack_3_RepelDistance;//�ĤH�h�L2_����3_���h/�����Z��    
    public string enemySoldier2_Attack_3_Effect;//�ĤH�h�L2_����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float enemySoldier2_Attack_3_ForwardDistance;//�ĤH�h�L2_����3_�����d�򤤤��I�Z������e��
    public float enemySoldier2_Attack_3_attackRadius;//�ĤH�h�L2_����3_�����b�|
    public bool enemySoldier2_Attack_3_IsAttackBehind;//�ĤH�h�L2_����3_�O�_�����I��ĤH
    #endregion

    #region �ĤH�h�L3(���Y�H)
    [Header("�ĤH�h�L3 ����1")]
    public float enemySoldier3_Attack_1_Damge;//�ĤH�h�L3_����1_�ˮ`
    public int enemySoldier3_Attack_1_RepelDirection;//�ĤH�h�L3_����1_���h��V(0:���h 1:����)
    public float enemySoldier3_Attack_1_RepelDistance;//�ĤH�h�L3_����1_���h/�����Z��    
    public string enemySoldier3_Attack_1_Effect;//�ĤH�h�L3_����1_�ĪG(�����̼��񪺰ʵe�W��)        
    public float enemySoldier3_Attack_1_ForwardDistance;//�ĤH�h�L3_����1_�����d�򤤤��I�Z������e��
    public float enemySoldier3_Attack_1_attackRadius;//�ĤH�h�L3_����1_�����b�|
    public bool enemySoldier3_Attack_1_IsAttackBehind;//�ĤH�h�L3_����1_�O�_�����I��ĤH

    [Header("�ĤH�h�L3 ����2")]
    public float enemySoldier3_Attack_2_Damge;//�ĤH�h�L3_����2_�ˮ`
    public int enemySoldier3_Attack_2_RepelDirection;//�ĤH�h�L3_����2_���h��V(0:���h 1:����)
    public float enemySoldier3_Attack_2_RepelDistance;//�ĤH�h�L3_����2_���h/�����Z��    
    public string enemySoldier3_Attack_2_Effect;//�ĤH�h�L1_����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float enemySoldier3_Attack_2_ForwardDistance;//�ĤH�h�L3_����2_�����d�򤤤��I�Z������e��
    public float enemySoldier3_Attack_2_attackRadius;//�ĤH�h�L3_����2_�����b�|
    public bool enemySoldier3_Attack_2_IsAttackBehind;//�ĤH�h�L3_����2_�O�_�����I��ĤH

    [Header("�ĤH�h�L13����3")]
    public float enemySoldier3_Attack_3_Damge;//�ĤH�h�L3_����3_�ˮ`
    public int enemySoldier3_Attack_3_RepelDirection;//�ĤH�h�L3_����3_���h��V(0:���h 1:����)
    public float enemySoldier3_Attack_3_RepelDistance;//�ĤH�h�L3_����3_���h/�����Z��    
    public string enemySoldier3_Attack_3_Effect;//�ĤH�h�L3_����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float enemySoldier3_Attack_3_ForwardDistance;//�ĤH�h�L3_����3_�����d�򤤤��I�Z������e��
    public float enemySoldier3_Attack_3_attackRadius;//�ĤH�h�L3_����3_�����b�|
    public bool enemySoldier3_Attack_3_IsAttackBehind;//�ĤH�h�L3_����3_�O�_�����I��ĤH
    #endregion

    #region �u��Boss
    [Header("�u��Boss ����1")]
    public float guardBoss_Attack_1_Damge;//�u��Boss_����1_�ˮ`
    public int guardBoss_Attack_1_RepelDirection;//�u��Boss_����1_���h��V(0:���h 1:����)
    public float guardBoss_Attack_1_RepelDistance;//�u��Boss_����1_���h/�����Z��    
    public string guardBoss_Attack_1_Effect;//�u��Boss_����1_�ĪG(�����̼��񪺰ʵe�W��)        
    public float guardBoss_Attack_1_ForwardDistance;//�u��Boss_����1_�����d�򤤤��I�Z������e��
    public float guardBoss_Attack_1_attackRadius;//�u��Boss_����1_�����b�|
    public bool guardBoss_Attack_1_IsAttackBehind;//�u��Boss1_����1_�O�_�����I��ĤH

    [Header("�u��Boss ����2")]
    public float guardBoss_Attack_2_Damge;//�u��Boss_����2_�ˮ`
    public int guardBoss_Attack_2_RepelDirection;//�u��Boss_����2_���h��V(0:���h 1:����)
    public float guardBoss_Attack_2_RepelDistance;//�u��Boss_����2_���h/�����Z��    
    public string guardBoss_Attack_2_Effect;//�u��Boss_����2_�ĪG(�����̼��񪺰ʵe�W��)        
    public float guardBoss_Attack_2_ForwardDistance;//�u��Boss_����2_�����d�򤤤��I�Z������e��
    public float guardBoss_Attack_2_attackRadius;//�u��Boss_����2_�����b�|
    public bool guardBoss_Attack_2_IsAttackBehind;//�u��Boss1_����2_�O�_�����I��ĤH

    [Header("�u��Boss ����3")]
    public float guardBoss_Attack_3_Damge;//�u��Boss_����3_�ˮ`
    public int guardBoss_Attack_3_RepelDirection;//�u��Boss_����3_���h��V(0:���h 1:����)
    public float guardBoss_Attack_3_RepelDistance;//�u��Boss_����3_���h/�����Z��    
    public string guardBoss_Attack_3_Effect;//�u��Boss_����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float guardBoss_Attack_3_ForwardDistance;//�u��Boss_����3_�����d�򤤤��I�Z������e��
    public float guardBoss_Attack_3_attackRadius;//�u��Boss_����3_�����b�|
    public bool guardBoss_Attack_3_IsAttackBehind;//�u��Boss_����3_�O�_�����I��ĤH

    [Header("�u��Boss ����4")]
    public float guardBoss_Attack_4_Damge;//�u��Boss_����4_�ˮ`
    public int guardBoss_Attack_4_RepelDirection;//�u��Boss_����4_���h��V(0:���h 1:����)
    public float guardBoss_Attack_4_RepelDistance;//�u��Boss_����4_���h/�����Z��    
    public string guardBoss_Attack_4_Effect;//�u��Boss_����4_�ĪG(�����̼��񪺰ʵe�W��)        
    public float guardBoss_Attack_4_ForwardDistance;//�u��Boss_����4_�����d�򤤤��I�Z������e��
    public float guardBoss_Attack_4_attackRadius;//�u��Boss_����4_�����b�|
    public bool guardBoss_Attack_4_IsAttackBehind;//�u��Boss_����4_�O�_�����I��ĤH
    #endregion

    #region Boss
    [Header("Boss ����1")]
    public float bossAttack1_Damge;//Boss����1_�ˮ`
    public int bossAttack1_RepelDirection;//Boss����1_���h��V(0:���h 1:����)
    public float bossAttack1_RepelDistance;//Boss����1_���h/�����Z��    
    public string bossAttack1_Effect;//Boss����1_�ĪG(�����̼��񪺰ʵe�W��)
    public float bossAttack1_FloatSpeed;//Boss����1_����t��
    public float bossAttack1_LifeTime;//Boss����1_�ͦs�ɶ�

    [Header("Boss ����2")]
    public float bossAttack2_Damge;//Boss����2_�ˮ`
    public int bossAttack2_RepelDirection;//Boss����2_���h��V(0:���h 1:����)
    public float bossAttack2_RepelDistance;//Boss����2_���h/�����Z��    
    public string bossAttack2_Effect;//Boss����2_�ĪG(�����̼��񪺰ʵe�W��)
    public float bossAttack2_FloatSpeed;//Boss����2_����t��
    public float bossAttack2_LifeTime;//Boss����2_�ͦs�ɶ�

    [Header("Boss ����3")]
    public float bossAttack3_Damge;//Boss����3_�ˮ`
    public int bossAttack3_RepelDirection;//Boss����3_���h��V(0:���h 1:����)
    public float bossAttack3_RepelDistance;//Boss����3_���h/�����Z��    
    public string bossAttack3_Effect;//Boss����3_�ĪG(�����̼��񪺰ʵe�W��)        
    public float bossAttack3_ForwardDistance;//Boss����3_�����d�򤤤��I�Z������e��
    public float bossAttack3_attackRadius;//Boss����3_�����b�|
    public bool bossAttack3_IsAttackBehind;//Boss����3_�O�_�����I��ĤH
    #endregion

    /// <summary>
    /// �غc�l
    /// </summary>
    private GameData_NumericalValue()
    {
        //�@�q
        gravity = 6.8f;//���O
        criticalBonus = 1.3f;//�����ˮ`�[��
        levelNames = new string[] { "�Ĥ@�� : ��d�x", "�̲׳� : �O�s��" };//���d�W��

        //Buff�W�[�ƭ�
        buffAbleString = new string[] { "�ͩR", "�ˮ`", "���m", "�l��", "����", "�^��" };//Buff�W�q��r
        buffAbleValue = new float[] { 30, 25, 25, 20, 25, 1 };//Buff�W�q�ƭ�(%)

        //��v��
        distance = 2.6f;//�P���a�Z��        
        limitUpAngle = 35;//����V�W����
        limitDownAngle = 13;//����V�U����
        //cameraAngle = 20;//��v������

        //���a
        playerHp = 850;//���a�ͩR��850
        playerMoveSpeed = 6.5f;//���a���ʳt��  6.5
        playerJumpForce = 11.05f;//���a���D�O
        playerCriticalRate = 15;//���a�����v
        playerDodgeSeppd = 6.3f;//���a�{���t��
        playerSelfHealTime = 5;//���a�ۨ��^�_�ɶ�(��)

        //���I
        strongholdHp = 350;//���IHP350

        //�P���h�LHP
        allianceSoldier1_Hp = 40;//�P���h�L1_�ͩR��

        //�ĤHHP
        boss_Hp = 10;//Boss_�ͩR��
        enemySoldier1_Hp = 80;//���Y�H_�ͩR��
        enemySoldier2_Hp = 60;//�}�b��_�ͩR��
        enemySoldier3_Hp = 40;//�ĤH�h�L3_�ͩR��
        guardBoss_Hp = 600;//�����u��Boss_�ͩR��600

        #region �Ԥh
        //�Ԥh ���q����1
        warriorNormalAttack_1_Damge = 30;//�Ԥh���q����1_�ˮ`
        warriorNormalAttack_1_RepelDirection = 0;//�Ԥh���q����1_���h��V(0:���h 1:����)
        warriorNormalAttack_1_RepelDistance = 30;//�Ԥh���q����1_���h/�����Z��    
        warriorNormalAttack_1_Effect = "Pain";//�Ԥh���q����1_�ĪG(�����̼��񪺰ʵe�W��)            
        warriorNormalAttack_1_ForwardDistance = 1.3f;//�Ԥh���q����1_�����d�򤤤��I�Z������e��
        warriorNormalAttack_1_attackRadius = 1.5f;//�Ԥh���q����1_�����b�|    
        warriorNormalAttack_1_IsAttackBehind = false;//�Ԥh���q����1_�O�_�����I��ĤH

        //�Ԥh ���q����2
        warriorNormalAttack_2_Damge = 33;//�Ԥh���q����1_�ˮ`
        warriorNormalAttack_2_RepelDirection = 0;//�Ԥh���q����1_���h��V(0:���h 1:����)
        warriorNormalAttack_2_RepelDistance = 30;//�Ԥh���q����1_���h/�����Z��    
        warriorNormalAttack_2_Effect = "Pain";//�Ԥh���q����1_�ĪG(�����̼��񪺰ʵe�W��)            
        warriorNormalAttack_2_ForwardDistance = 1.3f;//�Ԥh���q����1_�����d�򤤤��I�Z������e��
        warriorNormalAttack_2_attackRadius = 1.5f;//�Ԥh���q����1_�����b�|    
        warriorNormalAttack_2_IsAttackBehind = false;//�Ԥh���q����1_�O�_�����I��ĤH

        //�Ԥh ���q����3
        warriorNormalAttack_3_Damge = 36;//�Ԥh���q����1_�ˮ`
        warriorNormalAttack_3_RepelDirection = 0;//�Ԥh���q����1_���h��V(0:���h 1:����)
        warriorNormalAttack_3_RepelDistance = 45;//�Ԥh���q����1_���h/�����Z��    
        warriorNormalAttack_3_Effect = "Pain";//�Ԥh���q����1_�ĪG(�����̼��񪺰ʵe�W��)            
        warriorNormalAttack_3_ForwardDistance = 0.0f;//�Ԥh���q����1_�����d�򤤤��I�Z������e��
        warriorNormalAttack_3_attackRadius = 3.5f;//�Ԥh���q����1_�����b�|    
        warriorNormalAttack_3_IsAttackBehind = true;//�Ԥh���q����1_�O�_�����I��ĤH

        //�Ԥh ���D����
        warriorJumpAttack_Damage = 37;//�Ԥh���D����_�ˮ`
        warriorJumpAttack_RepelDirection = 0;//�Ԥh���D����_���h��V(0:���h 1:����)
        warriorJumpAttac_kRepelDistance = 50;//�Ԥh���D����_���h�Z��
        warriorJumpAttack_Effect = "Pain";//�Ԥh���D�����ĪG(�����̼��񪺰ʵe�W��)
        warriorJumpAttack_ForwardDistance = 0.77f;//�Ԥh���q����3_�����d�򤤤��I�Z������e��
        warriorJumpAttack_attackRadius = 1.3f;//�Ԥh���q����3_�����b�|
        warriorJumpAttack_IsAttackBehind = true;//�Ԥh���q����3_�O�_�����I��ĤH

        //�Ԥh �ޯ����1
        warriorSkillAttack_1_Damge = 50;//�Ԥh�ޯ����2_�ˮ`
        warriorSkillAttack_1_RepelDirection = 0;//�Ԥh�ޯ����2_���h��V(0:���h 1:����)
        warriorSkillAttack_1_RepelDistance = 40;//�Ԥh�ޯ����2_���h/�����Z��    
        warriorSkillAttack_1_Effect = "Pain";//�Ԥh���ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
        warriorSkillAttack_1_ForwardDistance = 1.6f;//�Ԥh�ޯ����2_�����d�򤤤��I�Z������e��
        warriorSkillAttack_1_attackRadius = 1.65f;//�Ԥh�ޯ����2_�����b�|
        warriorSkillAttack_1_IsAttackBehind = false;//�Ԥh�ޯ����2_�O�_�����I��ĤH

        //�Ԥh �ޯ����2
        warriorSkillAttack_2_Damge = 28;//�Ԥh�ޯ����2_�ˮ`
        warriorSkillAttack_2_RepelDirection = 0;//�Ԥh�ޯ����2_���h��V(0:���h 1:����)
        warriorSkillAttack_2_RepelDistance = 25;//�Ԥh�ޯ����2_���h/�����Z��    
        warriorSkillAttack_2_Effect = "Pain";//�Ԥh���ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
        warriorSkillAttack_2_ForwardDistance = 1.3f;//�Ԥh�ޯ����2_�����d�򤤤��I�Z������e��
        warriorSkillAttack_2_attackRadius = 1.5f;//�Ԥh�ޯ����2_�����b�|
        warriorSkillAttack_2_IsAttackBehind = false;//�Ԥh�ޯ����2_�O�_�����I��ĤH

        //�Ԥh �ޯ����3
        warriorSkillAttack_3_Damge = new float[] { 16, 16, 35 };//�Ԥh�ޯ����3_�ˮ`
        warriorSkillAttack_3_RepelDirection = new int[] { 0, 0, 0 };//�Ԥh�ޯ����3_���h��V(0:���h 1:����)
        warriorSkillAttack_3_RepelDistance = new float[] { 25, 25, 30 };//�Ԥh�ޯ����3_���h/�����Z��    
        warriorSkillAttack_3_Effect = new string[] { "Pain", "Pain", "Pain" };//�Ԥh���ޯ����3_�ĪG(�����̼��񪺰ʵe�W��)        
        warriorSkillAttack_3_ForwardDistance = new float[] { 1.3f, 1.3f, 0 };//�Ԥh�ޯ����3_�����d�򤤤��I�Z������e��
        warriorSkillAttack_3_attackRadius = new float[] { 1.45f, 1.45f, 5f };//�Ԥh�ޯ����3_�����b�|
        warriorSkillAttack_3_IsAttackBehind = new bool[] { false, false, true };//�Ԥh�ޯ����3_�O�_�����I��ĤH
        #endregion

        #region �}�b��
        //�}�b�� ���q����
        archerNormalAttack_Damge = new float[] { 11, 11, 16 };//�}�b�ⴶ�q����_�ˮ`
        archerNormalAttack_RepelDirection = new int[] { 0, 0, 0 };//�}�b�ⴶ�q����_���h��V(0:���h 1:����)
        archerNormalAttack_RepelDistance = new float[] { 10, 10, 10 };//�}�b�ⴶ�q����_���h/�����Z��        
        archerNormalAttack_Effect = new string[] { "Pain", "Pain", "Pain" };//�}�b�ⴶ�q����_�ĪG(�����̼��񪺰ʵe�W��)     
        archerNormalAttack_FloatSpeed = new float[] { 30, 30, 30 };//�}�b�ⴶ�q��������t��
        archerNormalAttack_LifeTime = new float[] { 0.6f, 0.6f, 0.6f };//�}�b�ⴶ�q�����ͦs�ɶ�

        //�}�b�� ���D����
        archerJumpAttack_Damage = 36;//�}�b����D�����ˮ`
        archerJumpAttack_RepelDirection = 0;//�}�b����D�������h��V(0:���h 1:����)
        archerJumpAttack_RepelDistance = 25;//�}�b����D���� ���h�Z��
        archerJumpAttack_Effect = "Pain";//�}�b����D�����ĪG(�����̼��񪺰ʵe�W��)
        archerJumpAttack_ForwardDistance = 0;//�}�b����D����_�����d�򤤤��I�Z������e��
        archerJumpAttack_attackRadius = 1.4f;//�}�b����D����_�����b�|
        archerJumpAttack_IsAttackBehind = false;//�}�b����D����_�O�_�����I��ĤH

        //�}�b�� �ޯ����1
        archerSkillAttack_1_Damage = 11;//�}�b��ޯ����1_�ˮ`
        archerSkillAttack_1_RepelDirection = 12;//�}�b��ޯ����1_���h��V(0:���h 1:����)
        archerSkillAttack_1_Repel = 13;//�}�b��ޯ����1_���h�Z��        
        archerSkillAttack_1_Effect = "Pain";//�}�b��ޯ����1_�ĪG(�����̼��񪺰ʵe�W��)
        archerSkillAttack_1_FlightSpeed = 30;//�}�b��ޯ����1_����t��
        archerSkillAttack_1_LifeTime = 0.4f;//�}�b��ޯ����1_�ͦs�ɶ�

        //�}�b�� �ޯ����2
        archerSkillAttack_2_Damge = 35;//�}�b��ޯ����2_�ˮ`
        archerSkillAttack_2_RepelDirection = 0;//�}�b��ޯ����2_���h��V(0:���h 1:����)
        archerSkillAttack_2_RepelDistance = 30;//�}�b��ޯ����2_���h/�����Z��    
        archerSkillAttack_2_Effect = "Pain";//�}�b��ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
        archerSkillAttack_2_ForwardDistance = 1.3f;//�}�b��ޯ����2_�����d�򤤤��I�Z������e��
        archerSkillAttack_2_attackRadius = 1.4f;//�}�b��ޯ����2_�����b�|
        archerSkillAttack_2_IsAttackBehind = false;//�}�b��ޯ����2_�O�_�����I��ĤH

        //�}�b�� �ޯ����3
        archerSkillAttack_3_Damge = 13;//�}�b��ޯ����3_�ˮ`
        archerSkillAttack_3_RepelDirection = 1;//�}�b��ޯ����3_���h��V(0:���h 1:����)
        archerSkillAttack_3_RepelDistance = 0;//�}�b��ޯ����3_���h/�����Z��    
        archerSkillAttack_3_Effect = "Pain";//�}�b��ޯ����3_�ĪG(�����̼��񪺰ʵe�W��)        
        archerSkillAttack_3_ForwardDistance = 0;//�}�b��ޯ����3_�����d�򤤤��I�Z������e��
        archerSkillAttack_3_attackRadius = 5.0f;//�}�b��ޯ����3_�����b�|
        archerSkillAttack_3_IsAttackBehind = true;//�}�b��ޯ����3_�O�_�����I��ĤH
        #endregion

        #region �k�v
        //�k�v���q����1
        magicianNormalAttack_1_Damage = 9;//�k�v���q����1_�ˮ`
        magicianNormalAttack_1_RepelDirection = 0;//�k�v���q����1_���h��V(0:���h 1:����)
        magicianNormalAttack_1_Repel = 5;//�k�v���q����1_���h�Z��        
        magicianNormalAttack_1_Effect = "Pain";//�k�v���q����1_�ĪG(�����̼��񪺰ʵe�W��)
        magicianNormalAttack_1_FlightSpeed = 40;//�k�v���q����1_����t��
        magicianNormalAttack_1_LifeTime = 1f;//�k�v���q����1_�ͦs�ɶ�

        //�k�v���q����2
        magicianNormalAttack_2_Damge = 7;//�k�v���q����2_�ˮ`
        magicianNormalAttack_2_RepelDirection = 0;//�k�v���q����2_���h��V(0:���h 1:����)
        magicianNormalAttack_2_RepelDistance = 7;//�k�v���q����2_���h/�����Z��    
        magicianNormalAttack_2_Effect = "Pain";//�k�v���q����2_�ĪG(�����̼��񪺰ʵe�W��)        
        magicianNormalAttack_2_ForwardDistance = 8.0f;//�k�v���q����2_�����d�򤤤��I�Z������e��
        magicianNormalAttack_2_attackRange = new Vector3(1, 1f, 16);//�k�v���q����2_�����d��
        magicianNormalAttack_2_IsAttackBehind = false;//�k�v���q����2_�O�_�����I��ĤH

        //�k�v���q����3
        magicianNormalAttack_3_Damge = 14;//�k�v���q����3_�ˮ`
        magicianNormalAttack_3_RepelDirection = 0;//�k�v���q����3_���h��V(0:���h 1:����)
        magicianNormalAttack_3_RepelDistance = 5f;//�k�v���q����3_���h/�����Z��    
        magicianNormalAttack_3_Effect = "Pain";//�k�v���q����3_�ĪG(�����̼��񪺰ʵe�W��)        
        magicianNormalAttack_3_ForwardDistance = 0;//�k�v���q����3_�����d�򤤤��I�Z������e��
        magicianNormalAttack_3_attackRadius = 8.0f;//�k�v���q����3_�����b�|
        magicianNormalAttack_3_IsAttackBehind = true;//�k�v���q����3_�O�_�����I��ĤH

        //�k�v���D����
        magicianJumpAttack_Damage = 37;//�k�v���D����_�ˮ`
        magicianJumpAttack_RepelDirection = 0;//�k�v���D����_���h��V(0:���h 1:����)  
        magicianJumpAttack_RepelDistance = 40;//�k�v���D����_���h�Z��
        magicianJumpAttack_Effect = "Pain";//�k�v���D����_�ĪG(�����̼��񪺰ʵe�W��)
        magicianJumpAttack_ForwardDistance = 0;//�k�v���D����_�����d�򤤤��I�Z������e��
        magicianJumpAttack_attackRadius = 1.3f;//�k�v���D����_�����b�|
        magicianJumpAttack_IsAttackBehind = true;//�k�v���D����_�O�_�����I��ĤH

        //�k�v�ޯ����1
        magicianSkillAttack_1_HealValue = 8;//�k�v���q����1_�v���q(%)    
        magicianSkillAttack_1_ForwardDistance = 0;//�k�v���q����1_�v���d�򤤤��I�Z������e��
        magicianSkillAttack_1_attackRange = 8;//�k�v���q����1_�v���b�|
        magicianSkillAttack_1_IsAttackBehind = true;//�k�v���q����1_�O�_�v���I�����

        /*//�k�v�ޯ����2
        magicianSkillAttack_2_Damge = new float[] { 4, 4, 5, 4, 5, 4, 4, 7, 9, 12};//�k�v�ޯ����2_�ˮ`
        magicianSkillAttack_2_RepelDirection = new int[] { 0, 0, 0, 0 ,0, 0, 0, 0, 0, 0};//�k�v�ޯ����2_���h��V(0:���h 1:����)
        magicianSkillAttack_2_RepelDistance = new float[] { 25, 5, 5, 5, 5, 5, 5, 15, 10, 65};//�k�v�ޯ����2_���h/�����Z��    
        magicianSkillAttack_2_Effect = new string[] { "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain", "Pain" };//�k�v�ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
        magicianSkillAttack_2_ForwardDistance = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};//�k�v�ޯ����2_�����d�򤤤��I�Z������e��
        magicianSkillAttack_2_attackRadius = new float[] { 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f};//�k�v�ޯ����2_�����b�|
        magicianSkillAttack_2_IsAttackBehind = new bool[] { false, false, false, false, false, false, false, false, false, false };//�k�v�ޯ����2_�O�_�����I��ĤH*/

        //�k�v�ޯ����2
        magicianSkillAttack_2_Damge = 32;//�k�v�ޯ����2_�ˮ`
        magicianSkillAttack_2_RepelDirection = 0;//�k�v�ޯ����2_���h��V(0:���h 1:����)
        magicianSkillAttack_2_RepelDistance = 10;//�k�v�ޯ����2_���h/�����Z��    
        magicianSkillAttack_2_Effect = "Pain";//�k�v�ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
        magicianSkillAttack_2_ForwardDistance = 5;//�k�v�ޯ����2_�����d�򤤤��I�Z������e��
        magicianSkillAttack_2_attackRadius = 10.0f;//�k�v�ޯ����2_�����b�|
        magicianSkillAttack_2_IsAttackBehind = false;//�k�v�ޯ����2_�O�_�����I��ĤH

        //�k�v�ޯ����3
        magicianSkillAttack_3_Damge = 35;//�k�v�ޯ����3_�ˮ`
        magicianSkillAttack_3_RepelDirection = 0;//�k�v�ޯ����3_���h��V(0:���h 1:����)
        magicianSkillAttack_3_RepelDistance = 15;//�k�v�ޯ����3_���h/�����Z��    
        magicianSkillAttack_3_Effect = "Pain";//�k�v�ޯ����3_�ĪG(�����̼��񪺰ʵe�W��)        
        magicianSkillAttack_3_ForwardDistance = 5;//�k�v�ޯ����3_�����d�򤤤��I�Z������e��
        magicianSkillAttack_3_attackRadius = 10.0f;//�k�v�ޯ����3_�����b�|
        magicianSkillAttack_3_IsAttackBehind = true;//�k�v�ޯ����3_�O�_�����I��ĤH
        #endregion

        #region �ĤH�h�L1(���Y�H)
        //�ĤH�h�L1 ����1
        enemySoldier1_Attack_1_Damge = 13;//�ĤH�h�L1_����1_�ˮ`
        enemySoldier1_Attack_1_RepelDirection = 0;//�ĤH�h�L1_����1_���h��V(0:���h 1:����)
        enemySoldier1_Attack_1_RepelDistance = 0;//�ĤH�h�L1_����1_���h/�����Z��    
        enemySoldier1_Attack_1_Effect = "Pain";//�ĤH�h�L1_����1_�ĪG(�����̼��񪺰ʵe�W��)        
        enemySoldier1_Attack_1_ForwardDistance = 0;//�ĤH�h�L1_����1_�����d�򤤤��I�Z������e��
        enemySoldier1_Attack_1_attackRadius = 1.3f;//�ĤH�h�L1_����1_�����b�|
        enemySoldier1_Attack_1_IsAttackBehind = true;//�ĤH�h�L1_����1_�O�_�����I��ĤH

        //�ĤH�h�L1 ����2
        enemySoldier1_Attack_2_Damge = 9;//�ĤH�h�L1_����2_�ˮ`
        enemySoldier1_Attack_2_RepelDirection = 0;//�ĤH�h�L1_����2_���h��V(0:���h 1:����)
        enemySoldier1_Attack_2_RepelDistance = 0;//�ĤH�h�L1_����2_���h/�����Z��    
        enemySoldier1_Attack_2_Effect = "Pain";//�ĤH�h�L1_����2_�ĪG(�����̼��񪺰ʵe�W��)        
        enemySoldier1_Attack_2_ForwardDistance = 1.4f;//�ĤH�h�L1_����2_�����d�򤤤��I�Z������e��
        enemySoldier1_Attack_2_attackRadius = 1.3f;//�ĤH�h�L1_����2_�����b�|
        enemySoldier1_Attack_2_IsAttackBehind = false;//�ĤH�h�L1_����2_�O�_�����I��ĤH

        //�ĤH�h�L1 ����3
        enemySoldier1_Attack_3_Damge = 6;//�ĤH�h�L1_����3_�ˮ`
        enemySoldier1_Attack_3_RepelDirection = 0;//�ĤH�h�L1_����3_���h��V(0:���h 1:����)
        enemySoldier1_Attack_3_RepelDistance = 0;//�ĤH�h�L1_����3_���h/�����Z��    
        enemySoldier1_Attack_3_Effect = "Pain";//�ĤH�h�L1_����3_�ĪG(�����̼��񪺰ʵe�W��)        
        enemySoldier1_Attack_3_ForwardDistance = 1.4f;//�ĤH�h�L1_����3_�����d�򤤤��I�Z������e��
        enemySoldier1_Attack_3_attackRadius = 1.3f;//�ĤH�h�L1_����3_�����b�|
        enemySoldier1_Attack_3_IsAttackBehind = false;//�ĤH�h�L1_����3_�O�_�����I��ĤH
        #endregion

        #region �ĤH�h�L2(�}�b��)
        //�ĤH�h�L2 ����1
        enemySoldier2_Attack1_Damge = 5;//�ĤH�h�L2_����1_�ˮ`
        enemySoldier2_Attack1_RepelDirection = 0;//�ĤH�h�L2_����1_���h��V(0:���h 1:����)
        enemySoldier2_Attack1_RepelDistance = 0;//�ĤH�h�L2_����1_���h/�����Z��    
        enemySoldier2_Attack1_Effect = "Pain";//�ĤH�h�L2_����1_�ĪG(�����̼��񪺰ʵe�W��)
        enemySoldier2_Attack1_FloatSpeed = 30;//�ĤH�h�L2_����1_����t��
        enemySoldier2_Attack1_LifeTime = 0.45f;//�ĤH�h�L2_����1_�ͦs�ɶ�

        //�ĤH�h�L2 ����2
        enemySoldier2_Attack2_Damge = 5;//�ĤH�h�L2_����2_�ˮ`
        enemySoldier2_Attack2_RepelDirection = 0;//�ĤH�h�L2_����2_���h��V(0:���h 1:����)
        enemySoldier2_Attack2_RepelDistance = 0;//�ĤH�h�L2_����2_���h/�����Z��    
        enemySoldier2_Attack2_Effect = "Pain";//�ĤH�h�L2_����2_�ĪG(�����̼��񪺰ʵe�W��)
        enemySoldier2_Attack2_FloatSpeed = 30;//�ĤH�h�L2_����2_����t��
        enemySoldier2_Attack2_LifeTime = 0.45f;//�ĤH�h�L2_����2_�ͦs�ɶ�

        //�ĤH�h�L2 ����3
        enemySoldier2_Attack_3_Damge = 9;//�ĤH�h�L2_����3_�ˮ`
        enemySoldier2_Attack_3_RepelDirection = 0;//�ĤH�h�L2_����3_���h��V(0:���h 1:����)
        enemySoldier2_Attack_3_RepelDistance = 0;//�ĤH�h�L2_����3_���h/�����Z��    
        enemySoldier2_Attack_3_Effect = "Pain";//�ĤH�h�L2_����3_�ĪG(�����̼��񪺰ʵe�W��)        
        enemySoldier2_Attack_3_ForwardDistance = 1.4f;//�ĤH�h�L2_����3_�����d�򤤤��I�Z������e��
        enemySoldier2_Attack_3_attackRadius = 1.3f;//�ĤH�h�L2_����3_�����b�|
        enemySoldier2_Attack_3_IsAttackBehind = false;//�ĤH�h�L2_����3_�O�_�����I��ĤH
        #endregion

        #region �ĤH�h�L3(���Y�H)
        //�ĤH�h�L3 ����1
        enemySoldier3_Attack_1_Damge = 15;//�ĤH�h�L3_����1_�ˮ`
        enemySoldier3_Attack_1_RepelDirection = 0;//�ĤH�h�L3_����1_���h��V(0:���h 1:����)
        enemySoldier3_Attack_1_RepelDistance = 0;//�ĤH�h�L3_����1_���h/�����Z��    
        enemySoldier3_Attack_1_Effect = "Pain";//�ĤH�h�L3_����1_�ĪG(�����̼��񪺰ʵe�W��)        
        enemySoldier3_Attack_1_ForwardDistance = 0;//�ĤH�h�L3_����1_�����d�򤤤��I�Z������e��
        enemySoldier3_Attack_1_attackRadius = 1.5f;//�ĤH�h�L3_����1_�����b�|
        enemySoldier3_Attack_1_IsAttackBehind = true;//�ĤH�h�L3_����1_�O�_�����I��ĤH

        //�ĤH�h�L3 ����2
        enemySoldier3_Attack_2_Damge = 11;//�ĤH�h�L3_����2_�ˮ`
        enemySoldier3_Attack_2_RepelDirection = 0;//�ĤH�h�L3_����2_���h��V(0:���h 1:����)
        enemySoldier3_Attack_2_RepelDistance = 0;//�ĤH�h�L3_����2_���h/�����Z��    
        enemySoldier3_Attack_2_Effect = "Pain";//�ĤH�h�L3_����2_�ĪG(�����̼��񪺰ʵe�W��)        
        enemySoldier3_Attack_2_ForwardDistance = 1.4f;//�ĤH�h�L3_����2_�����d�򤤤��I�Z������e��
        enemySoldier3_Attack_2_attackRadius = 1.3f;//�ĤH�h�L3_����2_�����b�|
        enemySoldier3_Attack_2_IsAttackBehind = true;//�ĤH�h�L3_����2_�O�_�����I��ĤH

        //�ĤH�h�L3 ����3
        enemySoldier3_Attack_3_Damge = 10;//�ĤH�h�L3_����3_�ˮ`
        enemySoldier3_Attack_3_RepelDirection = 0;//�ĤH�h�L3_����3_���h��V(0:���h 1:����)
        enemySoldier3_Attack_3_RepelDistance = 0;//�ĤH�h�L3_����3_���h/�����Z��    
        enemySoldier3_Attack_3_Effect = "Pain";//�ĤH�h�L3_����3_�ĪG(�����̼��񪺰ʵe�W��)        
        enemySoldier3_Attack_3_ForwardDistance = 1.4f;//�ĤH�h�L3_����3_�����d�򤤤��I�Z������e��
        enemySoldier3_Attack_3_attackRadius = 1.3f;//�ĤH�h�L3_����3_�����b�|
        enemySoldier3_Attack_3_IsAttackBehind = false;//�ĤH�h�L3_����3_�O�_�����I��ĤH
        #endregion

        #region �u��Boss
        //�u��Boss ����1
        guardBoss_Attack_1_Damge = 17;//�u��Boss_����1_�ˮ`
        guardBoss_Attack_1_RepelDirection = 0;//�u��Boss_����1_���h��V(0:���h 1:����)
        guardBoss_Attack_1_RepelDistance = 0;//�u��Boss_����1_���h/�����Z��    
        guardBoss_Attack_1_Effect = "Pain";//�u��Boss_����1_�ĪG(�����̼��񪺰ʵe�W��)        
        guardBoss_Attack_1_ForwardDistance = 1.4f;//�u��Boss_����1_�����d�򤤤��I�Z������e��
        guardBoss_Attack_1_attackRadius = 1.3f;//�u��Boss_����1_�����b�|
        guardBoss_Attack_1_IsAttackBehind = false;//�u��Boss_����1_�O�_�����I��ĤH

        //�u��Boss ����2
        guardBoss_Attack_2_Damge = 17;//�u��Boss_����2_�ˮ`
        guardBoss_Attack_2_RepelDirection = 0;//�u��Boss_����2_���h��V(0:���h 1:����)
        guardBoss_Attack_2_RepelDistance = 0;//�u��Boss_����2_���h/�����Z��    
        guardBoss_Attack_2_Effect = "Pain";//�u��Boss_����2_�ĪG(�����̼��񪺰ʵe�W��)        
        guardBoss_Attack_2_ForwardDistance = 1.4f;//�u��Boss_����2_�����d�򤤤��I�Z������e��
        guardBoss_Attack_2_attackRadius = 1.3f;//�u��Boss_����2_�����b�|
        guardBoss_Attack_2_IsAttackBehind = false;//�u��Boss_����2_�O�_�����I��ĤH

        //�u��Boss ����3
        guardBoss_Attack_3_Damge = 35;//�u��Boss_����3_�ˮ`
        guardBoss_Attack_3_RepelDirection = 0;//�u��Boss_����3_���h��V(0:���h 1:����)
        guardBoss_Attack_3_RepelDistance = 0;//�u��Boss_����3_���h/�����Z��    
        guardBoss_Attack_3_Effect = "Pain";//�u��Boss_����3_�ĪG(�����̼��񪺰ʵe�W��)        
        guardBoss_Attack_3_ForwardDistance = 1.4f;//�u��Boss_����3_�����d�򤤤��I�Z������e��
        guardBoss_Attack_3_attackRadius = 1.3f;//�u��Boss_����3_�����b�|
        guardBoss_Attack_3_IsAttackBehind = false;//�u��Boss_����3_�O�_�����I��ĤH

        //�u��Boss ����4
        guardBoss_Attack_4_Damge = 33;//�u��Boss_����4_�ˮ`
        guardBoss_Attack_4_RepelDirection = 0;//�u��Boss_����4_���h��V(0:���h 1:����)
        guardBoss_Attack_4_RepelDistance = 0;//�u��Boss_����4_���h/�����Z��    
        guardBoss_Attack_4_Effect = "Pain";//�u��Boss_����4_�ĪG(�����̼��񪺰ʵe�W��)        
        guardBoss_Attack_4_ForwardDistance = 1.4f;//�u��Boss1_����4_�����d�򤤤��I�Z������e��
        guardBoss_Attack_4_attackRadius = 1.3f;//�u��Boss_����4_�����b�|
        guardBoss_Attack_4_IsAttackBehind = false;//�u��Boss_����4_�O�_�����I��ĤH
        #endregion

        #region Boss
        //Boss ����1
        bossAttack1_Damge = 55;//Boss����1_�ˮ`
        bossAttack1_RepelDirection = 0;//Boss����1_���h��V(0:���h 1:����)
        bossAttack1_RepelDistance = 0;//Boss����1_���h/�����Z��    
        bossAttack1_Effect = "Pain";//Boss����1_�ĪG(�����̼��񪺰ʵe�W��)
        bossAttack1_FloatSpeed = 30;//Boss����1_��������t��
        bossAttack1_LifeTime = 1.5f;//Boss����1_�ͦs�ɶ�

        //Boss ����2
        bossAttack2_Damge = 9;//Boss����2_�ˮ`
        bossAttack2_RepelDirection = 0;//Boss����2_���h��V(0:���h 1:����)
        bossAttack2_RepelDistance = 0;//Boss����2_���h/�����Z��    
        bossAttack2_Effect = "Pain";//Boss����2_�ĪG(�����̼��񪺰ʵe�W��)
        bossAttack2_FloatSpeed = 35;//Boss����2_��������t��
        bossAttack2_LifeTime = 0.85f;//Boss����2_�ͦs�ɶ�

        //Boss ����3
        bossAttack3_Damge = 48;//Boss����3_�ˮ`
        bossAttack3_RepelDirection = 0;//Boss����3_���h��V(0:���h 1:����)
        bossAttack3_RepelDistance = 0;//Boss����3_���h/�����Z��    
        bossAttack3_Effect = "Pain";//Boss����3_�ĪG(�����̼��񪺰ʵe�W��)        
        bossAttack3_ForwardDistance = 0;//Boss����3_�����d�򤤤��I�Z������e��
        bossAttack3_attackRadius = 10;//Boss����3_�����b�|
        bossAttack3_IsAttackBehind = true;//Boss����3_�O�_�����I��ĤH
        #endregion
}
}

/// <summary>
/// �C���ƭȤ���
/// </summary>
[CreateAssetMenu(fileName = "NumericalValue", menuName = "ScriptableObjects/NumericalValue", order = 1)]
public class ScriptableObject_NumericalValue : ScriptableObject
{
    public GameData_NumericalValue numericalValue;
}
