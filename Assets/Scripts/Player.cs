using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public int maxJumps = 2;
    public Rigidbody2D rb;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Respawn")]
    public Transform respawnPoint;
    public float respawnDelay = 0.5f;
    public float fallLimitY = -10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Wall Check")]
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public float wallRadius = 0.2f;
    public LayerMask wallLayer;
    public float wallSlideSpeed = 1.5f;

    [Header("Wall Jump")]
    public float wallJumpForceX = 6f;
    public float wallJumpForceY = 8f;

    [Header("Visual")]
    public SpriteRenderer visualRenderer;
    public Animator animator;

    [Header("Animation")]
    public float doubleJumpAnimTime = 0.25f;
    public float hitAnimTime = 0.35f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip damageSound;

    private float move;
    private float currentSpeed;
    private int jumpsLeft;

    private bool isRunning;
    private bool isGrounded;
    private bool touchingLeftWall;
    private bool touchingRightWall;
    private bool isWallSliding;
    private bool canWallJump;
    private bool isHit;
    private bool isDead;

    private float doubleJumpTimer;
    private float hitTimer;
    private string currentAnimation;

    private const string IDLE = "Frog_Idle";
    private const string RUN = "Frog_Run";
    private const string JUMP = "Frog_Jump";
    private const string FALL = "Frog_Fall";
    private const string DOUBLE_JUMP = "Frog_DoubleJump";
    private const string WALL_SLIDE = "Frog_WallJump";
    private const string HIT = "Frog_Hit";

    void Start()
    {
        jumpsLeft = maxJumps;
        currentSpeed = walkSpeed;

        currentHealth = maxHealth;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (HUDManager.instance != null)
            HUDManager.instance.UpdateHealth(currentHealth);
    }

    void Update()
    {
        if (isDead)
            return;

        ReadInput();
        CheckCollisions();
        HandleJump();
        HandleFlip();
        UpdateHit();
        UpdateAnimation();
        CheckFallDeath();

    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float yVelocity = isWallSliding ? -wallSlideSpeed : rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(move * currentSpeed, yVelocity);
    }

    void ReadInput()
    {
        move = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            move = -1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            move = 1f;

        isRunning = Keyboard.current.leftShiftKey.isPressed ||
                    Keyboard.current.rightShiftKey.isPressed;

        currentSpeed = isRunning ? runSpeed : walkSpeed;
    }

    void CheckCollisions()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        touchingLeftWall = Physics2D.OverlapCircle(
            wallCheckLeft.position,
            wallRadius,
            wallLayer
        );

        touchingRightWall = Physics2D.OverlapCircle(
            wallCheckRight.position,
            wallRadius,
            wallLayer
        );

        bool touchingWall = touchingLeftWall || touchingRightWall;

        isWallSliding = touchingWall && !isGrounded && rb.linearVelocity.y < 0f;

        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpsLeft = maxJumps;
            canWallJump = true;
            doubleJumpTimer = 0f;
        }

        if (isWallSliding)
        {
            canWallJump = true;
        }
    }

    void HandleJump()
    {
        if (!JumpPressed())
            return;

        if (isWallSliding && canWallJump)
        {
            WallJump();
            return;
        }

        if (jumpsLeft <= 0)
            return;

        bool isDoubleJump = !isGrounded && jumpsLeft == 1;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpsLeft--;

        PlaySound(jumpSound);

        if (isDoubleJump)
        {
            doubleJumpTimer = doubleJumpAnimTime;
            PlayAnimation(DOUBLE_JUMP);
        }
    }

    void WallJump()
    {
        canWallJump = false;
        isWallSliding = false;
        jumpsLeft = 0;
        doubleJumpTimer = 0f;

        float direction = touchingLeftWall ? 1f : -1f;

        rb.linearVelocity = new Vector2(
            direction * wallJumpForceX,
            wallJumpForceY
        );

        PlaySound(jumpSound);
        PlayAnimation(JUMP);
    }

    bool JumpPressed()
    {
        return Keyboard.current.spaceKey.wasPressedThisFrame ||
               Keyboard.current.wKey.wasPressedThisFrame ||
               Keyboard.current.upArrowKey.wasPressedThisFrame;
    }

    void HandleFlip()
    {
        if (isWallSliding || move == 0)
            return;

        visualRenderer.flipX = move < 0;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0 || isDead)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        if (HUDManager.instance != null)
            HUDManager.instance.UpdateHealth(currentHealth);

        isHit = true;
        hitTimer = hitAnimTime;

        PlaySound(damageSound);
        PlayAnimation(HIT);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        move = 0f;
        rb.linearVelocity = Vector2.zero;

        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }

        rb.linearVelocity = Vector2.zero;

        currentHealth = maxHealth;

        if (HUDManager.instance != null)
            HUDManager.instance.UpdateHealth(currentHealth);

        jumpsLeft = maxJumps;
        isDead = false;
        isHit = false;
        isWallSliding = false;
        canWallJump = true;
        doubleJumpTimer = 0f;
        hitTimer = 0f;

        if (visualRenderer != null)
            visualRenderer.flipX = false;

        PlayAnimation(IDLE);
    }

    void CheckFallDeath()
    {
        if (transform.position.y <= fallLimitY)
        {
            currentHealth = 0;

            if (HUDManager.instance != null)
                HUDManager.instance.UpdateHealth(currentHealth);

            Die();
        }
    }

    void UpdateHit()
    {
        if (!isHit)
            return;

        hitTimer -= Time.deltaTime;

        if (hitTimer <= 0f)
            isHit = false;
    }

    void UpdateAnimation()
    {
        if (isHit)
        {
            PlayAnimation(HIT);
            return;
        }

        if (doubleJumpTimer > 0f)
        {
            doubleJumpTimer -= Time.deltaTime;
            PlayAnimation(DOUBLE_JUMP);
            return;
        }

        if (isWallSliding)
        {
            PlayAnimation(WALL_SLIDE);
            return;
        }

        if (!isGrounded)
        {
            PlayAnimation(rb.linearVelocity.y > 0 ? JUMP : FALL);
            return;
        }

        PlayAnimation(move != 0 ? RUN : IDLE);
    }

    void PlayAnimation(string animationName)
    {
        if (currentAnimation == animationName)
            return;

        animator.Play(animationName);
        currentAnimation = animationName;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);

        if (wallCheckLeft != null)
            Gizmos.DrawWireSphere(wallCheckLeft.position, wallRadius);

        if (wallCheckRight != null)
            Gizmos.DrawWireSphere(wallCheckRight.position, wallRadius);
    }
}