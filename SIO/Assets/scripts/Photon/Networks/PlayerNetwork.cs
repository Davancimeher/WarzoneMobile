﻿using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using ExitGames.UtilityScripts;

[RequireComponent(typeof(PhotonView))]
public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }
    private PhotonView photonView;

    public int PlayersInGame = 0;  

    private ExitGames.Client.Photon.Hashtable _playerCostumeProperties = new ExitGames.Client.Photon.Hashtable();

    private PlayerMouvement CurrentPlayer;

    public PlayerManagement _PlayerManagement;

    private Coroutine _Pingcoroutine;
    public bool SceneLoaded = false;
    public SceneManagement SceneManagement;
    public string nickName = "guest";
    public bool FirstConnection = false;

    public byte PrefabID=0;

    public List<string> prefabsName = new List<string>
    {
        "Machou",
        "berz"
    };

    //#region timer region
    //bool startTimer = false;
    //double timerIncrementValue;
    //double startTime;
    //[SerializeField] double timer = 20;
    //public Text TimerText;
    //ExitGames.Client.Photon.Hashtable CustomeValue;
    //#endregion
    void Awake()
    {
        if(PlayerNetwork.Instance == null)
        {
            PlayerNetwork.Instance = this;
        }
        else
        {
            if(PlayerNetwork.Instance != this)
            {
                Destroy(PlayerNetwork.Instance.gameObject);
                PlayerNetwork.Instance = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);

        photonView = GetComponent<PhotonView>();

       // PlayerName = "guest#" + Random.Range(1000, 9999);

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneFinishedLoading : "+scene.name);
        if (scene.name == "Game" && !SceneLoaded)
        {
            if (PhotonNetwork.isMasterClient)
            {
                SceneLoaded = true;
                MasterLoadedGame();
             }
            else
            {
                SceneLoaded = true;
                NonMasterLoadedScene();
            }
        }
    }

    private void MasterLoadedGame()
    {
        photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
        photonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }
    private void NonMasterLoadedScene()
    {
        photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
    }

    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        SceneManagement.canvas.SetActive(true);

        var async =  PhotonNetwork.LoadLevelAsync(1);
        SceneManagement.StartLoading(async);
    }

    [PunRPC]
    private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
    {

        PlayerManagement.Instance.addPlayerStats(photonPlayer);

        PlayersInGame++;
        if (PlayersInGame == PhotonNetwork.playerList.Length)
        {
            Debug.Log("All players in Game scene");

            photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }
    public void NewHealth(PhotonPlayer photonPlayer, int health)
    {
        photonView.RPC("RPC_NewHealth", photonPlayer, health);
    }
    public void NewCrew(PhotonPlayer photonPlayer, IAAgent agent)
    {
        photonView.RPC("RPC_Newcrew", PhotonTargets.Others, agent.AGENT.id);
    }

    [PunRPC]
    private void RPC_Newcrew(byte id)
    {
        //agentManagement = GameObject.FindObjectOfType<AgentManagement>();
        //agentManagement.IAagentDict[id].owner = photonView;
    }
    [PunRPC]
    private void RPC_NewHealth(int health)
    {
        if (CurrentPlayer == null) return;

        if (health <= 0)
        {
            PhotonNetwork.Destroy(CurrentPlayer.gameObject);
        }
        else
        {
            CurrentPlayer.Health = health;
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        float randomValue = Random.Range(0f, 5f);
        _PlayerManagement = GameObject.FindObjectOfType<PlayerManagement>();
         var players=  PhotonNetwork.player;
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), _PlayerManagement.spawnPoints[players.ID].position, Quaternion.identity, 0);
        CurrentPlayer = obj.GetComponent<PlayerMouvement>();
        PhotonView pv = obj.GetComponent<PhotonView>();
        PhotonPlayer player = PhotonNetwork.player;

        ExitGames.Client.Photon.Hashtable pp = new ExitGames.Client.Photon.Hashtable();
        pp.Add("PVID", pv.viewID);

        player.SetCustomProperties(pp);

        UIManagement uiM = GameObject.FindObjectOfType<UIManagement>();
        if(uiM != null)
        {
            uiM.DisableWaitingPanel();
        }
    }

    private IEnumerator C_SetPing()
    {
        while (PhotonNetwork.connected)
        {
            _playerCostumeProperties["Ping"] = PhotonNetwork.GetPing();
            PhotonNetwork.player.SetCustomProperties(_playerCostumeProperties);
            yield return new WaitForSeconds(5f);
        }
        yield break;
    }

    public void SetName(string Name)
    {
        if (string.IsNullOrEmpty(Name))
        {
            Name = "Guest";
        }
        PhotonPlayer player = PhotonNetwork.player;
        player.NickName = Name + "#" + Random.Range(1000, 9999);
        Debug.Log("player.NickName " + player.NickName);
        PlayerName = player.NickName;
    }
    public void SetPrefabId(byte id)
    {
        PrefabID = id;
    }
    private void OnConnectedToMaster()
    {
        if (_Pingcoroutine != null)
            StopCoroutine(_Pingcoroutine);
        _Pingcoroutine = StartCoroutine(C_SetPing());
    }
    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
}