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

    //����s�� �ĤH
    static int skeletonSoldierNumber;//�u�`�h�L

    void Awake()
    {       
        if(gameManagement != null)
        {
            Destroy(this);
            return;
        }
        gameManagement = this;
        objectHandle = ObjectHandle.GetObjectHandle;
        loadPath = GameDataManagement.Instance.loadPath;
    }

    void Start()
    {
        //�������󱾤W�p�a���I�I
        GameObject stageObject = GameObject.Find("StageObjects");        
        Transform[] allStageObject = stageObject.GetComponentsInChildren<Transform>();
        foreach (var item in allStageObject)
        {
            if (item.GetComponent<BoxCollider>()) OnSetMiniMapPoint(item, loadPath.miniMapMatirial_Object);
        }

        //���a�}��
        playerNumber = objectHandle.OnCreateObject(loadPath.playerCharacters);//���ͦܪ����
        objectNumber_Dictionary.Add("playerNumber", playerNumber);//�K�[�ܬ�����
        GameObject player = objectHandle.OnOpenObject(playerNumber);//���ͪ��a
        player.transform.position = new Vector3(0, 0.5f, 0);////�]�w��m
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//�]�w�p�a���I�I

        //���a�ޯ�_1
        playerSkill_1_Number = objectHandle.OnCreateObject(loadPath.playerSkill_1);
        objectNumber_Dictionary.Add("playerSkill_1_Number", playerSkill_1_Number);

        //�u�`�h�L
        skeletonSoldierNumber = objectHandle.OnCreateObject(loadPath.SkeletonSoldier);//���ͦܪ����
        objectNumber_Dictionary.Add("skeletonSoldierNumber", skeletonSoldierNumber);////�K�[�ܬ�����
        GameObject skeletonSoldier = objectHandle.OnOpenObject(skeletonSoldierNumber);//���;u�`�h�L
        skeletonSoldier.transform.position = new Vector3(3, 0.5f, 2);//�]�w��m
        OnSetMiniMapPoint(skeletonSoldier.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
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

    /// <summary>
    /// �]�w�p�a���I�I
    /// </summary>
    /// <param name="item">�n�K�[������</param>
    /// <param name="item">�I��������|</param>
    void OnSetMiniMapPoint(Transform item, string materialPath)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(loadPath.miniMapPoint));
        obj.transform.localEulerAngles = new Vector3(90, 0, 0);
        obj.transform.SetParent(item);
        MiniMapPoint map = obj.GetComponent<MiniMapPoint>();
        map.pointMaterial = Resources.Load<Material>(materialPath);
    }
}
