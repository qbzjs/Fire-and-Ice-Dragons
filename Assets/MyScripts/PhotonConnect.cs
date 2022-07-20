using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
    /// <param name="nickName">�ʺ�</param>
    public void OnConnectSetting(string nickName)
    {
        Debug.Log("�ǳƳs�u");

        PhotonNetwork.ConnectUsingSettings();//�]�w�s�u
        PhotonNetwork.AutomaticallySyncScene = true;

        //�]�w�ʺ�
        if (nickName == "") PhotonNetwork.NickName = "�X��" + Random.Range(0, 1000);
        else PhotonNetwork.NickName = nickName;
    }

    /// <summary>
    /// �n�J���\Ĳ�o
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("�s�u���\");

        StartSceneUI.Instance.OnIsConnected();   
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

        StartSceneUI.Instance.OnConnectModeSettingTip(tip: "�Ыةж�����");
    }

    /// <summary>
    /// �H���γЫةж��]�w
    /// </summary>
    public void OnRandomOrCreateRoomRoomSetting()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    /// <summary>
    /// �[�J�H���ж�����Ĳ�o
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("�[�J�H���ж�����:" + returnCode + ":" + message);

        StartSceneUI.Instance.OnConnectModeSettingTip(tip: "�[�J�H���ж�����");
    }

    /// <summary>
    /// ���w�ж��]�w
    /// </summary>
    /// <param name="roomName">���[�J�ж��W</param>
    public void OnSpecifyRoomSetting(string roomName)
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
        Debug.LogError("�[�J�ж�����:" + returnCode + ":" + message);

        StartSceneUI.Instance.OnConnectModeSettingTip(tip: "�䤣��ж�");
    }

    /// <summary>
    /// �[�J�ж�Ĳ�o
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("�[�J" + PhotonNetwork.CurrentRoom.Name + "�ж�");

        StartSceneUI.Instance.OnTidyConnectModeUI();
        OnReFreshPlayerNickName();
        OnSendRoomPlayerCharacters();
    }

    /// <summary>
    /// �����a�i�J�ж�
    /// </summary>
    /// <param name="newPlayer">�s���a</param>

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnReFreshPlayerNickName();
        OnSendRoomPlayerCharacters();
    }

    /// <summary>
    /// �����a���}�ж�
    /// </summary>
    /// <param name="otherPlayer">���}���a</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnReFreshPlayerNickName();       
        OnSendRoomPlayerCharacters();
    }

    /// <summary>
    /// ��s���a�ʺ�
    /// </summary>    
    void OnReFreshPlayerNickName()
    {
        //�M��List
        List<string> playerList = new List<string>(); 

        //�������a�ʺ�
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerList.Add(PhotonNetwork.PlayerList[i].NickName);
        }
        
        StartSceneUI.Instance.OnRefreshRoomPlayerNickName(playerList, PhotonNetwork.NickName);
    }

    /// <summary>
    /// �o�e�ж����a�}��
    /// </summary>
    public void OnSendRoomPlayerCharacters()
    {
        photonView.RPC("OnRefreshPlayerCharacters", RpcTarget.All, PhotonNetwork.NickName, GameDataManagement.Instance.selectRoleNumber);
    }

    /// <summary>
    /// ��s���a�}��
    /// </summary>
    /// <param name="nickName"></param>
    /// <param name="characters"></param>
    [PunRPC]
    void OnRefreshPlayerCharacters(string nickName, int characters )
    {        
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
           if(PhotonNetwork.PlayerList[i].NickName == nickName)
            {
                StartSceneUI.Instance.OnRefreshPlayerCharacters(i, characters);
                return;
            }
        }
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
    #endregion
}
