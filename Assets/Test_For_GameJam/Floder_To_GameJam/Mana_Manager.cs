using UnityEngine;

public class Mana_Manager : MonoBehaviour
{
    public GameObject Heart;
    public GameObject[] ManaPool = new GameObject[15];
    public static Mana_Manager instance;
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
        for (int i = 0; i < ManaPool.Length; i++)
        {
            ManaPool[i] = Instantiate(Heart,transform.position,Quaternion.identity);
            ManaPool[i].SetActive(false);
        }
    }
}
