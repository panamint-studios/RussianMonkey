﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public Transform shootyHand;
    public GameObject bulletPrefab;
    public GameObject fistPrefab;
    public Transform crosshair;
    public bool hasKey;

    Transform player;
    LineRenderer lr;
    float shotTimer = 0;

    private GameObject m_currentObj;

    [Range(0, 20)]
    [SerializeField]
    private float m_handDistance = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        lr = crosshair.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseControls();

        if (shotTimer >= 0)
        {
            shotTimer -= Time.deltaTime;
        }
        else
        {
            lr.positionCount = 0;
        }

        UseableInput();
    }

    private void UseableInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (m_currentObj == null) return;
            IUseable useable = m_currentObj.GetComponent<IUseable>();
            useable.OnUse();
        }
    }

    void MouseControls()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshair.position = targetPos + new Vector3(0, 0, 1);

        Vector3 handPos = player.position + (Vector3.Normalize(crosshair.position - player.position) * m_handDistance);
        shootyHand.position = new Vector3(handPos.x, handPos.y, 0);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerShoot();
        }

        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    GameState.Instance.SetGameState(PlayerState.State.Alarm);
        //}
    }

    //This is deprecated. Keeping for now.
    void PlayerPunch()
    {
        Vector3 shootDir = crosshair.position - shootyHand.position;

        Vector3 vectorToTarget = crosshair.position - shootyHand.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject fist = Instantiate(fistPrefab, shootyHand.position, q);
        //Destroy(bullet, 1f);
    }

    void PlayerShoot()
    {
        //Debug(?) visual
        //lr.positionCount = 2;
        //lr.SetPosition(0, shootyHand.position);
        //lr.SetPosition(1, transform.position);
        //shotTimer = 0.02f;

        //Vector3 shootDir = Vector3.Normalize(transform.position - player.position);
        //RaycastHit2D hit = Physics2D.Raycast(player.position + shootDir, shootDir * 100);

        //if (hit.collider != null)
        //{
        //    if(hit.collider.tag == "Shootable")
        //    {
        //        Rigidbody2D colliderRB = hit.collider.GetComponent<Rigidbody2D>();
        //        colliderRB.AddForce(shootDir * 250);
        //    }

        //}

        Vector3 shootDir = crosshair.position - shootyHand.position;

        Vector3 vectorToTarget = crosshair.position - shootyHand.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject bullet = Instantiate(bulletPrefab, shootyHand.position, q);
        Destroy(bullet, 1f);
    }

    //void OnCollisionStay(Collision collisionInfo)
    //{
    //    Debug.Log(collisionInfo.gameObject.tag);
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy_Blindspot")
        {
            //Display a fun little icon
            EnemyBrain enemy = other.GetComponentInParent<EnemyBrain>();
            enemy.ToggleKnockoutIcon(true);
        }

        if (other.GetComponent<IUseable>() != null)
        {
            m_currentObj = other.gameObject;
        }

        if (other.tag == "Key")
        {
            hasKey = true;
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy_Blindspot")
        {
            //Display a fun little icon
            EnemyBrain enemy = other.GetComponentInParent<EnemyBrain>();
            enemy.ToggleKnockoutIcon(false);
        }

        if (other.gameObject == m_currentObj)
        {
            m_currentObj = null;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Enemy_Blindspot")
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                //PlayerPunch();
                EnemyBrain enemy = other.GetComponentInParent<EnemyBrain>();
                enemy.TakeDamage(10, false);
            }
        }
       
        //if(other.GetComponent<>)
    }
}
