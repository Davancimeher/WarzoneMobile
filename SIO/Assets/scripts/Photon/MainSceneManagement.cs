using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainSceneManagement : MonoBehaviour
{

    public InputField PlayerName;
    public LobbyNetwork LobbyNetwork;
    public Text ConnectionStateText;

    public GameObject reconnection;
    public GameObject PlayerNameCanvas;
    public GameObject LobbyCanvas;
    public GameObject CurrentRoomCanvas;
    public GameObject setNameCanvas;
    public GameObject StartGameCanvas;
    public Button StartButton;

    public Transform OutSideParent;
    public Transform MainCanvasParent;
    public void OnClickSetName()
    {
        GetMMTypeUI();
        PlayerNetwork.Instance.SetInfoInPlayerPrefs(PlayerName.text);
        Debug.Log("Settings info player name : " + PlayerName.text);
        // LobbyNetwork.JoinLobby();
        //PlayerNameCanvas.SetActive(false);
    }
    public void OnClickQuickGame()
    {
        LobbyNetwork.QuickGame();
    }
    public void OnClickCostumeGame()
    {
        LobbyNetwork.JoinLobby();
    }
    public void Reconnect()
    {
        LobbyNetwork.ReconnectToMaster();
    }

    public void Update()
    {
        if (PhotonNetwork.connectionState == ConnectionState.Disconnected && !reconnection.gameObject.activeSelf)
        {
            if (!LobbyNetwork.changeRegionRequest)
                reconnection.SetActive(true);
        }
        if (PhotonNetwork.connectionState == ConnectionState.Disconnected && StartButton.interactable)
        {
            StartButton.interactable = false;
        }
         ConnectionStateText.text = PhotonNetwork.connectionState.ToString();


    }

    public void ClientConnectToMaster()
    {
        reconnection.SetActive(false);
        LobbyCanvas.SetActive(true);
        CurrentRoomCanvas.SetActive(true);
        if (PlayerNetwork.Instance.FirstConnection)
        {
            setNameCanvas.SetActive(true);
            PlayerNameCanvas.SetActive(true);
            PlayerNetwork.Instance.FirstConnection = false;
        }
        StartGameCanvas.transform.SetParent(OutSideParent);
    }
    public void ClientJoinLobby()
    {
        LobbyCanvas.transform.SetParent(MainCanvasParent);

        reconnection.transform.SetParent(OutSideParent);
        PlayerNameCanvas.transform.SetParent(OutSideParent);
        setNameCanvas.transform.SetParent(OutSideParent);
        CurrentRoomCanvas.transform.SetParent(OutSideParent);
        StartGameCanvas.transform.SetParent(OutSideParent);
    }
    public void ClientJoinedRoom()
    {
        CurrentRoomCanvas.transform.SetParent(MainCanvasParent);

        reconnection.transform.SetParent(OutSideParent);
        PlayerNameCanvas.transform.SetParent(OutSideParent);
        setNameCanvas.transform.SetParent(OutSideParent);
        LobbyCanvas.transform.SetParent(OutSideParent);
        StartGameCanvas.transform.SetParent(OutSideParent);

    }
    public void ClientDisconnectedUI()
    {
        reconnection.transform.SetParent(MainCanvasParent);

        CurrentRoomCanvas.transform.SetParent(OutSideParent);
        PlayerNameCanvas.transform.SetParent(OutSideParent);
        setNameCanvas.transform.SetParent(OutSideParent);
        LobbyCanvas.transform.SetParent(OutSideParent);
        StartGameCanvas.transform.SetParent(OutSideParent);

    }
    public void ClientLeftRoom()
    {
        StartGameCanvas.transform.SetParent(MainCanvasParent);

        reconnection.transform.SetParent(OutSideParent);
        PlayerNameCanvas.transform.SetParent(OutSideParent);
        setNameCanvas.transform.SetParent(OutSideParent);
        CurrentRoomCanvas.transform.SetParent(OutSideParent);
        LobbyCanvas.transform.SetParent(OutSideParent);

        // LobbyNetwork.JoinLobby();
    }
    public void GetMMTypeUI()
    {
        StartGameCanvas.transform.SetParent(MainCanvasParent);

        reconnection.transform.SetParent(OutSideParent);
        PlayerNameCanvas.transform.SetParent(OutSideParent);
        setNameCanvas.transform.SetParent(OutSideParent);
        CurrentRoomCanvas.transform.SetParent(OutSideParent);
        LobbyCanvas.transform.SetParent(OutSideParent);
    }
    public void BackToMain()
    {
        setNameCanvas.transform.SetParent(MainCanvasParent);

        reconnection.transform.SetParent(OutSideParent);
        PlayerNameCanvas.transform.SetParent(OutSideParent);
        CurrentRoomCanvas.transform.SetParent(OutSideParent);
        LobbyCanvas.transform.SetParent(OutSideParent);
        StartGameCanvas.transform.SetParent(OutSideParent);

    }
    #region photon Callback

    public void OnConnectedToMaster()
    {
        StartButton.interactable = true;
        //set start Game UI canvas
    }
    public void OnJoinedRoom()
    {
        ClientJoinedRoom();
    }
    public void ClientDisconnected()
    {
        if (!LobbyNetwork.changeRegionRequest)
        {
            ClientDisconnectedUI();
        }
    }
    private void OnLeftRoom()
    {
        ClientLeftRoom();
    }

    private void OnDisconnected(DisconnectCause cause)
    {
        ClientDisconnected();
    }

    private void OnJoinedLobby()
    {
        ClientJoinLobby();
    }

    #endregion

}
