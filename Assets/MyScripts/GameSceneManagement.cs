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
        player.transform.position = new Vector3(23, 2f, 40);////�]�w��m
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
    #region �@��
    /// <summary>
    /// �����欰
    /// </summary>
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
        obj.transform.SetParent(item);        

        //��m
        Vector3 itemBoxCenter = item.GetComponent<BoxCollider>().center;
        obj.transform.localPosition = new Vector3(0, 0, itemBoxCenter.z);

        //Size
        Vector3 itemBoxSize = item.GetComponent<BoxCollider>().size;        
        obj.transform.localScale = new Vector3(itemBoxSize.x, itemBoxSize.z, 1);

        //����
        obj.transform.localEulerAngles = new Vector3(90, 0, 0);

        //����y(�C��)
        obj.GetComponent<Renderer>().material = Resources.Load<Material>(materialPath);
    }
    #endregion

    #region �s�u
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
    /// �s�u�����T��
    /// </summary>
    /// <param name="targetID">�����̪���ID</param>
    /// <param name="attackerID">�����̪���ID</param>
    /// <param name="layer">������layer</param>
    /// <param name="damage">�y���ˮ`</param>
    /// <param name="animationName">����ʵe�W��</param>
    /// <param name="knockDirection">�����ĪG(0:���h, 1:����)</param>
    /// <param name="repel">���h�Z��</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnConnectGetHit(int targetID, int attackerID, string layer, float damage, string animationName, int knockDirection, float repel, bool isCritical)
    {
        GameObject attacker = null;

        //�j�M�����̪���
        foreach(var attack in connectObject_Dixtionary)
        {
            if (attack.Key == attackerID)
            {
                attacker = attack.Value;
                break;
            }
        }

        //�j�M�����̪���
        foreach (var obj in connectObject_Dixtionary)
        {
            if(obj.Key == targetID)
            {                
                obj.Value.GetComponent<CharactersCollision>().OnGetHit(attacker: attacker,//�����̪���
                                                                       layer: layer,//������layer
                                                                       damage: damage,//�y���ˮ`
                                                                       animationName: animationName,//�����ĪG(�����̼��񪺰ʵe�W��)
                                                                       knockDirection: knockDirection,//���h��V((0:���h 1:����))
                                                                       repel: repel,//���h�Z��
                                                                       isCritical: isCritical);//�O�_�z��
                break;
            }
        }               
    }

    /// <summary>
    /// �s�u�ʵe�]�w
    /// </summary>
    /// <typeparam name="T">�x��</typeparam>
    /// <param name="targetID">�ʵe�󴫥ؼ�ID</param>
    /// <param name="anmationName">�ʵe�󴫥ؼ�ID</param>
    /// <param name="animationType">�ʵeType</param>
    public void OnConnectAnimationSetting<T>(int targetID, string anmationName, T animationType) 
    {
        
        //�j�M�����̪���
        foreach (var target in connectObject_Dixtionary)
        {
            if (target.Key == targetID)
            {
                Animator animator = target.Value.GetComponent<Animator>();
                switch(animationType.GetType().Name)
                {
                    case "Boolean":
                        animator.SetBool(anmationName, Convert.ToBoolean(animationType));
                        break;
                    case "Single":
                        animator.SetFloat(anmationName, Convert.ToSingle(animationType));
                        break;
                    case "Int32":
                        animator.SetInteger(anmationName, Convert.ToInt32(animationType));
                        break;
                        
                }              
            }
        }
    }

    #endregion
}
