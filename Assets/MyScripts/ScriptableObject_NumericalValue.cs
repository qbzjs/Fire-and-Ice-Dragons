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

    [Header("��v��")]
    public float distance;//�P���a�Z��
    public float limitUpAngle;//����V�W����
    public float limitDownAngle;//����V�U����

    [Header("���a")]
    public float playerHp;//���a�ͩR��
    public float playerMoveSpeed;//���a���ʳt��
    public float playerJumpForce;//���a���D�O

    [Header("���a ���q����")]
    public float[] playerNormalAttackDamge;//���a���q�����ˮ`
    public float[] playerNormalAttackMoveDistance;//���a���q���� ���ʶZ��
    public float[] playerNormalAttackRepelDistance;//���a���q���� ���h/�����Z��
    public int[] playerNormalAttackRepelDirection;//���a���q�������h��V(0:���h 1:����)
    public string[] playerNormalAttackEffect;//���a���q�����ĪG(�����̼��񪺰ʵe�W��)
    public Vector3[] playerNormalAttackBoxSize;//���a���q����������Size

    [Header("���a ���D����")]
    public float playerJumpAttackDamage;//���a���D�����ˮ`
    public string playerJumpAttackEffect;//���a���D�����ĪG(�����̼��񪺰ʵe�W��)
    public float playerJumpAttackRepelDistance;//���a���D���� ���h�Z��
    public Vector3 playerJumpAttackBoxSize;//���a���D����������Size
    public int playerJumpAttackRepelDirection;//���a���D�������h��V(0:���h 1:����)

    [Header("���a �ޯ����_1")]
    public float playerSkillAttack_1_Damage;//�ޯ����_1_�����ˮ`
    public string playerSkillAttack_1_Effect;//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
    public float playerSkillAttack_1_FlyingSpeed;//�ޯ����_1_���󭸦�t��
    public float playerSkillAttack_1_LifeTime;//�ޯ����_1_�ͦs�ɶ�
    public float playerSkillAttack_1_Repel;//�ޯ����_1_���h�Z��
    public int playerSkillAttack_1_RepelDirection;//���a�ޯ�������h��V(0:���h 1:����)

    [Header("���a �u�`�h�L")]
    public float skeletonSoldierHp;//�u�`�h�L�ͩR��

    /// <summary>
    /// �غc�l
    /// </summary>
    public GameData_NumericalValue()
    {
        //�@�q
        gravity = 9.8f;//���O
        boxCollisionDistance = 0.5f;//�I���ضZ��(�P�𭱶Z��)

        //��v��
        distance = 2.6f;//�P���a�Z��        
        limitUpAngle = 35;//����V�W����
        limitDownAngle = 20;//����V�U����

        //���a
        playerHp = 100;//���a�ͩR��
        playerMoveSpeed = 10;//���a���ʳt��        
        playerJumpForce = 16;//���a���D�O

        //���a ���q����
        playerNormalAttackDamge = new float[] { 10, 10, 15 };//���a���q�����ˮ`
        playerNormalAttackMoveDistance = new float[] { 50, 50, 0 };//���a���q�������ʶZ��
        playerNormalAttackRepelDistance = new float[] { 70, 80, 18 };//���a���q���� ���h/�����Z��
        playerNormalAttackRepelDirection = new int[] { 0, 0, 1 };//���a���q�������h��V(0:���h 1:����)
        playerNormalAttackEffect = new string[] { "Pain", "Pain", "Pain" };//���a���q�����ĪG(�����̼��񪺰ʵe�W��)
        playerNormalAttackBoxSize = new Vector3[] { new Vector3(1, 1, 1), new Vector3(0.5f, 1, 0.5f), new Vector3(2, 2, 2) };//���a���q����������Size        

        //���a ���D����
        playerJumpAttackDamage = 10;//���a���D�����ˮ`
        playerJumpAttackEffect = "KnockBack";//���a���D�����ĪG(�����̼��񪺰ʵe�W��)
        playerJumpAttackRepelDistance = 50;//���a���D���� ���h�Z��
        playerJumpAttackBoxSize = new Vector3(1, 0.5f, 1);//���a���D����������Size
        playerJumpAttackRepelDirection = 0;//���a���D�������h��V(0:���h 1:����)

        //�ޯ����_1
        playerSkillAttack_1_Damage = 33;//�ޯ����_1_�����ˮ`
        playerSkillAttack_1_Effect = "KnockBack";//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
        playerSkillAttack_1_FlyingSpeed = 11.5f;//�ޯ����_1_���󭸦�t��
        playerSkillAttack_1_LifeTime = 0.75f;//�ޯ����_1_�ͦs�ɶ�
        playerSkillAttack_1_Repel = 70;//�ޯ����_1_���h�Z��
        playerSkillAttack_1_RepelDirection = 0;//���a���q�������h��V(0:���h 1:����)

        //�u�`�h�L
        skeletonSoldierHp = 50;//�u�`�h�L�ͩR��
    }
}

[CreateAssetMenu(fileName = "NumericalValue", menuName = "ScriptableObjects/NumericalValue", order = 1)]
public class ScriptableObject_NumericalValue : ScriptableObject
{
    public GameData_NumericalValue numericalValue;
}
