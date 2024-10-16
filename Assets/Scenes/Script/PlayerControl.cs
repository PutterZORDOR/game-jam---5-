using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D theRB;
    [SerializeField] float moveSpeed, jumpPower;
    public bool jumpready;
    public float jumpcd = 2.0f;
    public float jumped = 0.0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (jumped >= jumpcd)
        {
            jumpready = true;
        }
        else
        {
            jumped = jumped + Time.deltaTime;
            jumpready = false;
        }
        theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);

        if (Input.GetKeyDown(KeyCode.W) && jumpready)
        {
            if (Mathf.Abs(theRB.velocity.y) > 0.1f)
            {
                //anim.SetTrigger("jump");
            }

            theRB.velocity = new Vector2(theRB.velocity.x, jumpPower);
            jumped = 0.0f;
            jumpready = false;
        }

        //flip character when moving sideways
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            transform.localScale = Vector2.one;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }

    }
}
