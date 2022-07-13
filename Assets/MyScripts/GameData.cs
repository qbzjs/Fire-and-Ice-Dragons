using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �C�����
/// </summary>
public class GameData : MonoBehaviour
{
    static GameData gameData;
    public static GameData Instance => gameData;

    Dictionary<string, float> gameData_Float_Dictionary = new Dictionary<string, float>();//�����C���ƭ�(Float)
    Dictionary<string, float[]> gameData_FloatArray_Dictionary = new Dictionary<string, float[]>();//�����C���ƭ�(FloatArray)

    Dictionary<string, string> gameData_String_Dictionary = new Dictionary<string, string>();//�����C���ƭ�(String)
    Dictionary<string, string[]> gameData_StringArray_Dictionary = new Dictionary<string, string[]>();//�����C���ƭ�(StringArray)

    Dictionary<string, Vector3> gameData_Vectorg_Dictionary = new Dictionary<string, Vector3>();//�����C���ƭ�(Vector)
    Dictionary<string, Vector3[]> gameData_VectorgArray_Dictionary = new Dictionary<string, Vector3[]>();//�����C���ƭ�(VectorArray)

    //�@�q
    static float gravity;//���O

    //���a
    static float playerHp;//���a�ͩR��
    static float playerMoveSpeed;//���a���ʳt��
    static float playerJumpForce;//���a���D�O

    //���a ���q����
    static float[] playerNormalAttackDamge;//���a���q�����ˮ`
    static float[] playerNormalAttackMoveDistance;//���a���q�������ʶZ��
    static float[] playerNormalAttackRepelDistance;//���a���q���� ���h/�����Z��
    static float[] playerNormalAttackRepelDirection;//���a���q������V(0:���h 1:����)
    static string[] playerNormalAttackEffect;//���a���q�����ĪG(�����̼��񪺰ʵe�W��)
    static Vector3[] playerNormalAttackBoxSize;//���a���q����������Size

    //���a ���D����
    static float playerJumpAttackDamage;//���a���D�����ˮ`
    static string playerJumpAttackEffect;//���a���D�����ĪG(�����̼��񪺰ʵe�W��)
    static float playerJumpAttackRepelDistance;//���a���D���� ���h�Z��
    static Vector3 playerJumpAttackBoxSize;//���a���D����������Size

    //�ޯ����_1
    static float playerSkillAttack_1_Damage;//�ޯ����_1_�����ˮ`
    static string playerSkillAttack_1_Effect;//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
    static float playerSkillAttack_1_FlyingSpeed;//�ޯ����_1_���󭸦�t��
    static float playerSkillAttack_1_LifeTime;//�ޯ����_1_�ͦs�ɶ�
    static float playerSkillAttack_1_Repel;//�ޯ����_1_���h�Z��

    //�u�`�h�L
    static float skeletonSoldierHp;//�u�`�h�L�ͩR��

