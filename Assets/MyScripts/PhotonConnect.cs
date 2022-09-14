using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

/// <summary>
/// Photon�s�u
/// </summary>
public class PhotonConnect : MonoBehaviourPunCallbacks
{
    static PhotonConnect photonConnect;
    public static PhotonConnect Instance => photonConnect;

    void Awake()
    {
        if(photonConnect != null)
        {
            Destroy(this);
            return;
        }
        photonConnect = this;
        DontDestroyOnLoad(gameObject);
    }

    #region �s�u
    /// <summary>
    /// �s�u�]�w
    /// </summary>
    public void OnConnectSetting()
    {
        Debug.Log("�ǳƳs�u");

        PhotonNetwork.ConnectUsingSettings();//�]�w�s�u
        PhotonNetwork.AutomaticallySyncScene = true;//�}�Ҧ۰ʦP�B����
    }

    /// <summary>
    /// �n�J���\Ĳ�o
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("�s�u���\");

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.�}�l����)  StartSceneUI.Instance.OnIsConnected();
        GameDataManagement.Instance.isConnect = true;
    }

    /// <summary>
    /// ���u�]�w
    /// </summary>
    public void OnDisconnectSetting()
    {
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// ���uĲ�o
    /// </summary>
    /// <param name="cause">���u��]</param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("�w���u");       
        GameDataManagement.Instance.isConnect = false;
    }

    /// <summary>
    /// �]�w�ʺ�
    /// </summary>
    /// <param name="nickName">�ʺ�</param>
    public void OnSetNickName(string nickName)
    {
        //�]�w�ʺ�
        if (nickName == "") PhotonNetwork.NickName = "�X��" + UnityEngine.Random.Range(0, 1000);
        else PhotonNetwork.NickName = nickName;
    }
    #endregion

    #region �ж�
    /// <summary>
    /// �Ыةж��]�w
    /// </summary>
    /// <param name="roomName"><�ж��W��/param>
    public void OnCreateRoomSetting(string roomName)
    {
        if (roomName == "") roomName = PhotonNetwork.NickName;

        //(�����W��, �Ыةж����(MaxPlayers = �̤j�H��), �j�U����)
        PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions { MaxPlayers = 4 }, null);
    }

    /// <summary>
    /// �Ыةж�����Ĳ�o
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("�Ыةж�����:" + returnCode + ":" + message);

        if(GameDataManagement.Instance.stage == GameDataManagement.Stage.�}�l����) StartSceneUI.Instance.OnConnectModeSettingTip(tip: "�Ыةж�����");
    }

    /// <summary>
    /// �[�J�H���ж��]�w
    /// </summary>
    public void OnJoinRandomRoomRoomSetting()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// �[�J�H���ж�����Ĳ�o
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("�ثe�S���ж�:" + returnCode + ":" + message);

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.�}�l����) StartSceneUI.Instance.OnConnectModeSettingTip(tip: "�ثe�S���ж�");
    }

    /// <summary>
    /// �[�J���w�ж��]�w
    /// </summary>
    /// <param name="roomName">���[�J�ж��W</param>
    public void OnJoinSpecifyRoomSetting(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    /// <summary>
    /// �[�J�ж�����
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("�䤣��ж�:" + returnCode + ":" + message);

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.�}�l����)
        {
            if (returnCode == 32765) StartSceneUI.Instance.OnConnectModeSettingTip(tip: "�ж��w��");
            if (returnCode == 32760) StartSceneUI.Instance.OnConnectModeSettingTip(tip: "�䤣��ж�");
            else StartSceneUI.Instance.OnConnectModeSettingTip(tip: "�䤣��ж�");
        }
    }

    /// <summary>
    /// �[�J�ж�Ĳ�o
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("�[�J" + PhotonNetwork.CurrentRoom.Name + "�ж�");

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.�}�l����) StartSceneUI.Instance.OnTidyConnectModeUI(roomName: PhotonNetwork.CurrentRoom.Name);
        OnReFreshPlayerNickName();
        OnSendRoomPlayerCharacters();
        if (PhotonNetwork.IsMasterClient) OnSendLevelNumber(GameDataManagement.Instance.selectLevelNumber);
    }

    /// <summary>
    /// �����a�i�J�ж�
    /// </summary>
    /// <param name="newPlayer">�s���a</param>

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnReFreshPlayerNickName();
        OnSendRoomPlayerCharacters();
        if (PhotonNetwork.IsMasterClient) OnSendLevelNumber(GameDataManagement.Instance.selectLevelNumber);
    }

    /// <summary>
    /// �����a���}�ж�
    /// </summary>
    /// <param name="otherPlayer">���}���a</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnReFreshPlayerNickName();       
        OnSendRoomPlayerCharacters();
        if (PhotonNetwork.IsMasterClient) OnSendLevelNumber(GameDataManagement.Instance.selectLevelNumber);
    }

    /// <summary>
    /// ���}�ж��]�w
    /// </summary>
    public void OnLeaveRoomSetting()
    {
        Debug.Log("�ǳ����}�ж�");

        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// ���}�ж�Ĳ�o
    /// </summary>
    public override void OnLeftRoom()
    {
        Debug.Log("���}�ж�");        
    }

    /// <summary>
    /// ��s���a�ʺ�
    /// </summary>    
    public void OnReFreshPlayerNickName()
    {
        //�M��List
        List<string> playerList = new List<string>();

        //�������a�ʺ�
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerList.Add(PhotonNetwork.PlayerList[i].NickName);
        }

        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.�}�l����) StartSceneUI.Instance.OnRefreshRoomPlayerNickName(playerList, PhotonNetwork.NickName, PhotonNetwork.IsMasterClient);
    }

    /// <summary>
    /// �o�e�ж����a�}��
    /// </summary>
    public void OnSendRoomPlayerCharacters()
    {        
        photonView.RPC("OnRefreshPlayerCharacters", RpcTarget.All, GameDataManagement.Instance.selectRoleNumber);
    }

    /// <summary>
    /// ��s���a�}��
    /// </summary>
    /// <param name="characters">�}��s��</param>
    /// <param name="info">�ǰe�̰T��</param>
    [PunRPC]
    void OnRefreshPlayerCharacters(int characters, PhotonMessageInfo info)
    {        
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == info.Sender.NickName)
            {
                if (GameDataManagement.Instance.stage == GameDataManagement.Stage.�}�l����) StartSceneUI.Instance.OnRefreshPlayerCharacters(i, characters);
                return;
            }
        }
    }

    /// <summary>
    /// �o�e��ѰT��
    /// </summary>
    /// <param name="message">�T��</param>
    public void OnSendRoomChatMessage(string message)
    {
        photonView.RPC("OnRoomChatMessage", RpcTarget.All, message);
    }

    /// <summary>
    /// �ж��T��
    /// </summary>
    /// <param name="message">�T��</param>
    /// <param name="info">�ǰe�̰T��</param>
    [PunRPC]
    void OnRoomChatMessage(string message, PhotonMessageInfo info)
    {
        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.�}�l����) StartSceneUI.Instance.OnGetRoomChatMessage(info.Sender.NickName + ":" + message);
    }

    /// <summary>
    /// �o�e���d�s��
    /// </summary>
    /// <param name="level">��ܪ����d</param>
    public void OnSendLevelNumber(int level)
    {
        photonView.RPC("OnLevelNumber", RpcTarget.All, level);
    }

    /// <summary>
    /// ���d�s��
    /// </summary>
    /// <param name="level">��ܪ����d</param>
    /// <param name="info">�ǰe�̰T��</param>
    [PunRPC]
    void OnLevelNumber(int level, PhotonMessageInfo info)
    {
        if (GameDataManagement.Instance.stage == GameDataManagement.Stage.�}�l����)
        {
            StartSceneUI.Instance.OnRoomLevelText(level);
            GameDataManagement.Instance.selectLevelNumber = level;
        }
    }

    /// <summary>
    /// �}�l�C��
    /// </summary>
    /// <param name="level">�i�J���d�s��</param>
    public bool OnStartGame(int level)
    {
        bool isStartGame = false;               
        
        //2�H�H�W�}�l
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            isStartGame = true;
            
            photonView.RPC("OnLoadScene", RpcTarget.All, level);
            /*PhotonNetwork.LoadLevel("LevelScene" + level);
            PhotonNetwork.CurrentRoom.IsOpen = false;//�����ж�*/
        }
        else isStartGame = false;

        return isStartGame;
    }
    #endregion

    #region �C����
    /// <summary>
    /// ���J����
    /// </summary>
    /// <param name="level">�����s��</param>
    [PunRPC]
    void OnLoadScene(int level)
    {
        StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(level));
    }

    void OnStartIntoGame(int level)
    {
        PhotonNetwork.LoadLevel("LevelScene" + level);
        PhotonNetwork.CurrentRoom.IsOpen = false;//�����ж�
    }
    /// <summary>
    /// �ХD�洫Ĳ�o
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.LogError("�ХD�洫" + newMasterClient.ToString());
    }

    /// <summary>
    /// �o�e��s����
    /// </summary>
    /// <param name="enemyName">���˪���W��</param>
    public void OnSendRenewTask(string enemyName)
    {
        photonView.RPC("OnRenewTask", RpcTarget.Others, enemyName);
    }

    /// <summary>
    /// ��s����
    /// </summary>
    /// <param name="enemyName">���˪���W��</param>
    /// <param name="info">�o�e�̸�T</param>
    [PunRPC]
    void OnRenewTask(string enemyName, PhotonMessageInfo info)
    {
        //GameSceneManagement.Instance.taskNumber += 1;//�w�����Ǫ��ƶq
        GameSceneUI.Instance.SetEnemyLifeBarActive = false;
        GameSceneManagement.Instance.OnTaskText();//���Ȥ�r
        GameSceneUI.Instance.OnSetTip($"{enemyName}�w����", 5);//�]�w���ܤ�r
    }

    /// <summary>
    /// �o�e�C�����ܤ�r
    /// </summary>
    /// <param name="nickName">�o�e�̼ʺ�</param>
    public void OnSendGameTip(string nickName)
    {        
        photonView.RPC("OnGameTip", RpcTarget.Others, nickName);
    }

    /// <summary>
    /// �C�����ܤ�r
    /// </summary>
    /// <param name="nickName">�o�e�̼ʺ�</param>
    /// <param name="info">�ǰe�̰T��</param>
    [PunRPC]
    void OnGameTip(string nickName, PhotonMessageInfo info)
    {
        GameSceneUI.Instance.OnSetTip(nickName, 3);
    }

    /// <summary>
    /// �Ыت���
    /// </summary>
    /// <param name="path">prefeb���|</param>
    /// <returns></returns>
    public GameObject OnCreateObject(string path)
    {        
        return PhotonNetwork.Instantiate(path, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// �o�e����E�����A
    /// </summary>
    /// <param name="obj">����</param>
    /// <param name="active">�E�����A</param>
    public void OnSendObjectActive(GameObject obj, bool active)
    {        
        if (obj.GetComponent<PhotonView>())
        {            
            int id = obj.GetComponent<PhotonView>().ViewID;
            photonView.RPC("OnObjectActive", RpcTarget.Others, id, active);
        }
    }

    /// <summary>
    /// ����E��
    /// </summary>
    /// <param name="targetID">����ID</param>
    /// <param name="active">�E�����A</param>
    /// <param name="info">�ǰe�̰T��</param>
    [PunRPC]
    void OnObjectActive(int targetID, bool active, PhotonMessageInfo info)
    {
        GameSceneManagement.Instance.OnConnectObjectActive(targetID, active);
    }

    /// <summary>
    /// �o�e�����T��
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="position">��m</param>
    /// <param name="rotation">����</param>
    /// <param name="damage">����ˮ`</param>
    /// <param name="isCritical">�O�_�z��</param>
    /// <param name="repel">���h�Z��</param>
    /// <param name="attackerObjectID">�����̪���ID</param>
    /// <param name="attackerID">������ID</param>
    public void OnSendGetHit(int targetID, Vector3 position, Quaternion rotation, float damage,bool isCritical, float repel, int attackerObjectID, int attackerID)
    {        
        photonView.RPC("OnGetHit", RpcTarget.Others, targetID, position, rotation, damage, isCritical, repel, attackerObjectID, attackerID);
    }

    /// <summary>
    /// �����T��
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="position">��m</param>
    /// <param name="rotation">����</param>
    /// <param name="damage">����ˮ`</param>
    /// <param name="isCritical">�O�_�z��</param>
    /// <param name="repel">���h�Z��</param>
    /// <param name="attackerObjectID">�����̪���ID</param>
    /// <param name="attackerID">������ID</param>
    [PunRPC]
    void OnGetHit(int targetID, Vector3 position, Quaternion rotation, float damage, bool isCritical, float repel, int attackerObjectID, int attackerID)
    {

        GameSceneManagement.Instance.OnConnectGetHit(targetID, position, rotation, damage, isCritical, repel, attackerObjectID, attackerID);
    }

    /// <summary>
    /// �o�e���I�����T��
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="damage">����ˮ`</param>
    public void OnSendStrongholdGetHit(int targetID, float damage)
    {
        photonView.RPC("OnStrongholdGetHit", RpcTarget.Others, targetID, damage);
    }

    /// <summary>
    /// ���I�����T��
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="damage">����ˮ`</param>
    [PunRPC]
    void OnStrongholdGetHit(int targetID, float damage)
    {
        GameSceneManagement.Instance.OnConnectStrongholdGetHit(targetID, damage);
    }

    /// <summary>
    /// �o�e���v���T��
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="heal">�v���q</param>
    /// <param name="isCritical">�O�_�z��</param>
    public void OnSendGetHeal(int targetID, float heal, bool isCritical)
    {
        photonView.RPC("OnGetHeal", RpcTarget.Others, targetID, heal, isCritical);
    }

    /// <summary>
    /// ���v���T��
    /// </summary>
    /// <param name="targetID">�ؼ�ID</param>
    /// <param name="heal">�v���q</param>
    /// <param name="isCritical">�O�_�z��</param>
    [PunRPC]
    void OnGetHeal(int targetID, float heal, bool isCritical)
    {
        GameSceneManagement.Instance.OnConnectGetHeal(targetID, heal, isCritical);
    }

    /// <summary>
    /// �o�e�ʵe�T��
    /// </summary>
    /// <typeparam name="T">�x��</typeparam>
    /// <param name="targetID">�ʵe�󴫥ؼ�ID</param>
    /// <param name="anmationName">����ʵe�W��</param>
    /// <param name="animationType">�ʵeType</param>
    public void OnSendAniamtion<T>(int targetID, string anmationName, T animationType)
    {        
        switch (animationType.GetType().Name)
        {
            case "Boolean":                
                photonView.RPC("OnSetAniamtion_Boolean", RpcTarget.Others, targetID, anmationName, Convert.ToBoolean(animationType));
                break;
            case "Single":
                photonView.RPC("OnSetAniamtion_Single", RpcTarget.Others, targetID, anmationName, Convert.ToSingle(animationType));
                break;
            case "Int32":
                photonView.RPC("OnSetAniamtion_Int32", RpcTarget.Others, targetID, anmationName, Convert.ToInt32(animationType));
                break;
            case "String":
                photonView.RPC("OnSetAniamtion_String", RpcTarget.Others, targetID, anmationName, Convert.ToString(animationType));
                break;            
        }        
    }

    /// <summary>
    /// �]�w�ʵe_Boolean
    /// </summary>
    /// <param name="targetID">�ʵe�󴫥ؼ�ID</param>
    /// <param name="anmationName">����ʵe�W��</param>
    /// <param name="animationType">�ʵeType</param>
    [PunRPC]
    void OnSetAniamtion_Boolean(int targetID, string anmationName, bool animationType)
    {        
        GameSceneManagement.Instance.OnConnectAnimationSetting(targetID, anmationName, animationType);
    }

    /// <summary>
    /// �]�w�ʵe_Single
    /// </summary>
    /// <param name="targetID">�ʵe�󴫥ؼ�ID</param>
    /// <param name="anmationName">����ʵe�W��</param>
    /// <param name="animationType">�ʵeType</param>
    [PunRPC]
    void OnSetAniamtion_Single(int targetID, string anmationName, float animationType)
    {
        GameSceneManagement.Instance.OnConnectAnimationSetting(targetID, anmationName, animationType);
    }

    /// <summary>
    /// �]�w�ʵe_Int32
    /// </summary>
    /// <param name="targetID">�ʵe�󴫥ؼ�ID</param>
    /// <param name="anmationName">����ʵe�W��</param>
    /// <param name="animationType">�ʵeType</param>
    [PunRPC]
    void OnSetAniamtion_Int32(int targetID, string anmationName, int animationType)
    {
        GameSceneManagement.Instance.OnConnectAnimationSetting(targetID, anmationName, animationType);
    }

    /// <summary>
    /// �]�w�ʵe_String
    /// </summary>
    /// <param name="targetID">�ʵe�󴫥ؼ�ID</param>
    /// <param name="anmationName">����ʵe�W��</param>
    /// <param name="animationType">�ʵeType</param>
    [PunRPC]
    void OnSetAniamtion_String(int targetID, string anmationName, string animationType)
    {
        GameSceneManagement.Instance.OnConnectAnimationSetting(targetID, anmationName, anmationName);
    }

    /// <summary>
    /// �o�e�C�������T��(�ХD���}�C��)
    /// </summary>
    public void OnSendGameover()
    {
        photonView.RPC("OnGameOver", RpcTarget.Others);
    }

    /// <summary>
    /// �C������
    /// </summary>
    [PunRPC]
    void OnGameOver()
    {
        StartCoroutine(LoadScene.Instance.OnLoadScene("StartScene"));
    }

    /// <summary>
    /// �o�e�Ы�Boss�E��
    /// </summary>
    public void OnSendBossActive()
    {
        photonView.RPC("OnBossActive", RpcTarget.Others);
    }

    /// <summary>
    /// �Ы�Boss
    /// </summary>
    [PunRPC]
    void OnBossActive()
    {
        GameSceneManagement.Instance.isCreateBoss = true;
        //�O�ХD
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.FindObjectOfType<BossAI>().OnActive();
        }        
    }

    /// <summary>
    /// �o�e��L���a�ͩR��
    /// </summary>
    /// <param name="nickName">���a�ʺ�</param>
    /// <param name="hpProportion">�ͩR���</param>
    public void OnSendOtherPlayerLifeBar(string nickName, float hpProportion)
    {
        photonView.RPC("OnOtherPlayerLifeBare", RpcTarget.Others, nickName, hpProportion);
    }

    /// <summary>
    /// ��L���a�ͩR��
    /// </summary>
    /// <param name="nickName">���a�ʺ�</param>
    /// <param name="hpProportion">�ͩR���</param>
    [PunRPC]
    void OnOtherPlayerLifeBare(string nickName, float hpProportion)
    {
        GameSceneUI.Instance.OnSetOtherPlayerLifeBar(nickName, hpProportion);
    }

    /// <summary>
    /// �o�e�C���ɶ�
    /// </summary>
    /// <param name="gameTime">�C���ɶ�</param>
    public void OnSendGameTime(float gameTime)
    {
        photonView.RPC("OnGameTime", RpcTarget.All, gameTime);
    }

    /// <summary>
    /// �C���ɶ�
    /// </summary>
    /// <param name="gameTime">�C���ɶ�</param>
    [PunRPC]
    void OnGameTime(float gameTime)
    {
        //�C���ɶ�
        int minute = (int)gameTime / 60;
        int second = (int)gameTime % 60;
        
        GameSceneUI.Instance.playGameTimeOver_Text.text = $"�C �� �� �� : {minute} �� {second} ��"; 
    }

    /// <summary>
    /// �o�e�C������
    /// </summary>
    /// <param name="nickName">�ʺ�</param>
    /// <param name="MaxCombo">�̤j�s��</param>
    /// <param name="killNumber">������</param>
    /// <param name="accumulationDamage">�ֿn�ˮ`</param>
    /// <param name="gameTime">�C���ɶ�</param>
    public void OnSendGameScoring(string nickName, int MaxCombo, int killNumber, float accumulationDamage)
    {
        photonView.RPC("OnGameScoring", RpcTarget.All, nickName, MaxCombo, killNumber, accumulationDamage);
    }

    /// <summary>
    /// �C������
    /// </summary>
    /// <param name="nickName">�ʺ�</param>
    /// <param name="MaxCombo">�̤j�s��</param>
    /// <param name="killNumber">������</param>
    /// <param name="accumulationDamage">�ֿn�ˮ`</param>    
    [PunRPC]
    void OnGameScoring(string nickName, int MaxCombo, int killNumber, float accumulationDamage)
    {
        //�M��List
        List<string> playerList = new List<string>();

        //�������a�ʺ�
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerList.Add(PhotonNetwork.PlayerList[i].NickName);
        }

        GameSceneUI.Instance.OnConnectGameOver(playerList, nickName, MaxCombo, killNumber, accumulationDamage);
    }

    /// <summary>
    /// �o�e���a���`�T��
    /// </summary>
    public void OnSendPlayerDie()
    {
        photonView.RPC("OnPlayerDie", RpcTarget.All);
    }

    /// <summary>
    /// ���a���`�T��
    /// </summary>
    [PunRPC]
    void OnPlayerDie()
    {
        GameSceneManagement.Instance.lifePlayerNumber--;

        if(GameSceneManagement.Instance.lifePlayerNumber <= 0)
        {
            //�C�����G��r
            GameSceneManagement.Instance.isGameOver = true;
            GameSceneUI.Instance.OnSetGameResult(true, "�� ��");
            GameSceneManagement.Instance.OnSetGameOver(false);
            //GameSceneUI.Instance.connectGameOverResult_Text.text = " �� �� �` �� ";
            //�]�w�C������
            StartCoroutine(GameSceneManagement.Instance.OnSetGameOver(false));
        }
    }

    /// <summary>
    /// �o�e�i�J�U�@��
    /// </summary>
    public void OnSendIntoNexttLevel()
    {
        photonView.RPC("OnIntoNextLevel", RpcTarget.All);
    }

    /// <summary>
    /// �i�J�U�@��
    /// </summary>
    [PunRPC]
    void OnIntoNextLevel()
    {
        /* if (PhotonNetwork.IsMasterClient)
         {                      
             if(GameDataManagement.Instance.selectLevelNumber == 11) StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(12));
             if (GameDataManagement.Instance.selectLevelNumber == 12) StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(13));
         }*/

        PhotonNetwork.AutomaticallySyncScene = true;//�۰ʦP�B����

        if (GameSceneManagement.Instance.isVictory)
        {
            if (GameDataManagement.Instance.selectLevelNumber == 11)
            {
                GameDataManagement.Instance.selectLevelNumber = 12;
                StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(12));
                return;
            }

            if (GameDataManagement.Instance.selectLevelNumber == 12) StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(13));
        }
        else StartCoroutine(LoadScene.Instance.OnLoadScene_Connect(13));

    }

    /// <summary>
    /// �o�e���a�ʺٻPID
    /// </summary>
    /// <param name="nickName">�ʺ�</param>
    /// <param name="id">ID</param>
    public void OnSendPlayerNickNmaeAndID(string nickName, int id)
    {
        photonView.RPC("OnPlayerNickNameAndId", RpcTarget.All, nickName, id);
    }

    /// <summary>
    /// ���a�ʺٻPID
    /// </summary>
    /// <param name="nickName"></param>
    /// <param name="id"></param>
    [PunRPC]
    void OnPlayerNickNameAndId(string nickName, int id)
    {
        GameSceneManagement.Instance.OnCreatePlayerNameObject(nickName, id);
    }    
    #endregion
}
