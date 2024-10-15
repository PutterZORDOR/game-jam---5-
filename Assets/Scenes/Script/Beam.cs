using UnityEngine;

public class Beam : MonoBehaviour
{
    public float lifetime = 3f;  // How long the beam exists before being destroyed
    public float beamSpeed = 10f; // Speed of the beam
    public int damage = 10;      // Damage dealt by the beam

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on the Beam object!");
        }

        // Automatically destroy the beam after the lifetime expires
        Destroy(gameObject, lifetime);
    }

    // Call this function to set the velocity of the beam from outside (e.g., in the Boss script)
    public void SetBeamDirection(Vector2 direction)
    {
        if (rb != null)
        {
            rb.velocity = direction.normalized * beamSpeed;
            Debug.Log("Beam direction set: " + direction);
        }
        else
        {
            Debug.LogError("Beam Rigidbody2D is null!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Enter Detected"); // Check if the trigger event is firing

        // Optional: Handle interactions with the player or other objects
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Beam hit the player!");
        }
        else
        {
            Debug.Log("Beam hit something else: " + collision.gameObject.name);
        }
    }
}
