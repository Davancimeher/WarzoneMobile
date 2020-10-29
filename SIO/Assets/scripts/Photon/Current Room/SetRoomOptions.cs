using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetRoomOptions : MonoBehaviour
{
    private int _maxPlayer;
   
    private bool _isOpen;
    
    private int _maxTime;
   
    public Toggle isPrivateToggle;
    public InputField maxPlayerIPF;
    public InputField maxTimeIPF;
    public GameObject ApplyButton;
    public GameObject SettingsPanel;
    ExitGames.Client.Photon.Hashtable Costume = new ExitGames.Client.Photon.Hashtable();
    private void Start()
    {
        Costume.Add("Time", 0);
        Costume.Add("StartTime", -1);

    }
    private void GetSettingsFromUI()
    {
        _maxTime =int.Parse(maxTimeIPF.text);
        _maxPlayer = int.Parse(maxPlayerIPF.text);
        _isOpen = isPrivateToggle.isOn;
    }
    private void SetSettingsToUI()
    {
        maxPlayerIPF.text = PhotonNetwork.room.MaxPlayers.ToString();
      //  _maxTime = int.Parse(maxTimeIPF.text);
      //  _maxPlayer = int.Parse(maxPlayerIPF.text);
        isPrivateToggle.isOn = PhotonNetwork.room.IsVisible;
        maxTimeIPF.text = PhotonNetwork.room.CustomProperties["Time"].ToString();
        Debug.LogError("Time : " + PhotonNetwork.room.CustomProperties["Time"]);
    }
    public void OnClickRoomStates()
    {
        SetSettingsToUI();
        SettingsPanel.SetActive(!SettingsPanel.activeSelf);
        if (PhotonNetwork.isMasterClient)
        {
            isPrivateToggle.interactable = true;
            maxPlayerIPF.interactable = true;
            maxTimeIPF.interactable = true;
            ApplyButton.SetActive(true);
        }

    }
    public void OnClickApplySettings()
    {
        GetSettingsFromUI();

        PhotonNetwork.room.IsOpen = _isOpen;
        PhotonNetwork.room.IsVisible = _isOpen;
        PhotonNetwork.room.MaxPlayers = _maxPlayer;
        Costume["Time"] = _maxTime;
        PhotonNetwork.room.SetCustomProperties(Costume);

        Debug.Log("is visible : " + PhotonNetwork.room.IsVisible + " , max player : " + PhotonNetwork.room.MaxPlayers+" Time : "+ PhotonNetwork.room.CustomProperties["Time"]);
    }
   
}
