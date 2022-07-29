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

    [Header("���a")]
    public float playerHp;//���a�ͩR��
    public float playerMoveSpeed;//���a���ʳt��
    public float playerJumpForce;//���a���D�O
    public float playerCriticalRate;//���a�����v
    public float playerDodgeSeppd;//���a�{���t��

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
    public float warriorSkillAttack_1_Damge;//�Ԥh�ޯ�1_�ˮ`
    public int warriorSkillAttack_1_RepelDirection;//�Ԥh�ޯ�1_���h��V(0:���h 1:����)
    public float warriorSkillAttack_1_RepelDistance;//�Ԥh�ޯ�1_���h/�����Z��    
    public string warriorSkillAttack_1_Effect;//�Ԥh�ޯ�1_�ĪG(�����̼��񪺰ʵe�W��)
    public float warriorSkillAttack_1_FlightSpeed;//�Ԥh�ޯ�1_����t��
    public float warriorSkillAttack_1_LifeTime;//�Ԥh�ޯ�1_�ͦs�ɶ�

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
    public string archerSkillAttack_1_Effect;//�}�b��ޯ��������1_�ĪG(�����̼��񪺰ʵe�W��)
    public float archerSkillAttack_1_FlightSpeed;//�}�b��ޯ��������1_����t��
    public float archerSkillAttack_1_LifeTime;//�}�b��ޯ��������1_�ͦs�ɶ�

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

    [Header("�ĤH")]
    public float enemyHp;//�u�`�h�L�ͩR��

    /// <summary>
    /// �غc�l
    /// </summary>
    private GameData_NumericalValue()
    {
        //�@�q
        gravity = 9.8f;//���O
        criticalBonus = 1.3f;//�����ˮ`�[��
        levelNames = new string[] { "Level[1]:�ڬO�Ĥ@��", "Level[2]:�ĤG���٨S��", "Level[3]:�ĤT���b��"};//���d�W��

        //Buff�W�[�ƭ�
        buffAbleString = new string[] { "�ͩR��", "�ˮ`", "���m", "����", "�l��", "�^��" };//Buff�W�q��r
        buffAbleValue = new float[] { 100, 10, 5, 5, 3, 1};//Buff�W�q�ƭ�

        //��v��
        distance = 2.6f;//�P���a�Z��        
        limitUpAngle = 35;//����V�W����
        limitDownAngle = 20;//����V�U����

        //���a
        playerHp = 300;//���a�ͩR��
        playerMoveSpeed = 5;//���a���ʳt��        
        playerJumpForce = 14.5f;//���a���D�O
        playerCriticalRate = 15;//���a�����v
        playerDodgeSeppd = 3;//���a�{���t��

        #region �Ԥh
        //�Ԥh ���q����1
        warriorNormalAttack_1_Damge = 10;//�Ԥh���q����1_�ˮ`
        warriorNormalAttack_1_RepelDirection = 0;//�Ԥh���q����1_���h��V(0:���h 1:����)
        warriorNormalAttack_1_RepelDistance = 50;//�Ԥh���q����1_���h/�����Z��    
        warriorNormalAttack_1_Effect = "Pain";//�Ԥh���q����1_�ĪG(�����̼��񪺰ʵe�W��)            
        warriorNormalAttack_1_ForwardDistance = 1.3f;//�Ԥh���q����1_�����d�򤤤��I�Z������e��
        warriorNormalAttack_1_attackRadius = 1.2f;//�Ԥh���q����1_�����b�|    
        warriorNormalAttack_1_IsAttackBehind = false;//�Ԥh���q����1_�O�_�����I��ĤH

        //�Ԥh ���q����2
        warriorNormalAttack_2_Damge = 11;//�Ԥh���q����1_�ˮ`
        warriorNormalAttack_2_RepelDirection = 0;//�Ԥh���q����1_���h��V(0:���h 1:����)
        warriorNormalAttack_2_RepelDistance = 50;//�Ԥh���q����1_���h/�����Z��    
        warriorNormalAttack_2_Effect = "Pain";//�Ԥh���q����1_�ĪG(�����̼��񪺰ʵe�W��)            
        warriorNormalAttack_2_ForwardDistance = 1.3f;//�Ԥh���q����1_�����d�򤤤��I�Z������e��
        warriorNormalAttack_2_attackRadius = 1.2f;//�Ԥh���q����1_�����b�|    
        warriorNormalAttack_2_IsAttackBehind = false;//�Ԥh���q����1_�O�_�����I��ĤH

        //�Ԥh ���q����3
        warriorNormalAttack_3_Damge = 12;//�Ԥh���q����1_�ˮ`
        warriorNormalAttack_3_RepelDirection = 0;//�Ԥh���q����1_���h��V(0:���h 1:����)
        warriorNormalAttack_3_RepelDistance = 50;//�Ԥh���q����1_���h/�����Z��    
        warriorNormalAttack_3_Effect = "Pain";//�Ԥh���q����1_�ĪG(�����̼��񪺰ʵe�W��)            
        warriorNormalAttack_3_ForwardDistance = 0.5f;//�Ԥh���q����1_�����d�򤤤��I�Z������e��
        warriorNormalAttack_3_attackRadius = 1.55f;//�Ԥh���q����1_�����b�|    
        warriorNormalAttack_3_IsAttackBehind = true;//�Ԥh���q����1_�O�_�����I��ĤH

        //�Ԥh ���D����
        warriorJumpAttack_Damage = 11;//�Ԥh���D����_�ˮ`
        warriorJumpAttack_RepelDirection = 0;//�Ԥh���D����_���h��V(0:���h 1:����)
        warriorJumpAttac_kRepelDistance = 50;//�Ԥh���D����_���h�Z��
        warriorJumpAttack_Effect = "Pain";//�Ԥh���D�����ĪG(�����̼��񪺰ʵe�W��)
        warriorJumpAttack_ForwardDistance = 0.77f;//�Ԥh���q����3_�����d�򤤤��I�Z������e��
        warriorJumpAttack_attackRadius = 1.0f;//�Ԥh���q����3_�����b�|
        warriorJumpAttack_IsAttackBehind = false;//�Ԥh���q����3_�O�_�����I��ĤH

        //�Ԥh �ޯ����1
        warriorSkillAttack_1_Damge = 9;//�Ԥh�ޯ�1_�ˮ`
        warriorSkillAttack_1_RepelDirection = 0;//�Ԥh�ޯ�1_���h��V(0:���h 1:����)
        warriorSkillAttack_1_RepelDistance = 50;//�Ԥh�ޯ�1_���h/�����Z��    
        warriorSkillAttack_1_Effect = "Pain";//�Ԥh�ޯ�1_�ĪG(�����̼��񪺰ʵe�W��)
        warriorSkillAttack_1_FlightSpeed = 20;//�Ԥh�ޯ�1_����t��
        warriorSkillAttack_1_LifeTime = 0.2f;//�Ԥh�ޯ�1_�ͦs�ɶ�

        //�Ԥh �ޯ����2
        warriorSkillAttack_2_Damge = 13;//�Ԥh�ޯ����2_�ˮ`
        warriorSkillAttack_2_RepelDirection = 0;//�Ԥh�ޯ����2_���h��V(0:���h 1:����)
        warriorSkillAttack_2_RepelDistance = 50;//�Ԥh�ޯ����2_���h/�����Z��    
        warriorSkillAttack_2_Effect = "Pain";//�Ԥh���ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
        warriorSkillAttack_2_ForwardDistance = 1.3f;//�Ԥh�ޯ����2_�����d�򤤤��I�Z������e��
        warriorSkillAttack_2_attackRadius = 1.2f;//�Ԥh�ޯ����2_�����b�|
        warriorSkillAttack_2_IsAttackBehind = false;//�Ԥh�ޯ����2_�O�_�����I��ĤH

        //�Ԥh �ޯ����3
        warriorSkillAttack_3_Damge = new float[] { 11, 12, 15};//�Ԥh�ޯ����3_�ˮ`
        warriorSkillAttack_3_RepelDirection = new int[] { 0, 0, 1};//�Ԥh�ޯ����3_���h��V(0:���h 1:����)
        warriorSkillAttack_3_RepelDistance = new float[] { 50, 50, 15};//�Ԥh�ޯ����3_���h/�����Z��    
        warriorSkillAttack_3_Effect = new string[] { "Pain", "Pain", "Pain" };//�Ԥh���ޯ����3_�ĪG(�����̼��񪺰ʵe�W��)        
        warriorSkillAttack_3_ForwardDistance = new float[] { 0, 0, 0};//�Ԥh�ޯ����3_�����d�򤤤��I�Z������e��
        warriorSkillAttack_3_attackRadius = new float[] { 1.4f, 1.4f, 5};//�Ԥh�ޯ����3_�����b�|
        warriorSkillAttack_3_IsAttackBehind = new bool[] { true, true, true};//�Ԥh�ޯ����3_�O�_�����I��ĤH
        #endregion

        #region �}�b��
        //�}�b�� ���q����
        archerNormalAttack_Damge = new float[] { 11, 11, 16 };//�}�b�ⴶ�q����_�ˮ`
        archerNormalAttack_RepelDirection = new int[] { 0, 0, 0 };//�}�b�ⴶ�q����_���h��V(0:���h 1:����)
        archerNormalAttack_RepelDistance = new float[] { 30, 30, 30 };//�}�b�ⴶ�q����_���h/�����Z��        
        archerNormalAttack_Effect = new string[] { "Pain", "Pain", "Pain" };//�}�b�ⴶ�q����_�ĪG(�����̼��񪺰ʵe�W��)     
        archerNormalAttack_FloatSpeed = new float[] { 50, 50, 50};//�}�b�ⴶ�q��������t��
        archerNormalAttack_LifeTime = new float[] { 0.3f, 0.3f, 0.3f};//�}�b�ⴶ�q�����ͦs�ɶ�

        //�}�b�� ���D����
        archerJumpAttack_Damage = 11;//�}�b����D�����ˮ`
        archerJumpAttack_RepelDirection = 0;//�}�b����D�������h��V(0:���h 1:����)
        archerJumpAttack_RepelDistance = 50;//�}�b����D���� ���h�Z��
        archerJumpAttack_Effect = "Pain";//�}�b����D�����ĪG(�����̼��񪺰ʵe�W��)
        archerJumpAttack_ForwardDistance = 0;//�}�b����D����_�����d�򤤤��I�Z������e��
        archerJumpAttack_attackRadius = 1.2f;//�}�b����D����_�����b�|
        archerJumpAttack_IsAttackBehind = false;//�}�b����D����_�O�_�����I��ĤH

        //�}�b�� �ޯ����1
        archerSkillAttack_1_Damage = 11;//�}�b��ޯ��������1_�ˮ`
        archerSkillAttack_1_RepelDirection = 0;//�}�b��ޯ��������1_���h��V(0:���h 1:����)
        archerSkillAttack_1_Repel = 13;//�}�b��ޯ��������1_���h�Z��        
        archerSkillAttack_1_Effect = "Pain";//�}�b��ޯ��������1_�ĪG(�����̼��񪺰ʵe�W��)
        archerSkillAttack_1_FlightSpeed = 50;//�}�b��ޯ��������1_����t��
        archerSkillAttack_1_LifeTime = 0.3f;//�}�b��ޯ��������1_�ͦs�ɶ�

        //�}�b�� �ޯ����2
        archerSkillAttack_2_Damge = 11;//�}�b��ޯ����2_�ˮ`
        archerSkillAttack_2_RepelDirection = 0;//�}�b��ޯ����2_���h��V(0:���h 1:����)
        archerSkillAttack_2_RepelDistance = 50;//�}�b��ޯ����2_���h/�����Z��    
        archerSkillAttack_2_Effect = "Pain";//�}�b��ޯ����2_�ĪG(�����̼��񪺰ʵe�W��)        
        archerSkillAttack_2_ForwardDistance = 0;//�}�b��ޯ����2_�����d�򤤤��I�Z������e��
        archerSkillAttack_2_attackRadius = 1.3f;//�}�b��ޯ����2_�����b�|
        archerSkillAttack_2_IsAttackBehind = false;//�}�b��ޯ����2_�O�_�����I��ĤH

        //�}�b�� �ޯ����3
        archerSkillAttack_3_Damge = 5;//�}�b��ޯ����3_�ˮ`
        archerSkillAttack_3_RepelDirection = 1;//�}�b��ޯ����3_���h��V(0:���h 1:����)
        archerSkillAttack_3_RepelDistance = 0;//�}�b��ޯ����3_���h/�����Z��    
        archerSkillAttack_3_Effect = "Pain";//�}�b��ޯ����3_�ĪG(�����̼��񪺰ʵe�W��)        
        archerSkillAttack_3_ForwardDistance = 0;//�}�b��ޯ����3_�����d�򤤤��I�Z������e��
        archerSkillAttack_3_attackRadius = 5.0f;//�}�b��ޯ����3_�����b�|
        archerSkillAttack_3_IsAttackBehind = true;//�}�b��ޯ����3_�O�_�����I��ĤH
        #endregion

        //�ĤH
        enemyHp = 1000;//�u�`�h�L�ͩR��
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
