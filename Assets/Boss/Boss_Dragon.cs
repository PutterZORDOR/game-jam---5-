using System.Collections;
using UnityEngine;

public class Boss_Dragon : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform[] waypoints; // จุดการเคลื่อนที่
    public float moveSpeed = 2f; // ความเร็วในการเคลื่อนที่
    public float stopDuration = 2f; // ระยะเวลาหยุดที่แต่ละจุด

    [Header("Attack Settings")]
    public float attackCooldown = 2f; // คูลดาวน์การโจมตี
    public Transform[] meteorSpawnPoints; // จุดที่ spawn อุกกาบาต
    public int meteorCount = 3; // จำนวนอุกกาบาตที่ยิง

    [Header("Health Settings")]
    public int maxHealth = 100; // ค่าพลังชีวิตสูงสุด
    private int currentHealth; // ค่าพลังชีวิตปัจจุบัน

    [Header("Immortality Settings")]
    public bool isImmortal = false; // ตัวแปรระบุว่าอยู่ในสถานะอมตะหรือไม่

    [Header("Projectile Settings")]
    public int projectileCount = 4; // จำนวนลูกโปรเจคไทล์ที่ยิง
    public float projectileSpeed = 5f; // ความเร็วของโปรเจคไทล์
    public float shootCooldown = 5f; // คูลดาวน์การยิงโปรเจคไทล์

    private int currentWaypointIndex = 0; // ตัวแปรเก็บตำแหน่งจุดทางเดินปัจจุบัน
    private bool isAttacking = false; // ตัวแปรเช็คสถานะการโจมตี
    private bool isMoving = false; // ตัวแปรเช็คสถานะการเคลื่อนที่

    private void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(MoveAndAttackRoutine()); // เริ่มต้นการเคลื่อนที่และโจมตี
    }

    private void Update()
    {
        if (!isMoving)
        {
            MoveToWaypoint();
        }
    }

    private IEnumerator MoveAndAttackRoutine()
    {
        while (true)
        {
            // เปิดอัมมะตะ
            isImmortal = true;
            yield return new WaitForSeconds(2f); // เปิดอัมมะตะ 2 วินาที

            // เสกอุกกาบาต
            for (int i = 0; i < meteorCount; i++)
            {
                SpawnMeteor();
                yield return new WaitForSeconds(attackCooldown);
            }

            // ยิงโปรเจคไทล์
            StartCoroutine(ShootProjectilesRoutine());

            // ปิดอัมมะตะ
            isImmortal = false;

            // หยุดเคลื่อนที่ 5 วินาที
            isMoving = true;
            yield return new WaitForSeconds(5f);
            isMoving = false;
        }
    }

    private void MoveToWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform target = waypoints[currentWaypointIndex];
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // ตรวจสอบการถึงจุดหมาย
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                StartCoroutine(StopAtWaypoint());
            }
        }
    }

    private IEnumerator StopAtWaypoint()
    {
        isMoving = true;
        yield return new WaitForSeconds(stopDuration);
        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0; // รีเซ็ตการเดินทางเมื่อถึงจุดสุดท้าย
        }
        isMoving = false;
    }

    private void SpawnMeteor()
    {
        int randomIndex = Random.Range(0, meteorSpawnPoints.Length);
        Transform spawnPoint = meteorSpawnPoints[randomIndex];
        GameObject meteor = ObjectPool.instance.GetProjectile(0);
        if (meteor != null)
        {
            meteor.transform.position = spawnPoint.position;
            meteor.SetActive(true);
        }
    }

    private IEnumerator ShootProjectilesRoutine()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            // สุ่มมุมระหว่าง 20 ถึง 35 องศา
            float angle = Random.Range(20f, 35f);
            float horizontalAngle = Random.Range(-45f, 45f);

            // คำนวณทิศทางการยิง
            Vector2 direction = new Vector2(Mathf.Sin(horizontalAngle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));

            // ดึงโปรเจคไทล์จาก Object Pool
            GameObject projectile = ObjectPool.instance.GetProjectile(1);
            if (projectile != null)
            {
                projectile.transform.position = transform.position; // หรือตำแหน่งที่คุณต้องการ
                projectile.SetActive(true);

                // กำหนดความเร็ว
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed;
                }
            }

            yield return new WaitForSeconds(shootCooldown); // รอระยะเวลาคูลดาวน์
        }
    }

    public void TakeDamage(int damage)
    {
        if (isImmortal)
        {
            Debug.Log("Boss is immortal and takes no damage!");
            return;
        }

        currentHealth -= damage;
        Debug.Log($"Boss takes {damage} damage! Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss has died!");
        Destroy(gameObject);
    }
}
