using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //public static ObjectPool instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] private GameObject BulletPre;
    private int amountToPool = 6;

    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    // }

    void Start()
    {
       for (int i = 0; i < amountToPool; i++)
       {
            GameObject obj = Instantiate(BulletPre);
            obj.SetActive(false);
            pooledObjects.Add(obj);
       }
    }
    public GameObject GetPooledObject()
    {
         for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
