using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class GameData_LoadPath
{
    [Header("�}�l����")]
    public string startVideo;//�}�l�v��
    public string roleSelect_Button;//��ܸ}����s
    public string roleSelect_Sprite;//�}���ܹϤ�

    [Header("���J����")]
    public string LoadBackground_1;//���J�����I��_1

    [Header("���a")]
    public string playerCharacters_1;//���a�}��_1
    public string playerSkill_1;//���a�ޯ�_1

    private GameData_LoadPath()
    {
        //�}�l����
        startVideo = "Video/StartVideo";
        roleSelect_Button = "RoleSelectButton/RoleSelect_Button";//��ܸ}����s
        roleSelect_Sprite = "Sprites/StartScene/ChooseRoleScreen/Role";//�}���ܹϤ�

        //���J����
        LoadBackground_1 = "Sprites/LoadScene/LoadBackground_1";//���J�����I��

        //���a
        playerCharacters_1 = "Characters/PlayerCharacters_1";//���a�}��_1
        playerSkill_1 = "Skill/PlayerSkill_1";//���a�ޯ�_1
    }
}

[CreateAssetMenu(fileName = "LoadPath", menuName = "ScriptableObjects/LoadPath", order = 2)]
public class ScriptableObject_LoadPath : ScriptableObject
{
    public GameData_LoadPath loadPath;
}
