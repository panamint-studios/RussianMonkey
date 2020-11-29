using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string m_gameScene;
    [SerializeField]
    private string m_tutorialScene;
    [SerializeField]
    private GameObject m_modal;

    private bool m_isModalActive = false;

    public void PlayGame()
    {
        SceneManager.LoadScene(m_gameScene);
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene(m_tutorialScene);
    }

    public void ToggleModal()
    {
        m_isModalActive = !m_isModalActive;
        m_modal.SetActive(m_isModalActive);
    }
}
