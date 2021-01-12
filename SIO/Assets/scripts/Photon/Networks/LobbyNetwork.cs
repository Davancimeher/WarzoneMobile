using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Linq;

public class LobbyNetwork : MonoBehaviour
{
    public TMP_Dropdown regionDropdown;
    public GameObject warningPanel;
    public GameObject ReconectButton;
    public GameObject waitingAnimation;
    public Text regionText;

    public bool shouldChangeRegion = false;

    public bool changeRegionRequest = false;
    private KeyValuePair<CloudRegionCode,string> selectedRegion;

    private void InitDropDown()
    {
        List<string> regionNames = new List<string>(Statics.regionsDict.Values);
        regionDropdown.AddOptions(regionNames);
    }
    private void OnEnable()
    {
        InitDropDown();
        loadLastRegion();
    }

    void Start()
    {
        if (!PhotonNetwork.connected)
        {
            Debug.Log("Connecting to server ..");
            PhotonNetwork.ConnectToRegion(selectedRegion.Key, "0.0.0");
            // PhotonNetwork.ConnectUsingSettings("0.0.0");
            
        }
    }
    public void ReconnectToMaster()
    {
        if (!PhotonNetwork.connected)
        {
            Debug.Log("Connecting to server ..");
            PhotonNetwork.ConnectToRegion(selectedRegion.Key, "0.0.0");
            // PhotonNetwork.ConnectUsingSettings("0.0.0");
        }
    }
    public void ChangeRegionAndReconnect()
    {
        if (PhotonNetwork.connected)
        {
            changeRegionRequest = true;
            PhotonNetwork.Disconnect();
            ReconectButton.SetActive(false);
            waitingAnimation.SetActive(true);
            SaveLastRegion(selectedRegion.Key.ToString());
            StartCoroutine(Reconnect());
        }
       // StartCoroutine(ChangeRegionAndReconnectCouretine());
    }
    public  void OnConnectedToMaster()
    {
        regionText.text = "Connected to : " + selectedRegion.Value;

        if (warningPanel.activeSelf)
        {
            warningPanel.SetActive(false);
            ReconectButton.SetActive(true);
            waitingAnimation.SetActive(false);
        }
        var ss = PhotonNetwork.CloudRegion;
        //adress ,port,app id,version
       
        Debug.Log("CloudRegionCode : " + ss.ToString());

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

    public void RegionSelectedChanged(int index)
    {
        if(selectedRegion.Key != Statics.regionsDict.ElementAt(index).Key)
        {
            shouldChangeRegion = true;
            //show popop
            warningPanel.SetActive(true);
            selectedRegion = Statics.regionsDict.ElementAt(index);
        }
    }
    private void SaveLastRegion(string newRegion)
    {
        PlayerPrefs.SetString("Region", newRegion);
    }
    private void loadLastRegion()
    {
        if (PlayerPrefs.HasKey("Region"))
        {
            Enum.TryParse(PlayerPrefs.GetString("Region"), out CloudRegionCode region);

            if (Statics.regionsDict.ContainsKey(region))
            {
                var value = Statics.regionsDict[region];
                selectedRegion = new KeyValuePair<CloudRegionCode, string>(region, value);
                var index = Statics.regionsDict.Keys.ToList().IndexOf(region);
                regionDropdown.value = index;
                regionText.text = "Connected to : "+ selectedRegion.Value;
            }
        }
        else
        {
            selectedRegion = Statics.regionsDict.ElementAt(0);
            regionDropdown.value = 0;
        }
    }
    IEnumerator Reconnect()
    {
        yield return new WaitForSeconds(1f);

        while (PhotonNetwork.connectionState != ConnectionState.Disconnected)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log(PhotonNetwork.connectionState);
        }
        ReconnectToMaster();
        
        StopCoroutine(Reconnect());

    }
}
