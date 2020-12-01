using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public enum State
    {
        Default,
        Alarm,
        PoliceArrive
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

    public float currentTime
    {
        get
        {
            return m_currentTime;
        }
        set
        {
            m_currentTime = value;
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
    private float m_currentTime;
    private const float DEFAULT_TIME = 15.0f;

    public PlayerState()
    {
        m_cash = new NotifyProperty<int>();
        m_enemiesAlive = new NotifyProperty<int>();
        m_enemiesKilled = new NotifyProperty<int>();
        m_currentTime = DEFAULT_TIME;
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
    public GameObject currentExit
    {
        get
        {
            return m_currentExit.Value;
        }
        set
        {
            m_currentExit.Value = value;
        }
    }
    public event Action AlarmActivated;
    public event Action ExitUpdated
    {
        add
        {
            m_currentExit.Updated += value;
        }
        remove
        {
            m_currentExit.Updated -= value;
        }
    }

    private AlarmLightsController m_alarmLightsController;
    private AlarmEnemySpawner m_alarmEnemySpawner;
    private NotifyProperty<GameObject> m_currentExit;
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
        m_currentExit = new NotifyProperty<GameObject>();
        if (GameObject.Find("AlarmSystem") != null)
            m_alarmLightsController = GameObject.Find("AlarmSystem").GetComponent<AlarmLightsController>();
        if (GameObject.Find("AlarmSystem") != null)
            m_alarmEnemySpawner = GameObject.Find("AlarmSystem").GetComponent<AlarmEnemySpawner>();
    }

    public void SetGameState(PlayerState.State newState)
    {
        switch (newState)
        {
            case PlayerState.State.Default:
                m_alarmLightsController.StopAlarm();
                break;
            case PlayerState.State.Alarm:
                if (m_alarmLightsController)
                    m_alarmLightsController.StartAlarm();
                if (m_alarmEnemySpawner)
                    m_alarmEnemySpawner.StartAlarm();
                AlarmActivated?.Invoke();
                break;
            case PlayerState.State.PoliceArrive:
                break;
        }
        playerState.currentState = newState; 
    }

    public void UpdateTime(float deltaTime)
    {
        playerState.currentTime += deltaTime;
    }
}
