using UnityEngine;
using System.Collections;

public class EnemyGrounded : Enemy
{
    public float stopRange = 2f;   // Distance at which the enemy stops chasing and starts jumping
    public float jumpHeight = 3f;   // Height of the jump

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

            if (distanceToPlayer <= stopRange)
            {
                JumpAttack();
            }
            else
            {
                ChasePlayer(distanceToPlayer);
            }
        }
    }

    // Chase the player when within detection range
    private void ChasePlayer(float distanceToPlayer)
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

    // Jump towards the player and return to the original position
    private void JumpAttack()
    {
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        rb.velocity = Vector2.zero; // Stop moving before jumping

        // Jump up
        rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);

        // Wait for a moment before moving back to the original position
        yield return new WaitForSeconds(0.5f); // Adjust time as necessary

        // Return to the original position
        yield return StartCoroutine(ReturnToPosition());
    }

    private IEnumerator ReturnToPosition()
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = initialPosition; // Use inherited initialPosition from the base class

        while (elapsedTime < 0.5f) // Adjust duration as necessary
        {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we end up exactly at the target position
        transform.position = targetPos;
    }
}
