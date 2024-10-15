using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float Force;
    [SerializeField] private int damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    void OnEnable()
    {
        // Calculate direction toward the player, but only apply force along the x-axis.
        Vector3 direction = player.transform.position - transform.position;

        // Apply horizontal force to the projectile, letting gravity handle the y-axis.
        rb.velocity = new Vector2(Mathf.Sign(direction.x) * Force, rb.velocity.y);

        // Optionally rotate the projectile to face the movement direction
        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Add player damage logic here
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Wall"))
        {
            // Deactivate the projectile when hitting a wall or other obstacles
            gameObject.SetActive(false);
        }
    }
}