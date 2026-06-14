using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Patrol")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    [Header("Damage")]
    public int damage = 1;
    public float damageCooldown = 1f;

    [Header("Stomp")]
    public float stompBounceForce = 6f;
    public float stompCheckHeight = 0.2f;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;

    [Header("Death Effect")]
    public GameObject deathEffect;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip deathSound;

    private Transform targetPoint;
    private float startY;
    private float damageTimer;
    private bool isDead;

    void Start()
    {
        targetPoint = pointB;
        startY = transform.position.y;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead)
            return;

        Patrol();

        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    void Patrol()
    {
        if (pointA == null || pointB == null)
            return;

        float targetX = targetPoint.position.x;
        Vector2 targetPosition = new Vector2(targetX, startY);

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
            Flip();
        }
    }

    void Flip()
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = targetPoint.position.x < transform.position.x;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (!collision.CompareTag("Player"))
            return;

        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
        Player player = collision.GetComponent<Player>();

        if (playerRb == null || player == null)
            return;

        bool playerIsAbove = collision.transform.position.y > transform.position.y + stompCheckHeight;
        bool playerIsFalling = playerRb.linearVelocity.y < 0;

        if (playerIsAbove && playerIsFalling)
        {
            Die();

            playerRb.linearVelocity = new Vector2(
                playerRb.linearVelocity.x,
                stompBounceForce
            );

            return;
        }

        DamagePlayer(player);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (!collision.CompareTag("Player"))
            return;

        Player player = collision.GetComponent<Player>();

        if (player != null)
            DamagePlayer(player);
    }

    void DamagePlayer(Player player)
    {
        if (damageTimer > 0)
            return;

        player.TakeDamage(damage);
        damageTimer = damageCooldown;

        PlaySound(attackSound);
    }

    void Die()
    {
        isDead = true;

        Collider2D enemyCollider = GetComponent<Collider2D>();

        if (enemyCollider != null)
            enemyCollider.enabled = false;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        PlaySound(deathSound);

        Destroy(gameObject, 0.4f);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}