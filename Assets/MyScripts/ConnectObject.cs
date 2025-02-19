using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectObject : MonoBehaviourPunCallbacks
{    
    public int id;
    void Awake()
    {        
        //�s�u�Ҧ�
        if (GameDataManagement.Instance.isConnect)
        {
            id = GetComponent<PhotonView>().ViewID;
            GameSceneManagement.Instance.OnRecordConnectObject(id, gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(GetComponent<PhotonView>());
            Destroy(GetComponent<PhotonTransformView>());
        }
    }
}
