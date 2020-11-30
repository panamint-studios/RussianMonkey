using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<GameObject> floorSpawnPoints = new List<GameObject>();
    float m_SpawnFrequency = 5f;
    bool m_AlarmTriggered;
    float m_AlarmTimer;
    int m_AlarmFloorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            floorSpawnPoints.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_AlarmTriggered)
        {
            AlarmAction();

            //Check if finished spawning enemies
            if (m_AlarmFloorIndex >= floorSpawnPoints.Count)
                m_AlarmTriggered = false;
        }        
    }

    void AlarmAction()
    {
        m_AlarmTimer += Time.deltaTime;
        if(m_AlarmTimer > m_SpawnFrequency)
        {
            SpawnEnemiesOnFloor(m_AlarmFloorIndex);
            m_AlarmTimer = 0;
            m_AlarmFloorIndex++;
        }
    }

    void SpawnEnemiesOnFloor(int floorNum)
    {
        GameObject floor = floorSpawnPoints[floorNum];
        foreach(Transform child in floor.transform)
        {
            Instantiate(enemyPrefab, child.position, enemyPrefab.transform.rotation);
        }
    }

    public void StartAlarm()
    {
        m_AlarmTriggered = true;
    }

    public void StopAlarm()
    {
        m_AlarmTriggered = false;
    }
}
