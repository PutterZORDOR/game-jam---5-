using UnityEngine;
using System.Collections;

public class EnemyGrounded : Enemy
{
    public float stopRange = 2f;   // Distance at which the enemy stops chasing and starts jumping
    public float jumpHeight = 3f;  // Height of the jump
    public float jumpForce = 10f;  // Horizontal force to move the enemy toward the player

    private bool isJumping = false; // To prevent multiple jumps
    private bool isReturning = false; // To track when the enemy is returning to the original position

    protected override void Start()
    {
        base.Start(); // Call the base class's Start method
    }

    protected override void Update()
    {
        base.Update(); // Use base Update to handle patrol and CanAttack setting

        if (CanAttack)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= stopRange && !isJumping && !isReturning)
            {
                // If within range, perform a jump attack
                JumpAttack();
            }
            else if (!isJumping && !isReturning)
            {
                // Continue chasing the player if not jumping or returning
                ChasePlayer();
            }
        }

    }

    // Chase the player when they are within detection range but outside jump attack range
    private void ChasePlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = direction.normalized * Speed;

        // Flip the enemy sprite to face the player
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }

    // Jump towards the player
    private void JumpAttack()
    {
        if (!isJumping)
        {
            StartCoroutine(JumpCoroutine());
        }
    }

    private IEnumerator JumpCoroutine()
    {
        isJumping = true;

        // Calculate the jump direction towards the player
        Vector3 direction = (player.transform.position - transform.position).normalized;


        // Add a vertical and horizontal force to create a jump arc
        Vector2 jumpVector = new Vector2(direction.x * jumpForce, jumpHeight);
        rb.velocity = Vector2.zero; // Stop any current movement before jumping
        rb.AddForce(jumpVector, ForceMode2D.Impulse);

        // Wait for the jump to finish (you can adjust this timing)
        yield return new WaitForSeconds(1f);

        // After jumping, return to original patrol position
        StartCoroutine(ReturnToPosition());
    }

    // Move back to the initial patrol position after the jump attack
    private IEnumerator ReturnToPosition()
    {
        isReturning = true;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = initialPosition; // Use initial patrol position

        float returnDuration = 0.5f; // Adjust this for how long it takes to return

        while (elapsedTime < returnDuration)
        {
            // Smoothly move the enemy back to the original position
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / returnDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure exact position at the end
        transform.position = targetPos;
        isJumping = false;
        isReturning = false;
    }

}