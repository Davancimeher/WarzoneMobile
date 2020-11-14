using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGameManagement : MonoBehaviour
{
    public GameObject WaitingForPlayers;
    // Start is called before the first frame update

    public void DisableWaitingPanel()
    {
        WaitingForPlayers.SetActive(false);
    }
}
