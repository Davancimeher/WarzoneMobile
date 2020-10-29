using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    public int AvatarId;
    public Image AvatarImage;
    public PlayerInfo PlayerInfo;
    public void SetAvatar()
    {
        PlayerNetwork.Instance.SetInfoInPlayerPrefs(AvatarId);
        Debug.Log("Settings info avatar : " + AvatarId);
        PlayerInfo.SetSelectedAvatarButton(AvatarId);
    }
}
