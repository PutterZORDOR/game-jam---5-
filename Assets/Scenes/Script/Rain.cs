using UnityEngine;

public class Rain : MonoBehaviour
{
    public float lifetime = 5f;  // Time in seconds before the rain is destroyed
    public float fallSpeed = 2f; // Default fall speed

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set the velocity to make the rain fall down
        rb.velocity = new Vector2(0, -fallSpeed);

        // Destroy the rain after the lifetime duration
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Hit");
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Ground"))
        {
            // Deactivate the projectile when hitting a wall or other obstacles
            gameObject.SetActive(false);
        }
    }
}