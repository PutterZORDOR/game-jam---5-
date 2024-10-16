using Unity.VisualScripting;
using UnityEngine;

public class RangeEnemy : Enemy
{
    public GameObject Bullet;
    public Transform BulletPos;
    [SerializeField] private GameObject player;
    private float Timer;
    [SerializeField] private float ShootingTime;
    [SerializeField] private ObjectPools _op;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        // Calculate the horizontal distance (x-axis) in a side-scroller
        float distance = Mathf.Abs(transform.position.x - player.transform.position.x);

        // Check if player is within shooting range (you can adjust the range value)
        if (distance < 10.0f)
        {
            Timer += Time.deltaTime;
            if (Timer >= ShootingTime)
            {
                // Flip the enemy to face the player direction
                FlipEnemy();

                Shoot();
                Timer = 0;
            }
        }
    }

    void FlipEnemy()
    {
        // Flip the enemy's direction based on the player's position
        Vector3 scale = transform.localScale;
        scale.x = player.transform.position.x > transform.position.x ? 1 : -1;
        transform.localScale = scale;
    }

    void Shoot()
    {
        GameObject bullet = _op.GetPooledObject();

        if (bullet != null)
        {
            bullet.transform.position = BulletPos.position;
            bullet.SetActive(true);
        }
    }
}
