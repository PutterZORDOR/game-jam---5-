using UnityEngine;

public class background : MonoBehaviour
{
    public Transform player; // ผู้เล่น หรือจะใช้กล้องแทนก็ได้
    public float followSpeed = 0.1f; // ความเร็วในการติดตาม ค่าที่ต่ำจะทำให้มันเคลื่อนที่ตามช้า ๆ
    public Vector2 offset = new Vector2(0, 0); // ตำแหน่ง offset เพื่อให้ background อยู่ในตำแหน่งที่เหมาะสม

    void Update() {
        // ให้ background ขยับตามตำแหน่งของผู้เล่นหรือกล้อง โดยคำนึงถึง offset
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        // การเลื่อน background ให้ตามผู้เล่นด้วยความเร็วที่เหมาะสม
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
    }
}
