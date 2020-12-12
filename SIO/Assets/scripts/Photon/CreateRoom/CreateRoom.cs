using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
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
    {
        InitDropDowns();
    }
    private RoomOptions CreateRoomOptions()
    {
        GetValueFromUI();

        ExitGames.Client.Photon.Hashtable Costume = new ExitGames.Client.Photon.Hashtable();
        Costume.Add("Time", currentRoomTime);
        Costume.Add("Count", 1);

        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = currentroomType,
            IsOpen = true,
            MaxPlayers = currentMaxPlayers,
            PlayerTtl = 1,
            EmptyRoomTtl = 5,
            CustomRoomProperties = Costume,
            CleanupCacheOnLeave = true
        };

        return roomOptions;
    }
    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = CreateRoomOptions();

        if (PhotonNetwork.CreateRoom(currentRoomName, roomOptions, TypedLobby.Default))
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
    public void InitDropDowns()
    {
        InitTimeDropDown();
        InitMaxPlayersDropDown();
        InitRoomTypeDropDown();
    }
    #region WTF
    public void InitTimeDropDown()
    {
        List<TMP_Dropdown.OptionData> TimeList = new List<TMP_Dropdown.OptionData>();

        foreach (var time in times)
        {
            var option = new TMP_Dropdown.OptionData(time.ToString());
            TimeList.Add(option);
        }
        RoomTimeDropDown.AddOptions(TimeList);
    }
    public void InitMaxPlayersDropDown()
    {
        List<TMP_Dropdown.OptionData> maxPlayersList = new List<TMP_Dropdown.OptionData>();

        foreach (var max in maxPlayers)
        {
            var option = new TMP_Dropdown.OptionData(max.ToString());
            maxPlayersList.Add(option);
        }
        MaxPlayersDropDown.AddOptions(maxPlayersList);
    }
    public void InitRoomTypeDropDown()
    {
        List<TMP_Dropdown.OptionData> roomTypeList = new List<TMP_Dropdown.OptionData>();

        foreach (var type in RoomTypes)
        {
            var option = new TMP_Dropdown.OptionData(type);
            roomTypeList.Add(option);
        }
        RoomTypeDropDown.AddOptions(roomTypeList);
    }
    #endregion
}
