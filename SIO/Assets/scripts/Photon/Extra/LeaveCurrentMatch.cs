using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCurrentMatch : MonoBehaviour
{
    public void OnClickLeaveMatch()
    {
        Destroy(PlayerNetwork.Instance.gameObject);

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}
