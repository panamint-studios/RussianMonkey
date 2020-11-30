using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootyHand;
    public GameObject knockoutIcon;
    public GameObject keySprite;
    public GameObject keyPrefab; //For dropping a key on death
    public bool hasKey;
    private Transform player;

    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.

    public enum State
    {
        Patrolling,
        Wait,
        Attacking,
        Dead,
        Unconscious
    }

    public State currentState = State.Patrolling;

    private Rigidbody2D m_Rigidbody2D;

    private float m_Health = 10f;

    private float m_WaitDuration;
    private float m_CurrentWaitTimer;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private float m_ShootCooldownDuration;
    private float m_CurrentShootTimer;


    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GameState.Instance.playerState.enemiesAlive++;

        if (hasKey)
            keySprite.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        StateLogic();
        CheckState();
    }
    
    public void TakeDamage(float damage, bool lethal=true)
    {
        m_Health -= damage;

        if(m_Health <= 0 && currentState != State.Dead && currentState != State.Unconscious)
        {
            //Destroy(gameObject);
            ToggleKnockoutIcon(false);
            currentState = lethal ? State.Dead : State.Unconscious;
            EnemyTakedown enemyTakedown = this.GetComponent<EnemyTakedown>();
            enemyTakedown.PerformAction();

            if (hasKey)
            {
                keySprite.SetActive(false);
                Vector3 keySpawnPos = transform.position + new Vector3(0, 1, 0);
                Instantiate(keyPrefab, keySpawnPos, keyPrefab.transform.rotation);
            }

            if(m_FacingRight)
                transform.eulerAngles = new Vector3(0, 0, 90);
            else
                transform.eulerAngles = new Vector3(0, 0, -90);
        }
    }

    void StateLogic()
    {
        switch (currentState)
        {
            case State.Patrolling:
                float move = m_FacingRight ? 1 : -1;
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
                break;
            case State.Wait:
                break;
            case State.Attacking:
                ShootAtPlayer();
                break;
        }
    }

    void CheckState()
    {
        switch (currentState)
        {
            case State.Patrolling:
                WallCheck();
                EnemyCheck();
                break;
            case State.Wait:
                m_CurrentWaitTimer += Time.deltaTime;
                if(m_CurrentWaitTimer >= m_WaitDuration)
                {
                    StartState(State.Patrolling);
                }
                break;
        }
    }

    void StartState(State newState)
    {
        switch (newState)
        {
            case State.Wait:
                m_CurrentWaitTimer = 0;
                m_WaitDuration = 2f;
                break;
            case State.Patrolling:
                // Switch the way the player is labelled as facing.
                m_FacingRight = !m_FacingRight;

                // Multiply the player's x local scale by -1.
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                break;
            case State.Attacking:
                m_ShootCooldownDuration = 1f;
                m_CurrentShootTimer = 0f;
                break;
        }

        currentState = newState;
    }

    void EnemyCheck()
    {
        float perception = 50;
        Vector2 dir = m_FacingRight ? Vector2.right : Vector2.left;
        Vector2 forward = transform.TransformDirection(dir);
        var layerMask = ~(1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Ignore Raycast"));

        Vector2 offset = new Vector2(transform.position.x, transform.position.y + 0.5f);
        RaycastHit2D hit = Physics2D.CircleCast(offset, 1, forward, perception, layerMask);

        if (hit.collider != null)
        {
            if (hit.transform.tag == "Player")
            {
                StartState(State.Attacking);
            }
        }
    }

    void ShootAtPlayer()
    {

        if(m_CurrentShootTimer >= m_ShootCooldownDuration)
        {
            Debug.Log("Shooting!!!");
            Vector3 shootDir = shootyHand.position - player.position;

            Vector3 handPos = transform.position + Vector3.Normalize(player.position - transform.position);
            shootyHand.position = new Vector3(handPos.x, handPos.y, 0);

            float randomOffset = Random.Range(-1f, 1f);
            Vector3 vectorToTarget = player.position - shootyHand.position + new Vector3(0, randomOffset, 0);
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject bullet = Instantiate(bulletPrefab, shootyHand.position, q);
            Destroy(bullet, 1f);

            m_CurrentShootTimer = 0;
        }

        m_CurrentShootTimer += Time.deltaTime;
    }

    void WallCheck()
    {
        float perception = 50;
        Vector2 dir = m_FacingRight ? Vector2.right : Vector2.left;
        Vector2 forward = transform.TransformDirection(dir);
        var layerMask = 1 << LayerMask.NameToLayer("Environment");

        Vector2 offset = new Vector2(transform.position.x, transform.position.y + 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(offset, forward, perception, layerMask);
        //Wall check
        if (hit.collider != null)
        {
            //Will probably need to check the tag of the collider later
            float distance = Mathf.Abs(hit.point.x - transform.position.x);

            //Debug
            forward = transform.TransformDirection(dir) * distance;
            Debug.DrawRay(offset, forward, Color.green);

            if (distance < 2f)
            {
                StartState(State.Wait);
            }
        }
    }

    public void ToggleKnockoutIcon(bool showIcon)
    {
        if(currentState != State.Dead && currentState != State.Unconscious)
            knockoutIcon.SetActive(showIcon);
    }
}
