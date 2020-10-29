using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class PlayerManagement : MonoBehaviour
{
    public static PlayerManagement Instance;

    private List<PlayerStats> playerStats = new List<PlayerStats>();
    private int MyPing = 0;
    private int MyPL = 0;
    public Text PingText;
    public Text packetLossText;
    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();
    //TO CHANGE
    const int Health = 30;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
         StartCoroutine("showPing");
    }
    private void Update()
    {
     
    }
    public void FindPlayerGameObjects()
    {
        List<PlayerMouvement> playersMouvement = GameObject.FindObjectsOfType<PlayerMouvement>().ToList();
        foreach (var player in playersMouvement)
        {
            PhotonView playerPV = player.gameObject.GetComponent<PhotonView>();
            if (playerPV != null)
            {
                if (!players.ContainsKey(playerPV.viewID))
                {
                    players.Add(playerPV.viewID, player.gameObject);
                }
            }
        }
        Debug.LogError("Player count : " + players.Count);
    }
    public void DesActivateNavmechForOtherPlayer()
    {
        if(players.Count != PhotonNetwork.playerList.Length)
        {
            FindPlayerGameObjects();
        }
        foreach (var player in players.Values)
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (!pv.isMine)
            {
                player.GetComponent<NavMeshAgent>().enabled = false;
            }
            Debug.Log("siozhdhozdzd");
        }

    }

    public void addPlayerStats(PhotonPlayer photonPlayer)
    {
        int index = playerStats.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if(index == -1)
        {
            List<byte> crew = new List<byte>();
            playerStats.Add(new PlayerStats(photonPlayer, Health,crew));
        }
    }
    public void ModifyHealth(PhotonPlayer photonPlayer,int value)
    {
        int index = playerStats.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1)
        {
            playerStats[index].Health += value;
            PlayerNetwork.Instance.NewHealth(photonPlayer, playerStats[index].Health);
            Debug.Log(playerStats[index].Health);

        }

    }
    public void ModifyCrew(PhotonPlayer photonPlayer, IAAgent agent)
    {
        int index = playerStats.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1)
        {
            PlayerStats stats = playerStats[index];
            if (stats.Crew.Contains(agent.AGENT.id)) return;
            stats.Crew.Add(agent.AGENT.id);
            PlayerNetwork.Instance.NewCrew(photonPlayer, agent);
            Debug.Log(playerStats[index].Crew.Count);
        }
        else
        {
            addPlayerStats(photonPlayer);

            int index2 = playerStats.FindIndex(x => x.PhotonPlayer == photonPlayer);

            PlayerStats stats = playerStats[index2];
            if (stats.Crew.Contains(agent.AGENT.id)) return;
            stats.Crew.Add(agent.AGENT.id);
            PlayerNetwork.Instance.NewCrew(photonPlayer, agent);
            Debug.Log(playerStats[index2].Crew.Count);
        }

    }

    #region UI region

    IEnumerator showPing()
    {
        while (PhotonNetwork.connected)
        {
            MyPing = PhotonNetwork.GetPing();
            PingText.text = MyPing + " ms";

            MyPL = PhotonNetwork.PacketLossByCrcCheck;
            packetLossText.text = MyPL + " %";
            yield return new WaitForSeconds(5f);
        }
        yield break;
        
    }
    #endregion
}


public class PlayerStats
{
    public PlayerStats(PhotonPlayer photonPlayer,int health,List<byte> crew)
    {
        PhotonPlayer = photonPlayer;
        Health = health;
        crew = Crew;
    }
    public readonly PhotonPlayer PhotonPlayer;
    public int Health;
    public List<byte> Crew = new List<byte>();
}
