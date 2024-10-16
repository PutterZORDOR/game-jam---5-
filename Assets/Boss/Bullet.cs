using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifespan = 5f; // เวลาที่กระสุนจะอยู่ในโลกก่อนถูกทำลาย
    private Rigidbody2D rb; // ตัวแปรเก็บ Rigidbody2D
    private float timeElapsed; // ตัวแปรเก็บเวลาที่กระสุนอยู่ในโลก

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
            DestroySelf(); // เรียกฟังก์ชันทำลายตัวเอง
        }
    }

    private void OnEnable()
    {
        timeElapsed = 0f; // รีเซ็ตเวลาที่ผ่านไปเมื่อเปิดใช้งาน
    }
    private void DestroySelf()
    {
        gameObject.SetActive(false); // ปิดการใช้งาน GameObject แทนการทำลาย
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // เช็คว่าชนกับวัตถุใด
        if (other.CompareTag("Player")) // แท็กที่คุณใช้กับศัตรู
        {
            // ทำการโจมตีหรือทำลายศัตรูที่ถูกชน
            PlayerManager.instance.TakeDamgeAll(1);
            DestroySelf();
        }
    }
}
