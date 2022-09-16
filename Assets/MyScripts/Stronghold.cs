using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���I
/// </summary>
public class Stronghold : MonoBehaviourPunCallbacks
{
    public int id;
    GameData_NumericalValue NumericalValue;

    [Header("�ĴX���q�ͧL(0 == ���q1)")]
    public int stage;

    [Header("�ؿv���W��")]
    public string builidName;

    [Header("����")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    //�ͩR��
    public float maxHp;
    public float hp;

    //���ͤh�L�ɶ�
    float createSoldierTime;//���ͤh�L�ɶ�
    int maxSoldierNumber;//�̤j�h�L�ƶq
    float createTime;//���ͤh�L�ɶ�(�p�ɾ�)

    //�P�_
    bool isGetHit;//�O�_������

    private void Awake()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect)
        {
            id = GetComponent<PhotonView>().ViewID;
            GameSceneManagement.Instance.OnRecordConnectObject(id, gameObject);

        }
        else
        {
            Destroy(GetComponent<PhotonView>());
            Destroy(GetComponent<PhotonTransformView>());
        }
    }

    void Start()
    {
        NumericalValue = GameDataManagement.Instance.numericalValue;

        //�ͩR��
        maxHp = NumericalValue.strongholdHp;
        hp = maxHp;

        //���ͤh�L�ɶ�
        createSoldierTime = 15;//���ͤh�L�ɶ�
        maxSoldierNumber = 65;//�̤j�h�L�ƶq
        //createTime = createSoldierTime;//���ͤh�L�ɶ�(�p�ɾ�)
    }

    void Update()
    {
        //�D�s�u || �O�ХD
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            if (hp > 0)
            {
                createTime -= Time.deltaTime;//���ͤh�L�ɶ�(�p�ɾ�)
                
                if (stage <= GameSceneManagement.Instance.taskStage )
                {
                    
                    if (createTime <= 0)
                    {
                        int aiNumber = GameObject.FindObjectsOfType<AI>().Length;                        
                        if(aiNumber < maxSoldierNumber) GameSceneManagement.Instance.OnCreateSoldier(transform, gameObject.tag);
                        createTime = createSoldierTime;
                    }
                }               
            }
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="attackerLayer">������layer</param>
    /// <param name="damage">����ˮ`</param>
    public void OnGetHit(string attackerLayer, float damage)
    {        
        if (gameObject.tag == "Enemy" && attackerLayer == "Player")
        {
            isGetHit = true;//�O�_������

            hp -= damage;

            //�s�u
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendStrongholdGetHit(id, damage);
            }

            //�]�w�ͩR��
            GameSceneUI.Instance.OnSetEnemyLifeBarValue(builidName, hp / maxHp);
            GameSceneUI.Instance.SetEnemyLifeBarActive = true;

            if (hp <= 0)
            {
                hp = 0;

                if (audioSource)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }  

                /*//�s�u�Ҧ�
                if (GameDataManagement.Instance.isConnect)
                {
                    PhotonConnect.Instance.OnSendObjectActive(gameObject, false);
                }*/

                if (GameSceneManagement.Instance.taskStage < GameSceneManagement.Instance.taskText.Length)
                {
                    GameSceneUI.Instance.OnSetTip($"���}{builidName}", 7);//�]�w���ܤ�r
                }
                GameSceneUI.Instance.SetEnemyLifeBarActive = false;//�����ͩR��

                //�s�u����
                if (GameDataManagement.Instance.isConnect)
                {
                    PhotonConnect.Instance.OnSendRenewTask(builidName);//��s����
                    PhotonConnect.Instance.OnSendObjectActive(gameObject, false);
                }
                else
                {
                    GameSceneManagement.Instance.OnTaskText();//���Ȥ�r 
                }

                gameObject.SetActive(false);//��������
            }
        }
    }

    /// <summary>
    /// �s�u����
    /// </summary>
    /// <param name="damage">����ˮ`</param>
    public void OnConnectGetHit(float damage)
    {
        hp -= damage;
        if (hp <= 0) hp = 0;

        /*//�]�w�ͩR��
        GameSceneUI.Instance.OnSetEnemyLifeBarValue(builidName, hp / maxHp);
        GameSceneUI.Instance.SetEnemyLifeBarActive = true*/
    }
}
