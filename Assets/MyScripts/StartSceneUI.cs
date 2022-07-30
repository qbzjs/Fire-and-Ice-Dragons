using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// �}�l�����޲z
/// </summary>
public class StartSceneUI : MonoBehaviourPunCallbacks
{
    static StartSceneUI startSceneUI;
    public static StartSceneUI Instance => startSceneUI;

    GameData_LoadPath loadPath;
    GameData_NumericalValue numericalValue;       

    [Header("�v��")]
    VideoPlayer videoPlayer;
    double videoTime;//�v������
    AudioSource audioSource;

    [Header("�}�l�e��")]
    Image background_Image;//�I��
    Transform startScreen;//startScreen UI����        
    Text startTip_Text;//���ܤ�r
    float startTip_Text_alpha;////���ܤ�rAlpha
    int startTipGlintControl;//�{�{����    

    [Header("��ܼҦ��e��")]
    Transform selectModeScreen;//SelectModeScreen UI����    
    Transform modeSelectBackground_Image;//ModeSelectBackground_Image UI���� 
    Transform modeVolume;//ModeVolume UI����
    Button modeSingle_Button;//��H�Ҧ����s
    Button modeConnect_Button;//�s�u�Ҧ����s
    Button modeLeaveGame_Button;//���}�C�����s
    Button modeVolume_Button;//���q���s
    Scrollbar modeVolume_Scrollbar;//���qScrollBar
    bool isShowModeVolumeScrollBar;//�O�_��ܭ��qScrollBar
    float ModeVolumeScrollBarSizeX;//���qScrollBar SizeX
    Text modeTip_Text;//���ܤ�r
    InputField nickName_InputField;//�ʺٿ�J��    

    [Header("��ܸ}��e��")]
    Transform selectRoleScreen;//SelectRoleScreen UI����
    Button roleBack_Button;//��^���s
    Button roleConfirm_Button;//�}��T�w���s    
    [Header("��ܸ}��e��/�}���ܫ��s")]
    bool isSlideRoleButton;//�O�_�ưʸ}����s    
    GameObject roleSelect_Button;//�}���ܫ��s
    Sprite[] roleSelect_Sprite;//�}���ܹϤ�
    Button roleSelectRight_Button;//�}����s����(�k)
    Button roleSelectLeft_Button;//�}����s����(��)
    List<Transform> roleSelectButton_List = new List<Transform>();//�O���Ҧ��}���ܫ��s
    Image roleSelectBackGround_Image;//�}���ܭI��
    float roleSelectButtonSizeX;//�}���ܫ��sSizeX
    float roleSelectButtonSpacing;//�}���ܫ��s���Z
    float roleSelectButtonSlideSpeed;//�}��ưʫ��s�t��
    float mouseX;//Input MouseX    
    Image rolePicture_Image;//��ܪ��}��(�j��)
    [Header("��ܸ}��e��/Buff")]
    int[] equipBuff;//�˳ƪ�Buff
    Text EquipBuff_Text;//�˳ƪ�Buff��r
    public BuffDrop buffBox_1;//Buff�˳Ʈ�_1
    public BuffDrop buffBox_2;//Buff�˳Ʈ�_2    

    [Header("���d�e��")]
    Transform levelScreen;//LevelScreen UI����
    Button levelBack_Button;//��^���s 

    [Header("�s�u�Ҧ��e��")]
    Transform conncetModeScreen;//ConncetModScreen UI����
    Button connectModeBack_Button;//��^���s
    Button createRoom_Button;//�Ыةж����s
    Button randomRoom_Button;//�H���ж����s
    Button specifyRoom_Button;//���w�ж����s
    InputField specifyRoom_InputField;//�[�J�ж��W�ٿ�J��
    InputField createRoom_InputField;//�Ыةж��W�ٿ�J��    
    float connectModeTipTime;//���ܤ�r�ɶ�
    Text connectModeTip_Text;//���ܤ�r    

    [Header("�s�u�ж��e��")]
    Transform connectRoomScreen;//connectRoomScreen UI����
    Button connectRoomBack_Button;//��^���s
    List<RectTransform> roomPlayerList = new List<RectTransform>();//�����ж����a
    Sprite connectRoomRoleBackground;//�}��Ϥ��I��
    Button connectRoomLeftRole_Button;//�󴫸}��(��)
    Button connectRoomRightRole_Button;//�󴫸}��(�k)
    float connectRoomChangeRoleTime;//�󴫸}��ɶ�
    Text roomName_Text;//�ж��W��
    Button roomSendMessage_Button;//�o�e�T�����s
    Text chatBox_Text;//��ѰT����
    InputField roomMessage_InputField;//�T����J��
    List<string> roomChatList = new List<string>();//�����ж��T��
    Button roomLevelLeft_Button;//���d��ܫ��s(��)
    Button roomLevelRight_Button;//���d��ܫ��s(�k)
    Text roomSelectLevel_Text;//��ܪ����d
    Button roomStartGame_Button;//�}�l�C�����s
    Text roomTip_Text;//���ܤ�r
    float roomTipTime;//���ܤ�r�ɶ�

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
        OnStartScreenPrepare();
        OnChooseRoleScreenPrepare();
        OnLevelScreenPrepare();
        OnSelectModeScreenPrepare();
        OnConnectModeScreenPrepare();
        OnConnectRoomScreenPrepare();

