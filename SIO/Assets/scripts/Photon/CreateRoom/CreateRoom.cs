using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    #region statics variables
    private static List<int> times = new List<int> { 2, 5, 10 };

    private static List<byte> maxPlayers = new List<byte> { 2, 3, 4, 5 };

    private static List<string> RoomTypes = new List<string> { "Private", "Public" };
    #endregion

    public Text RoomName;
    public TMP_Dropdown MaxPlayersDropDown;
    public TMP_Dropdown RoomTypeDropDown;
    public TMP_Dropdown RoomTimeDropDown;

    public string currentRoomName;
    public byte currentMaxPlayers;
    public bool currentroomType;
    public int currentRoomTime;



    public void GetValueFromUI()
    {
        currentRoomName = RoomName.text;
        currentMaxPlayers = maxPlayers[MaxPlayersDropDown.value];
        currentRoomTime = times[RoomTimeDropDown.value];

        currentroomType = Convert.ToBoolean(RoomTypeDropDown.value);
    }
    private void OnEnable()
=======
    [SerializeField]
    private Text _roomName;
    private Text RoomName
>>>>>>> parent of d260ca6... UI cleanup
=======
    [SerializeField]
    private Text _roomName;
    private Text RoomName
>>>>>>> parent of d260ca6... UI cleanup
=======
    [SerializeField]
    private Text _roomName;
    private Text RoomName
>>>>>>> parent of d260ca6... UI cleanup
    {
        get { return _roomName; }
    }
    public void OnClick_CreateRoom()
    {
        ExitGames.Client.Photon.Hashtable Costume = new ExitGames.Client.Photon.Hashtable();
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        Costume.Add("Time", currentRoomTime);
        Costume.Add("Count", 1);

=======
        Costume.Add("Time", 5);
>>>>>>> parent of d260ca6... UI cleanup
=======
        Costume.Add("Time", 5);
>>>>>>> parent of d260ca6... UI cleanup
=======
        Costume.Add("Time", 5);
>>>>>>> parent of d260ca6... UI cleanup
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
            if (PhotonNetwork.room != null)
                Debug.Log("create room Succefully players count : " + PhotonNetwork.room.CustomProperties["Time"].ToString());

        }
        else
        {
            Debug.Log("create room failed to sent");
        }
    }
    private void OnClickQuickGame()
    {



    }
    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        Debug.Log("create room failed : " + codeAndMessage[1]);
    }
    private void OnCreatedRoom()
    {
        Debug.Log("room create  Succefully");
    }
}
