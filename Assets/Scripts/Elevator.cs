using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Elevator : MonoBehaviour, IUseable
{
    public int floor;

    private Coroutine m_coroutine;
    [SerializeField]
    private List<ElevatorFloor> m_floors;
    private GameObject m_compartment;
    private event Action<int> m_MoveUpLevel;
    private event Action<int> m_MoveDownLevel;
    private bool m_inUse = false;
    private bool m_inRange = false;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_compartment = new GameObject("compartment");
        m_compartment.transform.parent = transform;
        m_compartment.transform.localPosition = m_floors[floor].transform.localPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_inRange = true;
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_inRange = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnUse();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            m_MoveDownLevel?.Invoke(floor - 1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            m_MoveUpLevel?.Invoke(floor + 1);
        }
    }

    public void OnUse()
    {
        m_inUse = !m_inUse;
        if (m_inUse)
        {
            m_MoveDownLevel += LevelInputHandler;
            m_MoveUpLevel += LevelInputHandler;
        }
        else
        {
            m_MoveDownLevel -= LevelInputHandler;
            m_MoveUpLevel -= LevelInputHandler;
        }
    }

    private void LevelInputHandler(int newFloor)
    {

        if (newFloor < 0 || newFloor > m_floors.Count - 1 || m_coroutine != null) return;

        floor = newFloor;

        m_coroutine = StartCoroutine(GoToLevel_Coroutine(m_floors[floor].transform.localPosition, 1.5f));
    }

    private IEnumerator GoToLevel_Coroutine(Vector3 newPos, float time)
    {
        float elapsedTime = 0;
        Vector3 startPos = m_compartment.transform.localPosition;

        while(elapsedTime < time)
        {
            m_compartment.transform.localPosition = Vector3.Lerp(startPos, newPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        m_coroutine = null;
    }
}
