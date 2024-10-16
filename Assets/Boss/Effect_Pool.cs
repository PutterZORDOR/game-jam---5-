using UnityEngine;
using System.Collections.Generic;

public class Effect_Pool : MonoBehaviour
{
    public static Effect_Pool instance; // สร้าง Instance สำหรับ Singleton

    public GameObject explosionPrefab; // เอฟเฟกต์ระเบิด
    public int poolSize = 10; // ขนาดของ Object Pool
    private List<GameObject> explosionPool; // รายการเก็บเอฟเฟกต์ระเบิด

    private void Awake()
    {
        // ตรวจสอบว่า Instance มีอยู่แล้วหรือไม่
        if (instance == null)
        {
            instance = this; // กำหนด Instance ให้เป็น Singleton
            DontDestroyOnLoad(gameObject); // ทำให้ไม่ถูกทำลายเมื่อโหลดฉากใหม่
            InitializePool(); // เรียกใช้ฟังก์ชันเพื่อสร้าง Object Pool
        }
        else
        {
            Destroy(gameObject); // ทำลายอินสแตนซ์ใหม่
        }
    }

    private void InitializePool()
    {
        explosionPool = new List<GameObject>();

        // สร้างและเก็บเอฟเฟกต์ระเบิดใน Object Pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.SetActive(false);
            explosionPool.Add(explosion);
        }
    }

    public GameObject GetExplosion()
    {
        foreach (GameObject explosion in explosionPool)
        {
            if (!explosion.activeInHierarchy)
            {
                return explosion; // ส่งกลับเอฟเฟกต์ที่ไม่ทำงาน
            }
        }
        return null; // ไม่พบเอฟเฟกต์ที่ไม่ทำงาน
    }
}
