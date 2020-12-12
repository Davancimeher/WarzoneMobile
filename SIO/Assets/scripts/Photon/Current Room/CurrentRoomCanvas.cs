
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    public SceneManagement SceneManagement;

    public void OnClickStartSync()
    {
        if (!PhotonNetwork.isMasterClient) return;

        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = true;
        SceneManagement.canvas.SetActive(true);
        var async= PhotonNetwork.LoadLevelAsync(1);
        SceneManagement.StartLoading(async);
    }
    
}
