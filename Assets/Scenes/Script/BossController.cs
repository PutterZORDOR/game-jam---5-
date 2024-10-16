using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject rainPrefab;        // The prefab for the rain
    public GameObject beamPrefab;        // The prefab for the beam
    public GameObject shieldPrefab;      // The shield prefab for immunity
    public GameObject rainSpawnArea;     // The GameObject where rain should spawn

    public float minRainInterval = 1f;   // Minimum time for rain summon
    public float maxRainInterval = 3f;   // Maximum time for rain summon
    public int rainSpawnFrequency = 3;   // Number of raindrops to spawn each time
    public float beamDelay = 1f;         // Delay before shooting beams
    public float attackPhaseDurationMin = 2f; // Minimum duration of attack phase
    public float attackPhaseDurationMax = 5f; // Maximum duration of attack phase
    public float cooldownDuration = 3f;  // Time during which the boss is vulnerable after an attack phase
    public int maxHealth = 100;          // Boss's maximum health
    public int currentHealth;            // Boss's current health

    private Transform player;             // Reference to the player
    public bool isUsingSkill = false;    // A flag to check if a skill is in progress
    public bool isInAttackPhase = false; // A flag to check if the boss is in the attack phase
    public bool isInCooldown = false;    // A flag to check if the boss is in cooldown phase
    private GameObject shieldInstance;    // Reference to the active shield

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player object has the 'Player' tag.");
        }

        currentHealth = maxHealth; // Initialize health

        // Instantiate the shield but deactivate it initially
        shieldInstance = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        shieldInstance.transform.SetParent(transform);
        shieldInstance.SetActive(false);  // Shield starts inactive

        StartCoroutine(UseSkillsSequentially()); // Start the skill usage sequence
    }

    private void Update()
    {
        // Debug system to apply damage to boss when 'P' is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P key pressed: attempting to damage boss...");
            ApplyDamage(10); // Apply 10 damage for testing
        }
    }

    // Apply damage to the boss, but only if it's not in the attack phase or cooldown phase
    public void ApplyDamage(int damage)
    {
        if (!isInAttackPhase && !isInCooldown)
        {
            currentHealth -= damage;
            Debug.Log("Boss took damage! Current Health: " + currentHealth);


            DamageColorChange damageColorChange = GetComponent<DamageColorChange>();
            if (damageColorChange != null)
            {
                damageColorChange.TakeDamage();
            }

            // Check if the boss is dead
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        else if (isInAttackPhase)
        {
            Debug.Log("Boss is immune during the attack phase.");
        }
        else if (isInCooldown)
        {
            Debug.Log("Boss is in cooldown and vulnerable.");
        }
    }

    private void Die()
    {
        Debug.Log("Boss has died!");
        Destroy(gameObject); // Destroy the boss object
    }

    // Coroutine to use skills sequentially
    IEnumerator UseSkillsSequentially()
    {
        while (true)
        {
            if (!isUsingSkill)
            {
                isUsingSkill = true; // Mark that a skill is in progress

                // Activate shield for the attack phase
                ActivateShield();

                // Wait for shield activation time
                yield return new WaitForSeconds(1f);

                // Enter the attack phase
                isInAttackPhase = true;
                isInCooldown = false;
                Debug.Log("Entering attack phase. Shield activated.");

                // Randomly choose between shooting beams or summoning rain
                int skillChoice = Random.Range(0, 2);
                if (skillChoice == 0)
                {
                    yield return StartCoroutine(SummonRain());
                }
                else
                {
                    yield return StartCoroutine(ShootBeams());
                }

                // Randomly determine the duration of the attack phase
                float attackDuration = Random.Range(attackPhaseDurationMin, attackPhaseDurationMax);
                yield return new WaitForSeconds(attackDuration);

                // End attack phase
                isInAttackPhase = false;
                DeactivateShield(); // Deactivate the shield after the attack phase
                Debug.Log("Exiting attack phase. Shield deactivated.");

                // Enter cooldown phase
                isInCooldown = true;
                Debug.Log("Boss is in cooldown phase and vulnerable.");

                // Wait for cooldown duration where boss is vulnerable
                yield return new WaitForSeconds(cooldownDuration);

                // Exit cooldown phase
                isInCooldown = false;
                Debug.Log("Boss cooldown phase ended.");

                isUsingSkill = false; // Mark that the skill has finished
            }

            yield return null;
        }
    }

    // Activate the shield at the start of the attack phase
    private void ActivateShield()
    {
        if (shieldInstance != null && !shieldInstance.activeSelf)
        {
            shieldInstance.SetActive(true);
            Debug.Log("Shield activated.");
        }
    }

    // Deactivate the shield at the end of the attack phase
    private void DeactivateShield()
    {
        if (shieldInstance != null && shieldInstance.activeSelf)
        {
            shieldInstance.SetActive(false);
            Debug.Log("Shield deactivated.");
        }
    }

    // Summon rain randomly within the defined spawn area
    IEnumerator SummonRain()
    {
        while (isInAttackPhase) // Continue spawning while in attack phase
        {
            // Choose a random interval for the next rain summon
            float randomInterval = Random.Range(minRainInterval, maxRainInterval);
            yield return new WaitForSeconds(randomInterval);

            // Get the bounds of the rain spawn area
            BoxCollider2D spawnArea = rainSpawnArea.GetComponent<BoxCollider2D>();
            Vector2 spawnSize = spawnArea.size;
            Vector2 spawnCenter = spawnArea.offset;

            // Generate a random position within the bounds of the spawn area
            for (int i = 0; i < rainSpawnFrequency; i++)
            {
                // Get random position within the spawn area
                Vector3 randomPosition = new Vector3(
                    rainSpawnArea.transform.position.x + Random.Range(-spawnSize.x / 2 + spawnCenter.x, spawnSize.x / 2 + spawnCenter.x),
                    rainSpawnArea.transform.position.y + Random.Range(-spawnSize.y / 2 + spawnCenter.y, spawnSize.y / 2 + spawnCenter.y) + 2f, // Adjust to ensure it spawns above the area
                    rainSpawnArea.transform.position.z
                );

                GameObject rain = Instantiate(rainPrefab, randomPosition, Quaternion.identity);

                // Rain falls downwards
                rain.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -5f);

                // Delay before spawning the next raindrop
                yield return new WaitForSeconds(0.5f);
            }
        }
    }  

    // Shoot beams at the player and randomly
    IEnumerator ShootBeams()
    {
        yield return new WaitForSeconds(beamDelay);

        if (player != null)
        {
            // First beam toward the player
            GameObject beamToPlayer = Instantiate(beamPrefab, transform.position, Quaternion.identity);
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            beamToPlayer.GetComponent<Beam>().SetBeamDirection(directionToPlayer);
        }

        yield return new WaitForSeconds(1f);

        // Randomly shoot another beam in any direction
        GameObject randomBeam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        randomBeam.GetComponent<Beam>().SetBeamDirection(randomDirection);
    }
}
