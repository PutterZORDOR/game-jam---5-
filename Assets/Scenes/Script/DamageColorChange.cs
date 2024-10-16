using UnityEngine;
using System.Collections;

public class DamageColorChange : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // For 2D sprites
    private Color originalColor; // Store the original color of the object

    private void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Store the original color
        }
        else
        {
            Debug.LogError("No SpriteRenderer found on this GameObject.");
        }
    }

    // Call this function to simulate taking damage
    public void TakeDamage()
    {
        if (spriteRenderer != null)
        {
            // Start the color change coroutine
            StartCoroutine(ChangeColorOnDamage());
        }
    }

    private IEnumerator ChangeColorOnDamage()
    {
        // Change to red
        spriteRenderer.color = Color.red;

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Change back to the original color
        spriteRenderer.color = originalColor;
    }
}
