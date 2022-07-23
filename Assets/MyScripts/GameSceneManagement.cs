using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �C�������޲z����
/// </summary>
public class GameSceneManagement : MonoBehaviourPunCallbacks
{
    static GameSceneManagement gameSceneManagement;
    public static GameSceneManagement Instance => gameSceneManagement;
    ObjectHandle objectHandle = new ObjectHandle();
    public GameData_LoadPath loadPath;

    Dictionary<string, int> objectNumber_Dictionary = new Dictionary<string, int>();//�O���Ҧ�����s��
    public List<AttackBehavior> AttackBehavior_List = new List<AttackBehavior>();//�����Ҧ������欰    

    Dictionary<int, GameObject> connectObject_Dixtionary = new Dictionary<int, GameObject>();//�O���Ҧ��s�u����

    void Awake()
    {       
        if(gameSceneManagement != null)
        {
            Destroy(this);
            return;
        }
        gameSceneManagement = this;
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

        int number = 0;

        //���a�}��
        number = objectHandle.OnCreateObject(loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//���ͦܪ����
        objectNumber_Dictionary.Add("playerNumbering", number);//�K�[�ܬ�����
        GameObject player = OnRequestOpenObject(OnGetObjectNumber("playerNumbering"), loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//�}�Ҫ���
        player.transform.position = new Vector3(0, 0.7f, 0);////�]�w��m
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//�]�w�p�a���I�I   

        //���a�}��1_�ޯ�1
        number = objectHandle.OnCreateObject(loadPath.playerCharactersSkill_1);////���ͦܪ����
        objectNumber_Dictionary.Add("playerSkill_1_Numbering", number);//�K�[�ܬ�����

        //�u�`�h�L
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            number = objectHandle.OnCreateObject(loadPath.SkeletonSoldier);//���ͦܪ����
            objectNumber_Dictionary.Add("skeletonSoldierNumbering", number);////�K�[�ܬ�����
            GameObject skeletonSoldier = OnRequestOpenObject(OnGetObjectNumber("skeletonSoldierNumbering"), loadPath.SkeletonSoldier);//�}�Ҫ���
            skeletonSoldier.transform.position = new Vector3(3, 1.7f, 2);//�]�w��m
            OnSetMiniMapPoint(skeletonSoldier.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
        }
        
        //��L
        number = objectHandle.OnCreateObject(loadPath.hitNumber);//���ͦܪ����;//�����Ʀr
        objectNumber_Dictionary.Add("hitNumberNumbering", number);////�K�[�ܬ�����
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
    /// <param name="path">prefab���|</param>
    /// <returns></returns>
    public GameObject OnRequestOpenObject(int number, string path)
    {        
        GameObject obj = objectHandle.OnOpenObject(number, path);//�}�Ҫ���
        return obj;//�^�Ǫ���
    }

    /// <summary>
    /// �]�w�p�a���I�I
    /// </summary>
    /// <param name="item">�n�K�[������</param>
    /// <param name="item">�I��������|</param>
    public void OnSetMiniMapPoint(Transform item, string materialPath)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(loadPath.miniMapPoint));
        obj.transform.localEulerAngles = new Vector3(90, 0, 0);
        obj.transform.SetParent(item);
        MiniMapPoint map = obj.GetComponent<MiniMapPoint>();
        map.pointMaterial = Resources.Load<Material>(materialPath);
    }  

    /// <summary>
    /// �����s�u����
    /// </summary>
    /// <param name="id">����ID</param>
    /// <param name="obj">����</param>
    public void OnRecordConnectObject(int id, GameObject obj)
    {
        connectObject_Dixtionary.Add(id, obj);
    }

    /// <summary>
    /// �s�u����E�����A
    /// </summary>
    /// <param name="id">����ID</param>
    /// <param name="active">�E�����A</param>
    public void OnConnectObjectActive(int id, bool active)
    {
        foreach(var obj in connectObject_Dixtionary)
        {
            if (obj.Key == id) obj.Value.SetActive(active);
        }        
    }

    /// <summary>
    /// ����s�u����
    /// </summary>
    /// <param name="id">����ID</param>
    /// <returns></returns>
    public GameObject OnGetConnectObject(int id)
    {
        GameObject theObj = null;

        foreach (var obj in connectObject_Dixtionary)
        {
            if (obj.Key == id) theObj = obj.Value;
        }
        return theObj;
    }

    /// <summary>
    /// �s�u�ͩR�ƭ�
    /// </summary>
    /// <param name="numberID">�Ʀr����ID</param>
    /// <param name="targetID">�����ؼ�ID</param>
    /// <param name="damage">����ˮ`</param>
    /// <param name="isCritical">�O�_�z��</param>
    /// <param name="lifeBarID">�ͩR������ID</param>
    /// <param name="HpProportion">�ͩR���</param>
    public void OnConnectLifeValue(int numberID, int targetID, float damage, bool isCritical, int lifeBarID, float HpProportion)
    {   
        Transform target = null;

        //�ؼЪ���
        foreach (var obj in connectObject_Dixtionary)
        {
            if (obj.Key == targetID) target = obj.Value.transform;
        }

        //�����Ʀr
        foreach (var hitNumber in connectObject_Dixtionary)
        {
            if (hitNumber.Key == numberID) hitNumber.Value.GetComponent<HitNumber>().OnSetValue(target: target, damage: damage, color: isCritical ? Color.yellow : Color.red);
        }

        //�Y���ͩR��
        foreach (var lifeBar in connectObject_Dixtionary)
        {
            if (lifeBar.Key == lifeBarID) lifeBar.Value.GetComponent<LifeBar_Characters>().SetValue = HpProportion;
        }
    }
}
