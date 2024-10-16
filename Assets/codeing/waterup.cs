using UnityEngine;

public class waterup : MonoBehaviour
{
    public Transform targetPoint; // จุดเป้าหมายที่บล็อกจะหยุดเมื่อถึง
    public float speed = 2f; // ความเร็วในการเคลื่อนที่
    public Transform player; // ตำแหน่งของผู้เล่น

    private bool isMoving = true; // ตรวจสอบว่าบล็อกกำลังเคลื่อนที่อยู่หรือไม่

    void Update() {
        // ตรวจสอบว่าบล็อกยังคงเคลื่อนไหวอยู่
        if (isMoving) {
            // ขยับบล็อกเข้าหาจุดเป้าหมายด้วยความเร็วที่กำหนด
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

            // ตรวจสอบว่าบล็อกถึงจุดเป้าหมายหรือยัง
            if (Vector2.Distance(transform.position, targetPoint.position) < 0.01f) {
                // หยุดบล็อกเมื่อถึงจุดที่กำหนด
                isMoving = false;
                Debug.Log("Block has reached the target point and stopped.");
            }
        }
    }

    // ตรวจจับการชนระหว่างบล็อกและผู้เล่น
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // หยุดบล็อกเมื่อชนกับผู้เล่น
            isMoving = false;
            Debug.Log("Block has collided with the player and stopped.");
            Time.timeScale = 0f; // หยุดการทำงานของเกม
        }
    }
}