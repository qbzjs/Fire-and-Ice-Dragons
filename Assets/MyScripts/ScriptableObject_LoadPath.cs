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

    [Header("�}�b��")]
    public string archerNormalAttack_1_Arrow;//���q����_1�}�b����
    public string archerNormalAttack_2_Arrow;//���q����_2�}�b����
    public string archerNormalAttack_3_Arrow;//���q����_3�}�b����
    public string[] archerNormalAttackArrows;//�Ҧ����q�����}�b����
    public string archerSkilllAttack_1_Arrow;//�ޯ����_1�}�b����

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

        //�}�b��
        archerNormalAttack_1_Arrow = "Prefab/ShootObject/ArcherNormalAttack_1_Arrow";//���q����_1�}�b����
        archerNormalAttack_2_Arrow = "Prefab/ShootObject/ArcherNormalAttack_2_Arrow";//���q����_2�}�b����
        archerNormalAttack_3_Arrow = "Prefab/ShootObject/ArcherNormalAttack_3_Arrow";//���q����_3�}�b����
        archerNormalAttackArrows = new string[] { archerNormalAttack_1_Arrow , archerNormalAttack_2_Arrow , archerNormalAttack_3_Arrow };//�Ҧ����q�����}�b����
        archerSkilllAttack_1_Arrow = "Prefab/ShootObject/ArcherSkilllAttack_1_Arrow";//�ޯ����_1�}�b����

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
