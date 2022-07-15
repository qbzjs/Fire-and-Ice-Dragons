using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �C���޲z����
/// </summary>
public class GameManagement : MonoBehaviour
{
    static GameManagement gameManagement;
    public static GameManagement Instance => gameManagement;
    ObjectHandle objectHandle = new ObjectHandle();
    GameData_LoadPath loadPath;

    Dictionary<string, int> objectNumber_Dictionary = new Dictionary<string, int>();//�O���Ҧ�����s��
    public List<AttackBehavior> AttackBehavior_List = new List<AttackBehavior>();//�����Ҧ������欰    

    //����s�� ���a
    static int playerNumber;//���a
    static int playerSkill_1_Number;//���a�ޯ�1

    void Awake()
    {       
        if(gameManagement != null)
        {
            Destroy(this);
            return;
        }
        gameManagement = this;
        objectHandle = ObjectHandle.GetObjectHandle;
        loadPath = GameDataManagement.Insrance.loadPath;

        //���a�}��_1
        playerNumber = objectHandle.OnCreateObject(loadPath.playerCharacters_1);
        objectNumber_Dictionary.Add("playerNumber", playerNumber);
        GameObject player = objectHandle.OnOpenObject(playerNumber);//���ͪ��a
        player.transform.position = new Vector3(0, 0.5f, 0);

        //���a�ޯ�_1
        playerSkill_1_Number = objectHandle.OnCreateObject(loadPath.playerSkill_1);
        objectNumber_Dictionary.Add("playerSkill_1_Number", playerSkill_1_Number);
    }

    void Update()
    {        
        OnAttackBehavior();
    }

    //�����欰
    void OnAttackBehavior()
    {
        for (int i = 0; i < AttackBehavior_List.Count; i++)
        {
            AttackBehavior_List[i].function.Invoke();            
        }
    }

    /// <summary>
    /// �������s��
    /// </summary>
    /// <param name="objectNmae">�n�}�Ҫ�����W��</param>
    /// <returns></returns>
    public int OnGetObjectNumber(string objectNmae)
    {
        int value = -1;

        foreach (var obj in objectNumber_Dictionary)
        {
            if(obj.Key == objectNmae)
            {
                value = obj.Value;
            }
        }
        
        return value;
    }

    /// <summary>
    /// �n�D�}�Ҫ���
    /// </summary>
    /// <param name="number">����s��</param>
    /// <returns></returns>
    public GameObject OnRequestOpenObject(int number)
    {        
        GameObject obj = objectHandle.OnOpenObject(number);//�}�Ҫ���
        return obj;//�^�Ǫ���
    }
}
