using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIInGameManagement : MonoBehaviour
{
    public GameObject WaitingForPlayers;
    public GameObject endGamePanel;
    public GameObject inGamePanel;
    public GameObject countdownPanel;


    public Text time_ui;
    public Text matchRunning_ui;
    public Text CountDown_ui;

    public void updateTimeUI(int time_S)
    {
        Debug.Log("time_S " + time_S);

        string minutes = (time_S / 60).ToString("00");
        string seconds = (time_S % 60).ToString("00");
        time_ui.text = $"{minutes}:{seconds}";
    }
    public void updateCountdownUI(int countdown)
    {
        Debug.Log("countdown_s " + countdown);

        if (!countdownPanel.activeSelf)
        {
            countdownPanel.SetActive(true);
        }

        if(countdown > 0)
        {
            CountDown_ui.text = countdown.ToString();
        }
        else
        {
            CountDown_ui.text = "Go !";
        }
    }
    public void updateMatchState(MatchStateEnum matchState,int time_s,int countdown_s)
    {
        switch (matchState)
        {
            case MatchStateEnum.COUNTDOWN:
                DisableWaitingPanel();
                updateCountdownUI(countdown_s);
                break;
            case MatchStateEnum.RUNNING:
                disableCountDownPanel();
                updateTimeUI(time_s);
                inGamePanel.SetActive(true);
                break;
            case MatchStateEnum.ENDMATCH:
                ShowEndGame();
                inGamePanel.SetActive(false);
                break;
        }
    }



    public void disableCountDownPanel()
    {
        countdownPanel.SetActive(false);
    }
    private void DisableWaitingPanel()
    {
        WaitingForPlayers.SetActive(false);
    }

    private void ShowEndGame()
    {
        endGamePanel.SetActive(true);
    }
}
