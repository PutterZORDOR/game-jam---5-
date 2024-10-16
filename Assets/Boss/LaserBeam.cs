using System.Collections;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public Transform firePoint; // จุดที่เลเซอร์เริ่มต้น
    public float laserGrowthSpeed = 5f; // ความเร็วในการยืดเลเซอร์
    public float laserShrinkSpeed = 5f; // ความเร็วในการย่อเลเซอร์
    public float laserMaxScale = 10f; // ขนาดสูงสุดของเลเซอร์
    public float laserDuration = 2f; // ระยะเวลาที่ยืดเลเซอร์
    public LayerMask hitLayer; // เลเยอร์ที่เลเซอร์สามารถชนได้

    private Vector3 initialScale; // ขนาดเริ่มต้นของเลเซอร์
    private Vector3 initialPosition; // ตำแหน่งเริ่มต้นของเลเซอร์
    private bool laserActive = false; // สถานะของเลเซอร์ว่าเปิดอยู่หรือไม่
    private bool shrinking = false; // สถานะของการย่อเลเซอร์

    public GameObject pre_laser; // เลเซอร์ที่จะโชว์ก่อน
    public GameObject actualLaser; // เลเซอร์จริงที่ยิง

    public void FireLaser()
    {
        // แสดง pre_laser
        pre_laser.SetActive(true);
        actualLaser.SetActive(false); // ซ่อนเลเซอร์จริงก่อน

        StartCoroutine(HandleLaserPreparation());
    }

    private IEnumerator HandleLaserPreparation()
    {
        // รอ 1.2 วินาที
        yield return new WaitForSeconds(1.2f);

        // ซ่อน pre_laser และแสดงเลเซอร์จริง
        pre_laser.SetActive(false);
        actualLaser.SetActive(true);

        laserActive = true;
        shrinking = false;
        transform.localScale = initialScale;
        transform.position = initialPosition;

        StartCoroutine(HandleLaserDuration());
    }

    void Update()
    {
        if (laserActive)
        {
            ExtendLaser();
        }
        else if (shrinking)
        {
            ShrinkLaser(); // ย่อเลเซอร์ค่อยๆ กลับไปที่ขนาดเดิม
        }
    }

    void ExtendLaser()
    {
        // เพิ่มค่า scale.x ของเลเซอร์ทีละน้อยตามความเร็วที่กำหนด
        transform.localScale += new Vector3(laserGrowthSpeed * Time.deltaTime, 0, 0);

        // ปรับตำแหน่งของเลเซอร์ให้ปลายขยายไปทางขวา
        transform.position = new Vector3(initialPosition.x + (transform.localScale.x - initialScale.x) / 2, transform.position.y, transform.position.z);

        // ตรวจสอบว่าขนาดเกินขนาดสูงสุดหรือไม่
        if (transform.localScale.x >= laserMaxScale)
        {
            laserActive = false; // หยุดการยืดเมื่อถึงขนาดสูงสุด
        }

        // ตรวจจับการชนระหว่างการยืดเลเซอร์
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, transform.localScale.x, hitLayer);
        if (hit.collider != null)
        {
            // ถ้าเลเซอร์ชน Player ทำดาเมจ
            if (hit.collider.CompareTag("Player"))
            {
                PlayerManager.instance.TakeDamgeAll(1);
            }

            // หยุดการยืดเมื่อชนกับวัตถุ
            laserActive = false;
        }
    }

    void ShrinkLaser()
    {
        // ค่อยๆ ลดค่า scale.x กลับไปที่ขนาดเริ่มต้น
        transform.localScale -= new Vector3(laserShrinkSpeed * Time.deltaTime, 0, 0);

        // ปรับตำแหน่งของเลเซอร์ให้กลับไปที่ตำแหน่งเริ่มต้น
        transform.position = new Vector3(initialPosition.x + (transform.localScale.x - initialScale.x) / 2, transform.position.y, transform.position.z);

        // หยุดการย่อเลเซอร์เมื่อกลับไปเท่าขนาดเริ่มต้น
        if (transform.localScale.x <= initialScale.x)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerManager.instance.TakeDamgeAll(1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerManager.instance.TakeDamgeAll(1);
        }
    }
}
