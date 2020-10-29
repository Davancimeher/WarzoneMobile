using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public List<Button> AvatarsButton = new List<Button>();

    public List<Button> PrefabssButton = new List<Button>();

    private Dictionary<int, Button> AvatarsDict = new Dictionary<int, Button>();
    private Dictionary<int, Button> PrefabDict = new Dictionary<int, Button>();

    public InputField NameinputField;

    private Button AvatarSelectedButton = null;
    private Button PrefabSelectedButton = null;

    private void Start()
    {
        foreach (var avatarButton in AvatarsButton)
        {
            Avatar avatar = avatarButton.GetComponent<Avatar>();
            AvatarsDict.Add(avatar.AvatarId, avatarButton);
        }
        foreach (var prefabButton in PrefabssButton)
        {
            Character prefab = prefabButton.GetComponent<Character>();
            PrefabDict.Add(prefab.prefabId, prefabButton);
        }

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

        //set new button
        Button newButton = PrefabDict[id];

        Image newImage = newButton.GetComponent<Image>();
        newImage.color = Color.green;
       
        
        if (PrefabSelectedButton != null)
        {
            //reset oldbutton
            Image oldImage = PrefabSelectedButton.GetComponent<Image>();
            oldImage.color = Color.red;
        }

        PrefabSelectedButton = newButton;

    }
}
