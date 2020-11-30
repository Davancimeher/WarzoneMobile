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

    private int MyPing = 0;
    private int MyPL = 0;
    public Text PingText;
    public Text packetLossText;
    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
   // public List<Transform> spawnPoints = new List<Transform>();

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

    //TO REMOVE
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
