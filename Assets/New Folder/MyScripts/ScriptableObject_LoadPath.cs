using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// �C��������|�޲z
/// </summary>
[System.Serializable]
public class GameData_LoadPath
{
    [Header("�}�l����")]
    public string roleSelect_Button;//��ܸ}����s
    public string roleSelect_Sprite;//�}���ܹϤ�

    [Header("���J����")]
    public string LoadBackground_1;//���J�����I��_1

    [Header("�p�a��")]
    public string miniMapMatirial_Floor;//�p�a�ϧ���(�a�O)
    public string miniMapMatirial_Object;//�p�a�ϧ���(����)
    public string miniMapMatirial_Player;//�p�a�ϧ���(���a)
    public string miniMapMatirial_OtherPlayer;//�p�a�ϧ���(��L���a)
    public string miniMapMatirial_Enemy;//�p�a�ϧ���(�ĤH)
    public string miniMapPoint;//�p�a��(�I)

    [Header("���a�}��")]
    public string Warrior;//���a�}��
    public string Magician;//���a�}��
    public string Archer;//���a�}��
    public string[] allPlayerCharacters;//�Ҧ����a�}��

    [Header("���a�}��1_�ޯ�")]
    public string playerCharactersSkill_1;//���a�}��1_�ޯ�1

    [Header("�ĤH")]
    public string enemy;//�ĤH

    [Header("��L")]
    public string hitNumber;//������r
    public string lifeBar;//�ͩR��

    private GameData_LoadPath()
    {
        //�}�l����
        roleSelect_Button = "Prefab/UI/RoleSelect_Button";//��ܸ}����s
        roleSelect_Sprite = "Sprites/StartScene/SelectRoleScreen/Role";//�}���ܹϤ�

        //���J����
        LoadBackground_1 = "Sprites/LoadScene/LoadBackground_1";//���J�����I��

        //�p�a��
        miniMapMatirial_Floor = "Matirials/MiniMap/MiniMpa_Floor";//�p�a�ϧ���(�a�O)
        miniMapMatirial_Object = "Matirials/MiniMap/MiniMpa_Object";//�p�a�ϧ���(����)
        miniMapMatirial_Player = "Matirials/MiniMap/MiniMap_Player";//�p�a�ϧ���(���a)
        miniMapMatirial_OtherPlayer = "Matirials/MiniMap/MiniMap_OtherPlayer";//�p�a�ϧ���(��L���a)
        miniMapMatirial_Enemy = "Matirials/MiniMap/MiniMap_Enemy";//�p�a�ϧ���(�ĤH)
        miniMapPoint = "Prefab/UI/MiniMapPoint";//�p�a��(�I)

        //���a�}��
        Warrior = "Prefab/Characters/Player/1_Warrior";//�Ԥh
        Magician = "Prefab/Characters/Player/2_Magician";//�]�k�v
        Archer = "Prefab/Characters/Player/3_Archer";//�}�b��
        allPlayerCharacters = new string[] { Warrior , Magician, Archer };//�Ҧ����a�}��

        //���a�ޯ�
        playerCharactersSkill_1 = "Prefab/ShootObject/PlayerCharacters1_Skill_1";//���a�}��1_�ޯ�1

        //�ĤH
        enemy = "Prefab/Characters/Enemy/Enemy";//�ĤH

        //��L
        hitNumber = "Prefab/UI/HitNumber_Text";//������r
        lifeBar = "Prefab/UI/LifeBar";//�ͩR��
    }
}

/// <summary>
/// �C��������|�޲z����
/// </summary>
[CreateAssetMenu(fileName = "LoadPath", menuName = "ScriptableObjects/LoadPath", order = 2)]
public class ScriptableObject_LoadPath : ScriptableObject
{
    public GameData_LoadPath loadPath;
}
