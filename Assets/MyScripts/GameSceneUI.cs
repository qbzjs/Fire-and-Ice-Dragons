using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    static GameSceneUI gameSceneUI;
    public static GameSceneUI Instance => gameSceneUI;

    //���a�ͩR��
    float playerHpProportion;
    Image playerLifeBarFront_Image;//�ͩR��(�e)
    Image playerLifeBarMid_Image;//�ͩR��(��)

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
    }
        
    void Update()
    {
        OnPlayerLifeBarBehavior();
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
}
