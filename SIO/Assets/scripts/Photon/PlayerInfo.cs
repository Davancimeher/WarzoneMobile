using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    private Dictionary<int, Button> AvatarsDict = new Dictionary<int, Button>();
    public Dictionary<int, Character> PrefabDict = new Dictionary<int, Character>();

    public InputField NameinputField;

    private Button AvatarSelectedButton = null;
    private GameObject PrefabSelectedButton = null;

    private void Start()
    {
        GetPlayerInfo();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void SetPlayerName()
    {
        PlayerNetwork.Instance.SetInfoInPlayerPrefs(NameinputField.text);
        Debug.Log("Settings info player name : " + NameinputField.text);
    }
    public void GetPlayerInfo()
    {
        if (!string.IsNullOrEmpty(PlayerNetwork.Instance.PlayerName))
        {
            NameinputField.text = PlayerNetwork.Instance.PlayerName;
        }

        if (AvatarsDict.ContainsKey(PlayerNetwork.Instance.AvatarId))
        {
            SetSelectedAvatarButton(PlayerNetwork.Instance.AvatarId);
        }
        if (PrefabDict.ContainsKey(PlayerNetwork.Instance.PrefabID))
        {
            SetSelectedPrefabButton(PlayerNetwork.Instance.PrefabID);
        }
    }
    public void SetSelectedAvatarButton(int id)
    {

        //set new button
        Button newButton = AvatarsDict[id];

        Image newImage = newButton.GetComponent<Image>();
        newImage.color = Color.green;

        if (AvatarSelectedButton != null)
        {
            //reset oldbutton
            Image oldImage = AvatarSelectedButton.GetComponent<Image>();
            oldImage.color = Color.red;
        }


        AvatarSelectedButton = newButton;

    }
    public void SetSelectedPrefabButton(int id)
    {
        if(PrefabSelectedButton == null)
        {
            PrefabSelectedButton = PrefabDict[id].SelectedMask;
            PrefabSelectedButton.SetActive(true);
        }
        else
        {
            PrefabSelectedButton.SetActive(false);
            GameObject NewMask = PrefabDict[id].SelectedMask;
            NewMask.SetActive(true);
            PrefabSelectedButton = NewMask;
        }
        //set new button
      

    }
}
