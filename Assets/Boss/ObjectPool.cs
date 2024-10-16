using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Projectile Prefabs")]
    public GameObject meteorPrefab;
    public GameObject bombPrefab;
    public GameObject bulletPrefab;

    [Header("Pool Settings")]
    public int poolSize = 15; // ขนาดของ pool
    public GameObject[][] projectilePools; // array สำหรับเก็บ pool ของแต่ละประเภท

    public static ObjectPool instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        projectilePools = new GameObject[3][]; // เตรียม 3 Pool สำหรับ Meteor, Bomb และ Bullet

        // สร้าง pool สำหรับ Meteor
        projectilePools[0] = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            projectilePools[0][i] = Instantiate(meteorPrefab, transform.position, Quaternion.identity);
            projectilePools[0][i].SetActive(false);
        }

        // สร้าง pool สำหรับ Bomb
        projectilePools[1] = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            projectilePools[1][i] = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            projectilePools[1][i].SetActive(false);
        }

        // สร้าง pool สำหรับ Bullet
        projectilePools[2] = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            projectilePools[2][i] = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            projectilePools[2][i].SetActive(false);
        }
    }

    // ฟังก์ชันสำหรับดึงวัตถุจาก Pool ตามประเภท
    public GameObject GetProjectile(int type)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!projectilePools[type][i].activeInHierarchy)
            {
                return projectilePools[type][i]; // ถ้าพบวัตถุที่ไม่ได้ถูกใช้งาน ให้คืนค่า
            }
        }

        return null; // ถ้าไม่มีวัตถุที่พร้อมใช้งาน
    }

    // ฟังก์ชันคืนวัตถุกลับ Pool
    public void ReturnProjectile(GameObject projectile, int type)
    {
        projectile.SetActive(false);
    }
}
