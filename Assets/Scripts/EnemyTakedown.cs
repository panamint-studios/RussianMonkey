using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakedown : MonoBehaviour, IAction
{
    EnemyBrain enemyBrain;
    // Start is called before the first frame update
    void Start()
    {
        enemyBrain = this.GetComponent<EnemyBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PerformAction()
    {
        if(enemyBrain.currentState == EnemyBrain.State.Dead && GameState.Instance != null)
        {
            GameState.Instance.playerState.enemiesKilled++;
            GameState.Instance.playerState.enemiesAlive--;
        }
        else
        {

        }
    }
}
