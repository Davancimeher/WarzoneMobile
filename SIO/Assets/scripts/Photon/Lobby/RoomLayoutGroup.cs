using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoomLayoutGroup : MonoBehaviour
{
    public Transform Parent;
    [SerializeField]
    private GameObject _roomListingPrefab;
    private GameObject RoomListingPrefab
    {
        get { return _roomListingPrefab; }
    }
    private LobbyCanvas _LobbyCanvas;

    private List<RoomListing> _roomListingsButtons = new List<RoomListing>();
    private List<RoomListing> RoomListingsButtons
    {
        get { return _roomListingsButtons; }
    }
    private bool CanRefresh = false;
    private float time=5;
    public Button refreshButton;
    private void Awake()
    {
        _LobbyCanvas = GetComponent<LobbyCanvas>();
    }
    private void Update()
    {
        if (!CanRefresh && time>0)
        {
            time -= Time.deltaTime;
        }
        if (time <= 0 && !CanRefresh)
        {
            CanRefresh = true;
            time = 0;
        }
    }
    private void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.LogError("OnRoomListUpdate");
    }
    private void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in rooms)
        {
            RoomRecieved(room);
        }
        RemoveOldRooms();
    }
    public void OnClickRefrechRoomList()
    {
        if (CanRefresh)
        {
            List<RoomInfo> rooms = PhotonNetwork.GetRoomList().ToList();
            
            Debug.Log("rooms : " + rooms.Count);
            foreach (RoomInfo room in rooms)
            {
                RoomRecieved(room);
            }
            RemoveOldRooms();
            time = 5;
            CanRefresh = false;
        }
        else
        {
            Debug.Log(time);
        }
    }

    private void RoomRecieved(RoomInfo room)
    {
        int index = RoomListingsButtons.FindIndex(x => x.RoomName == room.Name);
        if (index == -1)
        {
            if(room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(Parent, false);
                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                RoomListingsButtons.Add(roomListing);

                index = (RoomListingsButtons.Count - 1);

            }
        }
        if(index != -1)
        {
            RoomListing roomListing = RoomListingsButtons[index];
            roomListing.SetRoomNameText(room.Name);
            roomListing.SetRoomInfo(room);
            roomListing.Updated = true;
        }
    }
    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();
        

        foreach (RoomListing roomListing in RoomListingsButtons)
        {
            if (!roomListing.Updated)
                removeRooms.Add(roomListing);
            else
                roomListing.Updated = false;
        }

        foreach (RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;
            RoomListingsButtons.Remove(roomListing);
            Destroy(roomListingObj);
        }
    }
}
