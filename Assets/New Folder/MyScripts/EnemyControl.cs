using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ĤH�����
/// </summary>
public class EnemyControl : MonoBehaviourPunCallbacks
{
    Animator animator;
    
    private void Awake()
    {        
        gameObject.layer = LayerMask.NameToLayer("Enemy");//�]�wLayer

        animator = GetComponent<Animator>();

        if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();

        //�s�u && ���O�ۤv��
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            GameSceneManagement.Instance.OnSetMiniMapPoint(transform, GameSceneManagement.Instance.loadPath.miniMapMatirial_Enemy);//�]�w�p�a���I�I
            this.enabled = false;
            return;
        }
    }
}
