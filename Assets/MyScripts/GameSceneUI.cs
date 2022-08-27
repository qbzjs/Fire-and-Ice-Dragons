using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviourPunCallbacks
{
    static GameSceneUI gameSceneUI;
    public static GameSceneUI Instance => gameSceneUI;
       
    [Header("���a�ͩR��")]        
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
    bool isGameOver;//�O�_�C������

    [Header("���ܤ�r")]
    public Text tip_Text;//���ܤ�r
    float tipTime;//��r��ܮɶ�
    public Text task_Text;//���Ȥ�r

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
        //���a�ͩR��
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

        //��L
        tip_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Tip_Text");//���ܤ�r
        tip_Text.color = new Color(tip_Text.color.r, tip_Text.color.g, tip_Text.color.b, tipTime);
        task_Text = ExtensionMethods.FindAnyChild<Text>(transform, "Task_Text");//���Ȥ�r
    }
        
    void Update()
    {        
        OnPlayerLifeBarBehavior();//���a�ͩR���欰
        OnOptionsUI();//�ﶵ����
        OnTipText();//���ܤ�r
    }
  
    /// <summary>
    /// �]�w�C������UI
    /// </summary>
    /// <param name="clearance">�O�_�L��</param>
    public void OnSetGameOverUI(bool clearance)
    {
        isGameOver = true;//�C������

        //�}�ҿﶵ
        if (isOptions)
        {
            isOptions = false;
            options.gameObject.SetActive(isOptions);
        }

        //�}�ҹC������UI
        gameOver.gameObject.SetActive(isGameOver);

        //���G��r
        if (clearance) gameOverResult_Text.text = "�L��";
        else gameOverResult_Text.text = "����";

        //��ܷƹ�
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

            //��ܷƹ�
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;            
        }

        if(isOptions)
        {
            //���q
            GameDataManagement.Instance.musicVolume = volume_Scrollbar.value;
            audioSource.volume = GameDataManagement.Instance.musicVolume;
        }
    }

    /// <summary>
    /// ���}�C��
    /// </summary>
    void OnLeaveGame()
    {
        isOptions = false;
        options.gameObject.SetActive(isOptions);

        //�s�u
        if (GameDataManagement.Instance.isConnect)
        {
            if (PhotonNetwork.IsMasterClient)
            {               
                PhotonConnect.Instance.OnSendGameTip("�Ǫ�: " + PhotonNetwork.NickName + " ���}�C��\n�m�C������...�n");
            }
            else PhotonConnect.Instance.OnSendGameTip("���a: " + PhotonNetwork.NickName + " ���}�C��");
        }
        
        StartCoroutine(LoadScene.Instance.OnLoadScene("StartScene"));        
    }

    /// <summary>
    /// �~��C��
    /// </summary>
    void OnContinueGame()
    {
        isOptions = false;
        options.gameObject.SetActive(isOptions);

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
}
