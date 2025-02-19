using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// 開始場景管理
/// </summary>
public class StartSceneUI : MonoBehaviourPunCallbacks
{
    static StartSceneUI startSceneUI;
    public static StartSceneUI Instance => startSceneUI;

    GameData_LoadPath loadPath;
    GameData_NumericalValue numericalValue;
    [SerializeField] AudioSource thisAudioSource;
    [SerializeField] AudioClip[] thisAudioClip;

    [Header("影片")]
    VideoPlayer videoPlayer;
    double videoTime;//影片長度
    AudioSource audioSource;

    [Header("音樂")]
    public AudioClip[] audioClips;

    [Header("開始畫面")]
    Image background_Image;//背景
    Transform startScreen;//startScreen UI控制        
    Text startTip_Text;//提示文字
    float startTip_Text_alpha;////提示文字Alpha
    int startTipGlintControl;//閃爍控制    

    [Header("選擇模式畫面")]
    Transform selectModeScreen;//SelectModeScreen UI控制    
    Transform modeSelectBackground_Image;//ModeSelectBackground_Image UI控制 
    Transform modeVolume;//ModeVolume UI控制
    Button modeSingle_Button;//單人模式按鈕
    Button modeConnect_Button;//連線模式按鈕
    Button modeLeaveGame_Button;//離開遊戲按鈕
    Button modeVolume_Button;//音量按鈕
    Scrollbar modeVolume_Scrollbar;//音量ScrollBar
    bool isShowModeVolumeScrollBar;//是否顯示音量ScrollBar
    float ModeVolumeScrollBarSizeX;//音量ScrollBar SizeX            

    [Header("選擇腳色畫面")]
    Transform selectRoleScreen;//SelectRoleScreen UI控制
    Button roleBack_Button;//返回按鈕
    Button roleConfirm_Button;//腳色確定按鈕    
    [Header("選擇腳色畫面/腳色選擇按鈕")]
    // Sprite[] roleSelect_Sprite;//腳色選擇圖片
    Texture[] roleSelect_Sprite;
    Button roleSelectRight_Button;//腳色按鈕移動(右)
    Button roleSelectLeft_Button;//腳色按鈕移動(左)
    // Image rolePicture_Image;//選擇的腳色(大圖)
    RawImage rolePicture_Image;
    [Header("選擇腳色畫面/Buff")]
    int[] equipBuff;//裝備的Buff
    Text EquipBuff_Text;//裝備的Buff文字
    public BuffDrop buffBox_1;//Buff裝備框_1
    public BuffDrop buffBox_2;//Buff裝備框_2    

    [Header("關卡畫面")]
    Transform levelScreen;//LevelScreen UI控制
    Button levelBack_Button;//返回按鈕 

    [Header("設定暱稱畫面")]
    Transform setNickNameScreen;//SetNickNameScreen UI控制
    InputField nickName_InputField;//暱稱輸入框
    Button nickNameConfirm_Button;//暱稱確定按鈕
    Text nickNameTip_Text;//提示文字
    Button nickNameBack_Button;//返回按鈕

    [Header("連線模式畫面")]
    Transform conncetModeScreen;//ConncetModScreen UI控制
    Button connectModeBack_Button;//返回按鈕
    Button createRoom_Button;//創建房間按鈕
    Button randomRoom_Button;//隨機房間按鈕
    Button specifyRoom_Button;//指定房間按鈕
    InputField specifyRoom_InputField;//加入房間名稱輸入框
    InputField createRoom_InputField;//創建房間名稱輸入框    
    float connectModeTipTime;//提示文字時間
    Text connectModeTip_Text;//提示文字        

    [Header("連線房間畫面")]
    Transform connectRoomScreen;//connectRoomScreen UI控制
    Button connectRoomBack_Button;//返回按鈕
    List<Transform> roomPlayerTransformList = new List<Transform>();//房間玩家腳色圖片物件
    List<RectTransform> roomPlayerList = new List<RectTransform>();//紀錄房間玩家
    public Texture connectRoomRoleBackground_Texture;//腳色圖片背景(原圖)
    Texture connectRoomRoleBackground;//腳色圖片背景
    Button connectRoomLeftRole_Button;//更換腳色(左)
    Button connectRoomRightRole_Button;//更換腳色(右)
    float connectRoomChangeRoleTime;//更換腳色時間
    Text roomName_Text;//房間名稱
    Button roomSendMessage_Button;//發送訊息按鈕
    Text chatBox_Text;//聊天訊息框
    InputField roomMessage_InputField;//訊息輸入框
    List<string> roomChatList = new List<string>();//紀錄房間訊息
    Button roomLevelLeft_Button;//關卡選擇按鈕(左)
    Button roomLevelRight_Button;//關卡選擇按鈕(右)
    Text roomSelectLevel_Text;//選擇的關卡
    Button roomStartGame_Button;//開始遊戲按鈕
    public Text roomTip_Text;//提示文字
    public float roomTipTime;//提示文字時間
    public Text roomStartGameTip_Text;//開始遊戲提示文字
    public float roomStartGameTipTime;//開始遊戲提示文字時間

