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
    public Dictionary<int, GameObject> connectObject_Dictionary = new Dictionary<int, GameObject>();//�O���Ҧ��s�u����
    public List<AttackMode> AttackMode_List = new List<AttackMode>();//�����Ҧ������欰    
    public List<int> AllPlayerID_List = new List<int>();//�����s�u���aID
    public GameObject thisPlayerObject;//���a���a����

    //�ĤH�X���I
    Transform[] enemySoldiers1_Stage1Point;//�ĤH�h�L1_���q1�X���I
    Transform[] enemySoldiers2_Stage1Point;//�ĤH�h�L2_���q1�X���I
    Transform[] guardBoss_Stage2Point;//��������Boss_���q2�X���I
    Transform[] enemySoldiers1_Stage4Point;//�ĤH�h�L1_���q3�X���I
    Transform[] enemySoldiers2_Stage4Point;//�ĤH�h�L2_���q3�X���I
    Transform[] enemySoldiers3_Stage4Point;//�ĤH�h�L3_���q3�X���I
    Transform[] enemySoldiers2_Stage5Point;//�ĤH�h�L2_���q4�X���I
    Transform[] enemySoldiers3_Stage5Point;//�ĤH�h�L3_���q4�X���I

    //�ڤ�X���I
    Transform[] allianceSoldier1_Stage1Point;//�ڤ�h�L1_���q1�X���I
    Transform[] allianceSoldier1_Stage4Point;//�ڤ�h�L1_���q4�X���I

    //��2���X���I
    [SerializeField]Transform[] brithPoint_Level12;
    float level12CreateSoldierTime;//���ͮɶ�
    [SerializeField] float level12SoldierTime;//���ͮɶ�(�p�ɾ�)

    //����    
    public bool isGameOver;//�O�_�C������
    public bool isVictory;//�O�_�L��
    public string[] taskText;//�U���q���Ȥ�r
    public string[] tipTaskText;//���ܥ��Ȥ�r
    public int taskStage;//�ثe���ȶ��q
    public int[] taskNeedNumber;//�U���q���ȩһݼƶq
    public int taskNumber;//�w�������ȼƶq
    GameObject strongholdStage3;//��3���q���I
    public GameObject[] strongholdStage4;//��4���q���I
    GameObject strongholdStage5;//��5���q���I
                                
    public bool isCreateBoss;//�O�_�w�Ы�Boss
    public int lifePlayerNumber;//�ͦs�����a�ƶq    

    //�i�����
    float gateSpeed;//�������ʳt��
    bool[] stageGateOpen;//�Ӷ��q�����}�Ҫ��A
    GameObject stage1_Gate;//���q1����

    public Color thiscolor;

    [Header("�S�O�ϥ�")]
    public GameObject BossTargetObject;//Boss�l�H�ؼЪ���

    void Awake()
    {
        Time.timeScale = 1;

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
                player.transform.position = new Vector3(348f, -23.8f, -25f);
                player.transform.rotation = Quaternion.Euler(0, -85, 0);//�]�w����
            }
            if (GameDataManagement.Instance.selectLevelNumber == 12)//��2��
            {
                player.transform.position = new Vector3(17, -0.5f, -15f);
                player.transform.rotation = Quaternion.Euler(0, 270, 0);//�]�w����
            }
        }
        else//�s�u��m
        {
            lifePlayerNumber = PhotonNetwork.CurrentRoom.PlayerCount;//�ͦs�����a�ƶq

            if (GameDataManagement.Instance.selectLevelNumber == 11)//��1��
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                    {
                        player.transform.position = new Vector3(345, -23.9f, -27.8f + (i * 1.5f));
                        player.transform.rotation = Quaternion.Euler(0, -85, 0);//�]�w����
                    }
                }
            }
            if (GameDataManagement.Instance.selectLevelNumber == 12)//��2��
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                    {
                        player.transform.position = new Vector3(17, -0.5f, -15 + (i * 1.5f));
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
        number = objectHandle.OnCreateObject(loadPath.bossAttack1);//����1����
        objectNumber_Dictionary.Add("bossAttack1", number);//�K�[�ܬ�����
        number = objectHandle.OnCreateObject(loadPath.bossAttack2);//����2����
        objectNumber_Dictionary.Add("bossAttack2", number);//�K�[�ܬ�����
        number = objectHandle.OnCreateObject(loadPath.bossAttack3);//����3����
        objectNumber_Dictionary.Add("bossAttack3", number);//�K�[�ܬ�����
        number = objectHandle.OnCreateObject(loadPath.bossAttack4);//����4����
        objectNumber_Dictionary.Add("bossAttack4", number);//�K�[�ܬ�����

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

        #region ��1��
        if (GameDataManagement.Instance.selectLevelNumber == 11)
        {
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
            //�ĤH�h�L1_���q4�X���I
            enemySoldiers1_Stage4Point = new Transform[GameObject.Find("EnemySoldiers1_Stage4Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers1_Stage4Point").transform.childCount; i++)
            {
                enemySoldiers1_Stage4Point[i] = GameObject.Find("EnemySoldiers1_Stage4Point").transform.GetChild(i);
            }

            //�ĤH�h�L2_���q4�X���I
            enemySoldiers2_Stage4Point = new Transform[GameObject.Find("EnemySoldiers2_Stage4Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers2_Stage4Point").transform.childCount; i++)
            {
                enemySoldiers2_Stage4Point[i] = GameObject.Find("EnemySoldiers2_Stage4Point").transform.GetChild(i);
            }

            //�ĤH�h�L3_���q3�X���I
            enemySoldiers3_Stage4Point = new Transform[GameObject.Find("EnemySoldiers3_Stage4Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers3_Stage4Point").transform.childCount; i++)
            {
                enemySoldiers3_Stage4Point[i] = GameObject.Find("EnemySoldiers3_Stage4Point").transform.GetChild(i);
            }

            //�ڤ�h�L1_���q4�X���I
            allianceSoldier1_Stage4Point = new Transform[GameObject.Find("AllianceSoldier1_Stage4Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("AllianceSoldier1_Stage4Point").transform.childCount; i++)
            {
                allianceSoldier1_Stage4Point[i] = GameObject.Find("AllianceSoldier1_Stage4Point").transform.GetChild(i);
            }

            //�ĤH�h�L2_���q4�X���I
            enemySoldiers2_Stage5Point = new Transform[GameObject.Find("EnemySoldiers2_Stage5Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers2_Stage5Point").transform.childCount; i++)
            {
                enemySoldiers2_Stage5Point[i] = GameObject.Find("EnemySoldiers2_Stage5Point").transform.GetChild(i);
            }

            //�ĤH�h�L3_���q4�X���I
            enemySoldiers3_Stage5Point = new Transform[GameObject.Find("EnemySoldiers3_Stage5Point").transform.childCount];
            for (int i = 0; i < GameObject.Find("EnemySoldiers3_Stage5Point").transform.childCount; i++)
            {
                enemySoldiers3_Stage5Point[i] = GameObject.Find("EnemySoldiers3_Stage5Point").transform.GetChild(i);
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
            tipTaskText = new string[] { "�e�i�Գ�\n���}�ӰϾ��I", "���˫����u��", "���}��������", "�i�J����\n���}�Ҧ����I", "�e�i�}�a�s�ڤ���" };//���ܥ��Ȥ�r
            taskText = new string[] { "�w���}���I : ", "�w���˦u�� : ", "�w���}���� : ", "�w���}���I : ", "�w�}�a���� : " };//�Ӷ��q���Ȥ�r
            //�U���q���ȩһ�������
            taskNeedNumber = new int[] { 2,//���q1
                                     guardBoss_Stage2Point.Length,//���q2
                                     1,//���q3
                                     3,//���q4
                                     1};//���q5

            //���ȴ���
            StartCoroutine(OnTaskTipText(taskTipValue: tipTaskText[taskStage].ToString()));

            //���Ȥ�r
            OnTaskText();

            //���Ȫ���
            strongholdStage3 = GameObject.Find("Stronghold_Enemy3");//��3���q���I
            strongholdStage3.SetActive(false);
            for (int i = 0; i < strongholdStage4.Length; i++)//��4���q���I
            {
                strongholdStage4[i].SetActive(false);
            }
            strongholdStage5 = GameObject.Find("Stronghold_Enemy7");//��5���q���I
            strongholdStage5.SetActive(false);
        }
        #endregion

        #region ��2��
        if (GameDataManagement.Instance.selectLevelNumber == 12)
        {
            //�D�s�u || �O�ХD
            if (!GameDataManagement.Instance.isConnect || PhotonNetwork.IsMasterClient)
            {
                brithPoint_Level12 = new Transform[GameObject.Find("BrithPoint_Level12").transform.childCount];
                for (int i = 0; i < brithPoint_Level12.Length; i++)
                {
                    brithPoint_Level12[i] = GameObject.Find("BrithPoint_Level12").transform.GetChild(i);
                }

                level12CreateSoldierTime = 10;//���ͮɶ�
                level12SoldierTime = level12CreateSoldierTime;//���ͮɶ�(�p�ɾ�)

                GameSceneManagement.Instance.OnCreateBoss();//����boss
            }

            taskNumber = -1;//�w�������ȼƶq
            tipTaskText = new string[] { "�� �� �� �s" };//���ܥ��Ȥ�r
            taskText = new string[] { "�� �� �� �s" };//�Ӷ��q���Ȥ�r
            //�U���q���ȩһ�������
            taskNeedNumber = new int[] { 1 };//���q1

            //���ȴ���
            StartCoroutine(OnTaskTipText(taskTipValue: tipTaskText[taskStage].ToString()));

            //���Ȥ�r
            OnTaskText();
        }
        #endregion
    }

    void Update()
    {
        OnAttackBehavior();//�����欰
        OnGate();//�i�����
        OnLevel12CreateSoldier();//��2�����ͤh�L
    }

    #region ����
    /// <summary>
    /// ��2�����ͤh�L
    /// </summary>
   void OnLevel12CreateSoldier()
    {
        if (GameDataManagement.Instance.selectLevelNumber == 12 && !isGameOver)
        {
            //�D�s�u || �O�ХD
            if (!GameDataManagement.Instance.isConnect || PhotonNetwork.IsMasterClient)
            {
                level12SoldierTime -= Time.deltaTime;//���ͮɶ�(�p�ɾ�)

                if(level12SoldierTime <= 0)
                {
                    level12SoldierTime = level12CreateSoldierTime;

                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, brithPoint_Level12[0], "Enemy"));
                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, brithPoint_Level12[1], "Enemy"));
                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, brithPoint_Level12[2], "Enemy"));
                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_3", loadPath.enemySoldier_3, brithPoint_Level12[3], "Enemy"));
                    StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_3", loadPath.enemySoldier_3, brithPoint_Level12[4], "Enemy"));
                    
                }
            }
        }
    }

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
        //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_TaskObject);//�]�w�p�a���I�I 
    }

    /// <summary>
    /// �i�����
    /// </summary>
    void OnGate()
    {
        if (taskStage == 3)//��3���q�L��
        {
            //if (stage1_Gate.GetComponent<BoxCollider>().enabled) stage1_Gate.GetComponent<BoxCollider>().enabled = false;
            if (stage1_Gate.transform.position.y < -12)
            {
                //���a�b�d��
                if (Physics.CheckSphere(stage1_Gate.transform.position, 20, 1 << LayerMask.NameToLayer("Player")))
                {
                    if (!stageGateOpen[0])
                    {
                        stage1_Gate.GetComponent<AudioSource>().Play();
                        stageGateOpen[0] = true;
                    }
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
                //GameObject AIObject = null;

                //�P�_�ثe���ȶ��q
                switch (taskStage)
                {
                    case 0://���q1
                        // ���ͦP���h�L1                                                    
                        for (int i = 0; i < allianceSoldier1_Stage1Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("allianceSoldier_1", loadPath.allianceSoldier_1, allianceSoldier1_Stage1Point[i], "Alliance"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("allianceSoldier_1"), loadPath.allianceSoldier_1);//�}�Ҫ���
                            AIObject.transform.position = allianceSoldier1_Stage1Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Alliance";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Alliance");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��*/
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_OtherPlayer);//�]�w�p�a���I�I
                        }
                        //���ͼĤH�h�L1
                        for (int i = 0; i < enemySoldiers1_Stage1Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, enemySoldiers1_Stage1Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//�}�Ҫ���
                            AIObject.transform.position = enemySoldiers1_Stage1Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��   */
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }
                        //���ͼĤH�h�L2                      
                        for (int i = 0; i < enemySoldiers2_Stage1Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_2", loadPath.enemySoldier_2, enemySoldiers2_Stage1Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_2);//�}�Ҫ���
                            AIObject.transform.position = enemySoldiers2_Stage1Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��*/
                           // OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }
                        break;
                    case 1://���q2                       
                           //���ͫ����u��Boss
                        for (int i = 0; i < guardBoss_Stage2Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemyGuardBoss", loadPath.guardBoss, guardBoss_Stage2Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemyGuardBoss"), loadPath.guardBoss);//�}�Ҫ���
                            AIObject.transform.position = guardBoss_Stage2Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��    */
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_TaskObject);//�]�w�p�a���I�I                            
                        }
                        break;
                    case 2://���q3 
                        //�}�Ҿ���
                        strongholdStage3.SetActive(true);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(strongholdStage3, true);
                        break;
                    case 3://���q4
                        for (int i = 0; i < strongholdStage4.Length; i++)//��4���q���I
                        {
                            strongholdStage4[i].SetActive(true);
                            if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(strongholdStage4[i], true);
                        }
                        // ���ͦP���h�L1                       
                        for (int i = 0; i < allianceSoldier1_Stage4Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("allianceSoldier_1", loadPath.allianceSoldier_1, allianceSoldier1_Stage4Point[i], "Alliance"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("allianceSoldier_1"), loadPath.allianceSoldier_1);//�}�Ҫ���
                            AIObject.transform.position = allianceSoldier1_Stage4Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Alliance";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Alliance");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��*/
                           // OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_OtherPlayer);//�]�w�p�a���I�I
                        }
                        //���ͼĤH�h�L1
                        for (int i = 0; i < enemySoldiers1_Stage4Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_1", loadPath.enemySoldier_1, enemySoldiers1_Stage4Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_1"), loadPath.enemySoldier_1);//�}�Ҫ���
                            AIObject.transform.position = enemySoldiers1_Stage4Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��   */
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }
                        //���ͼĤH�h�L2                      
                        for (int i = 0; i < enemySoldiers2_Stage4Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_2", loadPath.enemySoldier_2, enemySoldiers2_Stage4Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_2);//�}�Ҫ���
                            AIObject.transform.position = enemySoldiers2_Stage4Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��*/
                            //OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }
                        //���ͼĤH�h�L3                      
                        for (int i = 0; i < enemySoldiers3_Stage4Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_3", loadPath.enemySoldier_3, enemySoldiers3_Stage4Point[i], "Enemy"));
                           /* AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_3"), loadPath.enemySoldier_3);//�}�Ҫ���
                            AIObject.transform.position = enemySoldiers3_Stage4Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��*/
                           // OnSetMiniMapPoint(AIObject.transform, loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
                        }
                        break;
                    case 4://���q5 
                        //���}����
                        strongholdStage5.SetActive(true);
                        if (GameDataManagement.Instance.isConnect) PhotonConnect.Instance.OnSendObjectActive(strongholdStage5, true);

                        //���ͼĤH�h�L2                      
                        for (int i = 0; i < enemySoldiers2_Stage5Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_2", loadPath.enemySoldier_2, enemySoldiers2_Stage5Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_2"), loadPath.enemySoldier_2);//�}�Ҫ���
                            AIObject.transform.position = enemySoldiers2_Stage5Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l��                                                   */
                        }

                        //���ͼĤH�h�L3                      
                        for (int i = 0; i < enemySoldiers3_Stage5Point.Length; i++)
                        {
                            StartCoroutine(OnDelayCreateInitalSoldier("enemySoldier_3", loadPath.enemySoldier_3, enemySoldiers3_Stage5Point[i], "Enemy"));
                            /*AIObject = OnRequestOpenObject(OnGetObjectNumber("enemySoldier_3"), loadPath.enemySoldier_3);//�}�Ҫ���
                            AIObject.transform.position = enemySoldiers3_Stage5Point[i].position;//�]�w��m
                            AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            AIObject.tag = "Enemy";//�]�wTag
                            AIObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer
                            AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
                            AIObject.GetComponent<AI>().OnInitial();//��l�� */                                                  
                        }
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
                for (int i = 0; i < 2; i++)
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
                        for (int j = 2; j < 4; j++)
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
                        for (int i = 0; i < 1; i++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_1", loadPath.enemySoldier_1, createPoint, objTag, i, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //���ͼĤH�h�L2
                        for (int j = 1; j < 2; j++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_2", loadPath.enemySoldier_2, createPoint, objTag, j, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //���ͼĤH�h�L3
                        for (int k = 2; k < 4; k++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_3", loadPath.enemySoldier_3, createPoint, objTag, k, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                    }
                    #endregion       
                    break;
                case 4://���q5
                    #region �ĤH���I
                    if (objTag == "Enemy")
                    {
                        //���ͼĤH�h�L1
                        for (int i = 0; i < 1; i++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_1", loadPath.enemySoldier_1, createPoint, objTag, i, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //���ͼĤH�h�L2
                        for (int j = 1; j < 2; j++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_2", loadPath.enemySoldier_2, createPoint, objTag, j, UnityEngine.Random.Range(0.0f, 1.5f)));
                        }
                        //���ͼĤH�h�L3
                        for (int k = 2; k < 5; k++)
                        {
                            StartCoroutine(OnDelayCreateSoldier_Enemy("enemySoldier_3", loadPath.enemySoldier_3, createPoint, objTag, k, UnityEngine.Random.Range(0.0f, 1.5f)));
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
        AIObject.transform.position = createPoint.position + createPoint.forward * (-2 + (number * 1.5f));//�]�w��m
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
        //AIObject.transform.position = (createPoint.position + createPoint.forward * 10) + (createPoint.right * (number * 1.5f));//�]�w��m
        AIObject.transform.position = (createPoint.position + createPoint.forward * 10 ) + (createPoint.right * (number * -1.5f)) + (Vector3.up * 1);//�]�w��m
        AIObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        AIObject.tag = objTag;//�]�wTag
        AIObject.layer = LayerMask.NameToLayer(objTag);//�]�wLayer         
        AIObject.GetComponent<CharactersCollision>().OnInitial();//��l��
        AIObject.GetComponent<AI>().OnInitial();//��l��
    }

    /// <summary>
    /// ���𲣥ͪ�l�h�L
    /// </summary>
    /// <param name="soldierName">�h�L�W��</param>
    /// <param name="soldierPath">�h�L���|</param>
    /// <param name="createPoint">�����I</param>
    /// <param name="objTag">ObjTag</param>
    /// <returns></returns>
    IEnumerator OnDelayCreateInitalSoldier(string soldierName, string soldierPath, Transform createPoint, string objTag)
    {
        yield return new WaitForSeconds(0.1f);

        GameObject AIObject = null;
        AIObject = OnRequestOpenObject(OnGetObjectNumber(soldierName), soldierPath);//�}�Ҫ���
        //AIObject.transform.position = createPoint.position + createPoint.forward * (10 + (number * 2f));//�]�w��m
        //AIObject.transform.position = (createPoint.position + createPoint.forward * 10) + (createPoint.right * (number * 1.5f));//�]�w��m
        AIObject.transform.position = createPoint.position;//�]�w��m
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
            GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n" + taskNumber + " / " + taskNeedNumber[taskStage]);
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
                AI[] ais = GameObject.FindObjectsOfType<AI>();
                for (int i = 0; i < ais.Length; i++)
                {
                    ais[i].gameObject.SetActive(false);
                }

                
                GameSceneUI.Instance.OnSetGameResult(true, "�� �Q");
                StartCoroutine(OnSetGameOver(true));//�]�w�C������
            }
            else//�i�J�U���q
            {
                taskNumber = 0;//�w�������ȼƶq                                
                StartCoroutine(OnTaskTipText(taskTipValue: tipTaskText[taskStage]));//���ȴ���   
                GameSceneUI.Instance.OnSetTaskText(taskValue: taskText[taskStage] + "\n" + taskNumber + " / " + taskNeedNumber[taskStage]);

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

        //�s�u
        if(GameDataManagement.Instance.isConnect)
        {
            PhotonConnect.Instance.OnSendGameScoring(PhotonNetwork.NickName, GameSceneUI.Instance.MaxCombo, GameSceneUI.Instance.killNumber, GameSceneUI.Instance.accumulationDamage);
            if (PhotonNetwork.IsMasterClient) PhotonConnect.Instance.OnSendGameTime(GameSceneUI.Instance.playerGameTime);
        }

        yield return new WaitForSeconds(4);

        PhotonNetwork.AutomaticallySyncScene = true;//�۰ʦP�B����
        isGameOver = true;
        isVictory = result;//�O�_�L��
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
        obj.transform.localPosition = new Vector3(itemBoxCenter.x, -1, itemBoxCenter.z);

        /*//Size
        if (item.gameObject.layer != LayerMask.NameToLayer("Player") && item.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            Vector3 itemBoxSize = item.GetComponent<BoxCollider>().size;
            obj.transform.localScale = new Vector3(itemBoxSize.x, itemBoxSize.z, 1);
        }
        else obj.transform.localScale = new Vector3(5, 5, 5);*/

        if (item.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            obj.transform.localScale = new Vector3(12, 12, 12);//���a
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, 0, obj.transform.localPosition.z);
        }
        else if (item.GetComponent<CharactersCollision>().isTaskObject)
        {
            obj.transform.localScale = new Vector3(25, 25, 25);//����
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, -2, obj.transform.localPosition.z);
        }
        else
        {
            obj.transform.localScale = new Vector3(10, 10, 10);
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, -3, obj.transform.localPosition.z);
        }


        //����
        obj.transform.localEulerAngles = new Vector3(90, 0, 0);

        //����y(�C��)
        obj.GetComponent<Renderer>().material = Resources.Load<Material>(materialPath);
    }
    #endregion

    #region �s�u
    /// <summary>
    /// �]�wBoss�ؼЪ���
    /// </summary>
    /// <param name="id"></param>
    public void OnSetBossTarget(int id)
    {
        BossTargetObject = connectObject_Dictionary[id];
    }

    /// <summary>
    /// �����Ҧ��s�u���a
    /// </summary>
    /// <param name="id">����ID</param>
    public void OnRecordConnectPlayer(int id)
    {
        AllPlayerID_List.Add(id);        
    }
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
        if (connectObject_Dictionary[targetID] != null)
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
    }

    /// <summary>
    /// �s�u����
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="position">��m</param>
    /// <param name="rotation">����</param>
    /// <param name="damage">����ˮ`</param>
    /// <param name="isCritical">�O�_�z��</param>
    /// <param name="repel">���h�Z��</param>
    /// <param name="attackerObjectID">�����̪���ID</param>
    /// <param name="attackerID">������ID</param>
    public void OnConnectGetHit(int targetID, Vector3 position, Quaternion rotation, float damage, bool isCritical, float repel, int attackerObjectID, int attackerID)
    {
        CharactersCollision collision = connectObject_Dictionary[targetID].GetComponent<CharactersCollision>();
        if (collision != null)
        {
            GameObject attackObj = connectObject_Dictionary[attackerObjectID].gameObject;
            GameObject attacker = connectObject_Dictionary[attackerID].gameObject;
            collision.OnConnectOtherGetHit(position, rotation, damage, isCritical, repel, attackObj, attacker);
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

    /// <summary>
    /// �Ыت��a�W�٪���
    /// </summary>
    /// <param name="nickName">�W��</param>
    /// <param name="id">ID</param>
    public void OnCreatePlayerNameObject(string nickName, int id)
    {
        //�W�٪���
        ObjectName objectName = Instantiate(Resources.Load<GameObject>(GameDataManagement.Instance.loadPath.objectName)).GetComponent<ObjectName>();//�W�٪���
        objectName.OnSetName(connectObject_Dictionary[id].transform, nickName, thiscolor, 1.85f);        
    }
    #endregion
}
