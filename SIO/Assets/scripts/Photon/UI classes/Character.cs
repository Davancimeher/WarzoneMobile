using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Character : MonoBehaviour
{
    public byte prefabId;
    public Image PrefabImageImage;
    public PlayerInfo PlayerInfo;
    public GameObject SelectedMask;

    public void SetPrefab()
    {
        PlayerNetwork.Instance.SetInfoInPlayerPrefs(prefabId);
        Debug.Log("Settings info prefab id : " + prefabId);
        PlayerInfo.SetSelectedPrefabButton(prefabId);

    }
}
