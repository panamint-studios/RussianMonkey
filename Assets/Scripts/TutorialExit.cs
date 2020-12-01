using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialExit : MonoBehaviour, IUseable
{
    [SerializeField]
    private string m_gameScene;

    public void OnUse()
    {
        SceneManager.LoadScene(m_gameScene);
    }
}
