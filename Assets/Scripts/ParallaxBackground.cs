using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject m_player;
    private float m_xOffset = 0;
    private Vector2 m_lastPos;
    [SerializeField]
    private float MOVE_RATE = 0.07f;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");

        Debug.Assert(m_player != null);

        m_lastPos = m_player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_xOffset = (m_lastPos.x - m_player.transform.position.x) * MOVE_RATE;
        var newPos = transform.position;
        newPos.x += m_xOffset;
        transform.position = newPos;

        m_lastPos = m_player.transform.position;
    }
}
