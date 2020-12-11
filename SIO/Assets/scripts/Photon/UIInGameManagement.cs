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

    public Animator uiTextAnimator;
    public AudioSource audioSource;
    public void updateTimeUI(int time_S)
    {
        string minutes = (time_S / 60).ToString("0");
        string seconds = (time_S % 60).ToString("00");
        time_ui.text = $"{minutes}:{seconds}";
        LastChanceWarning(time_S);
    }
    public void updateCountdownUI(int countdown)
    {
        if (!countdownPanel.activeSelf)
        {
            countdownPanel.SetActive(true);
        }

        if (countdown > 0)
        {
            CountDown_ui.text = countdown.ToString();
        }
        else
        {
            CountDown_ui.text = "Go !";
        }
    }
    public void updateMatchState(MatchStateEnum matchState, int time_s, int countdown_s)
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
                Time.timeScale = 0;
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

    private void LastChanceWarning(int _time)
    {
        if (_time == 30)
        {
            uiTextAnimator.SetBool("30s Left", true);
            audioSource.Play();
        }
    }
}
