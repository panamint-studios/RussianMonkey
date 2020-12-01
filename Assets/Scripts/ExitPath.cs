using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPath : MonoBehaviour
{
    [SerializeField]
    private float m_distanceThreshold = 0.5f;
    [SerializeField]
    private float m_period = 2.0f;
    [SerializeField]
    private float m_distanceToBounce = 2.0f;
    [SerializeField]
    private float m_distanceFromPlayer = 2.0f;

    private Image m_image;

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ShowExitPath());
    }

    private void Update()
    {
        
    }

    private IEnumerator ShowExitPath()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var exit = GameState.Instance.currentExit;
        var elapsed = 0.0f;
        var step = 0.0f;

        while (DistanceFromExit(player, exit) > m_distanceThreshold)
        {
            m_image.enabled = true;
            BounceSprite(player, exit, step);
            elapsed += Time.deltaTime;
            step = Mathf.Sin(((elapsed % m_period) / m_period) * Mathf.PI);
            yield return null;
        }
        m_image.enabled = false;
    }

    private void BounceSprite(GameObject player, GameObject exit, float step)
    {
        Vector2 curVec = (exit.transform.position - player.transform.position);
        curVec.Normalize();
        var start = m_distanceFromPlayer * curVec;
        var end = (m_distanceToBounce * curVec) + start;
        transform.localPosition = Vector2.Lerp(start, end, step);
        //Quaternion lookRot = Quaternion.LookRotation(curVec);
        //lookRot.eulerAngles = new Vector3(
        //    0,
        //    0,
        //    -lookRot.eulerAngles.x);
        float angle = Mathf.Atan2(curVec.y, curVec.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        q.eulerAngles = new Vector3(
            q.eulerAngles.x,
            q.eulerAngles.y,
            q.eulerAngles.z + 90f);
        transform.localRotation = q;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private float DistanceFromExit(GameObject player, GameObject exit)
    {
        return Math.Abs(Vector3.Distance(player.transform.position, exit.transform.position));
    }
}
