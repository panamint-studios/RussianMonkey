using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
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

    public PlayerState()
    {
        m_cash = new NotifyProperty<int>();
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
    }
}
