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

    [Header("�Ԥh")]
    public string warriorSkillAttack_1;//�ޯ����_1

    [Header("�}�b��")]
    public string archerNormalAttack_1;//���q����_1
    public string archerNormalAttack_2;//���q����_2
    public string archerNormalAttack_3;//���q����_3
    public string[] archerAllNormalAttack;//�Ҧ����q����
    public string archerSkilllAttack_1;//�ޯ����_1

    [Header("�k�v")]
    public string magicianNormalAttack_1;//���q����_1

    [Header("�ĤH")]
    public string enemySoldier_1;//�ĤH�h�L1
    public string enemySoldier_2;//�ĤH�h�L2

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

        //�Ԥh
        warriorSkillAttack_1 = "Prefab/ShootObject/Warrior/WarriorSkillAttack_1";//�ާ�����_1

        //�}�b��
        archerNormalAttack_1 = "Prefab/ShootObject/Archer/ArcherNormalAttack_1";//���q����_1
        archerNormalAttack_2 = "Prefab/ShootObject/Archer/ArcherNormalAttack_2";//���q����_2
        archerNormalAttack_3 = "Prefab/ShootObject/Archer/ArcherNormalAttack_3";//���q����_3
        archerAllNormalAttack = new string[] { archerNormalAttack_1 , archerNormalAttack_2 , archerNormalAttack_3 };//�Ҧ����q��������
        archerSkilllAttack_1 = "Prefab/ShootObject/Archer/ArcherSkilllAttack_1";//�ޯ����_1

        //�k�v
        magicianNormalAttack_1 = "Prefab/ShootObject/Magician/MagicianNormalAttack_1";//���q����_1

        //�ĤH
        enemySoldier_1 = "Prefab/Characters/Enemy/EnemySoldier_1";//�ĤH�h�L1
        enemySoldier_2 = "Prefab/Characters/Enemy/EnemySoldier_2";//�ĤH�h�L2


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
