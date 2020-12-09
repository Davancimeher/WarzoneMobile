using System.IO;
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
    public bool FirstConnection = false;

    public byte PrefabID=0;
    public int AvatarId = 0;
    public NetworkMatch networkMatch;
   
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
        GeInfoFromPlayerprefs();

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public void  SetInfoInPlayerPrefs(int _avatarId)
    {
        AvatarId = _avatarId;
        PlayerPrefs.SetInt("AvatarId", AvatarId);
    }
    public void SetInfoInPlayerPrefs(string _nickName)
    {
        if (string.IsNullOrEmpty(_nickName) && string.IsNullOrEmpty(PlayerName))
        {
            _nickName = "Guest";
        }
        else if (!string.IsNullOrEmpty(_nickName) && string.IsNullOrEmpty(PlayerName))
        {
            PlayerPrefs.SetString("Name", _nickName);
        }
        else if (string.IsNullOrEmpty(_nickName) && !string.IsNullOrEmpty(PlayerName))
        {
            _nickName = PlayerName;
        }
        else if(_nickName != PlayerName)
        {
            PlayerPrefs.SetString("Name", _nickName);
        }

        PhotonPlayer player = PhotonNetwork.player;
        player.NickName = _nickName + "#" + Random.Range(1000, 9999);
        Debug.Log("player.NickName " + player.NickName);
        PlayerName = _nickName;

    }
    public void SetInfoInPlayerPrefs(byte _prefabId)
    {
        PrefabID = _prefabId;
        PlayerPrefs.SetInt("PrefabId", (int)PrefabID);
    }
    public void SetInfoInPlayerPrefs()
    {
        PlayerPrefs.SetString("Name", PlayerName);
        PlayerPrefs.SetInt("PrefabId", (int)PrefabID);
        PlayerPrefs.SetInt("AvatarId", AvatarId);
    }
    public void GeInfoFromPlayerprefs()
    {
        if (PlayerPrefs.HasKey("Name"))
            PlayerName = PlayerPrefs.GetString("Name");
        if (PlayerPrefs.HasKey("PrefabId"))
            PrefabID = (byte)PlayerPrefs.GetInt("PrefabId");
        if (PlayerPrefs.HasKey("AvatarId"))
            AvatarId =  PlayerPrefs.GetInt("AvatarId");
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

            networkMatch = GameObject.FindObjectOfType<NetworkMatch>();
            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("start match");
                networkMatch.StartMatch();
            }
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
        _PlayerManagement = GameObject.FindObjectOfType<PlayerManagement>();

        string PrefabName = DataHolder.Instance.prefabsName[PrefabID];

        int playerSpawnIndex = PhotonNetwork.player.ID % _PlayerManagement.spawnPoints.Count;

        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Players Prefab", PrefabName), _PlayerManagement.spawnPoints[playerSpawnIndex].position, Quaternion.identity, 0);
        CurrentPlayer = obj.GetComponent<PlayerMouvement>();
        PhotonView pv = obj.GetComponent<PhotonView>();
        PhotonPlayer player = PhotonNetwork.player;

        ExitGames.Client.Photon.Hashtable pp = new ExitGames.Client.Photon.Hashtable();
        pp.Add("PVID", pv.viewID);

        player.SetCustomProperties(pp);

        UIInGameManagement uiM = GameObject.FindObjectOfType<UIInGameManagement>();
        //if(uiM != null)
        //{
        //    uiM.DisableWaitingPanel();
        //}
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
    private void OnConnectedToMaster()
    {
        if (_Pingcoroutine != null)
            StopCoroutine(_Pingcoroutine);
        _Pingcoroutine = StartCoroutine(C_SetPing());
    }
    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
}
