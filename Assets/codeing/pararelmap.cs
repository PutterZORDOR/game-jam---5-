using UnityEngine;

public class pararelmap : MonoBehaviour
{
    public GameObject cam; // กล้องหลัก
    public float parallaxEffect; // ความเร็วของ parallax effect
    private float length, startPos; // เก็บตำแหน่งเริ่มต้นและความยาวของ sprite

    void Start() {
        // บันทึกตำแหน่งเริ่มต้นและความยาวของ sprite ที่ใช้เป็นพื้นหลัง
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x; // ความยาวของ sprite
    }

    void Update() {
        // คำนวณการเคลื่อนที่ของพื้นหลังตาม parallax effect
        float distance = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // ตรวจสอบการวนซ้ำของพื้นหลัง เมื่อกล้องเคลื่อนที่เกินระยะของ sprite
        float cameraOffset = cam.transform.position.x * (1 - parallaxEffect);

        // ถ้าเลื่อนกล้องไปทางขวาเกินขอบของ sprite ก็รีเซ็ตตำแหน่ง startPos
        if (cameraOffset > startPos + length) {
            startPos += length;
        } else if (cameraOffset < startPos - length) {
            startPos -= length;
        }
    }
}