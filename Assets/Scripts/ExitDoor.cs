using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ExitDoor : MonoBehaviour,
    IUseable
{
    [SerializeField]
    private Sprite m_open;
    [SerializeField]
    private Sprite m_closed;
    [SerializeField]
    private Color m_endColor;
    [SerializeField]
    private Image m_fadeOutScreen;
    [SerializeField]
    private string m_scoreScreen;

    private bool m_isOpen = false;
    private bool m_canOpen = false;
    private const float FADE_OUT_TIME = 2.0f;
    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = m_closed;
    }

    public void OnUse()
    {
        if (!m_isOpen && m_canOpen)
        {
            m_spriteRenderer.sprite = m_open;
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        float elapsed = 0f;
        Color startColor = m_fadeOutScreen.color;

        while (elapsed < FADE_OUT_TIME)
        {
            m_fadeOutScreen.color = Color.Lerp(startColor, m_endColor, elapsed / FADE_OUT_TIME);
            elapsed += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(WaitThenLoad());
    }

    private IEnumerator WaitThenLoad()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(m_scoreScreen);
    }

    public void SetAsExit()
    {
        m_canOpen = true;
        GameState.Instance.currentExit = gameObject;
    }
}
