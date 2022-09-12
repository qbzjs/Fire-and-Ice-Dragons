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
    public string miniMapMatirial_TaskObject;//�p�a�ϧ���(���Ȫ���)    
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

    [Header("�ڤ�P���h�L�}��")]
    public string allianceSoldier_1;//�P���h�L1

    [Header("�ĤH����")]
    public string boss;//Boss
    public string enemySoldier_1;//�ĤH�h�L1
    public string enemySoldier_2;//�ĤH�h�L2
    public string enemySoldier_3;//�ĤH�h�L3
    public string guardBoss;//�����u��Boss

    [Header("�u��Boss����")]
    public string guardBossAttack_1;//����1

    [Header("�ĤH�h�L2����")]
    public string enemySoldier2Attack_Arrow;//�}�b

    [Header("Boss����")]
    public string bossAttack1;//Boss����1����(�������)

    [Header("��L")]    
    public string hitNumber;//������r
    public string lifeBar;//�ͩR��
    public string headLifeBar_Enemy;//�Y���ͩR��_�ĤH
    public string headLifeBar_Alliance;//�Y���ͩR��_�P��
    public string objectName;//����W��

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
        miniMapMatirial_TaskObject = "Matirials/MiniMap/MiniMapMatirial_TaskObject";//�p�a�ϧ���(���Ȫ���)
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


        //�ڤ�P���h�L�}��
        allianceSoldier_1 = "Prefab/Characters/Alliance/AllianceSoldier_1"; ;//�P���h�L1

        //�ĤH����
        boss = "Prefab/Characters/Enemy/Boss";//Boss
        enemySoldier_1 = "Prefab/Characters/Enemy/EnemySoldier_1";//�ĤH�h�L1
        enemySoldier_2 = "Prefab/Characters/Enemy/EnemySoldier_2";//�ĤH�h�L2
        enemySoldier_3 = "Prefab/Characters/Enemy/EnemySoldier_3";//�ĤH�h�L3
        guardBoss = "Prefab/Characters/Enemy/GuardBoss";//�����u��Boss

        //�u��Boss����
        guardBossAttack_1 = "Prefab/ShootObject/GuardBoss/Attack1_GuardBoss";//����1

        //�ĤH�h�L2����
        enemySoldier2Attack_Arrow = "Prefab/ShootObject/EnemySoldier2/Attack_Arrow";//�}�b

        //Boss����
        bossAttack1 = "Prefab/ShootObject/Boss/Attack1";//Boss����1����(�������)

        //��L
        hitNumber = "Prefab/UI/HitNumber_Text";//������r
        lifeBar = "Prefab/UI/LifeBar";//�ͩR��
        headLifeBar_Enemy = "Prefab/HeadLifeBar/HeadLifeBar_Enemy";//�Y���ͩR��_�ĤH
        headLifeBar_Alliance = "Prefab/HeadLifeBar/HeadLifeBar_Alliance";//�Y���ͩR��_�P��
        objectName = "Prefab/UI/ObjectName_Text";//����W��
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
