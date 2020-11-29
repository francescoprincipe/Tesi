using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    int roomSize;

    public bool offlineMode = true;

    public void SetOfflineMode(bool value)
    {
        this.offlineMode = value;
       // PhotonNetwork.OfflineMode = value;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void QuickStart(bool offline)
    {
        SetOfflineMode(offline);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Creating room");
        int randomNumber = Random.Range(0, 10000);
        int maxPlayers = offlineMode ? 1 : roomSize;
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)maxPlayers

        };
        PhotonNetwork.CreateRoom("Room" + randomNumber, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room, trying again");
        CreateRoom();
    }

    public void QuickCancel()
    {
        PhotonNetwork.LeaveRoom();
    }


}
