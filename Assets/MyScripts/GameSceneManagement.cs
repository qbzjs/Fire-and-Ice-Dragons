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
    public List<AttackMode> AttackMode_List = new List<AttackMode>();//�����Ҧ������欰    

    Dictionary<int, GameObject> connectObject_Dictionary = new Dictionary<int, GameObject>();//�O���Ҧ��s�u����

    //�X���I
    Transform[] enemySoldiers1_Stage1Point;//�ĤH�h�L1_���q1�X���I
    Transform[] enemySoldiers2_Stage1Point;//�ĤH�h�L2_���q1�X���I
    Transform guardBoss_Stage2Point;//��������Boss_���q2�X���I
    Transform[] enemySoldiers1_Stage3Point;//�ĤH�h�L1_���q3�X���I
    Transform[] enemySoldiers2_Stage3Point;//�ĤH�h�L2_���q3�X���I
    Transform[] enemySoldiers3_Stage3Point;//�ĤH�h�L3_���q3�X���I

    //����
    string[] taskText;//�U���q���Ȥ�r
    int taskStage;//�ثe���ȶ��q
    public int[] taskKillNumber;//�U���q���ȩһ�������
    public int KillEnemyNumber;//�w�����Ǫ��ƶq

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
        /*//�������󱾤W�p�a���I�I
        GameObject stageObject = GameObject.Find("StageObjects");        
        Transform[] allStageObject = stageObject.GetComponentsInChildren<Transform>();
        foreach (var item in allStageObject)
        {
            if (item.GetComponent<BoxCollider>()) OnSetMiniMapPoint(item, loadPath.miniMapMatirial_Object);
        }*/

        int number = 0;

        //���a�}��
        number = objectHandle.OnCreateObject(loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//���ͦܪ����
        objectNumber_Dictionary.Add("playerNumbering", number);//�K�[�ܬ�����
        GameObject player = OnRequestOpenObject(OnGetObjectNumber("playerNumbering"), loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//�}�Ҫ���
        player.transform.position = new Vector3(227.5f, -23.6f, -23.5f);        
        player.transform.rotation = Quaternion.Euler(0, -60, 0);//�]�w����
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//�]�w�p�a���I�I           

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

        //�u��Boss����
        number = objectHandle.OnCreateObject(loadPath.guardBossAttack_1);//����1����
        objectNumber_Dictionary.Add("guardBossAttack_1", number);//�K�[�ܬ�����

        //�ĤH�h�L2����
        number = objectHandle.OnCreateObject(loadPath.enemySoldier2Attack_Arrow);//�}�b����
        objectNumber_Dictionary.Add("enemySoldier2Attack_Arrow", number);//�K�[�ܬ�����

        #region �ĤH�X���I
        //�ĤH�h�L1_���q1�X���I
        enemySoldiers1_Stage1Point = new Transform[GameObject.Find("EnemySoldiers1_Stage1Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers1_Stage1Point").transform.childCount; i++)
        {
            enemySoldiers1_Stage1Point[i] = GameObject.Find("EnemySoldiers1_Stage1Point").transform.GetChild(i);        
        }

        //�ĤH�h�L2_���q1�X���I
        enemySoldiers2_Stage1Point = new Transform[GameObject.Find("EnemySoldiers2_Stage1Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers2_Stage1Point").transform.childCount; i++)
        {
            enemySoldiers2_Stage1Point[i] = GameObject.Find("EnemySoldiers2_Stage1Point").transform.GetChild(i);
        }

        //��������Boss_���q2�X���I
        guardBoss_Stage2Point = GameObject.Find("GuardBoss_Stage2Point").transform.GetChild(0);

        //�ĤH�h�L1_���q3�X���I
        enemySoldiers1_Stage3Point = new Transform[GameObject.Find("EnemySoldiers1_Stage3Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers1_Stage3Point").transform.childCount; i++)
        {
            enemySoldiers1_Stage3Point[i] = GameObject.Find("EnemySoldiers1_Stage3Point").transform.GetChild(i);
        }

        //�ĤH�h�L2_���q3�X���I
        enemySoldiers2_Stage3Point = new Transform[GameObject.Find("EnemySoldiers2_Stage3Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers2_Stage3Point").transform.childCount; i++)
        {
            enemySoldiers2_Stage3Point[i] = GameObject.Find("EnemySoldiers2_Stage3Point").transform.GetChild(i);
        }

        //�ĤH�h�L3_���q3�X���I
        enemySoldiers3_Stage3Point = new Transform[GameObject.Find("EnemySoldiers3_Stage3Point").transform.childCount];
        for (int i = 0; i < GameObject.Find("EnemySoldiers3_Stage3Point").transform.childCount; i++)
        {
            enemySoldiers3_Stage3Point[i] = GameObject.Find("EnemySoldiers3_Stage3Point").transform.GetChild(i);
        }
        #endregion

        //�ЫؼĤH
        OnCreateEnemy();

        //����
        taskText = new string[] { "���˸Ӱϰ�Ҧ��Ǫ�", "���˫����u��", "���˫����ϰ�Ҧ��Ǫ�" };//�Ӷ��q���Ȥ�r
        //�U���q���ȩһ�������
        taskKillNumber = new int[] { enemySoldiers1_Stage1Point.Length + enemySoldiers2_Stage1Point.Length,//���q1
                                     1,//���q1
                                     enemySoldiers1_Stage3Point.Length + enemySoldiers2_Stage3Point.Length + enemySoldiers3_Stage3Point.Length};//���q3

        //���ȴ���
        StartCoroutine(OnTaskTipText(taskTipValue: taskText[taskStage].ToString()));

        //���Ȥ�r
        OnTaskText();
    }

    void Update()
    {
        //�����欰
        OnAttackBehavior();
    }

    #region ����
    /// <summary>
    /// �ЫؼĤH
    /// </summary>
    void OnCreateEnemy()
    {
        //���d1
        if(GameDataManagement.Instance.selectLevelNumber == 0)
        {
            
        }

        //���d2
        if (GameDataManagement.Instance.selectLevelNumber == 1)
        {
            //�D�s�u || �O�ХD
            if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
            {
                int number = 0;
                GameObject enemy = null;

                //�P�_�ثe���ȶ��q
                switch (taskStage)
                {
                    case 0://���q1
                        //���ͼĤH�h�L1
                        number = objectHandle.OnCreateObject(loadPath.enemySoldier_1);//���ͦܪ����
                        objectNumber_Dictionary.Add("enemySoldier_1", number);////�K�[�ܬ�����
                        for (int i = 0; i < enemySoldiers1_Stage1Point.Length; i++)                   
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//�}�Ҫ���
                            enemy.transform.position = enemySoldiers1_Stage1Point[i].position;//�]�w��m
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);                            
                            enemy.tag = "EnemySoldier_1";//�]�wTag�P�_HP
                            //OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }

                        //���ͼĤH�h�L2
                        number = objectHandle.OnCreateObject(loadPath.enemySoldier_2);//���ͦܪ����
                        objectNumber_Dictionary.Add("enemySoldier_2", number);////�K�[�ܬ�����
                        for (int i = 0; i < enemySoldiers2_Stage1Point.Length; i++)
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_1);//�}�Ҫ���
                            enemy.transform.position = enemySoldiers2_Stage1Point[i].position;//�]�w��m
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                            enemy.tag = "EnemySoldier_2";//�]�wTag�P�_HP
                            //OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }
                        break;
                    case 1://���q2
                        //���ͫ����u��Boss     
                        number = objectHandle.OnCreateObject(loadPath.guardBoss);//���ͦܪ����
                        objectNumber_Dictionary.Add("enemyGuardBoss", number);////�K�[�ܬ�����
                        //���ͫ����u��Boss
                        enemy = OnRequestOpenObject(OnGetObjectNumber("enemyGuardBoss"), loadPath.guardBoss);//�}�Ҫ���
                        enemy.transform.position = guardBoss_Stage2Point.position;//�]�w��m
                        enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                        enemy.tag = "GuardBoss";//�]�wTag�P�_HP
                        break;
                    case 2://���q3
                        //���ͼĤH�h�L1
                        for (int i = 0; i < enemySoldiers1_Stage3Point.Length; i++)
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//�}�Ҫ���
                            enemy.transform.position = enemySoldiers1_Stage3Point[i].position;//�]�w��m
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                            enemy.GetComponent<CharactersCollision>().OnInitial();//��l��
                            enemy.GetComponent<AI>().OnInitial();//��l��
                        }
                        //���ͼĤH�h�L2
                        for (int i = 0; i < enemySoldiers2_Stage3Point.Length; i++)
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_2);//�}�Ҫ���
                            enemy.transform.position = enemySoldiers2_Stage3Point[i].position;//�]�w��m
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                            enemy.GetComponent<CharactersCollision>().OnInitial();//��l��
                            enemy.GetComponent<AI>().OnInitial();//��l��
                        }
                        //���ͼĤH�h�L3
                        number = objectHandle.OnCreateObject(loadPath.enemySoldier_3);//���ͦܪ����
                        objectNumber_Dictionary.Add("enemySoldier_3", number);////�K�[�ܬ�����
                        for (int i = 0; i < enemySoldiers3_Stage3Point.Length; i++)
                        {
                            enemy = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_3"), loadPath.enemySoldier_3);//�}�Ҫ���
                            enemy.transform.position = enemySoldiers3_Stage3Point[i].position;//�]�w��m
                            enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                            enemy.tag = "EnemySoldier_3";//�]�wTag�P�_HP
                            //OnSetMiniMapPoint(enemy.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// ���ȴ���
    /// </summary>
    /// <param name="value">���ȴ��ܤ�r</param>
    /// <returns></returns>
    public IEnumerator OnTaskTipText(string taskTipValue)
    {
        yield return new WaitForSeconds(2);

        //�]�w���ܤ�r(���ȴ���)
        GameSceneUI.Instance.OnSetTip(tip: taskTipValue, showTime: 5);
    }

    /// <summary>
    /// ���Ȥ�r
    /// </summary>
    /// <param name="taskValue">���Ȥ�r</param>
    public void OnTaskText()
    {
        //���ȧP�w
        OnJudgeTask();

        //�]�w���Ȥ�r
        if (taskStage < taskKillNumber.Length)//���L��
        {
            GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n�ؼ�:" + KillEnemyNumber + "/" + taskKillNumber[taskStage]);
        }
    }

    /// <summary>
    /// ���ȧP�w
    /// </summary>
    void OnJudgeTask()
    {
        //�w�����Ǫ��ƶq >= ���ȩһ�������
        if (KillEnemyNumber >= taskKillNumber[taskStage])
        {            
            taskStage += 1;//�ثe���ȶ��q

            if (taskStage >= taskKillNumber.Length)//�L��
            {
                StartCoroutine(OnTaskTipText(taskTipValue: "�L��"));//���ȴ���   
                GameSceneUI.Instance.OnSetTaskText(taskValue: "�L��");

                StartCoroutine(OnClearance());//�L��
            }
            else//�i�J�U���q
            {
                KillEnemyNumber = 0;//�w�����Ǫ��ƶq                                
                StartCoroutine(OnTaskTipText(taskTipValue: taskText[taskStage]));//���ȴ���   
                GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n�ؼ�:" + KillEnemyNumber + "/" + taskKillNumber[taskStage]);

                //�ЫؼĤH
                OnCreateEnemy();
            }               
        }    
    }
    
    /// <summary>
    /// �L��
    /// </summary>
    /// <returns></returns>
    IEnumerator OnClearance()
    {
        yield return new WaitForSeconds(3);

        //�]�w�C������UI
        GameSceneUI.Instance.OnSetGameOverUI(clearance: true);
    }

    #endregion

    #region �@��    
    /// <summary>
    /// �����欰
    /// </summary>
    void OnAttackBehavior()
    {
        for (int i = 0; i < AttackMode_List.Count; i++)
        {
            AttackMode_List[i].function.Invoke();            
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
    /// <param name="knockDirection">���h��V</param>
    /// <param name="repel">���h�Z��</param>
    /// <param name="attackerObjectID">�����̪���ID</param>
    public void OnConnectGetHit(int targetID, Vector3 position, Quaternion rotation, float damage, bool isCritical, int knockDirection, float repel, int attackerObjectID)
    {       
        CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
        if (collision != null)
        {
            GameObject attackObj = connectObject_Dictionary[attackerObjectID].gameObject;
            collision.OnConnectOtherGetHit(position, rotation, damage, isCritical, knockDirection, repel, attackObj);
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
