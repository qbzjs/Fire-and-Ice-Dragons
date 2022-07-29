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
    float playerHpProportion;
    Image playerLifeBarFront_Image;//�ͩR��(�e)
    Image playerLifeBarMid_Image;//�ͩR��(��)

    [Header("�ﶵ")]
    Transform options;//Options UI����
    bool isOptions;//�O�_�}�_�ﶵ����
    Button leaveGame_Button;//���}�C�����s
    Button continueGame_Button;//�~��C�����s

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

        //�ﶵ
        options = ExtensionMethods.FindAnyChild<Transform>(transform, "Options");//Options UI����
        options.gameObject.SetActive(false);
        leaveGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "LeaveGame_Button");//���}�C�����s
        leaveGame_Button.onClick.AddListener(OnLeaveGame);
        continueGame_Button = ExtensionMethods.FindAnyChild<Button>(transform, "ContinueGame_Button");//�~��C�����s
        continueGame_Button.onClick.AddListener(OnContinueGame);
    }
        
    void Update()
    {
        OnPlayerLifeBarBehavior();
        OnOptions();
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
    void OnOptions()
    {
        //�}�ҿﶵ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOptions = !isOptions; 
            options.gameObject.SetActive(isOptions);
        }
    }

    /// <summary>
    /// ���}�C��
    /// </summary>
    void OnLeaveGame()
    {
        isOptions = false;
        options.gameObject.SetActive(isOptions);

        //�s�u�Ҧ� & �O�ХD
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            PhotonConnect.Instance.OnSendGameover();
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
    }   
}
