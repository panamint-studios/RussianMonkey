using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScore : MonoBehaviour
{
    private Text m_text;

    private void Awake()
    {
        m_text = GetComponent<Text>();
    }

    private void Start()
    {
        GameState.Instance.playerState.CashUpdated += UpdateScore;
    }

    private void UpdateScore()
    {
        m_text.text = GameState.Instance.playerState.cash.ToString();
    }
}