        OnInital();
    }

    void Update()
    {
        OnStopVideo();
        OnTipTextGlintControl();
        OnSlideRoleButton();
        OnConnectModeTip();
        OnConnectRoomChangeRoleTime();
        OnKeyboardSendChatMessage();
        OnRoomTipTime();
        OnMusicVolumeScrollBar();
    }

    /// <summary>
    /// ��l���A
    /// </summary>
    private void OnInital()
    {
        GameDataManagement.Instance.stage = GameDataManagement.Stage.�}�l����;
        GameDataManagement.Instance.selectRoleNumber = 0;//��ܪ��}��s��

        //�Ĥ@���i�J�C��
        if (!GameDataManagement.Instance.isNotFirstIntoGame)
        {
            videoPlayer.Play();//����v��
            GameDataManagement.Instance.isNotFirstIntoGame = true;
        }
        else
        {
            //�s�u���A
            if (PhotonNetwork.IsConnected) PhotonConnect.Instance.OnDisconnectSetting();//���u

            audioSource.volume = GameDataManagement.Instance.musicVolume;
            audioSource.Play();

            OnOpenSelectModeScreen();//�}�ҿ�ܼҦ��e��
        }
    }

    /// <summary>
    /// �}�l�e���w��
    /// </summary>
    void OnStartScreenPrepare()
    {        
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();//�v��   
        videoTime = videoPlayer.clip.length;//�v������
        audioSource = Camera.main.GetComponent<AudioSource>();//���� 
        audioSource.volume = GameDataManagement.Instance.musicVolume;
        audioSource.Stop();
        startScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "StartScreen");//startScreen UI����        
        startTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "StartTip_Text");//���ܤ�r
        background_Image = ExtensionMethods.FindAnyChild<Image>(transform, "Background_Image");//�I��
        background_Image.gameObject.SetActive(false);
        
        //�e��UI����
        startScreen.gameObject.SetActive(false);        
    }

    /// <summary>
    /// ��ܼҦ��e���w��
    /// </summary>
    void OnSelectModeScreenPrepare()
    {
        selectModeScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SelectModeScreen");//selectModeScreen UI����
        modeSelectBackground_Image = ExtensionMethods.FindAnyChild<Transform>(transform, "ModeSelectBackground_Image");//ModeSelectBackground_Image UI����                                                                                                    
        modeVolume = ExtensionMethods.FindAnyChild<Transform>(transform, "ModeVolume");//ModeVolume UI����
        modeSingle_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeSingle_Button");//��H�Ҧ����s
        modeSingle_Button.onClick.AddListener(OnIntoSelectRoleScreen);
        modeConnect_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeConnect_Button");//�s�u�Ҧ����s
        modeConnect_Button.onClick.AddListener(OnOpenConnectModeScreen);
        modeLeaveGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeLeaveGame_Button");//���}�C�����s
        modeLeaveGame_Button.onClick.AddListener(OnLeaveGame);
        modeVolume_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeVolume_Button");//���q���s
        modeVolume_Button.onClick.AddListener(OnShowMusicVolumeScrollBar);
        modeVolume_Scrollbar = ExtensionMethods.FindAnyChild<Scrollbar>(transform, "ModeVolume_Scrollbar");//���qScrollBar
        modeVolume_Scrollbar.value = GameDataManagement.Instance.musicVolume;
        modeVolume_Scrollbar.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        modeVolume_Scrollbar.handleRect.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        modeTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ModeTip_Text");//���ܤ�r
        modeTip_Text.enabled = false;
        nickName_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "NickName_InputField");//�ʺٿ�J�� 

        selectModeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// �﨤�e���w��
    /// </summary>
    void OnChooseRoleScreenPrepare()
    {
        //��ܸ}��e��
        selectRoleScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SelectRoleScreen");////SelectRoleScreen UI����        
        roleConfirm_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleConfirm_Button");//�}��T�w���s
        roleConfirm_Button.onClick.AddListener(OnIntoLeaveScreen);
        roleBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleBack_Button");//��^���s
        roleBack_Button.onClick.AddListener(OnSelectRoleScreenBackButton);
        //��ܸ}��e��/�}���ܫ��s
        roleSelectRight_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleSelectRight_Button");////�}����s����(�k)
        roleSelectRight_Button.onClick.AddListener(delegate { OnRoleButtonMove(direction:1); });
        roleSelectLeft_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleSelectLeft_Button");//�}����s����(��)
        roleSelectLeft_Button.onClick.AddListener(delegate { OnRoleButtonMove(direction :-1); });
        roleSelect_Button = Resources.Load<GameObject>(loadPath.roleSelect_Button);//�}���ܫ��s
        roleSelect_Sprite = Resources.LoadAll<Sprite>(loadPath.roleSelect_Sprite);//�}���ܹϤ�
        roleSelectBackGround_Image = ExtensionMethods.FindAnyChild<Image>(transform, "RoleSelectBackGround_Image");//�}���ܭI��
        roleSelectButtonSizeX = roleSelect_Button.GetComponent<RectTransform>().rect.width;//�}���ܫ��sSizeX
        roleSelectButtonSpacing = 20;//�}���ܫ��s���Z        

        //���͸}���ܫ��s
        for (int i = 0; i < roleSelect_Sprite.Length; i++)
        {
            Transform roleButton = Instantiate(roleSelect_Button).GetComponent<Transform>();
            roleButton.name = "RoleButton" + i;
            roleButton.SetParent(roleSelectBackGround_Image.transform);           
            roleButton.localPosition = new Vector3(roleSelectButtonSpacing + ((roleSelectButtonSizeX + roleSelectButtonSpacing) * i), 0, 0);
            roleButton.GetComponent<Image>().sprite = roleSelect_Sprite[i];

            OnSetRoleButtonFunction(roleButton.GetComponent<Button>(), i);            
            roleSelectButton_List.Add(roleButton);
        }
        
        //��ܪ��}��(�j��)
        rolePicture_Image = ExtensionMethods.FindAnyChild<Image>(transform, "RolePicture_Image");
        rolePicture_Image.sprite = roleSelectButton_List[0].GetComponent<Image>().sprite;

        equipBuff = new int[2];//�˳ƪ�Buff

        //Buff��ԹϤ�
        for (int i = 1; i <= 6; i++)
        {
            BuffButtonDrag buff =  ExtensionMethods.FindAnyChild<BuffButtonDrag>(transform, "Buff_" + i + "_Image").GetComponent<BuffButtonDrag>();
            buff.buffAble = i;//����Buff��O�s��
        }       

        //Buff�˳Ʈ�
        buffBox_1 = ExtensionMethods.FindAnyChild<BuffDrop>(transform, "SelectBuffBackground_1_Image").GetComponent<BuffDrop>();
        buffBox_1.buffBoxName = "buffBoxLeft";//�P�_�˳ƪ�Buff
        buffBox_2 = ExtensionMethods.FindAnyChild<BuffDrop>(transform, "SelectBuffBackground_2_Image").GetComponent<BuffDrop>();
        buffBox_2.buffBoxName = "buffBoxRight";//�P�_�˳ƪ�Buff

        //�˳ƪ�Buff��r
        EquipBuff_Text = ExtensionMethods.FindAnyChild<Text>(transform, "EquipBuff_Text").GetComponent<Text>();
        EquipBuff_Text.text = "";

        selectRoleScreen.gameObject.SetActive(false);
    }    

    /// <summary>
    /// ���d�e���w��
    /// </summary>
    void OnLevelScreenPrepare()
    {        
        levelScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "LevelScreen");//LevelScreen UI���� 

        //�Ҧ����d
        Transform allLevels = ExtensionMethods.FindAnyChild<Transform>(transform, "AllLevels");
        for (int i = 0; i < allLevels.childCount; i++)
        {
            Button levelButton = ExtensionMethods.FindAnyChild<Button>(transform, "Level" + (i + 1) + "_Button");//���d���s            
            OnSetLevelButtonFunction(levelButton: levelButton, level: i + 1);
        }

        roomName_Text = ExtensionMethods.FindAnyChild<Text>(transform, "RoomName_Text");//�ж��W��

        levelBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "LevelBack_Button");//��^���s
        levelBack_Button.onClick.AddListener(OnLevelScreenBackButton);

        levelScreen.gameObject.SetActive(false);
    }   

    /// <summary>
    /// �s�u�Ҧ��w��
    /// </summary>
    void OnConnectModeScreenPrepare()
    {        
        conncetModeScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "ConncetModeScreen");//ConncetModScreen UI����
        createRoom_Button = ExtensionMethods.FindAnyChild<Button>(transform, "CreateRoom_Button");//�Ыةж����s
        createRoom_Button.onClick.AddListener(OnCreateRoomButton);
        randomRoom_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RandomRoom_Button");//�H���ж����s
        randomRoom_Button.onClick.AddListener(OnRandomRoomButton);
        specifyRoom_Button = ExtensionMethods.FindAnyChild<Button>(transform, "SpecifyRoom_Button");//���w�ж����s
        specifyRoom_Button.onClick.AddListener(OnSpecifyRoomButton);
        specifyRoom_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "SpecifyRoom_InputField");//�[�J�ж��W�ٿ�J��
        createRoom_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "CreateRoom_InputField");//�Ыةж��W�ٿ�J��
        connectModeTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ConnectModeTip_Text");//���ܤ�r
        connectModeTipTime = 0;
        Color color = new Color(connectModeTip_Text.color.r, connectModeTip_Text.color.g, connectModeTip_Text.color.b, connectModeTipTime);
        connectModeTip_Text.color = color;
        connectModeBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ConnectModeBack_Button");//��^���s
        connectModeBack_Button.onClick.AddListener(OnConnectModeBackButton);

        conncetModeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// �s�u�ж��e���w��
    /// </summary>
    void OnConnectRoomScreenPrepare()
    {
        connectRoomScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "ConnectRoomScreen");//ConnectRoomScreen UI����
        connectRoomBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ConnectRoomBack_Button");//��^���s
        connectRoomBack_Button.onClick.AddListener(OnConnectRoomScreenBackButton);

        //�ж����aUI
        for (int i = 0; i < 4; i++)
        {
            roomPlayerList.Add(ExtensionMethods.FindAnyChild<RectTransform>(transform, "Player" + (i + 1) + "_Image"));
            ExtensionMethods.FindAnyChild<Transform>(roomPlayerList[i], "Left_Button").gameObject.SetActive(false);
            ExtensionMethods.FindAnyChild<Transform>(roomPlayerList[i], "Right_Button").gameObject.SetActive(false);
        }
        connectRoomRoleBackground = roomPlayerList[0].GetComponent<Image>().sprite;//�}��Ϥ��I��

        roomSendMessage_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoomSendMessage_Button");//�o�e�T�����s
        roomSendMessage_Button.onClick.AddListener(OnRoomSendMessage);
        chatBox_Text = ExtensionMethods.FindAnyChild<Text>(transform, "ChatBox_Text");//��ѰT����
        roomMessage_InputField = ExtensionMethods.FindAnyChild<InputField>(transform, "RoomMessage_InputField");//�T����J��

        roomLevelLeft_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoomLevelLeft_Button");//���d��ܫ��s(��)
        roomLevelLeft_Button.onClick.AddListener(delegate { OnRoomSelectLevelButton(-1); });
        roomLevelRight_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoomLevelRight_Button");//���d��ܫ��s(�k)
        roomLevelRight_Button.onClick.AddListener(delegate { OnRoomSelectLevelButton(1); });
        roomSelectLevel_Text = ExtensionMethods.FindAnyChild<Text>(transform, "RoomSelectLevel_Text");//��ܪ����d
        roomSelectLevel_Text.text = GameDataManagement.Instance.numericalValue.levelNames[GameDataManagement.Instance.selectLevelNumber];
        roomStartGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoomStartGame_Button");//�}�l�C�����s
        roomStartGame_Button.onClick.AddListener(OnStartConnectGame);
        roomTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "RoomTip_Text");//���ܤ�r
        Color color = new Color(roomTip_Text.color.r, roomTip_Text.color.g, roomTip_Text.color.b, roomTipTime);
        roomTip_Text.color = color;

        connectRoomScreen.gameObject.SetActive(false);
    }

    #region �}�l�e��  
    /// <summary>
    /// �v������
    /// </summary>
    void OnStopVideo()
    {
        //StartScene���}��
        if (!background_Image.gameObject.activeSelf)
        {
            if (videoTime > 0) videoTime -= Time.deltaTime;//�v���ɶ�
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

            //�v�����񵲧�
            if(videoTime <= 0)
            {
                videoPlayer.Stop();
                startScreen.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {                    
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
    /// �}�ҿ�ܼҦ��e��
    /// </summary>
    void OnOpenSelectModeScreen()
    {
        background_Image.gameObject.SetActive(true);
        selectModeScreen.gameObject.SetActive(true);
        startScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���ܤ�r�{�{����
    /// </summary>
    void OnTipTextGlintControl()
    {
        if (startTip_Text != null)
        {
            startTip_Text_alpha += startTipGlintControl * Time.deltaTime;
            if (startTip_Text_alpha >= 1) startTipGlintControl = -1;
            if (startTip_Text_alpha <= 0) startTipGlintControl = 1;
            Color col = startTip_Text.color;
            col.a = startTip_Text_alpha;
            startTip_Text.color = col;
        }
    }
    #endregion

    #region ��ܼҦ��e��   
    /// <summary>
    /// �i�J��ܸ}��e��
    /// </summary>
    void OnIntoSelectRoleScreen()
    {        
        selectRoleScreen.gameObject.SetActive(true);
        startScreen.gameObject.SetActive(false);
        selectModeScreen.gameObject.SetActive(false);
        modeLeaveGame_Button.gameObject.SetActive(false);
        modeVolume.gameObject.SetActive(false);
    }

    /// <summary>
    /// �}�ҳs�u�Ҧ��e��
    /// </summary>
    void OnOpenConnectModeScreen()
    {              
        PhotonConnect.Instance.OnConnectSetting(nickName:nickName_InputField.text);

        modeTip_Text.enabled = true;
        modeSelectBackground_Image.gameObject.SetActive(false);
        modeLeaveGame_Button.gameObject.SetActive(false);
        modeVolume.gameObject.SetActive(false);
    }

    /// <summary>
    /// �n�J���\
    /// </summary>
    public void OnIsConnected()
    {
        conncetModeScreen.gameObject.SetActive(true);
        modeSelectBackground_Image.gameObject.SetActive(true);
        modeTip_Text.enabled = false;
        selectModeScreen.gameObject.SetActive(false);
        nickName_InputField.text = "";

        //�s�u�Ҧ����s
        createRoom_Button.enabled = true;
        randomRoom_Button.enabled = true;
        specifyRoom_Button.enabled = true;
    }

    /// <summary>
    /// ��ܭ��qScrollBar
    /// </summary>
    void OnShowMusicVolumeScrollBar()
    {
        isShowModeVolumeScrollBar = !isShowModeVolumeScrollBar;
    }

    /// <summary>
    /// ���qScrollBar
    /// </summary>
    void OnMusicVolumeScrollBar()
    {
        //��� ���qScrollBar
        if (isShowModeVolumeScrollBar)
        {
            ModeVolumeScrollBarSizeX += Time.deltaTime;
            if (ModeVolumeScrollBarSizeX >= 1) ModeVolumeScrollBarSizeX = 1;
        }

        //���� ���qScrollBar
        if (!isShowModeVolumeScrollBar && modeVolume_Scrollbar.GetComponent<RectTransform>().localScale.x > 0)
        {
            ModeVolumeScrollBarSizeX -= Time.deltaTime;
            if (ModeVolumeScrollBarSizeX <= 0) ModeVolumeScrollBarSizeX = 0;
        }
       
        modeVolume_Scrollbar.GetComponent<Image>().color = new Color(1, 1, 1, ModeVolumeScrollBarSizeX);
        modeVolume_Scrollbar.handleRect.GetComponent<Image>().color = new Color(1, 1, 1, ModeVolumeScrollBarSizeX);

        //���q����
        GameDataManagement.Instance.musicVolume = modeVolume_Scrollbar.value;
        audioSource.volume = GameDataManagement.Instance.musicVolume;
    }

    /// <summary>
    /// ���}�C�����s
    /// </summary>
    void OnLeaveGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
    #endregion

    #region �﨤�e��
    /// <summary>
    /// �i�J���d�e��
    /// </summary>
    void OnIntoLeaveScreen()
    {
        levelScreen.gameObject.SetActive(true);
        selectRoleScreen.gameObject.SetActive(false);               
    }

    /// <summary>
    /// ��ܸ}��e����^���s
    /// </summary>
    void OnSelectRoleScreenBackButton()
    {
        selectModeScreen.gameObject.SetActive(true);
        selectRoleScreen.gameObject.SetActive(false);
        modeLeaveGame_Button.gameObject.SetActive(true);
        modeVolume.gameObject.SetActive(true);
    }

    /// <summary>
    /// �]�w�˳ƪ�Buff
    /// </summary>
    /// <param name="boxName">Buff�˳Ʈ�</param>
    /// <param name="buff">�W�[��Buff�s��</param>
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

        //�]�w��r
        string text = "";
        if (equipBuff[0] >= 0) text = numericalValue.buffAbleString[equipBuff[0]] + "+" + numericalValue.buffAbleValue[equipBuff[0]] + "\n";
        else text = "";        
        if (equipBuff[1] >= 0) text = text + numericalValue.buffAbleString[equipBuff[1]] + "+" + numericalValue.buffAbleValue[equipBuff[1]];
        else text = text + "";

        EquipBuff_Text.text = text;

        GameDataManagement.Instance.equipBuff[0] = equipBuff[0];
        GameDataManagement.Instance.equipBuff[1] = equipBuff[1];
    }

    /// <summary>
    /// �]�w�}����sFunction
    /// </summary>
    /// <param name="roleButton">�}����s</param>
    /// <param name="i">�s��</param>
    void OnSetRoleButtonFunction(Button roleButton, int i)
    {
        roleButton.onClick.AddListener(delegate { OnClickRoleButton(i); });
    }

    /// <summary>
    /// �I���}���s
    /// </summary>
    /// <param name="numbrt">�s��(��ܪ��}��)</param>
    void OnClickRoleButton(int number)
    {
        rolePicture_Image.sprite = roleSelectButton_List[number].GetComponent<Image>().sprite;//�]�w��ܪ��}��(�j��)
        GameDataManagement.Instance.selectRoleNumber = number;
    }

    /// <summary>
    /// �}����s����(���k���s)
    /// </summary>
    /// <param name="direction">���ʤ�V</param>
    void OnRoleButtonMove(int direction)
    {
        for (int i = 0; i < roleSelectButton_List.Count; i++)
        {
            roleSelectButton_List[i].localPosition = new Vector3(roleSelectButton_List[i].localPosition.x + direction * (roleSelectButtonSizeX + (roleSelectButtonSpacing )), 0, 0);
        }
    }
    
    /// <summary>
    /// �ưʸ}����s
    /// </summary>
    void OnSlideRoleButton()
    {
        //�P�_�O�_�b�I���W
        if (RectTransformUtility.RectangleContainsScreenPoint(roleSelectBackGround_Image.GetComponent<RectTransform>(), Input.mousePosition))
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSlideRoleButton = true;
                roleSelectButtonSlideSpeed = 40;
            }
        }

        if (isSlideRoleButton)
        {
            mouseX = Input.GetAxis("Mouse X");
            if (Input.GetMouseButtonUp(0)) isSlideRoleButton = false;
        }
        else 
        {
            if(roleSelectButtonSlideSpeed > 0) roleSelectButtonSlideSpeed -= 80 * Time.deltaTime;//�ưʳt�װI�h
            if (roleSelectButtonSlideSpeed <= 0) roleSelectButtonSlideSpeed = 0;
        }

        //�ưʸ}����s
        for (int i = 0; i < roleSelectButton_List.Count; i++)
        {            
            if(mouseX > 0.1f || mouseX < -0.1f) roleSelectButton_List[i].localPosition = roleSelectButton_List[i].localPosition + Vector3.right * mouseX * roleSelectButtonSlideSpeed;

            //��ɳ]�w
            if (roleSelectButton_List[i].localPosition.x <= -roleSelectButtonSizeX)
            {
                roleSelectButton_List[i].localPosition = new Vector3(((roleSelectButtonSizeX + roleSelectButtonSpacing) * (roleSelectButton_List.Count - 1) + ((roleSelectButtonSizeX + roleSelectButtonSpacing) + roleSelectButton_List[i].localPosition.x)), 0, 0);
            }
            if (roleSelectButton_List[i].localPosition.x >= (roleSelectButtonSizeX + roleSelectButtonSpacing) * (roleSelectButton_List.Count - 1))
            {
                roleSelectButton_List[i].localPosition = new Vector3((-roleSelectButtonSizeX - roleSelectButtonSpacing) + (roleSelectButton_List[i].localPosition.x - (roleSelectButtonSizeX + roleSelectButtonSpacing) * (roleSelectButton_List.Count - 1)), 0, 0);
            }
        }
    }
    #endregion

    #region ���d�e��

    /// <summary>
    /// �]�w���d���s�ƥ�
    /// </summary>
    /// <param name="levelButton"></param>
    /// <param name="level"></param>
    void OnSetLevelButtonFunction(Button levelButton, int level)
    {
        levelButton.onClick.AddListener(() => { OnSelectLecel(level: level); });
    }

    /// <summary>
    /// ������d
    /// </summary>
    /// <param name="level">��ܪ����d</param>
    void OnSelectLecel(int level)
    {
        background_Image.enabled = false;
        levelScreen.gameObject.SetActive(false);
        StartCoroutine(LoadScene.Instance.OnLoadScene("LevelScene" + level));
    }   

    /// <summary>
    /// ���d�e����^���s
    /// </summary>
    void OnLevelScreenBackButton()
    {
        selectRoleScreen.gameObject.SetActive(true);
        levelScreen.gameObject.SetActive(false);
    }
    #endregion

    #region �s�u�Ҧ��e��   
    /// <summary>
    /// ��^���s
    /// </summary>
    public void OnConnectModeBackButton()
    {
        PhotonConnect.Instance.OnDisconnectSetting();

        selectModeScreen.gameObject.SetActive(true);
        conncetModeScreen.gameObject.SetActive(false);
        modeLeaveGame_Button.gameObject.SetActive(true);
        modeVolume.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// �Ыةж����s
    /// </summary>
    void OnCreateRoomButton()
    {
        PhotonConnect.Instance.OnCreateRoomSetting(createRoom_InputField.text);

        OnConnectModeButtonActiveSetting(active: false);        
    }

    /// <summary>
    /// �H���ж����s
    /// </summary>
    void OnRandomRoomButton()
    {
        PhotonConnect.Instance.OnJoinRandomRoomRoomSetting();

        OnConnectModeButtonActiveSetting(active: false);
    }   

    /// <summary>
    /// ���w�ж����s
    /// </summary>
    void OnSpecifyRoomButton()
    {
        if (specifyRoom_InputField.text == "")
        {
            OnConnectModeSettingTip(tip: "�п�J�ж��W��");            
            return;
        }

        PhotonConnect.Instance.OnJoinSpecifyRoomSetting(specifyRoom_InputField.text);

        OnConnectModeButtonActiveSetting(active: false);
    }

    /// <summary>
    /// �s�u�Ҧ��e�����s����
    /// </summary>
    /// <param name="active">���s�O�_�Ұ�</param>
    void OnConnectModeButtonActiveSetting(bool active)
    {
        createRoom_Button.enabled = active;
        randomRoom_Button.enabled = active;
        specifyRoom_Button.enabled = active;
    }

    /// <summary>
    /// ��z�s�u�Ҧ�UI
    /// </summary>
    /// <param name="roomName">�[�J�����W��</param>
    public void OnTidyConnectModeUI(string roomName)
    {
        connectRoomScreen.gameObject.SetActive(true);
        conncetModeScreen.gameObject.SetActive(false);

        roomName_Text.text = "�ж�:" + roomName;
        createRoom_InputField.text = "";
        specifyRoom_InputField.text = "";
    }    

    /// <summary>
    /// �]�w���ܤ�r
    /// </summary>
    /// <param name="tip">���ܤ�r</param>
    public void OnConnectModeSettingTip(string tip)
    {
        connectModeTipTime = 3;//���ܤ�r�ɶ�
        connectModeTip_Text.text = tip;

        OnConnectModeButtonActiveSetting(active: true);
    }

    /// <summary>
    /// ���ܤ�r
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

    #region �s�u�ж�
    /// <summary>
    /// �s�u�ж���^���s
    /// </summary>
    void OnConnectRoomScreenBackButton()
    {
        PhotonConnect.Instance.OnLeaveRoomSetting();

        conncetModeScreen.gameObject.SetActive(true);
        connectRoomScreen.gameObject.SetActive(false);        
    }

    /// <summary>
    /// ��s���a�ʺ�
    /// </summary>
    /// <param name="playerList">���a�C��</param>
    /// <param name="selfNickName">�ۤv���ʺ�</param>
    /// <param name="isRoomMaster">�O�_���ХD</param>
    public void OnRefreshRoomPlayerNickName(List<string> playerList, string selfNickName, bool isRoomMaster)
    {
        //�}�l�C�����s
        roomStartGame_Button.gameObject.SetActive(isRoomMaster);

        //���d���s
        roomLevelLeft_Button.gameObject.SetActive(isRoomMaster);
        roomLevelRight_Button.gameObject.SetActive(isRoomMaster);

        //���s
        for (int j = 0; j < roomPlayerList.Count; j++)
        {
            ExtensionMethods.FindAnyChild<Text>(roomPlayerList[j], "ID_Text").text = "���ݪ��a";
            ExtensionMethods.FindAnyChild<Button>(roomPlayerList[j], "Left_Button").gameObject.SetActive(false);//�󴫸}��(��)
            ExtensionMethods.FindAnyChild<Button>(roomPlayerList[j], "Right_Button").gameObject.SetActive(false);//�󴫸}��(�k)
            roomPlayerList[j].GetComponent<Image>().sprite = connectRoomRoleBackground;
        }

        //��s
        for (int i = 0; i < playerList.Count; i++)
        {
            ExtensionMethods.FindAnyChild<Text>(roomPlayerList[i], "ID_Text").text = playerList[i];
            
            //��ܧ󴫸}����s
            if(selfNickName == playerList[i])
            {
                connectRoomLeftRole_Button = ExtensionMethods.FindAnyChild<Button>(roomPlayerList[i], "Left_Button");//�󴫸}��(��)
                connectRoomLeftRole_Button.gameObject.SetActive(true);
                connectRoomLeftRole_Button.onClick.AddListener(delegate { OnConnectRoomChangeRole(-1); });
                connectRoomRightRole_Button = ExtensionMethods.FindAnyChild<Button>(roomPlayerList[i], "Right_Button");//�󴫸}��(�k)
                connectRoomRightRole_Button.gameObject.SetActive(true);
                connectRoomRightRole_Button.onClick.AddListener(delegate { OnConnectRoomChangeRole(1); });
            }           
        }             
    }

    /// <summary>
    /// ��s���a�}��
    /// </summary>
    /// <param name="number"></param>
    /// <param name="characters"></param>
    public void OnRefreshPlayerCharacters(int number, int characters)
    {
        for (int i = 0; i < roomPlayerList.Count; i++)
        {
            if(i == number)
            {
                roomPlayerList[i].GetComponent<Image>().sprite = roleSelect_Sprite[characters];
                continue;
            }            
        }
    }

    /// <summary>
    /// �}��󴫮ɶ�(�קK�d��)
    /// </summary>
    void OnConnectRoomChangeRoleTime()
    {
        if (connectRoomChangeRoleTime > 0) connectRoomChangeRoleTime -= Time.deltaTime;
    }

    /// <summary>
    /// �󴫸}��
    /// </summary>
    /// <param name="value">��ܸ}��W��</param>
    void OnConnectRoomChangeRole(int value)
    {
        if (connectRoomChangeRoleTime <= 0)
        {
            connectRoomChangeRoleTime = 0.1f;//�קK�d��

            int role = GameDataManagement.Instance.selectRoleNumber;
            role += value;
            if (role < 0) role = roleSelect_Sprite.Length - 1;
            if (role > roleSelect_Sprite.Length - 1) role = 0;

            GameDataManagement.Instance.selectRoleNumber = role;
            PhotonConnect.Instance.OnSendRoomPlayerCharacters();
        }
    }

    /// <summary>
    /// ��L�o�e��ѰT��
    /// </summary>
    void OnKeyboardSendChatMessage()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) OnRoomSendMessage();
    }

    /// <summary>
    /// �o�e�T��
    /// </summary>
    void OnRoomSendMessage()
    {
        char[] charsToTrim = {' '};
        string mes = roomMessage_InputField.text.Trim(charsToTrim);
        PhotonConnect.Instance.OnSendRoomChatMessage(mes);

        roomMessage_InputField.text = "";
        roomMessage_InputField.ActivateInputField();//�i�H�@����J
    }

    /// <summary>
    /// �����ж���ѰT��
    /// </summary>
    /// <param name="message">�T��</param>
    public void OnGetRoomChatMessage(string message)
    {
        roomChatList.Add(message);
        if (roomChatList.Count > 6) roomChatList.RemoveAt(0);

        chatBox_Text.text = string.Join("\n", roomChatList);
    }

    /// <summary>
    /// ������d���s
    /// </summary>
    /// <param name="value">���d�W��</param>
    void OnRoomSelectLevelButton(int value)
    {
        int levelNumber = GameDataManagement.Instance.selectLevelNumber;
        levelNumber += value;
        if (levelNumber < 0) levelNumber = GameDataManagement.Instance.numericalValue.levelNames.Length - 1;
        if (levelNumber > GameDataManagement.Instance.numericalValue.levelNames.Length - 1) levelNumber = 0;

        GameDataManagement.Instance.selectLevelNumber = levelNumber;
        PhotonConnect.Instance.OnSendLevelNumber(levelNumber);
    }

    /// <summary>
    /// �ж����d��r
    /// </summary>
    /// <param name="level">��ܪ����d</param>
    public void OnRoomLevelText(int level)
    {        
        roomSelectLevel_Text.text = GameDataManagement.Instance.numericalValue.levelNames[level];
    }

    /// <summary>
    /// �}�l�s�u�C��
    /// </summary>
    void OnStartConnectGame()
    {
        if (PhotonConnect.Instance.OnStartGame(GameDataManagement.Instance.selectLevelNumber + 1))
        {     
            roomStartGame_Button.enabled = false;//�������s(�קK�s��)
        }
        else roomTipTime = 3;
    }    

    /// <summary>
    /// �ж����ܤ�r
    /// </summary>
    void OnRoomTipTime()
    {
        if (roomTipTime > 0)
        {
            roomTipTime -= Time.deltaTime;
            Color color = new Color(roomTip_Text.color.r, roomTip_Text.color.g, roomTip_Text.color.b, roomTipTime);
            roomTip_Text.color = color;
        }
    }
    #endregion
}
