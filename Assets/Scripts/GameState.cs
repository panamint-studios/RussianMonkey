using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public enum State
    {
        Default,
        Alarm
    }

    public int cash
    {
        get
        {
            return m_cash.Value;
        }
        set
        {
            m_cash.Value = value;
        }
    }

    public int enemiesAlive
    {
        get
        {
            return m_enemiesAlive.Value;
        }
        set
        {
            m_enemiesAlive.Value = value;
        }
    }

    public int enemiesKilled
    {
        get
        {
            return m_enemiesKilled.Value;
        }
        set
        {
            m_enemiesKilled.Value = value;
        }
    }

    public event Action CashUpdated
    {
        add
        {
            m_cash.Updated += value;
        }
        remove
        {
            m_cash.Updated -= value;
        }
    }
    private NotifyProperty<int> m_cash;
    private NotifyProperty<int> m_enemiesAlive;
    private NotifyProperty<int> m_enemiesKilled;
    public State currentState;

    public PlayerState()
    {
        m_cash = new NotifyProperty<int>();
        m_enemiesAlive = new NotifyProperty<int>();
        m_enemiesKilled = new NotifyProperty<int>();
    }
}

public class GameState : MonoBehaviour
{

    public static GameState Instance 
    { 
        get 
        { 
            return m_instance; 
        }
    }
    public PlayerState playerState { get; private set; }
    private AlarmLightsController alarmLightsController;
    private static GameState m_instance;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
        }
        Init();
        DontDestroyOnLoad(this);
    }

    private void Init()
    {
        playerState = new PlayerState();
        alarmLightsController = GameObject.Find("AlarmLights").GetComponent<AlarmLightsController>();
    }

    public void SetGameState(PlayerState.State newState)
    {
        switch (newState)
        {
            case PlayerState.State.Default:
                alarmLightsController.StopAlarm();
                break;
            case PlayerState.State.Alarm:
                if (alarmLightsController)
                    alarmLightsController.StartAlarm();
                break;
        }
        playerState.currentState = newState; 
    }
}
