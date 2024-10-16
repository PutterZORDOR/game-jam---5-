using System.Collections;
using UnityEngine;

public class LaserBeamY : MonoBehaviour
{
    public Transform firePoint; // จุดที่เลเซอร์เริ่มต้น
    public float laserGrowthSpeed = 5f; // ความเร็วในการยืดเลเซอร์
    public float laserShrinkSpeed = 5f; // ความเร็วในการย่อเลเซอร์
    public float laserMaxScale = 10f; // ขนาดสูงสุดของเลเซอร์
    public float laserDuration = 2f; // ระยะเวลาที่ยืดเลเซอร์
    public LayerMask hitLayer; // เลเยอร์ที่เลเซอร์สามารถชนได้

    private Vector3 initialScale; // ขนาดเริ่มต้นของเลเซอร์
    private Vector3 initialPosition; // ตำแหน่งเริ่มต้นของเลเซอร์
    private bool laserActive = true; // สถานะของเลเซอร์ว่าเปิดอยู่หรือไม่
    private bool shrinking = false; // สถานะของการย่อเลเซอร์

    void Start()
    {
        initialScale = transform.localScale; // เก็บขนาดเริ่มต้นของเลเซอร์
        initialPosition = transform.position; // เก็บตำแหน่งเริ่มต้นของเลเซอร์
        StartCoroutine(HandleLaserDuration()); // เริ่มนับเวลาของเลเซอร์
    }

    void Update()
    {
        if (laserActive)
        {
            ExtendLaser(); // ยืดเลเซอร์
        }
        else if (shrinking)
        {
            ShrinkLaser(); // ย่อเลเซอร์
        }
    }

    void ExtendLaser()
    {
        // เพิ่มค่า scale.y ของเลเซอร์ตามความเร็วที่กำหนด
        transform.localScale += new Vector3(0, laserGrowthSpeed * Time.deltaTime, 0);

        // ปรับตำแหน่งให้เลเซอร์อยู่ที่กลางของ firePoint
        transform.position = new Vector3(transform.position.x, initialPosition.y + (transform.localScale.y - initialScale.y) / 2, transform.position.z);

        // ตรวจสอบว่าขนาดเกินขนาดสูงสุดหรือไม่
        if (transform.localScale.y >= laserMaxScale)
        {
            laserActive = false; // หยุดการยืดเมื่อถึงขนาดสูงสุด
        }

        // ตรวจจับการชนระหว่างการยืดเลเซอร์
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, Vector2.up, transform.localScale.y, hitLayer);
        if (hit.collider != null)
        {
            // ถ้าเลเซอร์ชน Player ทำดาเมจ
            if (hit.collider.CompareTag("Player"))
            {
                PlayerManager player = hit.collider.GetComponent<PlayerManager>();
                if (player != null)
                {
                    player.TakeDamageHp(10); // ตัวอย่างทำดาเมจ
                }
            }

            // หยุดการยืดเมื่อชนกับวัตถุ
            laserActive = false;
        }
    }

    void ShrinkLaser()
    {
        // ค่อยๆ ลดค่า scale.y กลับไปที่ขนาดเริ่มต้น
        transform.localScale -= new Vector3(0, laserShrinkSpeed * Time.deltaTime, 0);

        // ปรับตำแหน่งของเลเซอร์ให้กลับไปที่ตำแหน่งเริ่มต้น
        transform.position = new Vector3(transform.position.x, initialPosition.y + (transform.localScale.y - initialScale.y) / 2, transform.position.z);

        // หยุดการย่อเลเซอร์เมื่อกลับไปเท่าขนาดเริ่มต้น
        if (transform.localScale.y <= initialScale.y)
        {
            transform.localScale = initialScale; // ตั้งให้ขนาดกลับไปที่ขนาดเริ่มต้นอย่างแม่นยำ
            shrinking = false; // หยุดการย่อเลเซอร์
        }
    }

    IEnumerator HandleLaserDuration()
    {
        // รอจนเวลาผ่านไปตามที่กำหนด
        yield return new WaitForSeconds(laserDuration);

        // เริ่มการย่อเลเซอร์เมื่อหมดเวลา
        shrinking = true;
    }
}
