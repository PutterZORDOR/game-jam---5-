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
    public float maxHealth = 100; // ค่าพลังชีวิตสูงสุด
    [SerializeField]private float currentHealth; // ค่าพลังชีวิตปัจจุบัน

    [Header("Immortality Settings")]
    public bool isImmortal = false; // ตัวแปรระบุว่าอยู่ในสถานะอมตะหรือไม่

    [Header("Projectile Settings")]
    public int projectileCount = 4; // จำนวนลูกโปรเจคไทล์ที่ยิง
    public float projectileSpeed = 5f; // ความเร็วของโปรเจคไทล์
    public float shootCooldown = 5f; // คูลดาวน์การยิงโปรเจคไทล์

    [Header("Player")]
    public GameObject player;

    [Header("Shooting Settings")]
    public Transform[] bulletSpawnPoints;

    [Header("Lazer")]
    public GameObject[] lazerX;
    public GameObject[] lazerY;

    private int currentWaypointIndex = 0; // ตัวแปรเก็บตำแหน่งจุดทางเดินปัจจุบัน
    private bool isMoving = false; // ตัวแปรเช็คสถานะการเคลื่อนที่
    private bool isUsingSkill = false; // ตัวแปรเช็คสถานะการใช้สกิล
    private bool isShooting = false; // ตัวแปรเช็คสถานะการยิง

    private bool changeState;
    private bool isChangingAttackPattern = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!isMoving)
        {
            MoveToWaypoint();
            if (!isShooting)
            {
                StartCoroutine(ShootAtPlayerContinuously());
            }
        }
        
        if(currentHealth > maxHealth * 0.5 && !changeState)
        {
            if (!isUsingSkill && !isMoving)
            {
                StartCoroutine(UseSkillPattern());
            }
        }
        else
        {
            //เล่น animation
        }
    }
    public void ChangeState()
    {
        moveSpeed = moveSpeed * 2.3f;
        changeState = true;

        if (!isChangingAttackPattern)
        {
            StartCoroutine(NewAttackPattern());
        }
    }
    private IEnumerator NewAttackPattern()
    {
        isChangingAttackPattern = true; // ตั้งค่าสถานะการเปลี่ยนแพทเทิร์นการโจมตีให้เป็นจริง

        // ยิงเลเซอร์ในแกน Y
        yield return ShootLaserY();

        // ยิงเลเซอร์ในแกน X
        yield return ShootLaserX();

        // หยุดอยู่กับที่
        isMoving = false;

        // เล่นฟังก์ชัน spawn meteor
        while (currentHealth > 0) // เมื่อบอสยังมีชีวิตอยู่
        {
            for (int j = 0; j < meteorCount; j++)
            {
                SpawnMeteor();
                yield return new WaitForSeconds(attackCooldown); // รอคูลดาวน์ระหว่างการ spawn
            }

            yield return new WaitForSeconds(2f); // รอระยะเวลาระหว่างการยิงอุกกาบาตครั้งต่อไป
        }

        isChangingAttackPattern = false; // เปลี่ยนสถานะการเปลี่ยนแพทเทิร์นการโจมตีกลับ
    }

    private IEnumerator ShootLaserX()
    {
        // ค้นหาจุดยิงเลเซอร์จาก lazerX
        foreach (GameObject laser in lazerX)
        {
            laser.SetActive(true); // เปิดใช้งานเลเซอร์
            LaserBeam laserBeam = laser.GetComponent<LaserBeam>(); // ดึงคอมโพเนนต์ LaserBeam

            if (laserBeam != null)
            {
                laserBeam.FireLaser(); // เรียกใช้เมธอดยิงเลเซอร์
            }

            // รอให้ยิงเสร็จ
            yield return new WaitForSeconds(1.3f); // รอระยะเวลาระหว่างการยิง
        }
    }
    private IEnumerator ShootLaserY()
    {
        // ค้นหาจุดยิงเลเซอร์จาก lazerY
        foreach (GameObject laser in lazerY)
        {
            laser.SetActive(true); // เปิดใช้งานเลเซอร์
            LaserBeam laserBeam = laser.GetComponent<LaserBeam>(); // ดึงคอมโพเนนต์ LaserBeam

            if (laserBeam != null)
            {
                laserBeam.FireLaser(); // เรียกใช้เมธอดยิงเลเซอร์
            }

            // รอให้ยิงเสร็จ
            yield return new WaitForSeconds(1.3f); // รอระยะเวลาระหว่างการยิง
        }
    }

    private IEnumerator ShootAtPlayerContinuously()
    {
        isShooting = true; // ตั้งค่าสถานะการยิงให้เป็นจริง
        while (true) // ลูปตลอดไป
        {
            // สุ่มเลือก spawn point
            int randomIndex = Random.Range(0, bulletSpawnPoints.Length);
            Transform bulletSpawnPoint = bulletSpawnPoints[randomIndex];

            // คำนวณทิศทางการยิงไปยังผู้เล่น
            Vector2 direction = (player.transform.position - bulletSpawnPoint.position).normalized;

            // ดึงโปรเจกไทล์จาก Object Pool
            GameObject projectile = ObjectPool.instance.GetProjectile(2);
            if (projectile != null)
            {
                projectile.transform.position = bulletSpawnPoint.position; // ใช้ตำแหน่งจุดยิง
                projectile.SetActive(true);

                // กำหนดความเร็ว
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed; // ตั้งค่าความเร็ว
                }
            }

            yield return new WaitForSeconds(shootCooldown); // รอคูลดาวน์ก่อนยิงอีกครั้ง
        }
    }

    private IEnumerator UseSkillPattern()
    {
        isUsingSkill = true;

        // เปิดอัมมะตะ
        isImmortal = true;

        // ใช้ทักษะ meteor สองครั้ง
        for (int i = 0; i < 2; i++)
        {
            // เสกอุกกาบาต
            for (int j = 0; j < meteorCount; j++)
            {
                SpawnMeteor();
                yield return new WaitForSeconds(attackCooldown);
            }

            // ยิงโปรเจคไทล์ในขณะที่ยิงอุกกาบาต
            StartCoroutine(ShootProjectilesRoutine());
        }

        isImmortal = false;
        isMoving = true;
        yield return new WaitForSeconds(6f);
        isMoving = false;

        isUsingSkill = false; // รีเซ็ตสถานะการใช้สกิล
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
        if (isImmortal)
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
    }

    private IEnumerator ShootProjectilesRoutine()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            // ตรวจสอบว่าบอสอยู่ในสถานะอมตะหรือไม่
            if (!isImmortal)
            {
                yield break; // ออกจาก routine หากอยู่ในสถานะอมตะ
            }

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

    public void TakeDamage(float damage)
    {
        if (isImmortal)
        {
            return;
        }

        currentHealth -= damage;

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
