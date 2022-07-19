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
    VideoPlayer videoPlayer;

    [Header("�}�l�e��")]
    Transform startScreen;//startScreen UI����        
    Text startTip_Text;//���ܤ�r
    float startTip_Text_alpha;////���ܤ�rAlpha
    int startTipGlintControl;//�{�{����

    [Header("��ܸ}��e��")]
    Transform selectRoleScreen;//SelectRoleScreen UI����
    Button roleConfirm_Button;//�}��T�w���s
    Button roleBack_Button;//��^���s
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
    Button level_1_Button;//���d1_���s
    int selectLevel;//��ܪ����d

    [Header("��ܼҦ��e��")] 
    Transform selectModeScreen;//SelectModeScreen UI����
    Button mode_Back_Button;//��^���s
    Button mode_Single_Button;//��H�Ҧ����s
    Button mode_Connect_Button;//�s�u�Ҧ����s

    [Header("�s�u�Ҧ��e��")]
    Transform connectModeScreen;//ConnectModeScreen UI����

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
        //�v��
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();
        videoPlayer.clip = Resources.Load<VideoClip>(loadPath.startVideo);

        //�}�l�e��
        startScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "StartScreen");//startScreen UI����        
        startTip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "StartTip_Text");//���ܤ�r

        //�e��UI����
        startScreen.gameObject.SetActive(false);        
    }

    /// <summary>
    /// �﨤�e���w��
    /// </summary>
    void OnChooseRoleScreenPrepare()
    {
        //��ܸ}��e��
        selectRoleScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SelectRoleScreen");////SelectRoleScreen UI����        
        roleConfirm_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleConfirm_Button");//�}��T�w���s
        roleConfirm_Button.onClick.AddListener(OnRoleConfirm);
        roleBack_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleBack_Button");//��^���s
        roleBack_Button.onClick.AddListener(OnBackSelectModeScreen);
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
        level_1_Button = ExtensionMethods.FindAnyChild<Button>(transform, "Level_1_Button");//���d1_���s
        level_1_Button.onClick.AddListener(() => { OnSelectLecel(level: 1); });

        levelScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// ��ܼҦ��e���w��
    /// </summary>
    void OnSelectModeScreenPrepare()
    {
        selectModeScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "SelectModeScreen");//selectModeScreen UI����
        mode_Back_Button = ExtensionMethods.FindAnyChild<Button>(transform, "Mode_Back_Button");//��^���s
        mode_Back_Button.onClick.AddListener(OnBackLevelScreen);
        mode_Single_Button = ExtensionMethods.FindAnyChild<Button>(transform, "Mode_Single_Button");//��H�Ҧ����s
        mode_Single_Button.onClick.AddListener(OnIntoSelectRoleScreen);
        mode_Connect_Button = ExtensionMethods.FindAnyChild<Button>(transform, "Mode_Connect_Button");//�s�u�Ҧ����s
        mode_Connect_Button.onClick.AddListener(OnOpenConnectModeScreen);

        selectModeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// �s�u�Ҧ��e���w��
    /// </summary>
    void OnConnectModeScreenPrepare()
    {
        connectModeScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "ConnectModeScreen");//ConnectModeScreen UI����

        connectModeScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        OnStopVideo();
        OnTipTextGlintControl();
        OnSlideRoleButton();
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
                    startScreen.gameObject.SetActive(false);
                    levelScreen.gameObject.SetActive(true);
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

    #region �﨤�e��
    /// <summary>
    /// �}��T�w
    /// </summary>
    void OnRoleConfirm()
    {
        selectRoleScreen.gameObject.SetActive(false);
        StartCoroutine(LoadScene.Instance.OnLoadScene("GameScene" + selectLevel));
    }

    /// <summary>
    /// ��^��ܼҦ��e��
    /// </summary>
    void OnBackSelectModeScreen()
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
        selectModeScreen.gameObject.SetActive(true);        
    }   
    #endregion

    #region ��ܼҦ��e��
    /// <summary>
    /// ��^���d�e��
    /// </summary>
    void OnBackLevelScreen()
    {
        levelScreen.gameObject.SetActive(true);
        selectModeScreen.gameObject.SetActive(false);
    }

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
        PhotonNetwork.ConnectUsingSettings();//�]�w�s�u
        connectModeScreen.gameObject.SetActive(true);
        selectModeScreen.gameObject.SetActive(false);
    }
    /// <summary>
    /// �n�J���\
    /// </summary>
    public override void OnConnectedToMaster()
    {
        //SceneManager.LoadScene("Lobby");        
    }
    #endregion
}
