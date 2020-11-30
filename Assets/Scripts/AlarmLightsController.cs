using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLightsController : MonoBehaviour
{
    SpriteRenderer m_Sprite;
    Color alertColor = Color.red;
    float m_alertCycleDuration = 3;
    float m_alertTimer = 0;
    public bool m_PlayAlarm;
    public AnimationCurve alertCurve;

    // Start is called before the first frame update
    void Start()
    {
        m_Sprite = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayAlarm)
            PlayAlarm();
        else
        {
            alertColor.a = 0;
            m_Sprite.color = alertColor;
        }
    }

    void PlayAlarm()
    {
        m_alertTimer += Time.deltaTime;
        if (m_alertTimer > m_alertCycleDuration)
            m_alertTimer = 0;

        float t = m_alertTimer / m_alertCycleDuration;
        float curveValue = alertCurve.Evaluate(t);
        alertColor.a = curveValue;
        m_Sprite.color = alertColor;
    }

    public void StartAlarm()
    {
        m_PlayAlarm = true;
    }

    public void StopAlarm()
    {
        m_PlayAlarm = false;
    }
}
