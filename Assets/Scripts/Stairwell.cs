using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairwell : MonoBehaviour,
    IUseable
{
    [SerializeField]
    private GameObject m_destination;

    private bool m_active = false;
    private const float TRAVEL_TIME = 0.75f;

    public void OnUse()
    {
        if (!m_active)
        {
            GoToDestination();
        }
    }

    private void GoToDestination()
    {
        m_active = true;
        StartCoroutine(GoToDestination_Coroutine());
    }

    private IEnumerator GoToDestination_Coroutine()
    {
        float elapsedTime = 0;
        var player = GameObject.FindGameObjectWithTag("Player");
        Vector3 startPos = player.transform.position;
        Vector3 endPos = m_destination.transform.position;

        while (elapsedTime < TRAVEL_TIME)
        {
            var goalPos = Vector3.Lerp(startPos, endPos, elapsedTime / TRAVEL_TIME);
            player.transform.position = goalPos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        m_active = false;
    }
}
