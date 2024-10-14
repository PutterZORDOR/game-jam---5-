using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Speed = 10;
    public GameObject player;
    protected Rigidbody2D rb; // Changed to protected so derived classes can access it
    public int health = 100;
    public int damage = 5;
    protected bool CanAttack = false;

    public float detectionRange = 5f;   // Distance to start chasing the player
    public float patrolSpeed = 2f;      // Speed during patrolling
    public float patrolRange = 3f;      // Distance to patrol left and right
    protected Vector3 initialPosition; // Protected for derived classes
    private float patrolDirection = 1f; // 1 for right, -1 for left

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        initialPosition = transform.position; // Save initial position for patrolling

        if (player == null)
        {
            Debug.LogError("Player not found");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            CanAttack = true;
        }
        else
        {
            // Patrol when the player is out of range
            Patrol();
            CanAttack = false;
        }

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Patrol left and right when the player is not in range
    protected void Patrol()
    {
        rb.velocity = new Vector2(patrolDirection * patrolSpeed, rb.velocity.y);

        // Flip direction if the enemy reaches the patrol limit
        if (transform.position.x >= initialPosition.x + patrolRange)
        {
            patrolDirection = -1f; // Move left
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x <= initialPosition.x - patrolRange)
        {
            patrolDirection = 1f; // Move right
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }

    // Handle collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(10); // Deal damage when colliding with the player
            Debug.Log(health);
        }
    }

    // Handle taking damage
    protected void TakeDamage(int dmg)
    {
        health -= dmg;
    }
}
