using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ExitDoor : MonoBehaviour,
    IUseable
{
    [SerializeField]
    private Sprite m_open;
    [SerializeField]
    private Sprite m_closed;

    private bool m_isOpen = false;
    private bool m_canOpen = false;
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
        }
    }

    public void SetAsExit()
    {
        m_canOpen = true;
        GameState.Instance.currentExit = gameObject;
    }
}
