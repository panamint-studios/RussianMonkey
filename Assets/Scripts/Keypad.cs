using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets._2D;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Keypad : MonoBehaviour, IUseable
{
    [Header("Events")]
    public UnityEvent CorrectCombination;
    public UnityEvent IncorrectCombination;
    public UnityEvent StartedCombination;

    [Header("Sprites for Guesses")]
    [SerializeField]
    private List<Sprite> m_options;
    [SerializeField]
    private Sprite m_waiting;
    [SerializeField]
    private Sprite m_success;
    [SerializeField]
    private Sprite m_fail;
    [Header("Input")]
    [SerializeField]
    private int m_combinationLength = 3;
    [SerializeField]
    private int m_correctInputs = 0;
    [SerializeField]
    private bool m_active = false;
    [SerializeField]
    private float m_secondsToWait = .75f;
    [Header("Layout")]
    [SerializeField]
    private float m_displayDistance = 1.5f;

    private float m_secondsSinceInput = 0;
    private int m_currentOption = 0;
    private GameObject m_inputDisplay;
    private SpriteRenderer m_guessRenderer;
    private SpriteRenderer m_keypadRenderer;
    private Animator m_animator;

    private enum EKeypadChoices
    {
        W,
        S,
        D,
        A
    }

    public void OnUse()
    {
        if (m_active)
        {
            Debug.Log("stopping active");
            IncorrectCombination?.Invoke();
            StopKeypad();
            return;
        }
        else
        {
            // Disable movement input unless the user hits a key
            Debug.Assert(m_options != null);
            ToggleInput(false);
            m_animator.enabled = true;
            m_active = true;
            Debug.Log("active=" + m_active);
            StartedCombination?.Invoke();
        }
    }

    private void ToggleInput(bool isEnabled)
    {
        var player = FindObjectOfType<Platformer2DUserControl>();
        player.ToggleInput(isEnabled);
    }

    private void StopKeypad()
    {
        // Relinquish control back to the user
        ToggleInput(true);
        m_correctInputs = 0;
        m_secondsSinceInput = 0;
        m_active = false;
        m_animator.enabled = false;
        m_guessRenderer.sprite = null;
        m_keypadRenderer.sprite = m_waiting;
    }

    private void Awake()
    {
        m_keypadRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_animator.enabled = false;
        m_keypadRenderer.sprite = m_waiting;

        m_inputDisplay = new GameObject("input_display");
        m_inputDisplay.transform.SetParent(transform);
        m_inputDisplay.transform.localPosition = new Vector3(0, m_displayDistance, 0);
        m_guessRenderer = m_inputDisplay.AddComponent<SpriteRenderer>();
        m_guessRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_active && m_secondsSinceInput >= m_secondsToWait)
        {
            if (m_guessRenderer.sprite == null)
            {
                ShowNewOption();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                EvaluateGuess(EKeypadChoices.W);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                EvaluateGuess(EKeypadChoices.A);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                EvaluateGuess(EKeypadChoices.S);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                EvaluateGuess(EKeypadChoices.D);
            }
        }
        else if (m_active)
        {
            m_secondsSinceInput += Time.deltaTime;
        }
    }

    private void ShowNewOption()
    {
        m_currentOption = UnityEngine.Random.Range(0, m_options.Count);
        m_guessRenderer.sprite = m_options[m_currentOption];
    }

    private void EvaluateGuess(EKeypadChoices choice)
    {
        if (((int)choice) == m_currentOption)
        {
            m_correctInputs += 1;
            if (m_correctInputs == m_combinationLength)
            {
                CorrectCombination?.Invoke();
                DoneKeypad(true);
            }
            else
            {
                m_guessRenderer.sprite = null;
                m_secondsSinceInput = 0;
            }
        }
        else
        {
            IncorrectCombination?.Invoke();
            DoneKeypad(false);
        }

    }

    private void DoneKeypad(bool isSuccess)
    {
        m_animator.enabled = false;
        m_active = false;
        m_guessRenderer.sprite = null;

        if (isSuccess)
        {
            m_keypadRenderer.sprite = m_success;
            ToggleInput(true);
        }
        else
        {
            m_keypadRenderer.sprite = m_fail;
            ToggleInput(true);
        }
        StartCoroutine(PauseThenReset());
    }

    private IEnumerator PauseThenReset()
    {
        yield return new WaitForSeconds(1.5f);
        StopKeypad();
    }
}
