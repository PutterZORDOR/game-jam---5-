using UnityEngine;
using UnityEngine.SceneManagement; // ใช้เพื่อจัดการการเปลี่ยน Scene
public class nextwaterup : MonoBehaviour
{
    public string sceneToLoad; // ชื่อของ Scene ที่จะโหลดเมื่อสัมผัสวัตถุ

    // ฟังก์ชันนี้จะถูกเรียกเมื่อมีการชนแบบ 2D (OnTriggerEnter2D)
    private void OnTriggerEnter2D(Collider2D other) {
        // ตรวจสอบว่าผู้ที่ชนเป็นผู้เล่นหรือไม่ (แท็กที่ใช้กับผู้เล่น)
        if (other.CompareTag("Player")) {
            // เปลี่ยน Scene ไปยัง Scene ที่กำหนดไว้
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