    void Awake()
    {
        if(startSceneUI != null)
        {
            Destroy(this);
            return;
        }
        startSceneUI = this;
                
        loadPath = GameDataManagement.Instance.loadPath;
        numericalValue = GameDataManagement.Instance.numericalValue;
    }

    void Start()
    {
        //顯示滑鼠
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        OnStartScreenPrepare();
        OnChooseRoleScreenPrepare();
        OnLevelScreenPrepare();
        OnSelectModeScreenPrepare();
        OnSetNickNamePrepare();
        OnConnectModeScreenPrepare();
        OnConnectRoomScreenPrepare();

        OnInital();
    }

    void Update()
    {
        OnStopVideo();
        OnTipTextGlintControl();        
        OnConnectModeTip();
        OnConnectRoomChangeRoleTime();
        OnKeyboardSendChatMessage();
        OnRoomTipTime();
        OnMusicVolumeScrollBar();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="number">音效編號</param>
    public void OnPlayAudio(int number)
    {        
        thisAudioSource.clip = thisAudioClip[number];
        thisAudioSource.Play();
    }

    /// <summary>
    /// 初始狀態
    /// </summary>
    private void OnInital()
    {
        GameDataManagement.Instance.stage = GameDataManagement.Stage.開始場景;
        GameDataManagement.Instance.selectRoleNumber = 0;//選擇的腳色編號
        for (int i = 0; i < GameDataManagement.Instance.equipBuff.Length; i++)
        {
            GameDataManagement.Instance.equipBuff[i] = -1;//裝備的Buff
        }

        //第一次進入遊戲
        if (!GameDataManagement.Instance.isNotFirstIntoGame)
        {
            if(videoPlayer.clip != null) videoPlayer.Play();//播放影片
            GameDataManagement.Instance.isNotFirstIntoGame = true;

            //播放背景音樂
            audioSource.clip = audioClips[0];
            audioSource.volume = GameDataManagement.Instance.musicVolume;
            audioSource.Play();
        }
        else
        {
            //連線狀態
            if (PhotonNetwork.IsConnected) PhotonConnect.Instance.OnDisconnectSetting();//離線

            //播放背景音樂
            if (audioSource.clip != null)
            {                
                audioSource.clip = audioClips[1];
                audioSource.volume = GameDataManagement.Instance.musicVolume;
                audioSource.Play();
            }

            OnOpenSelectModeScreen();//開啟選擇模式畫面
        }
    }

    #region 籌備
    /// <summary>
    /// 開始畫面籌備
    /// </summary>
    void OnStartScreenPrepare()
    {        
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();//影片   
        if(videoPlayer.clip) videoTime = videoPlayer.clip.length;//影片長度
        audioSource = Camera.main.GetComponent<AudioSource>();//音樂 
        audioSource.volume = GameDataManagement.Instance.musicVolume;
        audioSource.loop = true;
        audioSource.Stop();
        startScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "StartScreen");//startScreen UI控制        
        startTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "StartTip_Text");//提示文字
        startTip_Text.enabled = true;
        background_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Background_Image");//背景
        background_Image.gameObject.SetActive(false);
        
