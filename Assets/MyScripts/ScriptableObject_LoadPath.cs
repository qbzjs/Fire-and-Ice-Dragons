using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// �C��������|�޲z
/// </summary>
[System.Serializable]
public class GameData_LoadPath
{
    [Header("�}�l����")]
    public string startVideo;//�}�l�v��
    public string roleSelect_Button;//��ܸ}����s
    public string roleSelect_Sprite;//�}���ܹϤ�

    [Header("���J����")]
    public string LoadBackground_1;//���J�����I��_1

    [Header("�p�a��")]
    public string miniMapMatirial_Floor;//�p�a�ϧ���(�a�O)
    public string miniMapMatirial_Object;//�p�a�ϧ���(����)
    public string miniMapMatirial_Player;//�p�a�ϧ���(���a)
    public string miniMapMatirial_Enemy;//�p�a�ϧ���(�ĤH)
    public string miniMapPoint;//�p�a��(�I)

    [Header("���a�}��")]
    public string playerCharacters_1;//���a�}��
    public string playerCharacters_2;//���a�}��
    public string playerCharacters_3;//���a�}��
    public string playerCharacters_4;//���a�}��
    public string[] allPlayerCharacters;//�Ҧ����a�}��

    [Header("���a�}��1_�ޯ�")]
    public string playerCharactersSkill_1;//���a�}��1_�ޯ�1

    [Header("�u�`�h�L")]
    public string SkeletonSoldier;//�u�`�h�L

    [Header("��L")]
    public string hitNumber;//������r
    public string lifeBar;//�ͩR��

    private GameData_LoadPath()
    {
        //�}�l����
        startVideo = "Video/StartVideo";
        roleSelect_Button = "Prefab/UI/RoleSelect_Button";//��ܸ}����s
        roleSelect_Sprite = "Sprites/StartScene/SelectRoleScreen/Role";//�}���ܹϤ�

        //���J����
        LoadBackground_1 = "Sprites/LoadScene/LoadBackground_1";//���J�����I��

        //�p�a��
        miniMapMatirial_Floor = "Matirials/MiniMap/MiniMpa_Floor";//�p�a�ϧ���(�a�O)
        miniMapMatirial_Object = "Matirials/MiniMap/MiniMpa_Object";//�p�a�ϧ���(����)
        miniMapMatirial_Player = "Matirials/MiniMap/MiniMap_Player";//�p�a�ϧ���(���a)
        miniMapMatirial_Enemy = "Matirials/MiniMap/MiniMap_Enemy";//�p�a�ϧ���(�ĤH)
        miniMapPoint = "Prefab/UI/MiniMapPoint";//�p�a��(�I)

        //���a�}��
        playerCharacters_1 = "Prefab/Characters/Player/PlayerCharacters_1";//���a�}��1
        playerCharacters_2 = "Prefab/Characters/Player/PlayerCharacters_2";//���a�}��2
        playerCharacters_3 = "Prefab/Characters/Player/PlayerCharacters_3";//���a�}��3
        playerCharacters_4 = "Prefab/Characters/Player/PlayerCharacters_4";//���a�}��4
        allPlayerCharacters = new string[] { playerCharacters_1 , playerCharacters_2, playerCharacters_3, playerCharacters_4 };//�Ҧ����a�}��

        //���a�ޯ�
        playerCharactersSkill_1 = "Prefab/ShootObject/PlayerCharacters1_Skill_1";//���a�}��1_�ޯ�1

        //�u�`�h�L
        SkeletonSoldier = "Prefab/Characters/Enemy/SkeletonSoldier";//�u�`�h�L

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
