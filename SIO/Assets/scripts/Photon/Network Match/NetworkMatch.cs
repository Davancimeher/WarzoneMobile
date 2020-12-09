using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MatchStateEnum :  byte
{
    COUNTDOWN = 0,
    RUNNING = 1,
    ENDMATCH = 2
}
public class NetworkMatch : Photon.MonoBehaviour
{
    public MatchStateEnum MatchState = MatchStateEnum.COUNTDOWN;

    public int matchTime_S = 30;
    public int countdown_S = 2;


    public UIInGameManagement uiGameManagement;

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

            case NetworkEvent.sendUpdateCountdown:
                RecieveCountdownTime(content);
                break;
            case NetworkEvent.sendUpdateMatchState:
                RecieveSUpdateMatchState(content);

                break;
            case NetworkEvent.sendUpdateMatchTime:

                RecieveUpdateTime(content);

                break;
        }
    }
    public void StartMatch()
    {
        countdown_S = 5;
        Debug.Log("countdonw master : " + countdown_S);

        if (PhotonNetwork.isMasterClient)
        {
            MatchState = MatchStateEnum.COUNTDOWN;
            SendUpdateMatchState();
            uiGameManagement.updateMatchState(MatchState, matchTime_S, countdown_S);
        }
        StartCoroutine(UpdateCountDown());
    }

    public void ContinueMatch()
    {
        switch (MatchState)
        {
            case MatchStateEnum.COUNTDOWN:
                StartCoroutine(UpdateCountDown());
                break;
            case MatchStateEnum.RUNNING:
                StartCoroutine(UpdateMatchTime());
                break;
        }
    }
    private IEnumerator UpdateCountDown()
    {
        if (PhotonNetwork.isMasterClient)
        {
            while (MatchState == MatchStateEnum.COUNTDOWN)
            {
                yield return new WaitForSeconds(1);
                countdown_S--;

                if (countdown_S >= 0)
                {
                    SendUpdatecountdown();
                }
                else
                {

                    MatchState = MatchStateEnum.RUNNING;

                    SendUpdateMatchState();

                    StartCoroutine(UpdateMatchTime());

                    StopCoroutine(UpdateCountDown());

                }
            }

        }
    }

    private IEnumerator UpdateMatchTime()
    {
        if (PhotonNetwork.isMasterClient)
        {
            while (MatchState == MatchStateEnum.RUNNING)
            {
                yield return new WaitForSeconds(1);
                matchTime_S--;

                if (matchTime_S > 0)
                {
                    SendUpdateTime(matchTime_S);
                }
                else
                {
                    //send end match
                    MatchState = MatchStateEnum.ENDMATCH;

                    SendUpdateMatchState();

                    // StopCoroutine(UpdateMatchTime());
                }
            }

        }
    }

    private void RecieveCountdownTime(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            countdown_S = (int)datas[0];
            uiGameManagement.updateCountdownUI(countdown_S);
        }
    }

    private void RecieveSUpdateMatchState(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            MatchState = (MatchStateEnum)datas[0];
            uiGameManagement.updateMatchState(MatchState, matchTime_S, countdown_S);
        }
    }

    private void RecieveUpdateTime(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            matchTime_S = (int)datas[0];
            uiGameManagement.updateTimeUI(matchTime_S);
        }
    }
    private void SendUpdatecountdown()
    {
        object[] datas = new object[]
        {
            countdown_S
        };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.sendUpdateCountdown, datas, false, options);
    }

    private void SendUpdateMatchState()
    {
        object[] datas = new object[]
        {
            (byte)MatchState
        };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.sendUpdateMatchState, datas, false, options);

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

    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        Debug.LogError("OnMasterClientSwitched");
        if (PhotonNetwork.isMasterClient)
        {
            ContinueMatch();
        }
    }
}
