using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 30;
    public float damage = 5;
    public float range = 30;
    public bool lethal = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        range -= speed * Time.deltaTime;

        if (range <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //if (col.gameObject.tag != "Player")
        //{
        //    Destroy(gameObject);
        //}

        switch (col.gameObject.tag)
        {
            case ("Player"):
                Destroy(gameObject);
                break;
            case ("Enemy"):
                EnemyBrain enemyBrain = col.gameObject.GetComponent<EnemyBrain>();
                enemyBrain.TakeDamage(damage, lethal);
                Destroy(gameObject);
                break;
        }

        //Debug.Log(col.gameObject.name);

        if (col.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
