using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _roomNameText;
    private Text RoomNameText
    {
        get { return _roomNameText; }
    }

    [SerializeField]
    private Text _roomPlayersText;
    private Text RoomPlayersText
    {
        get { return _roomPlayersText; }
    }


    public string RoomName { get; private set; }
    public RoomInfo room;

    public bool Updated { get; set; }
   private void Start()
   {
        GameObject lobbyCanvasObj = MainCanvasManager.Instance.LobbyCanvas.gameObject;
        if (lobbyCanvasObj == null) return;

        LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(room));
   }
    private void OnDestroy()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }
    public void SetRoomNameText(string text)
    {
        RoomName = text;
        RoomNameText.text = RoomName;
    }
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        room = roomInfo;
        var playersCount = (int)room.CustomProperties["Count"];
        RoomPlayersText.text = playersCount + " / " + room.MaxPlayers;
    }
}