    private void Awake()
    {
        if(gameData != null)
        {
            Destroy(this);
            return;
        }
        gameData = this;

        //�@�q
        gravity = 9.8f;//���O
        gameData_Float_Dictionary.Add("gravity", gravity);

        //���a
        playerHp = 100;//���a�ͩR��
        playerMoveSpeed = 10;//���a���ʳt��        
        playerJumpForce = 16;//���a���D�O
        gameData_Float_Dictionary.Add("playerHp", playerHp);
        gameData_Float_Dictionary.Add("playerMoveSpeed", playerMoveSpeed);
        gameData_Float_Dictionary.Add("playerJumpForce", playerJumpForce);

        //���a ���q����
        playerNormalAttackDamge = new float[] { 10, 10, 15};//���a���q�����ˮ`
        playerNormalAttackMoveDistance = new float[] { 50, 50, 0 };//���a���q�������ʶZ��
        playerNormalAttackRepelDistance = new float[] { 70, 80, 18};//���a���q���� ���h/�����Z��
        playerNormalAttackRepelDirection = new float[] { 0, 0, 1};//���a���q���������ĪG(0:���h 1:����)
        playerNormalAttackEffect = new string[] { "Pain", "Pain", "Pain"};//���a���q�����ĪG(�����̼��񪺰ʵe�W��)
        playerNormalAttackBoxSize = new Vector3[] { new Vector3(1, 1, 1), new Vector3(0.5f, 1, 0.5f), new Vector3(2, 2, 2)};//���a���q����������Size        
        gameData_FloatArray_Dictionary.Add("playerNormalAttackDamge", playerNormalAttackDamge);
        gameData_FloatArray_Dictionary.Add("playerNormalAttackMoveDistance", playerNormalAttackMoveDistance);
        gameData_FloatArray_Dictionary.Add("playerNormalAttackRepelDistance", playerNormalAttackRepelDistance);
        gameData_FloatArray_Dictionary.Add("playerNormalAttackRepelDirection", playerNormalAttackRepelDirection);
        gameData_StringArray_Dictionary.Add("playerNormalAttackEffect", playerNormalAttackEffect);
        gameData_VectorgArray_Dictionary.Add("playerNormalAttackBoxSize", playerNormalAttackBoxSize);

        //���a ���D����
        playerJumpAttackDamage = 10;//���a���D�����ˮ`
        playerJumpAttackEffect = "KnockBack";//���a���D�����ĪG(�����̼��񪺰ʵe�W��)
        playerJumpAttackRepelDistance = 50;//���a���D���� ���h�Z��
        playerJumpAttackBoxSize = new Vector3(1, 0.5f, 1);//���a���D����������Size
        gameData_Float_Dictionary.Add("playerJumpAttackDamage", playerJumpAttackDamage);
        gameData_String_Dictionary.Add("playerJumpAttackEffect", playerJumpAttackEffect);
        gameData_Float_Dictionary.Add("playerJumpAttackRepelDistance", playerJumpAttackRepelDistance);
        gameData_Vectorg_Dictionary.Add("playerJumpAttackBoxSize", playerJumpAttackBoxSize);

        //�ޯ����_1
        playerSkillAttack_1_Damage = 33;//�ޯ����_1_�����ˮ`
        playerSkillAttack_1_Effect = "KnockBack";//�ޯ����_1_�����ĪG(�����̼��񪺰ʵe�W��)
        playerSkillAttack_1_FlyingSpeed = 11.5f;//�ޯ����_1_���󭸦�t��
        playerSkillAttack_1_LifeTime = 0.75f;//�ޯ����_1_�ͦs�ɶ�
        playerSkillAttack_1_Repel = 70;//�ޯ����_1_���h�Z��
        gameData_Float_Dictionary.Add("playerSkillAttack_1_Damage", playerSkillAttack_1_Damage);
        gameData_String_Dictionary.Add("playerSkillAttack_1_Effect", playerSkillAttack_1_Effect);
        gameData_Float_Dictionary.Add("playerSkillAttack_1_FlyingSpeed", playerSkillAttack_1_FlyingSpeed);
        gameData_Float_Dictionary.Add("playerSkillAttack_1_LifeTime", playerSkillAttack_1_LifeTime);
        gameData_Float_Dictionary.Add("playerSkillAttack_1_Repel", playerSkillAttack_1_Repel);

        //�u�`�h�L
        skeletonSoldierHp = 50;//�u�`�h�L�ͩR��
        gameData_Float_Dictionary.Add("skeletonSoldierHp", skeletonSoldierHp);
    }

    /// <summary>
    /// ����ƭ�(Float)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public float OnGetFloatValue (string search)
    {
        float value = 0;

        foreach(var data in gameData_Float_Dictionary)
        {         
            if(data.Key == search)
            {                
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// ����ƭ�(Float Array)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public float[] OnGetFloatArrayValue(string search)
    {
        float[] value = new float[] { };

        foreach (var data in gameData_FloatArray_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// ����ƭ�(String)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public string OnGetStringValue(string search)
    {
        string value = "";

        foreach (var data in gameData_String_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// ����ƭ�(String Array)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public string[] OnGetStringArrayValue(string search)
    {
        string[] value = new string[] { };

        foreach (var data in gameData_StringArray_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// ����ƭ�(Vector)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public Vector3 OnGetVectorValue(string search)
    {
        Vector3 value = new Vector3();

        foreach (var data in gameData_Vectorg_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }

    /// <summary>
    /// ����ƭ�(Vector Array)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public Vector3[] OnGetVectorArrayValue(string search)
    {
        Vector3[] value = new Vector3[] { };

        foreach (var data in gameData_VectorgArray_Dictionary)
        {
            if (data.Key == search)
            {
                value = data.Value;
            }
        }

        return value;
    }
}
