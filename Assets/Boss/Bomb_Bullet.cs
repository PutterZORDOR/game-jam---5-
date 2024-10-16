using UnityEngine;

public class Bomb_Bullet : MonoBehaviour
{
    public float minLifespan = 3f; // เวลาต่ำสุดที่กระสุนจะอยู่ในโลก
    public float maxLifespan = 5f; // เวลาสูงสุดที่กระสุนจะอยู่ในโลก
    public float damageRadius = 2f; // รัศมีในการทำดาเมจ
    public int damageAmount = 1; // จำนวนดาเมจที่ทำ
    private float lifespan; // เวลาที่กระสุนจะอยู่ในโลกก่อนถูกทำลาย
    private float timeElapsed; // ตัวแปรเก็บเวลาที่กระสุนอยู่ในโลก
    private Rigidbody2D rb; // ตัวแปรเก็บ Rigidbody2D

    private void Start()
    {
        timeElapsed = 0f; // เริ่มต้นเวลาที่ผ่านไปเป็น 0
    }

    private void Update()
    {
        // เพิ่มเวลาที่ผ่านไปในทุกเฟรม
        timeElapsed += Time.deltaTime;

        // ตรวจสอบว่าเวลาที่ผ่านไปมากกว่าหรือเท่ากับ lifespan หรือไม่
        if (timeElapsed >= lifespan)
        {
            Explode(); // เรียกฟังก์ชันระเบิด
        }
    }

    private void OnEnable()
    {
        lifespan = Random.Range(minLifespan, maxLifespan); // สุ่มเวลาระเบิดระหว่าง minLifespan ถึง maxLifespan
        timeElapsed = 0f; // รีเซ็ตเวลาที่ผ่านไปเมื่อเปิดใช้งาน
    }

    private void Explode()
    {
        // ทำดาเมจให้กับศัตรูในรัศมีที่กำหนด
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Player")) // เช็คว่าเป็นศัตรูหรือไม่
            {
                PlayerManager.instance.TakeDamgeAll(damageAmount); // ทำดาเมจให้ศัตรู
            }
        }
        GameObject explosionEffect = Effect_Pool.instance.GetExplosion();
        if (explosionEffect != null)
        {
            explosionEffect.transform.position = transform.position;
            explosionEffect.SetActive(true);
        }

        DestroySelf(); // ปิดการใช้งาน GameObject แทนการทำลาย
    }

    private void DestroySelf()
    {
        gameObject.SetActive(false); // ปิดการใช้งาน GameObject แทนการทำลาย
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Explode();
        }
    }
    private void OnDrawGizmos()
    {
        // กำหนดสีของ Gizmos
        Gizmos.color = Color.red;

        // วาดวงกลมแสดงรัศมีในการทำดาเมจ
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
