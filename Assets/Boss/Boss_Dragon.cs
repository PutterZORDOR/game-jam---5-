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
    private SpriteRenderer spriteRenderer;

    private bool isShootingLaser = false; // ตัวแปรเช็คสถานะการยิงเลเซอร์

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        else if (!isShootingLaser)
        {
            StartCoroutine(StopMovementForDuration(3f));
            StartCoroutine(UseMeteorAndShootProjectilesRoutine());
        }
    }
    private IEnumerator StopMovementForDuration(float duration)
    {
        isMoving = false;  // หยุดการเคลื่อนไหว
        yield return new WaitForSeconds(duration); // รอเวลาที่กำหนด
        isMoving = true;  // กลับมาเคลื่อนไหวอีกครั้ง
    }
    private IEnumerator UseMeteorAndShootProjectilesRoutine()
    {
        yield return StartCoroutine(UseSkillPattern());
        ExecuteLaserAttackPattern();
    }
    private void ExecuteLaserAttackPattern()
    {

        if (!isShootingLaser)
        {
            // เริ่มยิงเลเซอร์
            isShootingLaser = true;

            // ยิงเลเซอร์ที่ 1 และ 3 ของ lazerX
            ShootLaserX(0);
            ShootLaserX(2);

            // ใช้เวลาสั้น ๆ ก่อนยิงต่อไป
            StartCoroutine(WaitThenShootAgain());
        }
    }

    private IEnumerator WaitThenShootAgain()
    {
        yield return new WaitForSeconds(3.2f); // รอระยะเวลาสั้น ๆ

        // ยิงเลเซอร์ที่ 2 ของ lazerX และเลเซอร์ที่ 3 ของ lazerY
        ShootLaserX(1);
        ShootLaserY(2);

        // รอระยะเวลาสั้น ๆ ก่อนยิงต่อไป
        yield return new WaitForSeconds(3.2f);

        // ยิงเลเซอร์ที่ 1 และ 3 อีกครั้ง
        ShootLaserX(0);
        ShootLaserX(2);
        ShootLaserY(1);

        yield return StartCoroutine(NewAttackPattern());
    }

    private void ShootLaserX(int index)
    {
        if (index >= 0 && index < lazerX.Length)
        {
            GameObject laser = lazerX[index];
            laser.SetActive(true);
            LaserBeam laserBeam = laser.GetComponent<LaserBeam>();

            if (laserBeam != null)
            {
                laserBeam.FireLaser();
            }
        }
    }

    private void ShootLaserY(int index)
    {
        if (index >= 0 && index < lazerY.Length)
        {
            GameObject laser = lazerY[index];
            laser.SetActive(true);
            LaserBeamY laserBeam = laser.GetComponent<LaserBeamY>();

            if (laserBeam != null)
            {
                laserBeam.FireLaser();
            }
        }
    }

    public void ChangeState()
    {
        moveSpeed = moveSpeed * 2.3f;
        changeState = true;

        StartCoroutine(NewAttackPattern());
    }
    private IEnumerator NewAttackPattern()
    {
        // ยิงเลเซอร์ในแกน Y
        yield return StartCoroutine(ShootLaserY()); // เพิ่มการรอการทำงานของ Coroutine

        // ยิงเลเซอร์ในแกน X
        yield return StartCoroutine(ShootLaserX()); // เพิ่มการรอการทำงานของ Coroutine

        isShootingLaser = false;
    }

    private IEnumerator ShootLaserX()
    {
        Debug.Log("เริ่มยิงเลเซอร์ X"); // ตรวจสอบว่าฟังก์ชันถูกเรียก
                                        // ค้นหาจุดยิงเลเซอร์จาก lazerX
        foreach (GameObject laser in lazerX)
        {
            laser.SetActive(true); // เปิดใช้งานเลเซอร์
            LaserBeam laserBeam = laser.GetComponent<LaserBeam>(); // ดึงคอมโพเนนต์ LaserBeam

            if (laserBeam != null)
            {
                laserBeam.FireLaser(); // เรียกใช้เมธอดยิงเลเซอร์
                Debug.Log("ยิงเลเซอร์ X: " + laser.name); // ตรวจสอบเลเซอร์ที่ถูกยิง
            }
            else
            {
                Debug.LogError("ไม่พบ LaserBeam ใน: " + laser.name);
            }

            // รอให้ยิงเสร็จ
            yield return new WaitForSeconds(2.7f); // รอระยะเวลาระหว่างการยิง
        }
    }

    private IEnumerator ShootLaserY()
    {
        Debug.Log("เริ่มยิงเลเซอร์ Y"); // ตรวจสอบว่าฟังก์ชันถูกเรียก
                                        // ค้นหาจุดยิงเลเซอร์จาก lazerY
        foreach (GameObject laser in lazerY)
        {
            laser.SetActive(true); // เปิดใช้งานเลเซอร์
            LaserBeamY laserBeam = laser.GetComponent<LaserBeamY>(); // ดึงคอมโพเนนต์ LaserBeam

            if (laserBeam != null)
            {
                laserBeam.FireLaser(); // เรียกใช้เมธอดยิงเลเซอร์
                Debug.Log("ยิงเลเซอร์ Y: " + laser.name); // ตรวจสอบเลเซอร์ที่ถูกยิง
            }
            else
            {
                Debug.LogError("ไม่พบ LaserBeam ใน: " + laser.name);
            }

            // รอให้ยิงเสร็จ
            yield return new WaitForSeconds(2.7f); // รอระยะเวลาระหว่างการยิง
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
        StartCoroutine(FlashOnDamage());
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private IEnumerator FlashOnDamage()
    {
        Color originalColor = spriteRenderer.color; // บันทึกสีเดิม
        Color flashColor = Color.red; // สีที่จะใช้เมื่อถูกโจมตี

        // กระพิบ 2 ครั้ง
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = flashColor; // เปลี่ยนเป็นสีแดง
            yield return new WaitForSeconds(0.1f); // รอ 0.1 วินาที
            spriteRenderer.color = originalColor; // เปลี่ยนกลับเป็นสีเดิม
            yield return new WaitForSeconds(0.1f); // รอ 0.1 วินาที
        }
    }

    private void Die()
    {
        Debug.Log("Boss has died!");
        Destroy(gameObject);
    }
}
