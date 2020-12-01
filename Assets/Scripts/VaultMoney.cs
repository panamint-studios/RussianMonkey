using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaultMoney : MonoBehaviour, IUseable
{
    public Image fillupBar;
    public float m_fillDuration = 3.5f;
    public float m_fillAmount;
    public int m_value = 100;
    private bool isActivated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fillupBar.fillAmount = (m_fillAmount / m_fillDuration);

        if (isActivated)
        {
            //SceneManager.LoadScene(m_gameScene);

            m_fillAmount += Time.deltaTime;

            if (m_fillAmount >= m_fillDuration)
            {
                GameState.Instance.playerState.cash += m_value;
                Destroy(gameObject);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                isActivated = false;
                m_fillAmount = 0;
            }
        }
    }

    public void OnUse()
    {
        isActivated = true;
    }
}
