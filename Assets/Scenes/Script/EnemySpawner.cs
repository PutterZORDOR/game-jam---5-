using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public float spawnDelay = 0f;  // Delay before spawning the enemy
    public float spawnRange = 1f;   // Range around the spawner for spawning

    private bool hasSpawned = false; // To check if the enemy has already been spawned

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasSpawned)
        {
            SpawnEnemy();
            hasSpawned = true; // Prevents multiple spawns
            gameObject.SetActive(false); // Disable the spawner
        }
    }

    private void SpawnEnemy()
    {
        // Calculate a random spawn position around the spawner
        Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRange;
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }
}
