using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;

    private bool isWallSliding;
    public float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] Animator anim;

    [SerializeField] private Transform attackPoint; // ตำแหน่งที่จะตรวจสอบการปะทะของการโจมตี
    [SerializeField] private Vector2 attackBoxSize = new Vector2(1f, 0.5f); // ขนาดของกล่องโจมตี
    [SerializeField] private LayerMask enemyLayers; // เลเยอร์ของศัตรูที่ต้องการตรวจสอบการปะทะ
    public float attackCooldown = 1f; // คูลดาวน์โจมตี 1 วินาที
    private float attackCooldownTimer = 0f;

    private bool canDoubleJump;
    private bool isJumping = false;
    [SerializeField] bool isAttacking = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void ResetJumpingState()
    {
        isJumping = false;
    }
    private void ResetAttackState()
    {
        Debug.Log("Reset Attack State");
        isAttacking = false;
        if (IsJumping())
        {
            anim.SetBool("OnAir", true);
        }

    }
    private void Update()
    {
        if (!MenuManager.instance.isPaused)
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }
            if (Input.GetMouseButtonDown(0) && !isAttacking && attackCooldownTimer <= 0f)
            {
                isAttacking = true;
                anim.Play($"Attack {Random.Range(1,3)}");
                Invoke(nameof(ResetAttackState), 0.4f);
                attackCooldownTimer = attackCooldown;
            }


            if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded())
                {
                    isJumping = true;
                    anim.Play("Cat_Jump");
                    Invoke(nameof(ResetJumpingState), 1f);
                    rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                    canDoubleJump = true;
                }
                else if (canDoubleJump && PlayerManager.instance.Ability_DoubleJump)
                {
                    isJumping = true;
                    anim.Play("Cat_DoubleJump");
                    Invoke(nameof(ResetJumpingState), 1f);
                    rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                    canDoubleJump = false;
                }
            }

            if (IsGrounded() && !IsJumping() && !isJumping && !isAttacking)
            {
                anim.SetBool("OnAir", false);
                if (horizontal != 0)
                {
                    anim.Play("Cat_Walk");
                }
                else
                {
                    anim.Play("Cat_Idel");
                }
            }

            WallSlide();
            WallJump();

            if (!isWallJumping)
            {
                Flip();
            }
        }
    }
    private void Attack()
    {
        Debug.Log("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackBoxSize, 0f, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            //enemy.GetComponent<Enemy>().TakeDamage(10);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackBoxSize);
    }
    private bool IsJumping()
    {
        return !IsGrounded() && rb.velocity.y != 0;
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            anim.Play("Cat_Jump");

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}