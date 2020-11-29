using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField]
    private string m_mainMenuScene;
    [SerializeField]
    private string m_gameScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(m_gameScene);
    }

    public void PlayMainMenu()
    {
        SceneManager.LoadScene(m_mainMenuScene);
    }
}
