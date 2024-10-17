using UnityEngine;

public class playersound : MonoBehaviour
{
    public Animator animator; // เชื่อมต่อกับ Animator ที่ใช้เล่นแอนิเมชัน
    public float attackCooldown = 0.5f; // ระยะเวลาที่สามารถโจมตีได้ครั้งถัดไป
    private float nextAttackTime = 0f;

    void Update() {
        // ตรวจสอบว่าผู้เล่นกดปุ่มโจมตีและมีเวลาพอที่จะโจมตีอีกครั้งได้
        if (Time.time >= nextAttackTime && Input.GetButtonDown("Fire1")) {
            Attack(); // เรียกใช้ฟังก์ชันการโจมตี
            nextAttackTime = Time.time + attackCooldown; // ตั้งค่า cooldown สำหรับการโจมตีครั้งถัดไป
        }
    }

    void Attack() {
        // เรียกใช้งานแอนิเมชันโจมตี
        animator.SetTrigger("Attack");

        // เล่นเสียงโจมตีโดยใช้ AudioManager
        AudioManager.instance.PlaySFX("Attack_Sound");
    }
}
