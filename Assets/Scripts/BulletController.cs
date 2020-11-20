using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    float m_Speed = 30;
    float m_Damage = 5; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * m_Speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }

        switch (col.gameObject.tag)
        {
            case ("Player"):
                Destroy(gameObject);
                break;
            case ("Enemy"):
                EnemyBrain enemyBrain = col.gameObject.GetComponent<EnemyBrain>();
                enemyBrain.TakeDamage(m_Damage);
                Destroy(gameObject);
                break;
        }
    }
}
