using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss���
/// </summary>
public class BossField : MonoBehaviourPunCallbacks
{
    void Update()
    {
        if (!GameSceneManagement.Instance.isCreateBoss)
        {
            if (Physics.CheckBox(transform.position, new Vector3(5, 5, 20), Quaternion.identity, 1 << LayerMask.NameToLayer("Player")))
            {
                GameSceneManagement.Instance.isCreateBoss = true;

                //�D�s�u || �O�ХD
                if (!GameDataManagement.Instance.isConnect || PhotonNetwork.IsMasterClient)
                {
                    GameSceneManagement.Instance.OnCreateBoss();
                }
                else
                {
                    PhotonConnect.Instance.OnSendCreateBoss();
                }
            }
        }
        else Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(5, 5, 20));
    }
}
