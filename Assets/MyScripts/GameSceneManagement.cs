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

    //�ĤH�X���I
    Transform[] enemySoldiers1_Stage1Point;//�ĤH�h�L1_���q1�X���I
    Transform[] enemySoldiers2_Stage1Point;//�ĤH�h�L2_���q1�X���I
    Transform[] guardBoss_Stage2Point;//��������Boss_���q2�X���I
    Transform[] enemySoldiers1_Stage3Point;//�ĤH�h�L1_���q3�X���I
    Transform[] enemySoldiers2_Stage3Point;//�ĤH�h�L2_���q3�X���I
    Transform[] enemySoldiers3_Stage3Point;//�ĤH�h�L3_���q3�X���I

    //�ڤ�X���I
    Transform[] allianceSoldier1_Stage1Point;//�ڤ�h�L1_���q1�X���I

    //����
    string[] taskText;//�U���q���Ȥ�r
    public int taskStage;//�ثe���ȶ��q
    public int[] taskNeedNumber;//�U���q���ȩһݼƶq
    public int taskNumber;//�w�������ȼƶq
    GameObject strongholdStage3;//��3���q���I    
    public bool isCreateBoss;//�O�_�w�Ы�Boss

    //�i�����
    float gateSpeed;//�������ʳt��
    [SerializeField] bool[] stageGateOpen;//�Ӷ��q�����}�Ҫ��A
    [SerializeField] GameObject stage1_Gate;//���q1����

    void Awake()
    {
        if (gameSceneManagement != null)
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

        //�i�����
        stageGateOpen = new bool[] { false };//�Ӷ��q�����}�Ҫ��A
        gateSpeed = 1;//�������ʳt��
        stage1_Gate = GameObject.Find("Stage1_Gate");//���q1����

        int number = 0;

        //���a�}��
        number = objectHandle.OnCreateObject(loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//���ͦܪ����
        objectNumber_Dictionary.Add("playerNumbering", number);//�K�[�ܬ�����
        GameObject player = OnRequestOpenObject(OnGetObjectNumber("playerNumbering"), loadPath.allPlayerCharacters[GameDataManagement.Instance.selectRoleNumber]);//�}�Ҫ���
        OnSetMiniMapPoint(player.transform, loadPath.miniMapMatirial_Player);//�]�w�p�a���I�I           
        if (!GameDataManagement.Instance.isConnect)//���s�u��m
        {
            if (GameDataManagement.Instance.selectLevelNumber == 11)//��1��
            {
                player.transform.position = new Vector3(300f, -24f, -29f);
                player.transform.rotation = Quaternion.Euler(0, -85, 0);//�]�w����
            }
            if (GameDataManagement.Instance.selectLevelNumber == 12)//��2��
            {
                player.transform.position = new Vector3(32, -4f, -15f);
                player.transform.rotation = Quaternion.Euler(0, 270, 0);//�]�w����
            }
        }
        else//�s�u��m
        {
            if (GameDataManagement.Instance.selectLevelNumber == 11)//��1��
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                    {
                        player.transform.position = new Vector3(317f, -23.9f, -29f + (i * 2.5f));
                        player.transform.rotation = Quaternion.Euler(0, -60, 0);//�]�w����
                    }
                }
            }
            if (GameDataManagement.Instance.selectLevelNumber == 12)//��2��
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                    {
                        player.transform.position = new Vector3(32, -4f, -15f + (i * 1.5f));
                        player.transform.rotation = Quaternion.Euler(0, -60, 0);//�]�w����
                    }
                }
            }
        }

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

        //Boss����
        number = objectHandle.OnCreateObject(loadPath.bossAttack1);//����1����(�������)
        objectNumber_Dictionary.Add("bossAttack1", number);//�K�[�ܬ�����

        #region ��1��
        if (GameDataManagement.Instance.selectLevelNumber == 11)
        {
            #region ���ͤh�L
            //���ͦP���h�L1
            number = objectHandle.OnCreateObject(loadPath.allianceSoldier_1);//���ͦܪ����
            objectNumber_Dictionary.Add("allianceSoldier_1", number);////�K�[�ܬ�����

            //���ͼĤH�h�L1
            number = objectHandle.OnCreateObject(loadPath.enemySoldier_1);//���ͦܪ����
            objectNumber_Dictionary.Add("enemySoldier_1", number);////�K�[�ܬ�����

            //���ͼĤH�h�L2
            number = objectHandle.OnCreateObject(loadPath.enemySoldier_2);//���ͦܪ����
            objectNumber_Dictionary.Add("enemySoldier_2", number);////�K�[�ܬ�����

            //���ͼĤH�h�L3
            number = objectHandle.OnCreateObject(loadPath.enemySoldier_3);//���ͦܪ����
            objectNumber_Dictionary.Add("enemySoldier_3", number);////�K�[�ܬ�����

            //���ͫ����u��Boss     
            number = objectHandle.OnCreateObject(loadPath.guardBoss);//���ͦܪ����
            objectNumber_Dictionary.Add("enemyGuardBoss", number);////�K�[�ܬ�����
            #endregion

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
            guardBoss_Stage2Point = new Transform[GameObject.Find("GuardBoss_Stage2Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("GuardBoss_Stage2Point").transform.childCount; i++)
            {
                guardBoss_Stage2Point[i] = GameObject.Find("GuardBoss_Stage2Point").transform.GetChild(i);
            }
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

            #region �ڤ�P���X���I
            allianceSoldier1_Stage1Point = new Transform[GameObject.Find("AllianceSoldier1_Stage1Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("AllianceSoldier1_Stage1Point").transform.childCount; i++)
            {
                allianceSoldier1_Stage1Point[i] = GameObject.Find("AllianceSoldier1_Stage1Point").transform.GetChild(i);
            }
            #endregion

            //��l���q�ЫؼĤH
            OnInitialCreateEnemy();

            //����
            taskNumber = -1;//�w�������ȼƶq
            taskText = new string[] { "���}�ӰϩҦ����I", "���˫����u��", "���}�򤤫�������", "���}�����Ҧ����I" };//�Ӷ��q���Ȥ�r
                                                                                     //�U���q���ȩһ�������
            taskNeedNumber = new int[] { 2,//���q1
                                     guardBoss_Stage2Point.Length,//���q2
                                     1,//���q3
                                     1};//���q4

            //���ȴ���
            StartCoroutine(OnTaskTipText(taskTipValue: taskText[taskStage].ToString()));

            //���Ȥ�r
            OnTaskText();

            //���Ȫ���
            strongholdStage3 = GameObject.Find("Stronghold_Enemy3");//��3���q���I
            strongholdStage3.SetActive(false);
        }
        #endregion

        #region ��2��
        if (GameDataManagement.Instance.selectLevelNumber == 12)
        {
            //�D�s�u || �O�ХD
            if (!GameDataManagement.Instance.isConnect || PhotonNetwork.IsMasterClient)
            {
                GameSceneManagement.Instance.OnCreateBoss();
            }
        }
        #endregion
    }

    void Update()
    {
        OnAttackBehavior();//�����欰
        OnGate();//�i�����
    }

    #region ����

    /// <summary>
    /// ����Boss
    /// </summary>
    public void OnCreateBoss()
    {
        //�u��Boss����
        int number = objectHandle.OnCreateObject(loadPath.boss);//����1����
        objectNumber_Dictionary.Add("boss", number);//�K�[�ܬ�����
        GameObject AIObject = OnRequestOpenObject(OnGetObjectNumber("boss"), loadPath.boss);//�}�Ҫ���
        AIObject.transform.position = new Vector3(4.8f, 2.5f, -3f);//�]�w��m
        //AIObject.transform.position = new Vector3(76f, -11f, -28f);//�]�w��m
        AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        AIObject.tag = "Enemy";//�]�wTag
        AIObject.layer = LayerMask.NameToLayer("Boss");//�]�wLayer
        OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
    }

    /// <summary>
    /// �i�����
    /// </summary>
    void OnGate()
    {
        if (taskStage == 3)//��3���q�L��
        {
            if (stage1_Gate.transform.position.y < -12)
            {
                //���a�b�d��
                if (Physics.CheckSphere(stage1_Gate.transform.position, 20, 1 << LayerMask.NameToLayer("Player")))
                {
                    stageGateOpen[0] = true;
                }

                //���q1�����}��
                if (stageGateOpen[0]) stage1_Gate.transform.position = stage1_Gate.transform.position + Vector3.up * gateSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// ��l���q�ЫؼĤH/���Ȭy�{
    /// </summary>
    void OnInitialCreateEnemy()
    {
        //�D�s�u || �O�ХD
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            //���d1
            if (GameDataManagement.Instance.selectLevelNumber == 11)
            {
                //int number = 0;
                GameObject AIObject = null;

                //�P�_�ثe���ȶ��q
                switch (taskStage)
                {
                    case 0://���q1
                           // ���ͦP���h�L1                       
                        for (int i = 0; i < allianceSoldier1_Stage1Point.Length; i++)
                        {
                            AIObject = OnRequestOpenObject(OnGetObjectNumber("allianceSoldier_1"), loadPath.allianceSoldier_1);//�}�Ҫ���
                            AIObject.transform.position = allianceSoldier1_Stage1Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Alliance";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Alliance");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��
                            OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_OtherPlayer);//�]�w�p�a���I�I
                        }

                        //���ͼĤH�h�L1
                        for (int i = 0; i < enemySoldiers1_Stage1Point.Length; i++)
                        {
                            AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//�}�Ҫ���
                            AIObject.transform.position = enemySoldiers1_Stage1Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��   
                            OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }
                        //���ͼĤH�h�L2                      
                        for (int i = 0; i < enemySoldiers2_Stage1Point.Length; i++)
                        {
                            AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_1);//�}�Ҫ���
                            AIObject.transform.position = enemySoldiers2_Stage1Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��
                            OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }
                        break;
                    case 1://���q2                       
                           //���ͫ����u��Boss
                        for (int i = 0; i < guardBoss_Stage2Point.Length; i++)
                        {
                            AIObject = OnRequestOpenObject(OnGetObjectNumber("enemyGuardBoss"), loadPath.guardBoss);//�}�Ҫ���
                            AIObject.transform.position = guardBoss_Stage2Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��    
                            OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_TaskObject);//�]�w�p�a���I�I
                        }
                        break;
                    case 2://���q3 
                        //�}�Ҿ���
                        strongholdStage3.SetActive(true);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(strongholdStage3, true);
                        break;
                    case 3://���q4

                        break;
                }
            }

            //���d2
            if (GameDataManagement.Instance.selectLevelNumber == 12)
            {

            }
        }
    }

    /// <summary>
    /// ���ͤh�L
    /// </summary>
    /// <param name="createPoint"></param>
    /// <param name="objTag"></param>
    public void OnCreateSoldier(Transform createPoint, string objTag)
    {
        //�D�s�u || �O�ХD
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            #region �ڤ���I
            if (objTag == "Alliance")
            {
                //���ͼĤH�h�L1
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(OnDelayCreateSoldier_Alliance("allianceSoldier_1", loadPath.allianceSoldier_1, createPoint, objTag, i, UnityEngine.Random.Range(0.0f, 1.5f)));
                }
            }
            #endregion

            //�P�_�ثe���ȶ��q
            switch (taskStage)
            {
                case 0://���q1
                    #region �ĤH���I
                    if (objTag == "Enemy")
                    {
                        //���ͼĤH�h�L1
                        for (int i = 0; i < 3; i++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_1", loadPath.enemySoldier_1, createPoint, objTag, i, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //���ͼĤH�h�L2
                        for (int j = 3; j < 4; j++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_2", loadPath.enemySoldier_2, createPoint, objTag, j, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                    }
                    #endregion                    
                    break;
                case 3://���q4
                    #region �ĤH���I
                    if (objTag == "Enemy")
                    {
                        //���ͼĤH�h�L1
                        for (int i = 0; i < 3; i++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_1", loadPath.enemySoldier_1, createPoint, objTag, i, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //���ͼĤH�h�L2
                        for (int j = 3; j < 4; j++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_2", loadPath.enemySoldier_2, createPoint, objTag, j, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                    }
                    #endregion       
                    break;
            }
        }
    }

    /// <summary>
    /// ���𲣥ͤh�L_�ڤ�
    /// </summary>
    /// <param name="soldierName">�h�L�W��</param>
    /// <param name="soldierPath">�h�L���|</param>
    /// <param name="createPoint">���;��I</param>
    /// <param name="objTag">���ITag</param>
    /// <param name="number">�ثe�ƶq</param>
    /// <param name="waitTime">���ݮɶ�</param>
    /// <returns></returns>
    IEnumerator OnDelayCreateSoldier_Alliance(string soldierName, string soldierPath, Transform createPoint, string objTag, int number, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        GameObject AIObject = null;
        AIObject = OnRequestOpenObject(OnGetObjectNumber(soldierName), soldierPath);//�}�Ҫ���
        AIObject.transform.position = createPoint.position + createPoint.forward * (-2 + (number * 2f));//�]�w��m
        AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        AIObject.tag = objTag;//�]�wTag
        AIObject.layer = LayerMask.NameToLayer(objTag);//�]�wLayer         
        AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
        AIObject.GetComponent<AI>().OnInitial();//��l��
    }

    /// <summary>
    /// ���𲣥ͤh�L_�Ĥ�
    /// </summary>
    /// <param name="soldierName">�h�L�W��</param>
    /// <param name="soldierPath">�h�L���|</param>
    /// <param name="createPoint">���;��I</param>
    /// <param name="objTag">���ITag</param>
    /// <param name="number">�ثe�ƶq</param>
    /// <param name="waitTime">���ݮɶ�</param>
    /// <returns></returns>
    IEnumerator OnDelayCreateSoldier_Enemy(string soldierName, string soldierPath, Transform createPoint, string objTag, int number, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        GameObject AIObject = null;
        AIObject = OnRequestOpenObject(OnGetObjectNumber(soldierName), soldierPath);//�}�Ҫ���
        //AIObject.transform.position = createPoint.position + createPoint.forward * (10 + (number * 2f));//�]�w��m
        AIObject.transform.position = (createPoint.position + createPoint.forward * 10) + (createPoint.right * (number * 1.5f));//�]�w��m
        AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        AIObject.tag = objTag;//�]�wTag
        AIObject.layer = LayerMask.NameToLayer(objTag);//�]�wLayer         
        AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
        AIObject.GetComponent<AI>().OnInitial();//��l��
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
        taskNumber++;//�w�������ȼƶq

        //���ȧP�w
        OnJudgeTask();

        //�]�w���Ȥ�r
        if (taskStage < taskNeedNumber.Length)//���L��
        {
            GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n�ؼ�: " + taskNumber + " / " + taskNeedNumber[taskStage]);
        }
    }

    /// <summary>
    /// ���ȧP�w
    /// </summary>
    void OnJudgeTask()
    {
        //�w�����Ǫ��ƶq >= ���ȩһ�������
        if (taskNumber >= taskNeedNumber[taskStage])
        {
            taskStage += 1;//�ثe���ȶ��q

            if (taskStage >= taskNeedNumber.Length)//�L��
            {
                //StartCoroutine(OnTaskTipText(taskTipValue: "�ӧQ"));//���ȴ���                  
                GameSceneUI.Instance.OnSetGameResult(true, "�ӧQ");
                StartCoroutine(OnSetGameOver(true));//�]�w�C������
            }
            else//�i�J�U���q
            {
                taskNumber = 0;//�w�������ȼƶq                                
                StartCoroutine(OnTaskTipText(taskTipValue: taskText[taskStage]));//���ȴ���   
                GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n�ؼ�: " + taskNumber + " / " + taskNeedNumber[taskStage]);

                //��l���q�ЫؼĤH
                OnInitialCreateEnemy();
            }
        }
    }

    /// <summary>
    /// �]�w�C������
    /// </summary>
    /// <param name="result">���G</param>
    /// <returns></returns>
    public IEnumerator OnSetGameOver(bool result)
    {
        //�C��������������
        GameSceneUI.Instance.OnGameOverCloseObject();

        yield return new WaitForSeconds(3);
        GameSceneUI.Instance.OnSetGameResult(false, "");

        //�]�w�C������UI
        GameSceneUI.Instance.OnSetGameOverUI(clearance: result);
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
            if (obj.Key == objectNmae)
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

        /*//Size
        if (item.gameObject.layer != LayerMask.NameToLayer("Player") && item.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            Vector3 itemBoxSize = item.GetComponent<BoxCollider>().size;
            obj.transform.localScale = new Vector3(itemBoxSize.x, itemBoxSize.z, 1);
        }
        else obj.transform.localScale = new Vector3(5, 5, 5);*/
        obj.transform.localScale = new Vector3(5, 5, 5);


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
    /// �s�u���I����
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="damage">����ˮ`</param>
    public void OnConnectStrongholdGetHit(int targetID, float damage)
    {
        Stronghold stronghold = connectObject_Dictionary[targetID].GetComponent<Stronghold>();

        if (stronghold != null)
        {
            stronghold.OnConnectGetHit(damage);
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
