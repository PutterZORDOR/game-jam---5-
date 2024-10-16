using System.Collections;
using UnityEngine;

public class LaserBeamY : MonoBehaviour
{
    public Transform firePoint;
    public float laserGrowthSpeed = 5f;
    public float laserShrinkSpeed = 5f;
    public float laserMaxScale = 10f;
    public float laserDuration = 2f;
    public LayerMask hitLayer;

    private Vector3 initialScale;
    private Vector3 initialPosition;
    private bool laserActive = false; // ตั้งค่าเริ่มต้นเป็น false
    private bool shrinking = false;

    public GameObject pre_laser; // เลเซอร์ที่จะโชว์ก่อน
    private void Start()
    {
        initialScale = transform.localScale; // ตั้งค่าขนาดเริ่มต้นใน Start()
        initialPosition = transform.position; // ตั้งค่าตำแหน่งเริ่มต้น
    }

    public void FireLaser()
    {
        pre_laser.SetActive(true);

        StartCoroutine(HandleLaserPreparation());
    }

    private IEnumerator HandleLaserPreparation()
    {
        yield return new WaitForSeconds(1.2f);

        pre_laser.SetActive(false);

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
            ShrinkLaser();
        }
    }

    void ExtendLaser()
    {
        transform.localScale += new Vector3(0, laserGrowthSpeed * Time.deltaTime, 0);

        transform.position = new Vector3(transform.position.x, initialPosition.y + (transform.localScale.y - initialScale.y) / 2, transform.position.z);

        if (transform.localScale.y >= laserMaxScale)
        {
            laserActive = false;
        }

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.up, transform.localScale.y, hitLayer);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerManager player = hit.collider.GetComponent<PlayerManager>();
                if (player != null)
                {
                    player.TakeDamageHp(10);
                }
            }

            laserActive = false;
        }
    }

    void ShrinkLaser()
    {
        transform.localScale -= new Vector3(0, laserShrinkSpeed * Time.deltaTime, 0);

        transform.position = new Vector3(transform.position.x, initialPosition.y + (transform.localScale.y - initialScale.y) / 2, transform.position.z);

        if (transform.localScale.y <= initialScale.y)
        {
            transform.localScale = initialScale;
            shrinking = false;
        }
    }

    IEnumerator HandleLaserDuration()
    {
        yield return new WaitForSeconds(laserDuration);

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
