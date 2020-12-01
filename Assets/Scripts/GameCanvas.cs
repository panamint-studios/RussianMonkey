using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject m_policeTimer;
    [SerializeField]
    private GameObject m_policeTextObject;
    [SerializeField]
    private GameObject m_exitPath;

    private void Start()
    {
        GameState.Instance.AlarmActivated += ShowPoliceTimer;
        GameState.Instance.ExitUpdated += ShowExitPath;
    }

    private void ShowExitPath()
    {
        m_exitPath.SetActive(true);
    }

    private void ShowPoliceTimer()
    {
        print("Test!");
        m_policeTimer.SetActive(true);
        StartCoroutine(PoliceTimerCountdown());
    }

    private IEnumerator PoliceTimerCountdown()
    {
        float timeLeft = GameState.Instance.playerState.currentTime;
        Text text = m_policeTextObject.GetComponent<Text>();

        while (timeLeft > 0)
        {
            text.text = timeLeft.ToString("F2") + "s";
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        text.text ="0.0s";
        GameState.Instance.SetGameState(PlayerState.State.PoliceArrive);
    }
}
