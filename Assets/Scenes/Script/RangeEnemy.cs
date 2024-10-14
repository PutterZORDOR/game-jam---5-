using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    public GameObject Bullet;
    public Transform BulletPos;
    [SerializeField] private GameObject player;
    private float Timer;
    [SerializeField] private float ShootingTime;
    [SerializeField] private ObjectPool _op;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 1000000)
        {
            Timer += Time.deltaTime;
            if (Timer >= ShootingTime)
            {
                Shoot();
                Timer = 0;
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = _op.GetPooledObject();

        if (bullet != null) ;
        {
            bullet.transform.position = BulletPos.position;
            bullet.SetActive(true);
        }
    }
}