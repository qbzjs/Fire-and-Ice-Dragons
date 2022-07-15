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
    VideoPlayer videoPlayer;

    [Header("�}�l�e��")]
    Transform startScreen;//startScreen UI����        
    Text startTip_Text;//���ܤ�r
    float startTip_Text_alpha;////���ܤ�rAlpha
    int startTipGlintControl;//�{�{����

    [Header("��ܸ}��e��")]
    Transform chooseRoleScreen;//chooseRoleScreen UI����
    Button roleConfirm_Button;//�}��T�w���s
    [Header("��ܸ}��e��/�}���ܫ��s")]
    bool isSlideRoleButton;//�O�_�ưʸ}����s    
    GameObject roleSelect_Button;//�}���ܫ��s
    Sprite[] roleSelect_Sprite;//�}���ܹϤ�
    Button roleSelectRight_Button;//�}����s����(�k)
    Button roleSelectLeft_Button;//�}����s����(��)
    public List<Transform> roleSelectButton_List = new List<Transform>();//�O���Ҧ��}���ܫ��s
    Image roleSelectBackGround_Image;//�}���ܭI��
    float roleSelectButtonSizeX;//�}���ܫ��sSizeX
    float roleSelectButtonSpacing;//�}���ܫ��s���Z
    float roleSelectButtonSlideSpeed;//�}��ưʫ��s�t��
    float mouseX;//Input MouseX    
    Image rolePicture_Image;//��ܪ��}��(�j��)

    void Awake()
    {
        if(startSceneUI != null)
        {
            Destroy(this);
            return;
        }
        startSceneUI = this;
    }

    void Start()
    {
        loadPath = GameDataManagement.Insrance.loadPath;

        OnStartScreenPrepare();
        OnChooseRoleScreenPrepare();        
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

        startScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// �﨤�e���w��
    /// </summary>
    void OnChooseRoleScreenPrepare()
    {
        //��ܸ}��e��
        chooseRoleScreen = ExtensionMethods.FindAnyChild<Transform>(transform, "ChooseRoleScreen");////chooseRoleScreen UI����        
        roleConfirm_Button = ExtensionMethods.FindAnyChild<Button>(transform, "RoleConfirm_Button");//�}��T�w���s
        roleConfirm_Button.onClick.AddListener(OnRoleConfirm);
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

        rolePicture_Image = ExtensionMethods.FindAnyChild<Image>(transform, "RolePicture_Image");//��ܪ��}��(�j��)
        rolePicture_Image.sprite = roleSelectButton_List[0].GetComponent<Image>().sprite;

        chooseRoleScreen.gameObject.SetActive(false);
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
                startScreen.gameObject.SetActive(false);
                chooseRoleScreen.gameObject.SetActive(true);                
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
        chooseRoleScreen.gameObject.SetActive(false);
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
        GameDataManagement.Insrance.selectRoleNumber = number;
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
}
