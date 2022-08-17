﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class EnemyControl : MonoBehaviourPunCallbacks
{
    



    Animator animator;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");//設定Layer        
        gameObject.tag = "EnemySoldier_1";//設定Tag
        animator = GetComponent<Animator>();

 //       if (GetComponent<CharactersCollision>() == null) gameObject.AddComponent<CharactersCollision>();

        //連線 && 不是自己的
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            GameSceneManagement.Instance.OnSetMiniMapPoint(transform, GameSceneManagement.Instance.loadPath.miniMapMatirial_Enemy);//設定小地圖點點
            this.enabled = false;
            return;
        }
    }
}
