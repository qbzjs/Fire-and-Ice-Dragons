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
    public List<AttackMode> AttackBehavior_List = new List<AttackMode>();//�����Ҧ������欰    

    Dictionary<int, GameObject> connectObject_Dictionary = new Dictionary<int, GameObject>();//�O���Ҧ��s�u����

    void Awake()
    {       
        if(gameSceneManagement != null)
        {
            Destroy(this);
            return;
        }
        gameSceneManagement = this;

        GameDataManagement.Instance.stage = GameDataManagement.Stage.�C������;

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
        player.transform.position = new Vector3(222, -22, -60);//�]�w��m
        player.transform.rotation = Quaternion.Euler(0, -60, 0);//�]�w����
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//�]�w�p�a���I�I   

        //�Ԥh����
        number = objectHandle.OnCreateObject(loadPath.warriorSkillAttack_1);//�Ԥh�ޯ����_1����
        objectNumber_Dictionary.Add("warriorSkillAttack_1", number);//�K�[�ܬ�����

        //�}�b�⪫��
        number = objectHandle.OnCreateObject(loadPath.archerNormalAttack_1);//���q����_1����
        objectNumber_Dictionary.Add("archerNormalAttack_1", number);//�K�[�ܬ�����
        number = objectHandle.OnCreateObject(loadPath.archerNormalAttack_2);//���q����_2����
        objectNumber_Dictionary.Add("archerNormalAttack_2", number);//�K�[�ܬ�����
        number = objectHandle.OnCreateObject(loadPath.archerNormalAttack_3);//���q����_3����
        objectNumber_Dictionary.Add("archerNormalAttack_3", number);//�K�[�ܬ�����
        number = objectHandle.OnCreateObject(loadPath.archerSkilllAttack_1);//�ޯ����_1����
        objectNumber_Dictionary.Add("archerSkilllAttack_1", number);//�K�[�ܬ�����

        //�k�v����
        number = objectHandle.OnCreateObject(loadPath.magicianNormalAttack_1);//���q����_1����
        objectNumber_Dictionary.Add("magicianNormalAttack_1", number);//�K�[�ܬ�����

        //�ĤH�h�L1
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            number = objectHandle.OnCreateObject(loadPath.enemySoldier_1);//���ͦܪ����
            objectNumber_Dictionary.Add("enemySoldier_1", number);////�K�[�ܬ�����

            for (int i = 0; i < 1; i++)
            {                
                GameObject enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//�}�Ҫ���
                enemy.transform.position = new Vector3(188, -24, -37);//�]�w��m
                enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
            }
        }
    }

    void Update()
    {        
        OnAttackBehavior();   
        
        if(Input.GetKeyDown(KeyCode.O))
        {
            if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
            {
                GameObject enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//�}�Ҫ���
                CharactersCollision collision = enemy.GetComponent<CharactersCollision>();
                if (collision != null) collision.OnInitial();//��l��
                enemy.transform.position = new Vector3(24 + 2 * 1, 2f, 40);//�]�w��m                
                OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
            }
        }
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
        obj.transform.localPosition = new Vector3(itemBoxCenter.x, 0, itemBoxCenter.z);

        //Size
        if (item.gameObject.layer != LayerMask.NameToLayer("Player") && item.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            Vector3 itemBoxSize = item.GetComponent<BoxCollider>().size;
            obj.transform.localScale = new Vector3(itemBoxSize.x, itemBoxSize.z, 1);
        }
        else obj.transform.localScale = new Vector3(1, 1, 1);

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
        connectObject_Dictionary.Add(id, obj);
    }

    /// <summary>
    /// �s�u����E�����A
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="active">�E�����A</param>
    public void OnConnectObjectActive(int targetID, bool active)
    {
        connectObject_Dictionary[targetID].SetActive(active);

        if (active)
        {
            CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
            if (collision != null) collision.OnInitial();//��l��                
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
        Animator animator = connectObject_Dictionary[targetID].GetComponent<Animator>();
        if (animator != null)
        {
            switch (animationType.GetType().Name)
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
                case "String":
                    animator.SetTrigger(anmationName);
                    break;
            }
        }
    }

    /// <summary>
    /// �s�u����
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="position">��m</param>
    /// <param name="rotation">����</param>
    /// <param name="damage">����ˮ`</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnConnectGetHit(int targetID, Vector3 position, Quaternion rotation, float damage, bool isCritical)
    {       
        CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
        if (collision != null)
        {
            collision.OnConnectOtherGetHit(position, rotation, damage, isCritical);
        }
    }

    /// <summary>
    /// �s�u�v��
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="heal">�v���q</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnConnectGetHeal(int targetID, float heal, bool isCritical)
    {
        CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
        if (collision != null)
        {
            collision.OnConnectOtherGetHeal(heal, isCritical);
        }
    }
    #endregion
}
