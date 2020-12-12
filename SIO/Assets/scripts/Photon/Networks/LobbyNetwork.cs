using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
public class LobbyNetwork : MonoBehaviour
{
    void Start()
    {
        if (!PhotonNetwork.connected)
        {
            Debug.Log("Connecting to server ..");
            PhotonNetwork.ConnectUsingSettings("0.0.0");
        }
    }
    public void ReconnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings("0.0.0");
    }
    public  void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;
        PhotonNetwork.autoCleanUpPlayerObjects = true;
    }
    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public void QuickGame()
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 5,
            PlayerTtl = 1,
            EmptyRoomTtl = 5,
            CleanupCacheOnLeave = true
        };

        PhotonNetwork.JoinRandomRoom(null, 5, MatchmakingMode.FillRoom, TypedLobby.Default, "", null);
    }
    private void OnJoinedRoom() 
    { 

    }
    private void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnJoinRandomFailed");
        CreateRoom();

    }
    private void CreateRoom()
    {
        ExitGames.Client.Photon.Hashtable Costume = new ExitGames.Client.Photon.Hashtable();
        Costume.Add("Time", 5);
        Costume.Add("Count", 1);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 5,
            PlayerTtl = 1,
            EmptyRoomTtl = 5,
            CustomRoomProperties = Costume,
            CleanupCacheOnLeave = true
        };
        string roomName = PhotonNetwork.player.UserId.Split('-')[0].ToUpper();
        if (PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default))
        {
            Debug.Log("Room Name : " + PhotonNetwork.player.UserId);
            Debug.Log("create room Succefully sent");
        }
        else
        {
            Debug.Log("create room failed to sent");
        }
    }
    private void OnJoinedLobby()
    {
        Debug.Log("joined lobby");

        //if (!PhotonNetwork.inRoom)
        //{
        //    MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
        //}
    }
}
