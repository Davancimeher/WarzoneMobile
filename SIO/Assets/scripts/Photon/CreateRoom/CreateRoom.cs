using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get { return _roomName; }
    }
    public void OnClick_CreateRoom()
    {
        ExitGames.Client.Photon.Hashtable Costume = new ExitGames.Client.Photon.Hashtable();
        Costume.Add("Time", 5);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 5,
            PlayerTtl=1,
            EmptyRoomTtl=5,
            CustomRoomProperties=Costume,
            CleanupCacheOnLeave=true
        };
        if (PhotonNetwork.CreateRoom(RoomName.text,roomOptions,TypedLobby.Default))
        {
            Debug.Log("create room Succefully sent");
        }
        else
        {
            Debug.Log("create room failed to sent");
        }
    }
    private void OnClickQuickGame() {
    
    
    
    }
    private void OnPhotonCreateRoomFailed(object [] codeAndMessage) 
    {
        Debug.Log("create room failed : " + codeAndMessage[1]);
    }
    private void OnCreatedRoom()
    {
        Debug.Log("room create  Succefully");
    }
}