        //畫面UI控制
        startScreen.gameObject.SetActive(false);        
    }

    /// <summary>
    /// 選擇模式畫面籌備
    /// </summary>
    void OnSelectModeScreenPrepare()
    {
        selectModeScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SelectModeScreen");//selectModeScreen UI控制
        modeSelectBackground_Image = ExtensionMethods.FindAnyChild<Transform>(transform, "ModeSelectBackground_Image");//ModeSelectBackground_Image UI控制                                                                                                    
        modeVolume = ExtensionMethods.FindAnyChild<Transform>(transform, "ModeVolume");//ModeVolume UI控制
        modeSingle_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeSingle_Button");//單人模式按鈕
        modeSingle_Button.onClick.AddListener(OnIntoSelectRoleScreen);
        modeConnect_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeConnect_Button");//連線模式按鈕
        modeConnect_Button.onClick.AddListener(OnOpenSetNickNameScreen);
        modeLeaveGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeLeaveGame_Button");//離開遊戲按鈕
        modeLeaveGame_Button.onClick.AddListener(OnLeaveGame);
        modeVolume_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeVolume_Button");//音量按鈕
        modeVolume_Button.onClick.AddListener(OnShowMusicVolumeScrollBar);
        modeVolume_Scrollbar = ExtensionMethods.FindAnyChild<Scrollbar>(transform, "ModeVolume_Scrollbar");//音量ScrollBar
        modeVolume_Scrollbar.value = GameDataManagement.Instance.musicVolume;
        modeVolume_Scrollbar.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        modeVolume_Scrollbar.handleRect.GetComponent<Image>().color = new Color(1, 1, 1, 0);        

        selectModeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 選角畫面籌備
    /// </summary>
    void OnChooseRoleScreenPrepare()
    {
        //選擇腳色畫面
        selectRoleScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SelectRoleScreen");////SelectRoleScreen UI控制        
        roleConfirm_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleConfirm_Button");//腳色確定按鈕
        roleConfirm_Button.onClick.AddListener(OnIntoLeaveScreen);
        roleBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleBack_Button");//返回按鈕
        roleBack_Button.onClick.AddListener(OnSelectRoleScreenBackButton);
        //選擇腳色畫面/腳色選擇按鈕
        roleSelectRight_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleSelectRight_Button");////腳色按鈕移動(右)
        roleSelectRight_Button.onClick.AddListener(delegate { OnRoleButtonMove(number:1); });
        roleSelectLeft_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleSelectLeft_Button");//腳色按鈕移動(左)
        roleSelectLeft_Button.onClick.AddListener(delegate { OnRoleButtonMove(number: -1); });
        // roleSelect_Sprite = Resources.LoadAll<Sprite>(loadPath.roleSelect_Sprite);//腳色選擇圖片             
        roleSelect_Sprite = Resources.LoadAll<Texture>(loadPath.roleSelect_Sprite);

        //選擇的腳色(大圖)
        // rolePicture_Image = ExtensionMethods.FindAnyChild<Image>(transform, "RolePicture_Image");
        rolePicture_Image = ExtensionMethods.FindAnyChild<RawImage>(transform, "RolePicture_Image");
        // rolePicture_Image.sprite = roleSelect_Sprite[0];
        rolePicture_Image.texture = roleSelect_Sprite[0];

        equipBuff = new int[2];//裝備的Buff

        //Buff拖拉圖片
        for (int i = 1; i <= 6; i++)
        {
            BuffButtonDrag buff =  ExtensionMethods.FindAnyChild<BuffButtonDrag>(transform, "Buff_" + i + "_Image").GetComponent<BuffButtonDrag>();
            buff.buffAble = i;//給予Buff能力編號
        }       

        //Buff裝備框
        buffBox_1 = ExtensionMethods.FindAnyChild<BuffDrop>(transform, "SelectBuffBackground_1_Image").GetComponent<BuffDrop>();
        buffBox_1.buffBoxName = "buffBoxLeft";//判斷裝備的Buff
        buffBox_2 = ExtensionMethods.FindAnyChild<BuffDrop>(transform, "SelectBuffBackground_2_Image").GetComponent<BuffDrop>();
        buffBox_2.buffBoxName = "buffBoxRight";//判斷裝備的Buff

        //裝備的Buff文字
        EquipBuff_Text = ExtensionMethods.FindAnyChild<Text>(transform, "EquipBuff_Text").GetComponent<Text>();
        EquipBuff_Text.text = "";

        selectRoleScreen.gameObject.SetActive(false);
    }    

    /// <summary>
    /// 關卡畫面籌備
    /// </summary>
    void OnLevelScreenPrepare()
    {        
        levelScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "LevelScreen");//LevelScreen UI控制 

        //所有關卡
        Transform allLevels = ExtensionMethods.FindAnyChild<Transform>(transform, "AllLevels");
        for (int i = 0; i < allLevels.childCount; i++)
        {
            Button levelButton = ExtensionMethods.FindAnyChild<Button>(transform, "Level" + (i + 1) + "_Button");//關卡按鈕            
            OnSetLevelButtonFunction(levelButton: levelButton, level: i + 1);            
        }

        roomName_Text = ExtensionMethods.FindAnyChild<Text>(transform, "RoomName_Text");//房間名稱

        levelBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "LevelBack_Button");//返回按鈕
        levelBack_Button.onClick.AddListener(OnLevelScreenBackButton);

        levelScreen.gameObject.SetActive(false);
    }

    #region 設定暱稱畫面籌備
    void OnSetNickNamePrepare()
    {
        setNickNameScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SetNickNameScreen");//SetNickNameScreen UI控制
        nickNameConfirm_Button = ExtensionMethods.FindAnyChild<Button>(transform, "NickNameConfirm_Button");//暱稱確定按鈕
        nickNameConfirm_Button.onClick.AddListener(OnIntoConncetModeScreen);
        nickNameTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "NickNameTip_Text");//提示文字
        nickNameTip_Text.enabled = false;
        nickNameBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "NickNameBack_Button");//返回按鈕
        nickNameBack_Button.onClick.AddListener(OnNickNameBackButton);

        setNickNameScreen.gameObject.SetActive(false);
    }
    #endregion

    /// <summary>
    /// 連線模式籌備
    /// </summary>
    void OnConnectModeScreenPrepare()
    {        
        conncetModeScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "ConncetModeScreen");//ConncetModScreen UI控制
        createRoom_Button = ExtensionMethods.FindAnyChild<Button>(transform, "CreateRoom_Button");//創建房間按鈕
        createRoom_Button.onClick.AddListener(OnCreateRoomButton);
        randomRoom_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RandomRoom_Button");//隨機房間按鈕
        randomRoom_Button.onClick.AddListener(OnRandomRoomButton);
        specifyRoom_Button = ExtensionMethods.FindAnyChild<Button>(transform, "SpecifyRoom_Button");//指定房間按鈕
        specifyRoom_Button.onClick.AddListener(OnSpecifyRoomButton);
        specifyRoom_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "SpecifyRoom_InputField");//加入房間名稱輸入框
        createRoom_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "CreateRoom_InputField");//創建房間名稱輸入框
        connectModeTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ConnectModeTip_Text");//提示文字
        connectModeTipTime = 0;
        Color color = new Color(connectModeTip_Text.color.r, connectModeTip_Text.color.g, connectModeTip_Text.color.b, connectModeTipTime);
        connectModeTip_Text.color = color;
        connectModeBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ConnectModeBack_Button");//返回按鈕
        connectModeBack_Button.onClick.AddListener(OnConnectModeBackButton);
        nickName_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "NickName_InputField");//暱稱輸入框 


        conncetModeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 連線房間畫面籌備
    /// </summary>
    void OnConnectRoomScreenPrepare()
    {
        connectRoomScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "ConnectRoomScreen");//ConnectRoomScreen UI控制
        connectRoomBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ConnectRoomBack_Button");//返回按鈕
        connectRoomBack_Button.onClick.AddListener(OnConnectRoomScreenBackButton);

        //房間玩家UI
        for (int i = 0; i < 4; i++)
        {
            roomPlayerTransformList.Add(ExtensionMethods.FindAnyChild<Transform>(transform, "Player" + (i + 1)));
            roomPlayerList.Add(ExtensionMethods.FindAnyChild<RectTransform>(transform, "Player" + (i + 1) + "_Image"));
            ExtensionMethods.FindAnyChild<Transform>(roomPlayerTransformList[i], "Left_Button").gameObject.SetActive(false);
            ExtensionMethods.FindAnyChild<Transform>(roomPlayerTransformList[i], "Right_Button").gameObject.SetActive(false);
        }
        connectRoomRoleBackground = roomPlayerList[0].GetComponent<RawImage>().texture;//腳色圖片背景
        

        roomSendMessage_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoomSendMessage_Button");//發送訊息按鈕
        roomSendMessage_Button.onClick.AddListener(OnRoomSendMessage);
        chatBox_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ChatBox_Text");//聊天訊息框
        roomMessage_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "RoomMessage_InputField");//訊息輸入框

        roomLevelLeft_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoomLevelLeft_Button");//關卡選擇按鈕(左)
        roomLevelLeft_Button.onClick.AddListener(delegate { OnRoomSelectLevelButton(-1); });
        roomLevelRight_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoomLevelRight_Button");//關卡選擇按鈕(右)
        roomLevelRight_Button.onClick.AddListener(delegate { OnRoomSelectLevelButton(1); });
        roomSelectLevel_Text = ExtensionMethods.FindAnyChild<Text>(transform, "RoomSelectLevel_Text");//選擇的關卡
        roomSelectLevel_Text.text = GameDataManagement.Instance.numericalValue.levelNames[GameDataManagement.Instance.selectLevelNumber - 11];
        roomStartGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoomStartGame_Button");//開始遊戲按鈕
        roomStartGame_Button.onClick.AddListener(OnStartConnectGame);
        roomTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "RoomTip_Text");//提示文字
        Color color = new Color(roomTip_Text.color.r, roomTip_Text.color.g, roomTip_Text.color.b, roomTipTime);
        roomTip_Text.color = color;
        roomStartGameTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "RoomStartGameTip_Text");//開始遊戲提示文字
        Color color2 = new Color(roomStartGameTip_Text.color.r, roomStartGameTip_Text.color.g, roomStartGameTip_Text.color.b, roomTipTime);
        roomStartGameTip_Text.color = color2;

        connectRoomScreen.gameObject.SetActive(false);
    }

    #endregion

    #region 開始畫面  
    /// <summary>
    /// 影片停止
    /// </summary>
    void OnStopVideo()
    {
        //沒有影片
        if (videoPlayer.clip == null)
        {            
            startScreen.gameObject.SetActive(true);
        }

        //StartScene未開啟
        if (!background_Image.gameObject.activeSelf)
        {           
            if (videoTime > 0) videoTime -= Time.deltaTime;//影片時間
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (videoPlayer.isPlaying || videoTime <= 0)
                {
                    videoPlayer.Stop();
                    audioSource.Play();
                    startScreen.gameObject.SetActive(true);
                }
                else
                {                    
                    OnOpenSelectModeScreen();
                }
            }

            //影片撥放結束
            if (videoTime <= 0)
            {
                videoPlayer.Stop();
                startScreen.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                    
                    OnPlayAudio(0);//播放音效

                    startTip_Text.enabled = false;
                    OnOpenSelectModeScreen();
                }
            }            
        }
        else
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Stop();
                audioSource.Play();
            }
        }
    }

    /// <summary>
    /// 開啟選擇模式畫面
    /// </summary>
    void OnOpenSelectModeScreen()
    {
        audioSource.clip = audioClips[1];
        audioSource.volume = GameDataManagement.Instance.musicVolume;
        audioSource.Play();

        background_Image.gameObject.SetActive(true);
        selectModeScreen.gameObject.SetActive(true);
        startScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 提示文字閃爍控制
    /// </summary>
    void OnTipTextGlintControl()
    {
        if (startTip_Text != null)
        {
            startTip_Text_alpha += startTipGlintControl * Time.deltaTime;
            if (startTip_Text_alpha >= 2) startTipGlintControl = -2;
            if (startTip_Text_alpha <= 0) startTipGlintControl = 1;
            Color col = startTip_Text.color;
            col.a = startTip_Text_alpha;
            startTip_Text.color = col;
        }
    }
    #endregion

    #region 選擇模式畫面   
    /// <summary>
    /// 進入選擇腳色畫面
    /// </summary>
    void OnIntoSelectRoleScreen()
    {
        OnPlayAudio(0);//播放音效
        
        selectRoleScreen.gameObject.SetActive(true);
        startScreen.gameObject.SetActive(false);
        selectModeScreen.gameObject.SetActive(false);
        modeLeaveGame_Button.gameObject.SetActive(false);
        modeVolume.gameObject.SetActive(false);
    }

    /// <summary>
    /// 開啟設定暱稱畫面
    /// </summary>
    void OnOpenSetNickNameScreen()
    {
        OnPlayAudio(0);//播放音效
        
        setNickNameScreen.gameObject.SetActive(true);                
        nickName_InputField.gameObject.SetActive(true);//暱稱輸入框
        nickNameConfirm_Button.gameObject.SetActive(true);//暱稱確定按鈕
        nickNameBack_Button.gameObject.SetActive(true);//返回按鈕
        selectModeScreen.gameObject.SetActive(false);
    //  nickName_InputField.text = "訪客" + UnityEngine.Random.Range(0, 1000);
    //  預設暱稱,先暫時屏蔽,因點選時初始會出現白色背景框有點醜,訪客若未輸入暱稱,一樣會給予ID "訪客XXX",所以不影響
               
       /* modeSelectBackground_Image.gameObject.SetActive(false);
        modeLeaveGame_Button.gameObject.SetActive(false);
        modeVolume.gameObject.SetActive(false);*/
    }

    /// <summary>
    /// 登入成功
    /// </summary>
    public void OnIsConnected()
    {
        setNickNameScreen.gameObject.SetActive(false);
        conncetModeScreen.gameObject.SetActive(true);
        modeSelectBackground_Image.gameObject.SetActive(true);
        nickNameTip_Text.enabled = false;

        //連線模式按鈕
        createRoom_Button.enabled = true;
        randomRoom_Button.enabled = true;
        specifyRoom_Button.enabled = true;                       
    }

    /// <summary>
    /// 顯示音量ScrollBar
    /// </summary>
    void OnShowMusicVolumeScrollBar()
    {
        OnPlayAudio(1);//播放音效
        isShowModeVolumeScrollBar = !isShowModeVolumeScrollBar;
    }

    /// <summary>
    /// 音量ScrollBar
    /// </summary>
    void OnMusicVolumeScrollBar()
    {
        //顯示 音量ScrollBar
        if (isShowModeVolumeScrollBar)
        {
            ModeVolumeScrollBarSizeX += Time.deltaTime;
            if (ModeVolumeScrollBarSizeX >= 1) ModeVolumeScrollBarSizeX = 1;
        }

        //隱藏 音量ScrollBar
        if (!isShowModeVolumeScrollBar && modeVolume_Scrollbar.GetComponent<RectTransform>().localScale.x > 0)
        {
            ModeVolumeScrollBarSizeX -= Time.deltaTime;
            if (ModeVolumeScrollBarSizeX <= 0) ModeVolumeScrollBarSizeX = 0;
        }
       
        modeVolume_Scrollbar.GetComponent<Image>().color = new Color(1, 1, 1, ModeVolumeScrollBarSizeX);
        modeVolume_Scrollbar.handleRect.GetComponent<Image>().color = new Color(1, 1, 1, ModeVolumeScrollBarSizeX);

        //音量控制
        GameDataManagement.Instance.musicVolume = modeVolume_Scrollbar.value;
        audioSource.volume = GameDataManagement.Instance.musicVolume;
    }

    /// <summary>
    /// 離開遊戲按鈕
    /// </summary>
    void OnLeaveGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
    #endregion

    #region 選角畫面
    /// <summary>
    /// 進入關卡畫面
    /// </summary>
    void OnIntoLeaveScreen()
    {
        OnPlayAudio(0);//播放音效
        
        levelScreen.gameObject.SetActive(true);
        selectRoleScreen.gameObject.SetActive(false);               
    }

    /// <summary>
    /// 選擇腳色畫面返回按鈕
    /// </summary>
    void OnSelectRoleScreenBackButton()
    {
        OnPlayAudio(1);//播放音效

        selectModeScreen.gameObject.SetActive(true);
        selectRoleScreen.gameObject.SetActive(false);
        modeLeaveGame_Button.gameObject.SetActive(true);
        modeVolume.gameObject.SetActive(true);

        GameDataManagement.Instance.equipBuff = new int[2] { -1, -1 };//裝備的Buff
    }

    /// <summary>
    /// 設定裝備的Buff
    /// </summary>
    /// <param name="boxName">Buff裝備框</param>
    /// <param name="buff">增加的Buff編號</param>
    public void OnSetEquipBuff(string boxName, int buff)
    {
        switch (boxName)
        {
            case "buffBoxLeft":
                equipBuff[0] = buff - 1;
                break;
            case "buffBoxRight":
                equipBuff[1] = buff - 1;
                break;
        }        

        //設定文字
        string text = "";
        if (equipBuff[0] >= 0) text = numericalValue.buffAbleString[equipBuff[0]] + "+" + numericalValue.buffAbleValue[equipBuff[0]] + "%\n";
        else text = "";        
        if (equipBuff[1] >= 0) text = text + numericalValue.buffAbleString[equipBuff[1]] + "+" + numericalValue.buffAbleValue[equipBuff[1]] + "%";
        else text = text + "";

        EquipBuff_Text.text = text;

        GameDataManagement.Instance.equipBuff[0] = equipBuff[0];
        GameDataManagement.Instance.equipBuff[1] = equipBuff[1];
    }
    
    /// <summary>
    /// 腳色按鈕移動(左右按鈕)
    /// </summary>
    /// <param name="number">選擇腳色編號</param>
    void OnRoleButtonMove(int number)
    {
        OnPlayAudio(1);//播放音效

        int role = GameDataManagement.Instance.selectRoleNumber;
        role += number;
        if (role < 0) role = roleSelect_Sprite.Length - 1;
        if (role > roleSelect_Sprite.Length - 1) role = 0;

        // rolePicture_Image.sprite = roleSelect_Sprite[role];//設定選擇的腳色(大圖)
        rolePicture_Image.texture = roleSelect_Sprite[role];
        GameDataManagement.Instance.selectRoleNumber = role;       
    }   
    #endregion

    #region 關卡畫面

    /// <summary>
    /// 設定關卡按鈕事件
    /// </summary>
    /// <param name="levelButton"></param>
    /// <param name="level"></param>
    void OnSetLevelButtonFunction(Button levelButton, int level)
    {                
        levelButton.onClick.AddListener(() => { OnSelectLecel(level: level); });        
    }

    /// <summary>
    /// 選擇關卡
    /// </summary>
    /// <param name="level">選擇的關卡</param>
    void OnSelectLecel(int level)
    {
        OnPlayAudio(0);//播放音效

        background_Image.enabled = false;
        levelScreen.gameObject.SetActive(false);
        //GameDataManagement.Instance.selectLevelNumber = level - 1;//選擇的關卡
        //StartCoroutine(LoadScene.Instance.OnLoadScene("LevelScene" + level));
        GameDataManagement.Instance.selectLevelNumber = level + 10;//選擇的關卡(測試)        
        StartCoroutine(LoadScene.Instance.OnLoadScene("LevelScene" + (level +10)));// (測試)
    }   

    /// <summary>
    /// 關卡畫面返回按鈕
    /// </summary>
    void OnLevelScreenBackButton()
    {
        OnPlayAudio(1);//播放音效

        selectRoleScreen.gameObject.SetActive(true);
        levelScreen.gameObject.SetActive(false);
    }
    #endregion

    #region 設定暱稱畫面
    /// <summary>
    /// 進入連線模式畫面
    /// </summary>
    void OnIntoConncetModeScreen()
    {        
        OnPlayAudio(0);//播放音效

        nickNameTip_Text.enabled = true;//提示文字
        nickName_InputField.gameObject.SetActive(false);//暱稱輸入框
        nickNameConfirm_Button.gameObject.SetActive(false);//暱稱確定按鈕
        nickNameBack_Button.gameObject.SetActive(false);//返回按鈕

        PhotonConnect.Instance.OnConnectSetting();        
        PhotonConnect.Instance.OnSetNickName(nickName_InputField.text);//設定暱稱        
    }

    /// <summary>
    /// 返回按鈕
    /// </summary>
    void OnNickNameBackButton()
    {
        OnPlayAudio(1);//播放音效

        setNickNameScreen.gameObject.SetActive(false);
        selectModeScreen.gameObject.SetActive(true);
    }
    #endregion

    #region 連線模式畫面   
    /// <summary>
    /// 返回按鈕
    /// </summary>
    public void OnConnectModeBackButton()
    {
        OnPlayAudio(1);//播放音效

        PhotonConnect.Instance.OnDisconnectSetting();

        selectModeScreen.gameObject.SetActive(true);
        conncetModeScreen.gameObject.SetActive(false);
        modeLeaveGame_Button.gameObject.SetActive(true);
        modeVolume.gameObject.SetActive(true);        
    }
    
    /// <summary>
    /// 創建房間按鈕
    /// </summary>
    void OnCreateRoomButton()
    {        
        OnPlayAudio(0);//播放音效

        PhotonConnect.Instance.OnCreateRoomSetting(createRoom_InputField.text);

        OnConnectModeButtonActiveSetting(active: false);        
    }

    /// <summary>
    /// 隨機房間按鈕
    /// </summary>
    void OnRandomRoomButton()
    {
        OnPlayAudio(0);//播放音效

        PhotonConnect.Instance.OnJoinRandomRoomRoomSetting();

        OnConnectModeButtonActiveSetting(active: false);
    }   

    /// <summary>
    /// 指定房間按鈕
    /// </summary>
    void OnSpecifyRoomButton()
    {
        OnPlayAudio(0);//播放音效

        if (specifyRoom_InputField.text == "")
        {
            OnConnectModeSettingTip(tip: "請輸入房間名稱");            
            return;
        }
                

        PhotonConnect.Instance.OnJoinSpecifyRoomSetting(specifyRoom_InputField.text);

        OnConnectModeButtonActiveSetting(active: false);
    }

    /// <summary>
    /// 連線模式畫面按鈕控制
    /// </summary>
    /// <param name="active">按鈕是否啟動</param>
    void OnConnectModeButtonActiveSetting(bool active)
    {
        createRoom_Button.enabled = active;
        randomRoom_Button.enabled = active;
        specifyRoom_Button.enabled = active;
    }

    /// <summary>
    /// 整理連線模式UI
    /// </summary>
    /// <param name="roomName">加入防間名稱</param>
    public void OnTidyConnectModeUI(string roomName)
    {
        connectRoomScreen.gameObject.SetActive(true);
        conncetModeScreen.gameObject.SetActive(false);

        roomName_Text.text = "房間:" + roomName;
        createRoom_InputField.text = "";
        specifyRoom_InputField.text = "";
    }    

    /// <summary>
    /// 設定提示文字
    /// </summary>
    /// <param name="tip">提示文字</param>
    public void OnConnectModeSettingTip(string tip)
    {
        connectModeTipTime = 3;//提示文字時間
        connectModeTip_Text.text = tip;

        OnConnectModeButtonActiveSetting(active: true);
    }

    /// <summary>
    /// 提示文字
    /// </summary>
    void OnConnectModeTip()
    {
        if (connectModeTipTime > 0)
        {
            connectModeTipTime -= Time.deltaTime;
            Color color = new Color(connectModeTip_Text.color.r, connectModeTip_Text.color.g, connectModeTip_Text.color.b, connectModeTipTime);
            connectModeTip_Text.color = color;
        }
    }
    #endregion

    #region 連線房間
    /// <summary>
    /// 連線房間返回按鈕
    /// </summary>
    void OnConnectRoomScreenBackButton()
    {
        OnPlayAudio(1);//播放音效

        PhotonConnect.Instance.OnLeaveRoomSetting();

        conncetModeScreen.gameObject.SetActive(true);
        connectRoomScreen.gameObject.SetActive(false);        
    }

    /// <summary>
    /// 刷新玩家暱稱
    /// </summary>
    /// <param name="playerList">玩家列表</param>
    /// <param name="selfNickName">自己的暱稱</param>
    /// <param name="isRoomMaster">是否為房主</param>
    public void OnRefreshRoomPlayerNickName(List<string> playerList, string selfNickName, bool isRoomMaster)
    {
        //開始遊戲按鈕
        roomStartGame_Button.gameObject.SetActive(isRoomMaster);

        //關卡按鈕
        roomLevelLeft_Button.gameObject.SetActive(isRoomMaster);
        roomLevelRight_Button.gameObject.SetActive(isRoomMaster);

        //重製
        for (int j = 0; j < roomPlayerList.Count; j++)
        {
            ExtensionMethods.FindAnyChild<Text>(roomPlayerTransformList[j], "ID_Text").text = "";
            ExtensionMethods.FindAnyChild<Button>(roomPlayerTransformList[j], "Left_Button").gameObject.SetActive(false);//更換腳色(左)
            ExtensionMethods.FindAnyChild<Button>(roomPlayerTransformList[j], "Right_Button").gameObject.SetActive(false);//更換腳色(右)
            roomPlayerList[j].GetComponent<RawImage>().texture = connectRoomRoleBackground;
            roomPlayerList[j].sizeDelta = new Vector2(400, 700);
        }

        //更新
        for (int i = 0; i < playerList.Count; i++)
        {
            ExtensionMethods.FindAnyChild<Text>(roomPlayerTransformList[i], "ID_Text").text = playerList[i];
            
            //顯示更換腳色按鈕
            if(selfNickName == playerList[i])
            {
                connectRoomLeftRole_Button = ExtensionMethods.FindAnyChild<Button>(roomPlayerTransformList[i], "Left_Button");//更換腳色(左)
                connectRoomLeftRole_Button.gameObject.SetActive(true);
                connectRoomLeftRole_Button.onClick.AddListener(delegate { OnConnectRoomChangeRole(-1); });
                connectRoomRightRole_Button = ExtensionMethods.FindAnyChild<Button>(roomPlayerTransformList[i], "Right_Button");//更換腳色(右)
                connectRoomRightRole_Button.gameObject.SetActive(true);
                connectRoomRightRole_Button.onClick.AddListener(delegate { OnConnectRoomChangeRole(1); });
            }           
        }             
    }

    /// <summary>
    /// 刷新玩家腳色
    /// </summary>
    /// <param name="number">第幾位玩家</param>
    /// <param name="characters">腳色編號</param>
    public void OnRefreshPlayerCharacters(int number, int characters)
    {
        for (int i = 0; i < roomPlayerTransformList.Count; i++)
        {
            if(i == number)
            {
                // roomPlayerList[i].GetComponent<Image>().sprite = roleSelect_Sprite[characters];
                GameDataManagement.Instance.allConnectPlayerSelectRole[i] = characters;//所有連線玩家所選角色
                roomPlayerList[i].GetComponent<RawImage>().texture = roleSelect_Sprite[characters];
                roomPlayerList[i].sizeDelta = new Vector2(2000, 2000);
                continue;
            }            
        }
    }

    /// <summary>
    /// 腳色更換時間(避免卡鍵)
    /// </summary>
    void OnConnectRoomChangeRoleTime()
    {
        if (connectRoomChangeRoleTime > 0) connectRoomChangeRoleTime -= Time.deltaTime;
    }

    /// <summary>
    /// 更換腳色
    /// </summary>
    /// <param name="value">選擇腳色增減</param>
    void OnConnectRoomChangeRole(int value)
    {
        OnPlayAudio(1);//播放音效

        if (connectRoomChangeRoleTime <= 0)
        {
            connectRoomChangeRoleTime = 0.1f;//避免卡鍵

            int role = GameDataManagement.Instance.selectRoleNumber;
            role += value;
            if (role < 0) role = roleSelect_Sprite.Length - 1;
            if (role > roleSelect_Sprite.Length - 1) role = 0;

            GameDataManagement.Instance.selectRoleNumber = role;
            PhotonConnect.Instance.OnSendRoomPlayerCharacters();
        }
    }

    /// <summary>
    /// 鍵盤發送聊天訊息
    /// </summary>
    void OnKeyboardSendChatMessage()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) OnRoomSendMessage();
    }

    /// <summary>
    /// 發送訊息
    /// </summary>
    void OnRoomSendMessage()
    {
        char[] charsToTrim = {' '};
        string mes = roomMessage_InputField.text.Trim(charsToTrim);
        PhotonConnect.Instance.OnSendRoomChatMessage(mes);

        roomMessage_InputField.text = "";
        roomMessage_InputField.ActivateInputField();//可以一直輸入
    }

    /// <summary>
    /// 接收房間聊天訊息
    /// </summary>
    /// <param name="message">訊息</param>
    public void OnGetRoomChatMessage(string message)
    {
        roomChatList.Add(message);
        if (roomChatList.Count > 6) roomChatList.RemoveAt(0);

        chatBox_Text.text = string.Join("\n", roomChatList);
    }

    /// <summary>
    /// 選擇關卡按鈕
    /// </summary>
    /// <param name="value">關卡增減</param>
    void OnRoomSelectLevelButton(int value)
    {
        OnPlayAudio(1);//播放音效

        int levelNumber = GameDataManagement.Instance.selectLevelNumber;
        levelNumber += value;
        /*if (levelNumber < 0) levelNumber = GameDataManagement.Instance.numericalValue.levelNames.Length - 1;
        if (levelNumber > GameDataManagement.Instance.numericalValue.levelNames.Length - 1) levelNumber = 0;*/

        /*GameDataManagement.Instance.selectLevelNumber = levelNumber;
        PhotonConnect.Instance.OnSendLevelNumber(levelNumber);*/

        //測試
        if (levelNumber < 11) levelNumber = 12;
        if (levelNumber > 12) levelNumber = 11;
        GameDataManagement.Instance.selectLevelNumber = levelNumber;
        PhotonConnect.Instance.OnSendLevelNumber(levelNumber);
    }

    /// <summary>
    /// 房間關卡文字
    /// </summary>
    /// <param name="level">選擇的關卡</param>
    public void OnRoomLevelText(int level)
    {        
        roomSelectLevel_Text.text = GameDataManagement.Instance.numericalValue.levelNames[level - 11];
    }

    /// <summary>
    /// 開始連線遊戲
    /// </summary>
    void OnStartConnectGame()
    {
        if (PhotonConnect.Instance.OnStartGame(GameDataManagement.Instance.selectLevelNumber))//測試
        {
            OnPlayAudio(0);//播放音效

            roomStartGame_Button.enabled = false;//關閉按鈕(避免連按)
        }
        else
        {
            roomTipTime = 2;
            roomTip_Text.text = "2名玩家以上開始";
        }
    }    

    /// <summary>
    /// 房間提示文字
    /// </summary>
    void OnRoomTipTime()
    {
        if (roomTipTime > 0)
        {
            roomTipTime -= Time.deltaTime;
            Color color = new Color(roomTip_Text.color.r, roomTip_Text.color.g, roomTip_Text.color.b, roomTipTime);
            roomTip_Text.color = color;
        }

        if(roomStartGameTipTime > 0)//開始遊戲提示文字時間
        {
            roomStartGameTipTime -= Time.deltaTime;
            Color color = new Color(roomStartGameTip_Text.color.r, roomStartGameTip_Text.color.g, roomStartGameTip_Text.color.b, roomStartGameTipTime);
            roomStartGameTip_Text.color = color;                      
        }
    }
    #endregion
}
