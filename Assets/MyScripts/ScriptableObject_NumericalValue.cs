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
    public float[] warriorNormalAttackDamge;//���a���q�����ˮ`
    public float[] warriorNormalAttackMoveDistance;//���a���q���� ���ʶZ��
    public float[] warriorNormalAttackRepelDistance;//���a���q���� ���h/�����Z��
    public int[] warriorNormalAttackRepelDirection;//���a���q�������h��V(0:���h 1:����)
    public string[] warriorNormalAttackEffect;//���a���q�����ĪG(�����̼��񪺰ʵe�W��)
    public Vector3[] warriorNormalAttackBoxSize;//���a���q������Size

    [Header("�Ԥh ���D����")]
    public float warriorJumpAttackDamage;//���a���D�����ˮ`
    public string warriorJumpAttackEffect;//���a���D�����ĪG(�����̼��񪺰ʵe�W��)
    public float warriorJumpAttackRepelDistance;//���a���D���� ���h�Z��
    public Vector3 warriorJumpAttackBoxSize;//���a���D������Size
    public int warriorJumpAttackRepelDirection;//���a���D�������h��V(0:���h 1:����)

    [Header("�Ԥh �ޯ����")]
    public float[] warriorSkillAttackDamage;//�ޯ�����ˮ`
    public string[] warriorSkillAttackEffect;//�ޯ���������ĪG(�����̼��񪺰ʵe�W��)    
    public int[] warriorSkillAttackRepelDirection;//���a�ޯ�������h��V(0:���h 1:����)
    public float[] warriorSkillAttackRepel;//�ޯ�������h�Z��    
    public Vector3[] warriorSkillAttackBoxSize;//���a�ޯ����������Size

    [Header("���a �u�`�h�L")]
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
        warriorNormalAttackMoveDistance = new float[] { 50, 50, 50 };//�Ԥh�������ʶZ��
        warriorNormalAttackRepelDistance = new float[] { 120, 80, 60 };//�Ԥh���q���� ���h/�����Z��
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
        warriorSkillAttackDamage = new float[] { 10, 20, 30 };//�ޯ�����ˮ`
        warriorSkillAttackEffect = new string[] { "Pain", "Pain", "Pain" };//�ޯ���������ĪG(�����̼��񪺰ʵe�W��)    
        warriorSkillAttackRepelDirection = new int[] { 0, 0, 0 };//�ޯ�������h��V(0:���h 1:����)
        warriorSkillAttackRepel = new float[] { 50, 50, 50};//�ޯ�������h�Z��        
        warriorSkillAttackBoxSize = new Vector3[] { new Vector3(1.5f, 1.5f, 1.5f), new Vector3(1.5f, 1.5f, 1.5f) , new Vector3(1.5f, 1.5f, 1.5f) };//�ޯ����������Size         

        //�u�`�h�L
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
