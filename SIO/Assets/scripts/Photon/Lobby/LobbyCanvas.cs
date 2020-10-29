using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour
{
    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    public string LastRoomName;
    public RoomInfo LastRoom =null;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }
    public MainSceneManagement _MainSceneManagement;
   public void OnClickJoinRoom(RoomInfo roomName)
   {
            if (PhotonNetwork.JoinRoom(roomName.Name))
            {
              
            }
            else
            {
                Debug.Log("join room failed !");
            }
   }
}
