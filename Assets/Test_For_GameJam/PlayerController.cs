using System.Collections;
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

    [Header("Player Damage")]
    public float damage;
    public float criticalHitChance = 0.25f; // โอกาสในการทำ Critical Hit (25%)
    public float criticalDamageMultiplier = 3f; // ตัวคูณความเสียหายเมื่อทำ Critical Hit

    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackBoxSize = new Vector2(1f, 0.5f);
    [SerializeField] private LayerMask enemyLayers;
    public float attackCooldown = 1f;
    private float attackCooldownTimer = 0f;

    [Header("Dash")]
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashCooldown;
    private bool canDash = true;
    private bool isDashing;

    private bool canDoubleJump;
    private bool isJumping = false;
    [SerializeField] bool isAttacking = false;
    private bool Digging;
    [SerializeField] private int indexAttack;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
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
            if (isDashing)
            {
              anim.SetBool("OnAir", IsWalled());
              return;
            }else if (Digging)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            if (isDashing || PlayerManager.instance.IsDie || Digging)
            {
                return;
            }

            horizontal = Input.GetAxisRaw("Horizontal");

            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(0) && !isAttacking && attackCooldownTimer <= 0f)
            {
                isAttacking = true;
                indexAttack = Random.Range(1, 4);
                anim.Play($"Attack {indexAttack}");
                Invoke(nameof(ResetAttackState), 0.3f);
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

            if (IsGrounded() && !IsJumping() && !isJumping && !isAttacking && !isDashing)
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

    private void FixedUpdate()
    {
        if (!isWallJumping && !isDashing)
        {
            if (!Digging)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
        }
    }

    public void DigDown()
    {
        gameObject.tag = "Untagged";
        Digging = true;
        rb.velocity = Vector2.zero;
        anim.Play("Cat_DigDown");
    }
    public void FinishedDigDown()
    {
        StartCoroutine(WaitForDigUp());
    }
    private IEnumerator WaitForDigUp()
    {
        yield return new WaitForSeconds(5f);
        DigUp();
    }
    public void DigUp()
    {
        anim.Play("Cat_DigUp");
    }
    public void FinishedDigUp()
    {
        gameObject.tag = "Player";
        Digging = false;
        PlayerManager.instance.Immune = false;
    }
    public void Dashing()
    {
        anim.Play("Cat_Dash");
        StartCoroutine(Dash());
    }
    public void Die()
    {
        PlayerManager.instance.Die();
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void Attack()
    {
        float finalDamage = damage * PlayerManager.instance.damgeMulti;
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackBoxSize, 0f, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.gameObject.tag == "Boss 1")
            {
                if (indexAttack < 3)
                {
                    if (Random.value < criticalHitChance)
                    {
                        finalDamage *= criticalDamageMultiplier; // ทำความเสียหายเพิ่มขึ้น
                    }
                    enemy.GetComponent<BossController>().ApplyDamage(finalDamage);
                }
                else
                {
                    enemy.GetComponent<BossController>().ApplyDamage(damage * PlayerManager.instance.damgeMulti);
                }
            }
            else if (enemy.gameObject.tag == "Boss 2")
            {
                if (indexAttack < 3)
                {
                    if (Random.value < criticalHitChance)
                    {
                        finalDamage *= criticalDamageMultiplier; // ทำความเสียหายเพิ่มขึ้น
                    }
                    enemy.GetComponent<Boss_Dragon>().TakeDamage(finalDamage);
                }
                else
                {
                    enemy.GetComponent<Boss_Dragon>().TakeDamage(finalDamage);
                }
            }
            else
            {
                if (indexAttack < 3)
                {
                    if (Random.value < criticalHitChance)
                    {
                        finalDamage *= criticalDamageMultiplier; // ทำความเสียหายเพิ่มขึ้น
                    }
                    enemy.GetComponent<Enemy>().TakeDamage(finalDamage);
                }
                else
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage * PlayerManager.instance.damgeMulti);
                }
            }
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

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide() {
        // ตรวจสอบว่าตัวละครกำลังอยู่บนกำแพงหรือไม่
        if (IsWalled() && !IsGrounded() && horizontal != 0f) {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        } else {
            // ถ้าไม่เจอ wall ให้รีเซ็ตสถานะ wall sliding
            isWallSliding = false;
        }
    }

    private void WallJump() {
        // ถ้ากำลังอยู่ในสถานะ wall sliding
        if (isWallSliding) {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x; // เปลี่ยนทิศทางกระโดด
            wallJumpingCounter = wallJumpingTime;

            // ยกเลิกการเรียกฟังก์ชันหยุดการ Wall Jump
            CancelInvoke(nameof(StopWallJumping));
        } else {
            wallJumpingCounter -= Time.deltaTime;
        }

        // ตรวจสอบว่าผู้เล่นกดปุ่มกระโดดและยังอยู่ในระยะเวลา wall jumping หรือไม่
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f) {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            // ถ้าเจอกำแพงแล้วทิศทางไม่ตรงกัน ให้หมุนตัวละคร
            if (transform.localScale.x != wallJumpingDirection) {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            anim.Play("Cat_Jump");

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }

        // ตรวจสอบว่าหากตัวละครไม่อยู่บนกำแพง (IsWalled()) และเจอ layer อื่น เช่น พื้น (ground)
        if (!IsWalled() && IsGrounded()) {
            // รีเซ็ตสถานะ Wall Jumping หากไม่เจอกำแพงแล้ว
            isWallJumping = false;
            isWallSliding = false;
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
