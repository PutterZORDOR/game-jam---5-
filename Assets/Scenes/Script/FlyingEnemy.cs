using UnityEngine;
using System.Collections;

public class FlyingEnemy : Enemy
{
    public float hoverHeight = 5f;        // Adjustable height from the ground
    public float hoverSpeed = 2f;         // Speed of moving up and down while hovering
    public float soarSpeed = 15f;         // Speed when soaring down during attack
    public float pauseBeforeAttack = 1f;  // Pause duration before the attack
    public float attackRange = 10f;       // Range within which the enemy detects the player

    private Vector3 attackTarget;         // The player's position when the attack is triggered
    private bool isAttacking = false;     // Track if the enemy is in attack mode
    private bool isHovering = false;      // Track if the enemy is hovering
    private bool hasPaused = false;       // Track if the enemy has paused before attacking
    private float hoverOffset;            // Hovering offset for smooth movement
    private bool hasAttacked = false;     // Track if the enemy has already attacked

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hoverOffset = transform.position.y; // Set the initial hover position
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // Check distance to the player
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // If player is in range and not attacking, start hover and prepare for attack
        if (distanceToPlayer <= attackRange && !isAttacking && !hasAttacked)
        {
            CanAttack = true;

            if (!isHovering)
            {
                isHovering = true;
                rb.velocity = Vector2.zero; // Stop patrolling and start hovering
            }

            if (!hasPaused)
            {
                StartCoroutine(PauseBeforeAttack());
            }
        }
        else
        {
            // If the player is out of range, return to patrolling
            if (!isAttacking && !hasAttacked)
            {
                CanAttack = false;
                isHovering = false;
                hasPaused = false;
                Patrol();
            }
        }

        // Continue to hover if not attacking and the player is in range
        if (!isAttacking && isHovering)
        {
            Hover();
        }

        // If in attack mode, soar down towards the player
        if (isAttacking && !hasAttacked)
        {
            SoarDown();
        }
    }

    // Handle the hovering movement
    private void Hover()
    {
        // Raycast downward to find the ground directly below the enemy
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity);

        float targetY;

        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            float groundHeight = hit.point.y; // Get the y position of the ground
            targetY = groundHeight + hoverHeight; // Hover relative to ground
        }
        else
        {
            // If no ground is detected, hover at the initial hover height relative to the starting position
            targetY = hoverOffset + hoverHeight;
        }

        // Smoothly move the enemy up and down in a sine wave around the hover height
        float newY = targetY + Mathf.Sin(Time.time * hoverSpeed) * 0.5f; // Sine wave hover effect
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // Pause for a moment before attacking
    private IEnumerator PauseBeforeAttack()
    {
        hasPaused = true;  // Set the paused state
        rb.velocity = Vector2.zero;  // Stop the enemy's movement

        yield return new WaitForSeconds(pauseBeforeAttack); // Pause for a set duration

        // After the pause, set up for the attack
        attackTarget = player.transform.position; // Get the player's position
        isAttacking = true;  // Switch to attack mode
    }

    // Soar down towards the player's position
    private void SoarDown()
    {
        // Move the enemy towards the player's position at a fast speed
        Vector3 direction = (attackTarget - transform.position).normalized;
        rb.velocity = direction * soarSpeed;

        // Check if the enemy has reached the player's position or close to it
        if (Vector3.Distance(transform.position, attackTarget) <= 0.5f)
        {
            AttackPlayer();
        }
    }

    // Attack the player and destroy the enemy
    private void AttackPlayer()
    {
        // Your attack logic here (e.g., decrease player health)
        Debug.Log("Flying enemy attacking the player!");

        // Set hasAttacked to true to avoid multiple attacks
        hasAttacked = true;

        // Destroy the enemy after a short delay
        Destroy(gameObject, 0.5f);
    }

    // Patrol logic (inherited from the parent class)
    protected override void Patrol()
    {
        base.Patrol();  // Inherit patrol behavior from the base class
    }
}
