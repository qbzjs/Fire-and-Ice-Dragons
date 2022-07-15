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
    Transform roleSelectBackGround_Image;//�}���ܭI��
    GameObject roleSelect_Button;//�}���ܫ��s
    Sprite[] roleSelect_Sprite;//�}���ܹϤ�
    public List<Transform> roleSelectButton_List = new List<Transform>();//�O���Ҧ��}���ܫ��s

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
        roleSelectBackGround_Image = ExtensionMethods.FindAnyChild<Transform>(transform, "RoleSelectBackGround_Image");//�}���ܭI��
        roleSelect_Button = Resources.Load<GameObject>(loadPath.roleSelect_Button);//�}���ܫ��s
        roleSelect_Sprite = Resources.LoadAll<Sprite>(loadPath.roleSelect_Sprite);//�}���ܹϤ�     

        //���͸}���ܫ��s
        for (int i = 0; i < roleSelect_Sprite.Length; i++)
        {
            Transform role = Instantiate(roleSelect_Button).GetComponent<Transform>();
            role.SetParent(roleSelectBackGround_Image);           
            role.localPosition = new Vector3(10 + 160 * i, 0, 0);
            role.GetComponent<Image>().sprite = roleSelect_Sprite[i];

            roleSelectButton_List.Add(role);
        }

        chooseRoleScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        OnStopVideo();
        OnTipTextGlintControl();
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
    #endregion
}
