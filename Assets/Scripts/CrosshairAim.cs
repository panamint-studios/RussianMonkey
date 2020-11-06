using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairAim : MonoBehaviour
{
    Transform player;
    LineRenderer lr;
    float shotTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        lr = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseControls();

        if(shotTimer >= 0)
        {
            shotTimer -= Time.deltaTime;
        }
        else
        {
            lr.positionCount = 0;
        }
    }

    void MouseControls()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = targetPos + new Vector3(0, 0, 1);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerShoot();
        }
    }

    void PlayerShoot()
    {
        //Debug(?) visual
        lr.positionCount = 2;
        lr.SetPosition(0, player.position);
        lr.SetPosition(1, transform.position);
        shotTimer = 0.02f;

        Vector3 shootDir = Vector3.Normalize(transform.position - player.position);
        RaycastHit2D hit = Physics2D.Raycast(player.position + shootDir, shootDir * 100);

        if (hit.collider != null)
        {
            if(hit.collider.tag == "Shootable")
            {
                Rigidbody2D colliderRB = hit.collider.GetComponent<Rigidbody2D>();
                colliderRB.AddForce(shootDir * 250);
            }
            
        }
    }
}
