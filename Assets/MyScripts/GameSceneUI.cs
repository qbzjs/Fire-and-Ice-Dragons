using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviourPunCallbacks
{
    static GameSceneUI gameSceneUI;
    public static GameSceneUI Instance => gameSceneUI;

    [Header("MiniMap")]
    Transform miniMap;//�p�a��

    [Header("�ĤH�ͩR��")]
    Transform enemyLifeBar;//EnemyLifeBar UI����
    Image enemyLifeBarFront_Image;//�ͩR��(�e)
    Text enemyLifeBarName_Text;//�ĤH�W��

    [Header("���a�ͩR��")]
    Text playerNickName;//���a�ʺ�
    Sprite[] allPlayerHeadStickers;//�Ҧ����a�Y��
    Image headStickers_Image;//���a�Y��
    Image playerLifeBarFront_Image;//�ͩR��(�e)
    Image playerLifeBarMid_Image;//�ͩR��(��)
    float playerHpProportion;

    [Header("���aBuff")]
    Sprite[] buffSprites;//Buff�Ϥ�
    Image playerBuff_1;//���a�˳�Buff1
    Image playerBuff_2;//���a�˳�Buff2

    [Header("�ﶵ")]
    Transform options;//Options UI����    
    Button leaveGame_Button;//���}�C�����s
    Button continueGame_Button;//�~��C�����s
    AudioSource audioSource;//���ּ���
    Scrollbar volume_Scrollbar;//���qScrollBar
    public bool isOptions;//�O�_�}�_�ﶵ����

    [Header("�C������")]    
    Transform gameOver;//GameOver UI����
    Text gameOverResult_Text;//�C�����G��r
    Button backToStart_Button;//��^���s(�T�{���s)
    public bool isGameOver;//�O�_�C������
    Transform gameResult;//GameResult UI����
    Text gameResult_Text;//�C�����G��r

    [Header("���ܤ�r")]
    Image tipBackground_Image;//���ܤ�r�I��
    public Text tip_Text;//���ܤ�r
    float tipTime;//��r��ܮɶ�

    [Header("����")]
    Transform task;//Task UI����
    public Text task_Text;//���Ȥ�r

    [Header("����")]
    Text killNumber_Text;//�����Ƥ�r
    public int killNumber;//������
    Text comboNumber_Text;//�s���Ƥ�r
    public int comboNumber;//�s����
    float comboLifeTime;//�s���Ƥ�r�ɶ�
    Image comboBackground_Image;//�s���Ƥ�r�I��

    [Header("�p���O")]
    Text playGameTime_Text;//�C���ɶ���r
    Text maxKillNumber_Text;//�̤j�����Ƥ�r
    Text maxCombolNumber_Text;//�̤j�s���Ƥ�r
    Text accumulationDamageNumber_Text;//�ֿn�ˮ`
    public int MaxCombo;//�̤j�s����
    float playerGameTime;//�C���ɶ�
    public float accumulationDamage;//�ֿn�ˮ`

    [Header("�s�u���a")]
    Transform connectUI;//Connect UI ����
    Transform player1;//Player1 UI����
    Transform player2;//Player2 UI����
    Transform player3;//Player3 UI����
    Text player1Name_Text;//���a1�ʺ�
    Text player2Name_Text;//���a2�ʺ�
    Text player3Name_Text;//���a3�ʺ�
    Image player1LifeBarFront_Image;//���a1���
    Image player2LifeBarFront_Image;//���a2���
    Image player3LifeBarFront_Image;//���a3���
    Image[] allPlayerLifeBar;//�Ҧ����a���
    Image player1Head_Image;//���a1�Y��
    Image player2Head_Image;//���a2�Y��
    Image player3Head_Image;//���a3�Y��
    Image[] allPlayerHead_Image;//�Ҧ����a�Y��

    [Header("�s�u�O���O")]
    Transform connectGameOver;//ConnectGameOver UI����
    public Text connectGameOverResult_Text;//�s�u�C�����G
    Transform player1_Over;//Player1_Over UI����
    Transform player2_Over;//Player2_Over UI����
    Transform player3_Over;//Player3_Over UI����
    Transform player4_Over;//Player4_Over UI����
    Image player1_OverHead;//Player1�Y��
    Image player2_OverHead;//Player2�Y��
    Image player3_OverHead;//Player3�Y��
    Image player4_OverHead;//Player4�Y��
    Image[] allPlayer1_OverHead;
    Text player1_OverNickName;//Player1�ʺ�
    Text player2_OverNickName;//Player2�ʺ�
    Text player3_OverNickName;//Player3�ʺ�
    Text player4_OverNickName;//Player4�ʺ�
    Text[] allPlayer4_OverNickName;
    Text player1_MaxKillNumber_Text;//Player1������
    Text player2_MaxKillNumber_Text;//Player2������
    Text player3_MaxKillNumber_Text;//Player3������
    Text player4_MaxKillNumber_Text;//Player4������
    Text[] allPlayer_MaxKillNumber_Text;
    Text player1_MaxCombolNumber_Text;//Player1�̤j�s����
    Text player2_MaxCombolNumber_Text;//Player2�̤j�s����
    Text player3_MaxCombolNumber_Text;//Player3�̤j�s����
    Text player4_MaxCombolNumber_Text;//Player4�̤j�s����
    Text[] allPlayer4_MaxCombolNumber_Text;
    Text player1_AccumulationDamageNumber_Text;//Player1�ֿn�ˮ`
    Text player2_AccumulationDamageNumber_Text;//Player2�ֿn�ˮ`
    Text player3_AccumulationDamageNumber_Text;//Player3�ֿn�ˮ`
    Text player4_AccumulationDamageNumber_Text;//Player4�ֿn�ˮ`
    Text[] allPlayer_AccumulationDamageNumber_Text;
    Button backToStartOver_Button;//�^��start�������s

    void Awake()
    {
        if(gameSceneUI != null)
        {
            Destroy(this);
            return;
        }
        gameSceneUI = this;
    }

    void Start()
    {
        //�p�a��
        miniMap = ExtensionMethods.FindAnyChild<Transform>(transform, "MiniMap");//�p�a��
        if (GameDataManagement.Instance.selectLevelNumber == 12) miniMap.gameObject.SetActive(false);

        //�ĤH�ͩR��        
        enemyLifeBarName_Text = ExtensionMethods.FindAnyChild<Text>(transform, "EnemyLifeBarName_Text");//�ĤH�W��
        enemyLifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "EnemyLifeBarFront_Image");//�ͩR��(�e)
        enemyLifeBar = ExtensionMethods.FindAnyChild<Transform>(transform, "EnemyLifeBar");//EnemyLifeBar UI����
        enemyLifeBar.gameObject.SetActive(false);

        //���a�ͩR��
        playerNickName = ExtensionMethods.FindAnyChild<Text>(transform, "PlayerNickName");//���a�ʺ�
        if (GameDataManagement.Instance.isConnect)
        {
            playerNickName.text = PhotonNetwork.NickName;
        }
        else playerNickName.enabled = false;
        allPlayerHeadStickers = Resources.LoadAll<Sprite>("Sprites/PlayerHeadStickers");//�Ҧ����a�Y��
        headStickers_Image = ExtensionMethods.FindAnyChild<Image>(transform, "HeadStickers_Image");//���a�Y��
        headStickers_Image.sprite = allPlayerHeadStickers[GameDataManagement.Instance.selectRoleNumber];
        playerHpProportion = 1;
        playerLifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "PlayerLifeBarFront_Image");//�ͩR��(�e)
        playerLifeBarFront_Image.fillAmount = playerHpProportion;
        playerLifeBarMid_Image = ExtensionMethods.FindAnyChild<Image>(transform, "PlayerLifeBarMid_Image");//�ͩR��(��)
        playerLifeBarMid_Image.fillAmount = playerHpProportion;

        //���aBuff
        buffSprites = Resources.LoadAll<Sprite>("Sprites/Buff");
        playerBuff_1 = ExtensionMethods.FindAnyChild<Image>(transform, "PlayerBuff_1");//���a�˳�Buff1        
        playerBuff_2 = ExtensionMethods.FindAnyChild<Image>(transform, "PlayerBuff_2");//���a�˳�Buff2
        Image[] buffs = new Image[] { playerBuff_1, playerBuff_2 };
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            if (GameDataManagement.Instance.equipBuff[i] >= 0) buffs[i].sprite = buffSprites[GameDataManagement.Instance.equipBuff[i]];
            else
            {
                buffs[i].enabled = false;//�����S���˳ƪ�buff�Ϥ�

                //�洫��m
                if (GameDataManagement.Instance.equipBuff[0] < 0)
                {
                    Vector3 pos = buffs[0].rectTransform.localPosition;
                    buffs[1].rectTransform.localPosition = pos;
                }
            }
        }

        //�ﶵ
        options = ExtensionMethods.FindAnyChild<Transform>(transform, "Options");//Options UI����
        options.gameObject.SetActive(false);
        leaveGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "LeaveGame_Button");//���}�C�����s
        leaveGame_Button.onClick.AddListener(OnLeaveGame);
        continueGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ContinueGame_Button");//�~��C�����s
        continueGame_Button.onClick.AddListener(OnContinueGame);
        audioSource = Camera.main.GetComponent<AudioSource>();//���ּ���        
        audioSource.volume = GameDataManagement.Instance.musicVolume;
        audioSource.Play();
        volume_Scrollbar = ExtensionMethods.FindAnyChild<Scrollbar>(transform, "Volume_Scrollbar");//���qScrollBar
        volume_Scrollbar.value = GameDataManagement.Instance.musicVolume;

        //�C������                
        gameOver = ExtensionMethods.FindAnyChild<Transform>(transform, "GameOver");//GameOver UI����
        gameOver.gameObject.SetActive(false);
        gameOverResult_Text = ExtensionMethods.FindAnyChild<Text>(transform, "GameOverResult_Text");//�C�����G��r
        backToStart_Button = ExtensionMethods.FindAnyChild<Button>(transform, "BackToStart_Button");//��^���s(�T�{���s)        
        backToStart_Button.onClick.AddListener(OnLeaveGame);
        gameResult = ExtensionMethods.FindAnyChild<Transform>(transform, "GameResult");//GameResult UI����
        gameResult_Text = ExtensionMethods.FindAnyChild<Text>(transform, "GameResult_Text"); ;//�C�����G��r
        gameResult.gameObject.SetActive(false);        

        //��L
        tipBackground_Image = ExtensionMethods.FindAnyChild<Image>(transform, "TipBackground_Image");//���ܤ�r�I��
        tipBackground_Image.color = new Color(tipBackground_Image.color.r, tipBackground_Image.color.g, tipBackground_Image.color.b, tipTime); 
        tip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Tip_Text");//���ܤ�r
        tip_Text.color = new Color(tip_Text.color.r, tip_Text.color.g, tip_Text.color.b, tipTime);

        //����
        task = ExtensionMethods.FindAnyChild<Transform>(transform, "Task");//Task UI����
        task_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Task_Text");//���Ȥ�r

        //����
        killNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "KillNumber_Text");//�����Ƥ�r
        killNumber_Text.text = "�� �� �� : " + killNumber;
        comboNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ComboNumber_Text");//�s���Ƥ�r
        comboNumber_Text.enabled = false;        
        comboBackground_Image = ExtensionMethods.FindAnyChild<Image>(transform, "ComboBackground_Image");//�s���Ƥ�r�I��
        comboBackground_Image.enabled = false;

        //�p���O
        playGameTime_Text = ExtensionMethods.FindAnyChild<Text>(transform, "PlayGameTime_Text");//�C���ɶ���r
        maxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "MaxKillNumber_Text");//�̤j�����Ƥ�r
        maxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "MaxCombolNumber_Text");//�̤j�s���Ƥ�r
        accumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "AccumulationDamageNumber_Text");//�ֿn�ˮ`��r

        //�s�u�O���O
        connectGameOver = ExtensionMethods.FindAnyChild<Transform>(transform, "ConnectGameOver");//ConnectGameOver UI����
        connectGameOver.gameObject.SetActive(false);
        connectGameOverResult_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ConnectGameOverResult_Text");//�s�u�C�����G
        connectGameOverResult_Text.text = " �� �Q �` �� ";
        player1_Over = ExtensionMethods.FindAnyChild<Transform>(transform, "Player1_Over");//Player1_Over UI����
        player2_Over = ExtensionMethods.FindAnyChild<Transform>(transform, "Player2_Over");//Player2_Over UI����
        player3_Over = ExtensionMethods.FindAnyChild<Transform>(transform, "Player3_Over");//Player3_Over UI����
        player4_Over = ExtensionMethods.FindAnyChild<Transform>(transform, "Player4_Over");//Player4_Over UI����
        Transform[] allPlayerOver = new Transform[] { player1_Over, player2_Over, player3_Over, player4_Over };
        if (GameDataManagement.Instance.isConnect)
        {
            for (int i = PhotonNetwork.CurrentRoom.PlayerCount; i < allPlayerOver.Length; i++)
            {
                allPlayerOver[i].gameObject.SetActive(false);
            }
        }
        player1_OverHead = ExtensionMethods.FindAnyChild<Image>(transform, "Player1_OverHead");//Player1�Y��
        player2_OverHead = ExtensionMethods.FindAnyChild<Image>(transform, "Player2_OverHead");//Player2�Y��
        player3_OverHead = ExtensionMethods.FindAnyChild<Image>(transform, "Player3_OverHead");//Player3�Y��
        player4_OverHead = ExtensionMethods.FindAnyChild<Image>(transform, "Player4_OverHead");//Player4�Y��
        allPlayer1_OverHead = new Image[] { player1_OverHead, player2_OverHead, player3_OverHead, player4_OverHead };
        player1_OverNickName = ExtensionMethods.FindAnyChild<Text>(transform, "Player1_OverNickName");//Player1�ʺ�
        player2_OverNickName = ExtensionMethods.FindAnyChild<Text>(transform, "Player2_OverNickName");//Player2�ʺ�
        player3_OverNickName = ExtensionMethods.FindAnyChild<Text>(transform, "Player3_OverNickName");//Player3�ʺ�
        player4_OverNickName = ExtensionMethods.FindAnyChild<Text>(transform, "Player4_OverNickName");//Player4�ʺ�
        allPlayer4_OverNickName = new Text[] { player1_OverNickName, player2_OverNickName, player3_OverNickName, player4_OverNickName };
        player1_MaxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player1_MaxKillNumber_Text");//Player1������
        player2_MaxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player2_MaxKillNumber_Text");//Player2������
        player3_MaxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player3_MaxKillNumber_Text");//Player3������
        player4_MaxKillNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player4_MaxKillNumber_Text");//Player4������
        allPlayer_MaxKillNumber_Text = new Text[] { player1_MaxKillNumber_Text, player2_MaxKillNumber_Text, player3_MaxKillNumber_Text, player4_MaxKillNumber_Text };
        player1_MaxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player1_MaxCombolNumber_Text");//Player1�̤j�s����
        player2_MaxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player2_MaxCombolNumber_Text");//Player2�̤j�s����
        player3_MaxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player3_MaxCombolNumber_Text");//Player3�̤j�s����
        player4_MaxCombolNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player4_MaxCombolNumber_Text");//Player4�̤j�s����
        allPlayer4_MaxCombolNumber_Text = new Text[] { player1_MaxCombolNumber_Text, player2_MaxCombolNumber_Text, player3_MaxCombolNumber_Text, player4_MaxCombolNumber_Text };
        player1_AccumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player1_AccumulationDamageNumber_Text");//Player1�ֿn�ˮ`
        player2_AccumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player2_AccumulationDamageNumber_Text");//Player2�ֿn�ˮ`
        player3_AccumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player3_AccumulationDamageNumber_Text");//Player3�ֿn�ˮ`
        player4_AccumulationDamageNumber_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player4_AccumulationDamageNumber_Text");//Player4�ֿn�ˮ`
        allPlayer_AccumulationDamageNumber_Text = new Text[] { player1_AccumulationDamageNumber_Text, player2_AccumulationDamageNumber_Text, player3_AccumulationDamageNumber_Text, player4_AccumulationDamageNumber_Text };
        backToStartOver_Button = ExtensionMethods.FindAnyChild<Button>(transform, "BackToStartOver_Button");//�^��start�������s
        backToStartOver_Button.onClick.AddListener(OnLeaveGame);

        //�s�u���a
        connectUI = ExtensionMethods.FindAnyChild<Transform>(transform, "ConnectUI");//ConnectUI ����
        if (!GameDataManagement.Instance.isConnect) connectUI.gameObject.SetActive(false);
        if (GameDataManagement.Instance.isConnect)
        {
            connectUI.gameObject.SetActive(true);

            player1 = ExtensionMethods.FindAnyChild<Transform>(transform, "Player1");//Player1 UI����
            player2 = ExtensionMethods.FindAnyChild<Transform>(transform, "Player2");//Player1 UI����
            player3 = ExtensionMethods.FindAnyChild<Transform>(transform, "Player3");//Player1 UI����            
            Transform[] allPlayerUI = new Transform[] { player1, player2, player3 };            
            for (int i = PhotonNetwork.CurrentRoom.PlayerCount - 1; i < allPlayerUI.Length; i++)
            {
                allPlayerUI[i].gameObject.SetActive(false);
            }

            player1Name_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player1Name_Text");//���a3�ʺ�
            player2Name_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player2Name_Text");//���a3�ʺ�
            player3Name_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Player3Name_Text");//���a3�ʺ�
            Text[] allPlayerNickName = new Text[] { player1Name_Text, player2Name_Text, player3Name_Text };
            player1LifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player1LifeBarFront_Image");//���a1���
            player2LifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player2LifeBarFront_Image");//���a2���
            player3LifeBarFront_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player3LifeBarFront_Image");//���a3���
            allPlayerLifeBar = new Image[] { player1LifeBarFront_Image, player2LifeBarFront_Image, player3LifeBarFront_Image };
            player1Head_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player1Head_Image");//���a1�Y��
            player2Head_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player2Head_Image");//���a2�Y��
            player3Head_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Player3Head_Image");//���a3�Y��
            allPlayerHead_Image = new Image[] { player1Head_Image, player2Head_Image, player3Head_Image };//�Ҧ����a�Y��
            bool isTouchSelf = false;
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount - 1; i++)
            {
                allPlayerLifeBar[i].fillAmount = 1;                

                if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                {
                    isTouchSelf = true;
                    allPlayerNickName[i].text = PhotonNetwork.PlayerList[i + 1].NickName;                    
                    allPlayerHead_Image[i].sprite = allPlayerHeadStickers[GameDataManagement.Instance.allConnectPlayerSelectRole[i + 1]];                    
                    continue;
                }

                if (isTouchSelf)
                {
                    allPlayerNickName[i].text = PhotonNetwork.PlayerList[i + 1].NickName;
                    allPlayerHead_Image[i].sprite = allPlayerHeadStickers[GameDataManagement.Instance.allConnectPlayerSelectRole[i + 1]];                    
                }
                else
                {
                    allPlayerNickName[i].text = PhotonNetwork.PlayerList[i].NickName;
                    allPlayerHead_Image[i].sprite = allPlayerHeadStickers[GameDataManagement.Instance.allConnectPlayerSelectRole[i]];                    
                }
            }           
        }        
    }
        
    void Update()
    {        
        OnPlayerLifeBarBehavior();//���a�ͩR���欰
        OnOptionsUI();//�ﶵ����
        OnTipText();//���ܤ�r
        OnComboLifeTime();//�s���ƥͦs�ɶ�
        OnPlayGameTime();//�C���ɶ�
    }

    /// <summary>
    /// �]�w��L���a�ͩR��
    /// </summary>
    /// <param name="nickName">���a�ʺ�</param>
    /// <param name="hpProportion">�ͩR���</param>
    public void OnSetOtherPlayerLifeBar(string nickName, float hpProportion)
    {        
        int number = 0;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
            {                
                continue;
            }
            if (PhotonNetwork.PlayerList[i].NickName == nickName)
            {
                allPlayerLifeBar[number].fillAmount = hpProportion;
                return;
            }
            number++;
        }
    }

    /// <summary>
    /// �]�w�ĤH�ͩR��
    /// </summary>
    /// <param name="name">�ĤH�W��</param>
    /// <param name="value">�ͩR���</param>
    public void OnSetEnemyLifeBarValue(string name, float value)
    {      
        enemyLifeBarName_Text.text = name;//�]�w�W��
        enemyLifeBarFront_Image.fillAmount = value;//�]�w���
    }

    /// <summary>
    /// �]�w�ĤH�ͩR�����
    /// </summary>
    public bool SetEnemyLifeBarActive { set { enemyLifeBar.gameObject.SetActive(value); } }

    /// <summary>
    /// �C��������������
    /// </summary>
    public void OnGameOverCloseObject()
    {
        //��������UI
        task.gameObject.SetActive(false);
    }

    /// <summary>
    /// �]�w�C������UI
    /// </summary>
    /// <param name="clearance">�O�_�L��</param>
    public void OnSetGameOverUI(bool clearance)
    {
        isGameOver = true;//�C������
        if(!clearance) Time.timeScale = 0;

        //�}�ҿﶵ
        if (isOptions)
        {
            isOptions = false;
            options.gameObject.SetActive(isOptions);
        }        

        //��ܷƹ�
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //���G��r
        if (clearance) gameOverResult_Text.text = " �� �Q �` �� ";
        else gameOverResult_Text.text = " �� �� �` �� ";

        //�C���ɶ�
        int minute = (int)playerGameTime / 60;
        int second = (int)playerGameTime % 60;
        playGameTime_Text.text = $"�C �� �� �� : {minute} �� {second} ��";

        //�̤j������
        maxKillNumber_Text.text = $"�� �j �� �� �� : {killNumber}";

        //�̤j�s����
        maxCombolNumber_Text.text = $"�� �j �s �� �� : {MaxCombo}";

        //�ֿn�ˮ`
        accumulationDamageNumber_Text.text = $"�� �n �� �` : {((int)accumulationDamage).ToString()}";

        //�}�ҹC������UI
        if(GameDataManagement.Instance.isConnect)
        {
            connectGameOver.gameObject.SetActive(true);
            //�C���ɶ�
            Text playGameTimeOver_Text = ExtensionMethods.FindAnyChild<Text>(transform, "PlayGameTimeOver_Text");//�C���ɶ�
            playGameTimeOver_Text.text = $"�C �� �� �� : {minute} �� {second} ��";            
        }
        else gameOver.gameObject.SetActive(isGameOver);
    }

    /// <summary>
    /// �s�u�C������
    /// </summary>
    /// <param name="playerList">���a�C��</param>
    /// <param name="nickName">�ʺ�</param>
    /// <param name="MaxCombo">�̤j�s��</param>
    /// <param name="killNumber">������</param>
    /// <param name="accumulationDamage">�ֿn�ˮ`</param>
    public void OnConnectGameOver(List<string> playerList, string nickName, int MaxCombo, int killNumber, float accumulationDamage)
    {
        if (!PhotonNetwork.IsMasterClient) backToStartOver_Button.gameObject.SetActive(false);

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {                      
            if(nickName == playerList[i])
            {
                allPlayer1_OverHead[i].sprite = allPlayerHeadStickers[GameDataManagement.Instance.allConnectPlayerSelectRole[i]];
                allPlayer4_OverNickName[i].text = playerList[i];
                allPlayer_MaxKillNumber_Text[i].text = $"�� �j �� �� �� : {killNumber}";
                allPlayer4_MaxCombolNumber_Text[i].text = $"�� �j �s �� �� : {MaxCombo}";
                allPlayer_AccumulationDamageNumber_Text[i].text = $"�� �n �� �` : {((int)accumulationDamage).ToString()}";
            }
        }
    }

    /// <summary>
    /// �C���ɶ�
    /// </summary>
    void OnPlayGameTime()
    {
        playerGameTime = Time.realtimeSinceStartup;//�C���ɶ�
    }

    /// <summary>
    /// �s���ƥͦs�ɶ�
    /// </summary>
    void OnComboLifeTime()
    {
        if (comboLifeTime > 0)
        {
            comboLifeTime -= Time.deltaTime; ;//�s���Ƥ�r�ɶ�

            if(comboNumber_Text.fontSize > 50) comboNumber_Text.fontSize -= 2;//��r�Y�p
            comboNumber_Text.color = new Color(comboNumber_Text.color.r, comboNumber_Text.color.g, comboNumber_Text.color.b, comboLifeTime);//��r
            comboBackground_Image.color = new Color(comboBackground_Image.color.r, comboBackground_Image.color.g, comboBackground_Image.color.b, comboLifeTime);//�I��

            if (comboLifeTime <= 0)
            {
                comboNumber = 0;
                comboNumber_Text.enabled = false;
            }
        }
    }

    /// <summary>
    /// �]�w�s����
    /// </summary>
    public void OnSetComboNumber()
    {
        //��r
        comboLifeTime = 3;
        comboNumber++;//������
        comboNumber_Text.text = "�s�� : " + comboNumber;
        comboNumber_Text.enabled = true;
        comboNumber_Text.fontSize = 80;

        //�I��
        comboBackground_Image.enabled = true;

        //�̤j�s����
        if (MaxCombo < comboNumber) MaxCombo = comboNumber;
    }

    /// <summary>
    /// �]�w������
    /// </summary>
    public void OnSetKillNumber()
    {
        killNumber++;//������
        killNumber_Text.text = "�� �� �� : " + killNumber;
    }

    /// <summary>
    /// �]�w���a�ͩR���
    /// </summary>
    public float SetPlayerHpProportion { set { playerHpProportion = value; } }

    /// <summary>
    /// ���a�ͩR���欰
    /// </summary>
    void OnPlayerLifeBarBehavior()
    {
        if (playerHpProportion <= 0) playerHpProportion = 0;//���a�ͩR���

        playerLifeBarFront_Image.fillAmount = playerHpProportion;//�ͩR��(�e)
        if (playerLifeBarFront_Image.fillAmount < playerLifeBarMid_Image.fillAmount)//�ͩR��(��)
        {
            playerLifeBarMid_Image.fillAmount -= 0.5f * Time.deltaTime;
        }
    }

    /// <summary>
    /// �ﶵ����
    /// </summary>
    void OnOptionsUI()
    {
        //�}�ҿﶵ���� && �C���������i��
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            isOptions = !isOptions;                                
            options.gameObject.SetActive(isOptions);

            if (isOptions)
            {
                HitNumber[] hitNumbers = GameObject.FindObjectsOfType<HitNumber>();
                for (int i = 0; i < hitNumbers.Length; i++)
                {
                    Destroy(hitNumbers[i].gameObject);
                }

                if (!GameDataManagement.Instance.isConnect) Time.timeScale = 0;              

                //��ܷƹ�                
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                
                //��ܷƹ�                
                Cursor.lockState = CursorLockMode.Locked;
            }

            Cursor.visible = isOptions;
        }

        if (isOptions)
        {    
            //���q
            GameDataManagement.Instance.musicVolume = volume_Scrollbar.value;
            audioSource.volume = GameDataManagement.Instance.musicVolume;;
        }  
    }

    /// <summary>
    /// ���}�C��
    /// </summary>
    void OnLeaveGame()
    {
        isOptions = false;
        backToStartOver_Button.enabled = false;//�������s(�קK�s��)
        options.gameObject.SetActive(isOptions);
        Time.timeScale = 1;
        
        //�s�u
        if (GameDataManagement.Instance.isConnect)
        {
            if (PhotonNetwork.IsMasterClient)
            {               
                PhotonConnect.Instance.OnSendGameTip("�Ǫ� : " + PhotonNetwork.NickName + " ���}�C��\n�m�C������...�n");
            }
            else PhotonConnect.Instance.OnSendGameTip("���a : " + PhotonNetwork.NickName + " ���}�C��");
        }

        //�P�w�O�_�L��
        if (GameDataManagement.Instance.isConnect && GameSceneManagement.Instance.isVictory)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonConnect.Instance.OnSendIntoNexttLevel();
        }
        else StartCoroutine(LoadScene.Instance.OnLoadScene("StartScene"));
    }

    /// <summary>
    /// �~��C��
    /// </summary>
    void OnContinueGame()
    {
        isOptions = false;
        options.gameObject.SetActive(isOptions);
        
        if(!GameDataManagement.Instance.isConnect)Time.timeScale = 1;

        //��ܷƹ�
        Cursor.visible = isOptions;
        Cursor.lockState = CursorLockMode.Locked;
    }   

    /// <summary>
    /// �]�w���ܤ�r
    /// </summary>
    /// <param name="tip">���ܤ�r</param>
    /// <param name="showTime">���ܮɶ�</param>
    public void OnSetTip(string tip, float showTime)
    {
        tip_Text.text = tip;
        tipTime = showTime;
    }

    /// <summary>
    /// ���ܤ�r
    /// </summary>
    void OnTipText()
    {
        if (tipTime > 0)
        {
            tipTime -= Time.deltaTime;

            tip_Text.color = new Color(tip_Text.color.r, tip_Text.color.g, tip_Text.color.b, tipTime);
            tipBackground_Image.color = new Color(tipBackground_Image.color.r, tipBackground_Image.color.g, tipBackground_Image.color.b, tipTime);
        }
    }

    /// <summary>
    /// �]�w���Ȥ�r
    /// </summary>
    /// <param name="taskValue">���Ȥ�r</param>
    public void OnSetTaskText(string taskValue)
    {
        task_Text.text = taskValue;
    }

    /// <summary>
    /// �]�w�C�����G
    /// </summary>
    /// <param name="active">�O�_���</param>
    /// <param name="result">�C�����G��r</param>
    public void OnSetGameResult(bool active, string result)
    {        
        gameResult.gameObject.SetActive(active);
        gameResult_Text.text = result;
    }
}
