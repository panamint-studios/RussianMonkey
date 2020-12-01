using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject m_policeTimer;

    private void Awake()
    {
        FindObjectOfType<GameState>().AlarmActivated += ShowPoliceTimer;
    }

    private void ShowPoliceTimer()
    {
        m_policeTimer.SetActive(true);
    }
}
