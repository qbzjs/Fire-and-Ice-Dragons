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

    //�ͩR��
    float maxHp;
    float hp;

    //���ͤh�L�ɶ�
    float createSoldierTime;//���ͤh�L�ɶ�
    float createTime;//���ͤh�L�ɶ�(�p�ɾ�)

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
        createSoldierTime = 3;//���ͤh�L�ɶ�
        createTime = createSoldierTime;//���ͤh�L�ɶ�(�p�ɾ�)
    }

    void Update()
    {
        //�D�s�u || �O�ХD
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            if (hp > 0 && stage == GameSceneManagement.Instance.taskStage)
            {
                createTime -= Time.deltaTime;//���ͤh�L�ɶ�(�p�ɾ�)
                if (createTime <= 0)
                {
                    GameSceneManagement.Instance.OnCreateSoldier(transform, gameObject.tag);
                    createTime = createSoldierTime;
                }
            }
        }
        //����
        if (Input.GetKeyDown(KeyCode.K)) OnGetHit("Player", 1000);
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
            hp -= damage;

            //�s�u
            if (GameDataManagement.Instance.isConnect)
            {
                PhotonConnect.Instance.OnSendStrongholdGetHit(id, damage);
            }

            if (hp <= 0)
            {
                hp = 0;

                //����
                GameSceneManagement.Instance.OnTaskText();//���Ȥ�r                
                if (GameDataManagement.Instance.isConnect)//�s�u
                {
                    PhotonConnect.Instance.OnSendRenewTask();//��s����
                }

                //�s�u�Ҧ�
                if (GameDataManagement.Instance.isConnect)
                {                    
                    PhotonConnect.Instance.OnSendObjectActive(gameObject, false);
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
    }
}
