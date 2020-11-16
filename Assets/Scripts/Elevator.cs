using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, IUseable
{
    public int floor;

    private Coroutine m_coroutine;
    [SerializeField]
    private List<ElevatorFloor> m_floors;
    private GameObject m_compartment;
    private SpriteRenderer m_spriteRenderer;
    [SerializeField]
    private Sprite m_compartmentSprite;
    private event Action<int> m_MoveUpLevel;
    private event Action<int> m_MoveDownLevel;
    private bool m_inUse = false;
    private bool m_inRange = false;
    private GameObject m_player;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_compartment = new GameObject("compartment");
        m_spriteRenderer = m_compartment.AddComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = m_compartmentSprite;
        var boxCollider = m_compartment.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(2, 1.5f);
        boxCollider.isTrigger = true;
        

        m_compartment.transform.parent = transform;
        var initPos = m_floors[floor].transform.localPosition;
        m_compartment.transform.localPosition = AdjustPositionForSprite(initPos);
        m_compartment.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_inRange = true;
            Debug.Log("In range");
            m_player = collision.gameObject;
        }
    }

    private void Update()
    {
        if (m_inRange && Input.GetKeyDown(KeyCode.E))
        {
            OnUse();
        }

        if (m_inUse && Input.GetKeyDown(KeyCode.S))
        {
            m_MoveDownLevel?.Invoke(floor - 1);
        }

        if (m_inUse && Input.GetKeyDown(KeyCode.W))
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
            HidePlayer();
        }
        else
        {
            m_MoveDownLevel -= LevelInputHandler;
            m_MoveUpLevel -= LevelInputHandler;
            ShowPlayer();
        }
    }

    private void ShowPlayer()
    {
        m_player.transform.parent = null;
        m_player.transform.position = m_floors[floor].transform.position;
        m_player.SetActive(true);
    }

    private void HidePlayer()
    {
        m_player.SetActive(false);
        m_player.transform.parent = m_compartment.transform;
        m_player.transform.localPosition = Vector3.zero;
    }

    private void LevelInputHandler(int newFloor)
    {

        if (newFloor < 0 || newFloor > m_floors.Count - 1 || m_coroutine != null) return;

        floor = newFloor;

        m_coroutine = StartCoroutine(GoToLevel_Coroutine(AdjustPositionForSprite(m_floors[floor].transform.localPosition), 1.5f));
    }

    private IEnumerator GoToLevel_Coroutine(Vector3 newPos, float time)
    {
        float elapsedTime = 0;
        Vector3 startPos = m_compartment.transform.localPosition;

        while(elapsedTime < time)
        {
            var goalPos = Vector3.Lerp(startPos, newPos, elapsedTime / time);
            m_compartment.transform.localPosition = goalPos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        m_coroutine = null;
    }

    private Vector3 AdjustPositionForSprite(Vector3 inPos)
    {
        var res = inPos;
        res.x -= m_spriteRenderer.sprite.bounds.size.x / 2;
        res.y -= m_spriteRenderer.sprite.bounds.size.y / 2;
        return res;
    }
}
