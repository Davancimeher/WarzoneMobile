using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLayoutGroup : MonoBehaviour
{
    ExitGames.Client.Photon.Hashtable Costume = new ExitGames.Client.Photon.Hashtable();

    public MainSceneManagement _MainSceneManagement;
    public List<Transform> SpawnPonts;
    public GameObject UICamera;

    [SerializeField]
    private GameObject _playerListingPrefab;

    public Transform Parent;
    private GameObject PlayerListingPrefab
    {
        get { return _playerListingPrefab; }
    }

    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> PlayerListings
    {
        get { return _playerListings; }
    }
    private GameObject myObj;
    //CB photon
    private void OnJoinedRoom()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling();

        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
    {
        PlayerJoinedRoom(photonPlayer);

       
    }
    //CB photon
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer,false);

        //if (PhotonNetwork.isMasterClient)
        //{
        //    Costume.Clear();
        //    var playersCount = (int)PhotonNetwork.room.CustomProperties["playersCount"];
        //    Debug.Log("playersCount : " + playersCount);

        //    Costume.Add("playersCount", playersCount--);
        //    PhotonNetwork.room.SetCustomProperties(Costume);
        //}
    }


    private void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        if (photonPlayer == null)
        {
            return;
        }

        PlayerLeftRoom(photonPlayer,true);

        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        playerListingObj.transform.SetParent(Parent, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();

        playerListing.ApplyPhotonPlayer(photonPlayer);

        PlayerListings.Add(playerListing);

        if (photonPlayer.IsLocal)
        {
            int playerSpawnIndex = PhotonNetwork.player.ID % SpawnPonts.Count;
            string PlayerPrefabsName = DataHolder.Instance.prefabsName[PlayerNetwork.Instance.PrefabID];
            myObj = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Players Prefab", PlayerPrefabsName), SpawnPonts[playerSpawnIndex].position, Quaternion.identity, 0);
            Debug.Log("Instantiate waiting Player");
            UICamera.SetActive(false);

            
            
        }
    }
    private void PlayerLeftRoom(PhotonPlayer photonPlayer,bool forUI)
    {
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if(index != -1)
        {
           

            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
            if (!forUI)
            {
                if (photonPlayer.IsLocal)
                {
                    PhotonNetwork.Destroy(myObj);
                    Debug.Log("destroy waiting Player");
                }
               
            }
        }
        
    }
    private void DestroyAllPlayerListing()
    {
        foreach (Transform child in Parent)
        {
            GameObject.Destroy(child.gameObject);
        }
        PlayerListings.Clear();
    }
    public void OnClickLeaveRoom()
    {
        DestroyAllPlayerListing();
        UICamera.SetActive(true);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
}
