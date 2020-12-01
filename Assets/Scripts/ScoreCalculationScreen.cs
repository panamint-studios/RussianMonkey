using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreCalculationScreen : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField]
    private string m_mainMenuScene;
    [SerializeField]
    private string m_gameScene;
    
    [Header("UI")]
    [SerializeField]
    private GameObject m_cashText;
    [SerializeField]
    private GameObject m_enemiesKilledText;
    [SerializeField]
    private GameObject m_enemiesSparedText;
    [SerializeField]
    private GameObject m_timeBonusText;
    [SerializeField]
    private GameObject m_totalText;

    private int m_score;
    private int m_cashMultiplier = 3;
    private int m_enemiesKilledMultiplier = -100;
    private int m_enemiesSparedMultiplier = 500;
    private int m_timeBonusMultiplier = 5;
    private const float TALLY_TIME = 1.75f;
    private GameState m_gameState;

    public void PlayGame()
    {
        SceneManager.LoadScene(m_gameScene);
    }

    public void PlayMainMenu()
    {
        SceneManager.LoadScene(m_mainMenuScene);
    }

    private void Awake()
    {
        m_gameState = FindObjectOfType<GameState>();
        Debug.Assert(m_gameState != null, "Game state must be present for score calculation.");
    }

    private void Start()
    {
        if (m_gameState == null) return;
        SetCash(m_gameState.playerState.cash);
    }

    private void SetCash(int cash)
    {
        m_score += (cash * m_cashMultiplier);
        StartCoroutine(TallyScore(m_cashText, (cash * m_cashMultiplier), () => SetEnemiesKilled(m_gameState.playerState.enemiesKilled)));
    }

    private void SetEnemiesKilled(int enemiesKilled)
    {
        m_score += (enemiesKilled * m_enemiesKilledMultiplier);
        StartCoroutine(TallyScore(m_enemiesKilledText, (enemiesKilled * m_enemiesKilledMultiplier), () => SetEnemiesSpared(m_gameState.playerState.enemiesAlive)));
    }

    private void SetEnemiesSpared(int enemiesSpared)
    {
        m_score += (enemiesSpared * m_enemiesSparedMultiplier);
        StartCoroutine(TallyScore(m_enemiesSparedText, (enemiesSpared * m_enemiesSparedMultiplier), () => SetTimeBonus(100)));
    }

    private void SetTimeBonus(int time)
    {
        m_score += (time * m_timeBonusMultiplier);
        StartCoroutine(TallyScore(m_timeBonusText, (time * m_timeBonusMultiplier), () => SetTotal()));
    }

    private void SetTotal()
    {
        StartCoroutine(TallyScore(m_totalText, m_score));
    }

    private IEnumerator TallyScore(GameObject obj, int goal, Action callback=null)
    {
        float elapsedTime = 0;
        Text text = obj.GetComponent<Text>();

        while (elapsedTime < TALLY_TIME)
        {
            
            text.text = ((int)Mathf.Lerp(0, goal, elapsedTime / TALLY_TIME)).ToString();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        text.text = goal.ToString();
        if (callback != null)
            callback();
    }
}
