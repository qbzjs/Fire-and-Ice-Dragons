using UnityEngine;

/// <summary>
/// �C���ƭ�
/// </summary>
[System.Serializable]
public class GameData_NumericalValue
{
    [Header("�@�q")]
    public float gravity;//���O
    public float boxCollisionDistance;//�I���ضZ��(�P�𭱶Z��)
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

    [Header("�}�b�� ��¦�ƭ�")]
    public float arrowFloatSpeed;//�}�b����t��
    public float arrowLifeTime;//�}�b�ͦs�ɶ�

    [Header("�}�b�� ���q����")]
    public float[] archerNormalAttackDamge;//�}�b�ⴶ�q�����ˮ`
    public float[] archerNormalAttackRepelDistance;//�}�b�ⴶ�q���� ���h/�����Z��
    public int[] archerNormalAttackRepelDirection;//�}�b�ⴶ�q�������h��V(0:���h 1:����)
    public string[] archerNormalAttackEffect;//�}�b�ⴶ�q�����ĪG(�����̼��񪺰ʵe�W��)

    [Header("�}�b�� ���D����")]
    public float archerJumpAttackDamage;//�}�b����D�����ˮ`
    public string archerJumpAttackEffect;//�}�b����D�����ĪG(�����̼��񪺰ʵe�W��)
    public float archerJumpAttackRepelDistance;//�}�b����D���� ���h�Z��
    public Vector3 archerJumpAttackBoxSize;//�}�b����D������Size
    public int archerJumpAttackRepelDirection;//�}�b����D�������h��V(0:���h 1:����)  

    [Header("�}�b�� �ޯ����")]
    public float[] archerSkillAttackDamage;//�}�b��ޯ�����ˮ`
    public string[] archerSkillAttackEffect;//�}�b��ޯ���������ĪG(�����̼��񪺰ʵe�W��)    
    public int[] archerSkillAttackRepelDirection;//�}�b��ޯ�������h��V(0:���h 1:����)
    public float[] archerSkillAttackRepel;//�}�b��ޯ�������h�Z��    
    public Vector3[] archerSkillAttackBoxSize;//�}�b��ޯ����������Size

    [Header("�ĤH")]
    public float enemyHp;//�u�`�h�L�ͩR��

    /// <summary>
    /// �غc�l
    /// </summary>
    private GameData_NumericalValue()
    {
        //�@�q
        gravity = 9.8f;//���O
        boxCollisionDistance = 0.5f;//�I���ضZ��(�P�𭱶Z��)
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
        #endregion

    //�}�b�� ��¦�ƭ�
    arrowFloatSpeed = 20;//�}�b����t��
        arrowLifeTime = 1.5f;//�}�b�ͦs�ɶ�

        //�}�b�� ���q����
        archerNormalAttackDamge = new float[] { 11, 11, 16 };//�}�b�ⴶ�q�����ˮ`
        archerNormalAttackRepelDistance = new float[] { 50, 50, 50 };//�}�b�ⴶ�q���� ���h/�����Z��
        archerNormalAttackRepelDirection = new int[] { 0, 0, 0 };//�}�b�ⴶ�q�������h��V(0:���h 1:����)
        archerNormalAttackEffect = new string[] { "Pain", "Pain", "Pain" };//�}�b�ⴶ�q�����ĪG(�����̼��񪺰ʵe�W��)     

        //�}�b�� ���D����
        archerJumpAttackDamage = 11;//�}�b����D�����ˮ`
        archerJumpAttackEffect = "Pain";//�}�b����D�����ĪG(�����̼��񪺰ʵe�W��)
        archerJumpAttackRepelDistance = 50;//�}�b����D���� ���h�Z��
        archerJumpAttackBoxSize = new Vector3(1.5f, 1f, 1.5f);//�}�b����D����������Size
        archerJumpAttackRepelDirection = 0;//�}�b����D�������h��V(0:���h 1:����)

        //�}�b�� �ޯ����
        archerSkillAttackDamage = new float[] { 11, 12, 13 };//�}�b��ޯ�����ˮ`
        archerSkillAttackEffect = new string[] { "Pain", "Pain", "Pain" };//�}�b��ޯ���������ĪG(�����̼��񪺰ʵe�W��)    
        archerSkillAttackRepelDirection = new int[] { 0, 0, 0 };//�}�b��ޯ�������h��V(0:���h 1:����)
        archerSkillAttackRepel = new float[] { 50, 50, 50 };//�}�b��ޯ�������h�Z��        
        archerSkillAttackBoxSize = new Vector3[] { new Vector3(0, 0, 0), new Vector3(1.5f, 1.5f, 1.5f), new Vector3(5, 5, 5) };//�}�b��ޯ����������Size     

        //�ĤH
        enemyHp = 50;//�u�`�h�L�ͩR��
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
