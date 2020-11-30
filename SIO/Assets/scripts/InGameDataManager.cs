using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameDataManager : MonoBehaviour
{
    public static InGameDataManager instance;
    // Start is called before the first frame update
    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();



    private void Awake()
    {
        instance = this;
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
}
