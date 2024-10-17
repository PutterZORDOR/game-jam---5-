using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject rainPrefab;        // The prefab for the rain
    public GameObject rainSpawnArea;     // The GameObject where rain should spawn

    public float minRainInterval = 1f;   // Minimum time for rain summon
    public float maxRainInterval = 3f;   // Maximum time for rain summon
    public int rainSpawnFrequency = 3;   // Number of raindrops to spawn each time
    public float attackPhaseDurationMin = 2f; // Minimum duration of attack phase
    public float attackPhaseDurationMax = 5f; // Maximum duration of attack phase
    public float cooldownDuration = 3f;  // Time during which the boss is vulnerable after an attack phase
    public float maxHealth = 100;          // Boss's maximum health
    public float currentHealth;            // Boss's current health

    private Transform player;             // Reference to the player
    public bool isUsingSkill = false;    // A flag to check if a skill is in progress
    public bool isInAttackPhase;
    public bool isShield;
    public bool isInCooldown = false;    // A flag to check if the boss is in cooldown phase
    private Animator animator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player object has the 'Player' tag.");
        }
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // Initialize health

        isInAttackPhase = true;
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
    public void ApplyDamage(float damage)
    {
        // Check if the boss is immune (during attack phase)
        if (!isShield && !isInCooldown)
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
        else if (isInCooldown) // Boss vulnerable during cooldown
        {
            Debug.Log("Boss is in cooldown and vulnerable.");
            currentHealth -= damage;
            Debug.Log("Boss took damage during cooldown! Current Health: " + currentHealth);

            // Check if the boss is dead
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        animator.SetBool("Die", true);
        this.enabled = false;
    }

    // Coroutine to use skills sequentially
    IEnumerator UseSkillsSequentially()
    {
        while (true)
        {
            if (!isUsingSkill)
            {
                isUsingSkill = true; // Mark that a skill is in progress

                // Enter the attack phase
                isInCooldown = false;
                Debug.Log("Entering attack phase.");

                // Randomly choose to summon rain
                yield return StartCoroutine(SummonRain());

                // Randomly determine the duration of the attack phase
                float attackDuration = Random.Range(attackPhaseDurationMin, attackPhaseDurationMax);
                yield return new WaitForSeconds(attackDuration);

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

    // Summon rain randomly within the defined spawn area
    IEnumerator SummonRain()
    {
        while (isInAttackPhase) // Continue spawning while in attack phase
        {
            // Choose a random interval for the next rain summon
            float randomInterval = Random.Range(minRainInterval, maxRainInterval);
            yield return new WaitForSeconds(randomInterval);
            animator.SetBool("Armor", true);
            isShield = true;
            // Get the bounds of the rain spawn area
            BoxCollider2D spawnArea = rainSpawnArea.GetComponent<BoxCollider2D>();
            Vector2 spawnSize = spawnArea.size;
            Vector2 spawnCenter = spawnArea.offset;

            // Generate a random position within the bounds of the spawn area
            for (int i = 0; i < rainSpawnFrequency; i++)
            {
                // Get random position within the spawn area
                Vector3 randomPosition = new Vector3(
                    rainSpawnArea.transform.position.x + Random.Range(spawnCenter.x - spawnSize.x / 2, spawnCenter.x + spawnSize.x / 2),
                    rainSpawnArea.transform.position.y, // Adjust to ensure it spawns above the area
                    rainSpawnArea.transform.position.z
                );

                GameObject rain = Instantiate(rainPrefab, randomPosition, Quaternion.identity);

                // Rain falls downwards
                rain.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -5f);

                // Delay before spawning the next raindrop
                yield return new WaitForSeconds(0.5f);
            }
            animator.SetBool("Armor", false);
            isShield = false;
            isInAttackPhase = false;
        }
    }
}
