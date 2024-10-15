using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject rainPrefab;        // The prefab for the rain
    public GameObject beamPrefab;        // The prefab for the beam
    public float minRainInterval = 3f;   // Minimum time for rain summon
    public float maxRainInterval = 8f;   // Maximum time for rain summon
    public float minRainSize = 0.5f;     // Minimum size for rain
    public float maxRainSize = 2.5f;     // Maximum size for rain
    public float rainRangeX = 5f;        // Range on the X-axis for rain to spawn
    public float rainRangeY = 2f;        // Range on the Y-axis for rain to spawn
    public float beamDelay = 1f;         // Delay before shooting beams
    public float beamSpeed = 10f;        // Speed of the beams
    private Transform player;            // Reference to the player

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player object has the 'Player' tag.");
        }

        StartCoroutine(SummonRain());
        StartCoroutine(ShootBeams());
    }

    // Summon rain at random intervals and random size
    IEnumerator SummonRain()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minRainInterval, maxRainInterval));

            // Choose a random position within the defined range for rain to spawn
            Vector3 rainPosition = new Vector3(
                transform.position.x + Random.Range(-rainRangeX, rainRangeX),
                transform.position.y + Random.Range(-rainRangeY, rainRangeY),
                transform.position.z
            );

            GameObject rain = Instantiate(rainPrefab, rainPosition, Quaternion.identity);
            float randomSize = Random.Range(minRainSize, maxRainSize);
            rain.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        }
    }

    // Shoot two beams: one at player and one in a random direction
    IEnumerator ShootBeams()
    {
        while (true)
        {
            yield return new WaitForSeconds(beamDelay);

            if (player != null)
            {
                // Shoot first beam towards the player
                GameObject beamToPlayer = Instantiate(beamPrefab, transform.position, Quaternion.identity);
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                beamToPlayer.GetComponent<Rigidbody2D>().velocity = directionToPlayer * beamSpeed;
            }

            // Wait for 1 second before shooting the second beam
            yield return new WaitForSeconds(1f);

            // Shoot second beam in a random direction
            GameObject randomBeam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            randomBeam.GetComponent<Rigidbody2D>().velocity = randomDirection * beamSpeed;
        }
    }
}
