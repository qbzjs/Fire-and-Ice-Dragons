using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

/// <summary>
/// �}�l�����޲z
/// </summary>
public class StartSceneUI : MonoBehaviour
{
    static StartSceneUI startSceneUI;
    public static StartSceneUI Instance => startSceneUI;

    GameData_LoadPath loadPath;
    GameData_NumericalValue numericalValue;
    VideoPlayer videoPlayer;

    [Header("�}�l�e��")]
    Image background_Image;//�I��
    Transform startScreen;//startScreen UI����        
    Text startTip_Text;//���ܤ�r
    float startTip_Text_alpha;////���ܤ�rAlpha
    int startTipGlintControl;//�{�{����    

    [Header("��ܼҦ��e��")]
    Transform selectModeScreen;//SelectModeScreen UI����    
    Transform modeSelectBackground_Image;//ModeSelectBackground_Image UI���� 
    Button modeSingle_Button;//��H�Ҧ����s
    Button modeConnect_Button;//�s�u�Ҧ����s
    Text modeTip_Text;//���ܤ�r
    InputField nickName_InputField;//�ʺٿ�J��

    [Header("��ܸ}��e��")]
    Transform selectRoleScreen;//SelectRoleScreen UI����
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
    Button roleBack_Button;//��^���s

    [Header("���d�e��")]
    Transform levelScreen;//LevelScreen UI����
    Button level1_Button;//���d1_���s
    int selectLevel;//��ܪ����d
    Button levelBack_Button;//��^���s

    [Header("�s�u�Ҧ��e��")]
    Transform conncetModeScreen;//ConncetModScreen UI����
    Button createRoom_Button;//�Ыةж����s
    Button randomRoom_Button;//�H���ж����s
    Button specifyRoom_Button;//���w�ж����s
    InputField specifyRoom_InputField;//�[�J�ж��W�ٿ�J��
    InputField createRoom_InputField;//�Ыةж��W�ٿ�J��    
    float connectModeTipTime;//���ܤ�r�ɶ�
    Text connectModeTip_Text;//���ܤ�r
    Button connectModeBack_Button;//��^���s

    [Header("�s�u�ж��e��")]
    Transform connectRoomScreen;//connectRoomScreen UI����
    Button connectRoomBack_Button;//��^���s

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
        OnInital();
        OnStartScreenPrepare();
        OnChooseRoleScreenPrepare();
        OnLevelScreenPrepare();
        OnSelectModeScreenPrepare();
        OnConnectModeScreenPrepare();
        OnConnectRoomScreenPrepare();
    }

    void Update()
    {
        OnStopVideo();
        OnTipTextGlintControl();
        OnSlideRoleButton();
        OnConnectModeTip();
    }

    /// <summary>
    /// ��l���A
    /// </summary>
    private void OnInital()
    {
        GameDataManagement.Instance.selectRoleNumber = 0;//��ܪ��}��s��
        selectLevel = 0;//��ܪ����d
    }

    /// <summary>
    /// �}�l�e���w��
    /// </summary>
    void OnStartScreenPrepare()
    {
        
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();//�v��
        videoPlayer.clip = Resources.Load<VideoClip>(loadPath.startVideo);        
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
        modeSingle_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeSingle_Button");//��H�Ҧ����s
        modeSingle_Button.onClick.AddListener(OnIntoSelectRoleScreen);
        modeConnect_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ModeConnect_Button");//�s�u�Ҧ����s
        modeConnect_Button.onClick.AddListener(OnOpenConnectModeScreen);
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
        level1_Button = ExtensionMethods.FindAnyChild<Button>(transform, "Level1_Button");//���d1_���s
        level1_Button.onClick.AddListener(() => { OnSelectLecel(level: 1); });
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

        connectRoomScreen.gameObject.SetActive(false);
    }

    #region �}�l�e��  
    /// <summary>
    /// �v������
    /// </summary>
    void OnStopVideo()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Stop();
                startScreen.gameObject.SetActive(true);
            }
            else
            {
                if (startScreen.gameObject.activeSelf)
                {
                    background_Image.gameObject.SetActive(true);
                    selectModeScreen.gameObject.SetActive(true);
                    startScreen.gameObject.SetActive(false);                    
                }
            }
        }
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
        selectModeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// �}�ҳs�u�Ҧ��e��
    /// </summary>
    void OnOpenConnectModeScreen()
    {              
        PhotonConnect.Instance.OnConnectSetting(nickName:nickName_InputField.text);

        modeTip_Text.enabled = true;
        modeSelectBackground_Image.gameObject.SetActive(false);
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
            roleSelectButton_List[i].localPosition = roleSelectButton_List[i].localPosition + Vector3.right * mouseX * roleSelectButtonSlideSpeed;

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
    /// ������d
    /// </summary>
    /// <param name="level">��ܪ����d</param>
    void OnSelectLecel(int level)
    {
        selectLevel = level;//��ܪ����d
        levelScreen.gameObject.SetActive(false);
        StartCoroutine(LoadScene.Instance.OnLoadScene("GameScene" + selectLevel));
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
    void OnConnectModeBackButton()
    {
        PhotonConnect.Instance.OnDisconnectSetting();

        selectModeScreen.gameObject.SetActive(true);
        conncetModeScreen.gameObject.SetActive(false);        
    }

    
    /// <summary>
    /// �Ыةж����s
    /// </summary>
    void OnCreateRoomButton()
    {
        PhotonConnect.Instance.OnCreateRoomSetting(createRoom_InputField.text);

        OnConnectModeButtonActiveSetting(active: false);
        createRoom_InputField.text = "";
    }

    /// <summary>
    /// �H���ж����s
    /// </summary>
    void OnRandomRoomButton()
    {
        PhotonConnect.Instance.OnRandomOrCreateRoomRoomSetting();

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

        PhotonConnect.Instance.OnSpecifyRoomSetting(specifyRoom_InputField.text);

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
    /// �w�[�J�ж�
    /// </summary>
    public void OnIsJoinedRoom()
    {
        connectRoomScreen.gameObject.SetActive(true);
        conncetModeScreen.gameObject.SetActive(false);        

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
    #endregion
}
