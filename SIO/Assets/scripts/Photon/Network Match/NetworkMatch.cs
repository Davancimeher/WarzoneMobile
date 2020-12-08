using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkMatch : Photon.MonoBehaviour
{
    public int matchTime_S = 30;
    public bool MatchRunning = false;
    public Text time_ui;
    public Text matchRunning_ui;
    #region Callback region
    private void OnEnable()
    {

        PhotonNetwork.OnEventCall += OnPhotonEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.OnEventCall -= OnPhotonEvent;

    }
    #endregion

    private void OnPhotonEvent(byte eventCode, object content, int senderId)
    {
        NetworkEvent eventType = (NetworkEvent)eventCode;
        switch (eventType)
        {
            case NetworkEvent.sendStartTime:
                RecieveStartMatch(content);

                break;

            case NetworkEvent.sendUpdateMatchTime:

                RecieveUpdateTime(content);

                break;
            case NetworkEvent.sendEndMatch:
                RecieveEndMatch(content);
                break;
        }
    }
    public void StartMatchTime()
    {
        if (PhotonNetwork.isMasterClient)
        {
            // send start match
            MatchRunning = true;
            SendStartMatch();

        }
        StartCoroutine(updateMatchTime());
    }
        
    private IEnumerator updateMatchTime()
    {
        if (PhotonNetwork.isMasterClient)
        {
            while (MatchRunning)
            {
                Debug.LogError(matchTime_S);

                yield return new WaitForSeconds(1);
                matchTime_S--;
                Debug.LogError(matchTime_S);

                if (matchTime_S > 0)
                {
                    SendUpdateTime(matchTime_S);
                }
                else
                {
                    //send end match
                    SendEndMatch();
                }
            }
           
        }
    }
    private void RecieveStartMatch(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {

            MatchRunning = (bool)datas[0];
            matchRunning_ui.text = MatchRunning.ToString();
        }
    }
    private void RecieveUpdateTime(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            matchTime_S = (int)datas[0];
            time_ui.text = matchTime_S.ToString();
        }
    }
    private void RecieveEndMatch(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {

            MatchRunning = (bool)datas[0];
            matchRunning_ui.text = MatchRunning.ToString();
        }
    }
    private void SendStartMatch()
    {
        object[] datas = new object[]
        {
            true
        };

        //  Debug.Log("sending On Capturing : " + ID + " is  : " + OnCapturing);

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.sendStartTime, datas, false, options);

    }
    private void SendUpdateTime(int _matchTime)
    {

        object[] datas = new object[]
        {
            _matchTime
        };

        //  Debug.Log("sending On Capturing : " + ID + " is  : " + OnCapturing);

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.sendUpdateMatchTime, datas, false, options);
    }
    private void SendEndMatch()
    {
        object[] datas = new object[]
        {
            false
        };

        //  Debug.Log("sending On Capturing : " + ID + " is  : " + OnCapturing);

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.sendEndMatch, datas, false, options);

    }
}
