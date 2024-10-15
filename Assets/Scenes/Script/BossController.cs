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
    public float minRainSpeed = 2f;      // Minimum speed for rain fall
    public float maxRainSpeed = 8f;      // Maximum speed for rain fall
    public float beamDelay = 1f;         // Delay before shooting beams
    public float beamSpeed = 10f;        // Speed of the beams
    public float attackPhaseDurationMin = 2f; // Minimum duration of attack phase
    public float attackPhaseDurationMax = 5f; // Maximum duration of attack phase
    public GameObject shieldPrefab;       // The shield prefab for immunity

    private Transform player;             // Reference to the player
    [SerializeField] private bool isUsingSkill = false;    // A flag to check if a skill is in progress
    [SerializeField]private bool isInAttackPhase = false; // A flag to check if the boss is in the attack phase
    private GameObject currentShield;     // Reference to the active shield

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player object has the 'Player' tag.");
        }

        StartCoroutine(UseSkillsSequentially()); // Start the skill usage sequence
    }

    // Coroutine to use skills sequentially
    IEnumerator UseSkillsSequentially()
    {
        while (true)
        {
            if (!isUsingSkill)
            {
                isUsingSkill = true; // Mark that a skill is in progress

                // Create a shield for 1 second
                CreateShield();

                // Wait for the shield phase to finish
                yield return new WaitForSeconds(1f);

                // Enter the attack phase
                isInAttackPhase = true;

                // Randomly choose between shooting beams or summoning rain
                int skillChoice = Random.Range(0, 2);
                if (skillChoice == 0)
                {
                    // Summon rain
                    yield return StartCoroutine(SummonRain());
                }
                else
                {
                    // Shoot beams
                    yield return StartCoroutine(ShootBeams());
                }

                // Randomly determine the duration of the attack phase
                float attackDuration = Random.Range(attackPhaseDurationMin, attackPhaseDurationMax);
                yield return new WaitForSeconds(attackDuration);

                // End attack phase
                isInAttackPhase = false;
                Destroy(currentShield); // Destroy the shield after the attack phase
                currentShield = null;    // Clear the shield reference

                isUsingSkill = false; // Mark that the skill has finished
            }

            yield return null; // Wait until the next frame
        }
    }

    private void CreateShield()
    {
        // Instantiate the shield prefab at the boss's position
        currentShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        // Set the shield's parent to the boss to keep it in the right position
        currentShield.transform.SetParent(transform);
    }

    // Summon rain at random intervals, size, and speed
    IEnumerator SummonRain()
    {
        // Choose a random interval for the next rain summon
        float randomInterval = Random.Range(minRainInterval, maxRainInterval);
        yield return new WaitForSeconds(randomInterval);

        // Choose a random position within the defined range for rain to spawn
        Vector3 rainPosition = new Vector3(
            transform.position.x + Random.Range(-rainRangeX, rainRangeX),
            transform.position.y + Random.Range(-rainRangeY, rainRangeY),
            transform.position.z
        );

        // Instantiate the rain prefab at the random position
        GameObject rain = Instantiate(rainPrefab, rainPosition, Quaternion.identity);

        // Set random size for the rain
        float randomSize = Random.Range(minRainSize, maxRainSize);
        rain.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

        // Set random fall speed for the rain
        float randomRainSpeed = Random.Range(minRainSpeed, maxRainSpeed);
        rain.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -randomRainSpeed);

        // Wait for the rain to complete its action (e.g., fall for 2 seconds before the next skill)
        yield return new WaitForSeconds(2f);
    }

    // Shoot two beams: one at player and one in a random direction
    IEnumerator ShootBeams()
    {
        // Delay before shooting the first beam
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

        // Wait for the beam action to complete (e.g., 1 second before the next skill)
        yield return new WaitForSeconds(1f);
    }
}
