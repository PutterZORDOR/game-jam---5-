using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Projectile : MonoBehaviour {
    private GameObject player;
    private Rigidbody2D rb;
    public float Force;
    [SerializeField]private int damage;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    void OnEnable()
    {
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * Force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0,rot);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //player.GetComponent<Health>().TakeDamage(damage);//Player Damaged
            gameObject.SetActive(false);
        }

        else if(collision.CompareTag("Wall")|| collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }

    }
}