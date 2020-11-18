using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Rigidbody2D))]
public class BoomArmCamera : MonoBehaviour
{
    private GameObject m_player;
    private GameObject m_playerGraphics;
    private Rigidbody2D m_playerRigidBody;
    private Rigidbody2D m_rigidBody;
    private Vector3 m_velocity = Vector3.zero;
    private const float SMOOTH_TIME = 0.3f;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_playerGraphics = m_player.transform.Find("Graphics").gameObject;
        m_playerRigidBody = m_player.GetComponent<Rigidbody2D>();
        m_rigidBody = GetComponent<Rigidbody2D>();

        Debug.Assert(m_player != null);
        Debug.Assert(m_playerRigidBody != null);
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(
            m_player.transform.position.x,
            m_player.transform.position.y,
            -10);
    }

    // Update is called once per frame
    void Update()
    {
        var targetPosition = new Vector3(
            m_player.transform.position.x,
            m_player.transform.position.y,
            -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_velocity, SMOOTH_TIME);
    }
}
