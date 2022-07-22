using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectObject : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect && !photonView.IsMine)
        {
            GameSceneManagement.Instance.OnRecordConnectObject(GetComponent<PhotonView>().ViewID, gameObject);
            gameObject.SetActive(false);
        }
    }
}
