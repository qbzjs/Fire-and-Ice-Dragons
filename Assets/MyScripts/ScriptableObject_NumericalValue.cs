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

    [Header("�Ԥh ���q����")]
    public float[] warriorNormalAttackDamge;//�Ԥh���q�����ˮ`
    public float[] warriorNormalAttackRepelDistance;//�Ԥh���q���� ���h/�����Z��
    public int[] warriorNormalAttackRepelDirection;//�Ԥh���q�������h��V(0:���h 1:����)
    public string[] warriorNormalAttackEffect;//�Ԥh���q�����ĪG(�����̼��񪺰ʵe�W��)
    public Vector3[] warriorNormalAttackBoxSize;//�Ԥh���q������Size

    [Header("�Ԥh ���D����")]
    public float warriorJumpAttackDamage;//�Ԥh���D�����ˮ`
    public string warriorJumpAttackEffect;//�Ԥh���D�����ĪG(�����̼��񪺰ʵe�W��)
    public float warriorJumpAttackRepelDistance;//�Ԥh���D���� ���h�Z��
    public Vector3 warriorJumpAttackBoxSize;//�Ԥh���D������Size
    public int warriorJumpAttackRepelDirection;//�Ԥh���D�������h��V(0:���h 1:����)

    [Header("�Ԥh �ޯ����")]
    public float[] warriorSkillAttackDamage;//�Ԥh�ޯ�����ˮ`
    public string[] warriorSkillAttackEffect;//�Ԥh�ޯ���������ĪG(�����̼��񪺰ʵe�W��)    
    public int[] warriorSkillAttackRepelDirection;//�Ԥh�ޯ�������h��V(0:���h 1:����)
    public float[] warriorSkillAttackRepel;//�Ԥh�ޯ�������h�Z��    
    public Vector3[] warriorSkillAttackBoxSize;//�Ԥh�ޯ����������Size

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
        playerCriticalRate = 30;//���a�����v
        playerDodgeSeppd = 3;//���a�{���t��

        //�Ԥh ���q����
        warriorNormalAttackDamge = new float[] { 10, 10, 15 };//�Ԥh���q�����ˮ`
        warriorNormalAttackRepelDistance = new float[] { 50, 50, 50 };//�Ԥh���q���� ���h/�����Z��
        warriorNormalAttackRepelDirection = new int[] { 0, 0, 0 };//�Ԥh���q�������h��V(0:���h 1:����)
        warriorNormalAttackEffect = new string[] { "Pain", "Pain", "Pain" };//�Ԥh���q�����ĪG(�����̼��񪺰ʵe�W��)
        warriorNormalAttackBoxSize = new Vector3[] { new Vector3(1.5f, 1.5f, 1.5f), new Vector3(1.5f, 1.5f, 1.5f), new Vector3(1.5f, 1.5f, 1.5f) };//�Ԥh���q����������Size        

        //�Ԥh ���D����
        warriorJumpAttackDamage = 10;//�Ԥh���D�����ˮ`
        warriorJumpAttackEffect = "Pain";//�Ԥh���D�����ĪG(�����̼��񪺰ʵe�W��)
        warriorJumpAttackRepelDistance = 50;//�Ԥh���D���� ���h�Z��
        warriorJumpAttackBoxSize = new Vector3(1.5f, 1f, 1.5f);//�Ԥh���D����������Size
        warriorJumpAttackRepelDirection = 0;//�Ԥh���D�������h��V(0:���h 1:����)

        //�Ԥh �ޯ����
        warriorSkillAttackDamage = new float[] { 10, 20, 30 };//�Ԥh�ޯ�����ˮ`
        warriorSkillAttackEffect = new string[] { "Pain", "Pain", "Pain" };//�Ԥh�ޯ���������ĪG(�����̼��񪺰ʵe�W��)    
        warriorSkillAttackRepelDirection = new int[] { 0, 0, 0 };//�Ԥh�ޯ�������h��V(0:���h 1:����)
        warriorSkillAttackRepel = new float[] { 50, 50, 50};//�Ԥh�ޯ�������h�Z��        
        warriorSkillAttackBoxSize = new Vector3[] { new Vector3(1.5f, 1.5f, 1.5f), new Vector3(1.5f, 1.5f, 1.5f) , new Vector3(1.5f, 1.5f, 1.5f) };//�Ԥh�ޯ����������Size         

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
