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

    [Header("���a ���q����")]
    public float[] playerNormalAttackDamge;//���a���q�����ˮ`
    public float[] playerNormalAttackMoveDistance;//���a���q���� ���ʶZ��
    public float[] playerNormalAttackRepelDistance;//���a���q���� ���h/�����Z��
    public int[] playerNormalAttackRepelDirection;//���a���q�������h��V(0:���h 1:����)
    public string[] playerNormalAttackEffect;//���a���q�����ĪG(�����̼��񪺰ʵe�W��)
    public Vector3[] playerNormalAttackBoxSize;//���a���q������Size

    [Header("���a ���D����")]
    public float playerJumpAttackDamage;//���a���D�����ˮ`
    public string playerJumpAttackEffect;//���a���D�����ĪG(�����̼��񪺰ʵe�W��)
    public float playerJumpAttackRepelDistance;//���a���D���� ���h�Z��
    public Vector3 playerJumpAttackBoxSize;//���a���D������Size
    public int playerJumpAttackRepelDirection;//���a���D�������h��V(0:���h 1:����)

    [Header("���a �ޯ����_1")]
    public float playerSkillAttack_1_Damage;//�ޯ����_1_�����ˮ`
    public string playerSkillAttack_1_Effect;//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
    public float playerSkillAttack_1_FlyingSpeed;//�ޯ����_1_���󭸦�t��
    public float playerSkillAttack_1_LifeTime;//�ޯ����_1_�ͦs�ɶ�
    public float playerSkillAttack_1_Repel;//�ޯ����_1_���h�Z��
    public int playerSkillAttack_1_RepelDirection;//���a�ޯ����_1_���h��V(0:���h 1:����)

    [Header("���a �ޯ����_2")]
    public float playerSkillAttack_2_Damage;//�ޯ����_2_�����ˮ`
    public string playerSkillAttack_2_Effect;//�ޯ����_2_�����ĪG(�����̼��񪺰ʵe�W��)    
    public float playerSkillAttack_2_Repel;//�ޯ����_2_���h�Z��
    public int playerSkillAttack_2_RepelDirection;//���a�ޯ����_2_���h��V(0:���h 1:����)
    public Vector3 playerSkillAttack_2_BoxSize;//���a�ޯ����_2_������Size

    [Header("���a �ޯ����_3")]
    public float playerSkillAttack_3_Damage;//�ޯ����_3_�����ˮ`
    public string playerSkillAttack_3_Effect;//�ޯ����_3_�����ĪG(�����̼��񪺰ʵe�W��)    
    public float playerSkillAttack_3_Repel;//�ޯ����_3_���h�Z��
    public int playerSkillAttack_3_RepelDirection;//���a�ޯ����_3_���h��V(0:���h 1:����)
    public Vector3 playerSkillAttack_3_BoxSize;//���a�ޯ����_3_������Size

    [Header("���a �u�`�h�L")]
    public float skeletonSoldierHp;//�u�`�h�L�ͩR��

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
        playerMoveSpeed = 10;//���a���ʳt��        
        playerJumpForce = 16;//���a���D�O
        playerCriticalRate = 30;//���a�����v

        //���a ���q����
        playerNormalAttackDamge = new float[] { 10, 10, 15 };//���a���q�����ˮ`
        playerNormalAttackMoveDistance = new float[] { 50, 50, 0 };//���a���q�������ʶZ��
        playerNormalAttackRepelDistance = new float[] { 70, 80, 18 };//���a���q���� ���h/�����Z��
        playerNormalAttackRepelDirection = new int[] { 0, 0, 1 };//���a���q�������h��V(0:���h 1:����)
        playerNormalAttackEffect = new string[] { "Pain", "Pain", "Pain" };//���a���q�����ĪG(�����̼��񪺰ʵe�W��)
        playerNormalAttackBoxSize = new Vector3[] { new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1) };//���a���q����������Size        

        //���a ���D����
        playerJumpAttackDamage = 10;//���a���D�����ˮ`
        playerJumpAttackEffect = "KnockBack";//���a���D�����ĪG(�����̼��񪺰ʵe�W��)
        playerJumpAttackRepelDistance = 50;//���a���D���� ���h�Z��
        playerJumpAttackBoxSize = new Vector3(1, 0.5f, 1);//���a���D����������Size
        playerJumpAttackRepelDirection = 0;//���a���D�������h��V(0:���h 1:����)

        //���a �ޯ����_1
        playerSkillAttack_1_Damage = 33;//�ޯ����_1_�����ˮ`
        playerSkillAttack_1_Effect = "KnockBack";//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
        playerSkillAttack_1_FlyingSpeed = 11.5f;//�ޯ����_1_���󭸦�t��
        playerSkillAttack_1_LifeTime = 0.75f;//�ޯ����_1_�ͦs�ɶ�
        playerSkillAttack_1_Repel = 70;//�ޯ����_1_���h�Z��
        playerSkillAttack_1_RepelDirection = 0;//���a���q�������h��V(0:���h 1:����)

        //���a �ޯ����_2
        playerSkillAttack_2_Damage = 44;//�ޯ����_1_�����ˮ`
        playerSkillAttack_2_Effect = "Pain";//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
        playerSkillAttack_2_Repel = 18;//�ޯ����_1_���h�Z��
        playerSkillAttack_2_RepelDirection = 1;//���a���q�������h��V(0:���h 1:����)
        playerSkillAttack_2_BoxSize = new Vector3(1, 1, 1);//���a�ޯ����_2_������Size

        //���a �ޯ����_3
        playerSkillAttack_3_Damage = 55;//�ޯ����_1_�����ˮ`
        playerSkillAttack_3_Effect = "KnockBack";//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
        playerSkillAttack_3_Repel = 100;//�ޯ����_1_���h�Z��
        playerSkillAttack_3_RepelDirection = 0;//���a���q�������h��V(0:���h 1:����)
        playerSkillAttack_3_BoxSize = new Vector3(1, 1, 1);//���a�ޯ����_2_������Size

        //�u�`�h�L
        skeletonSoldierHp = 300;//�u�`�h�L�ͩR��
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
