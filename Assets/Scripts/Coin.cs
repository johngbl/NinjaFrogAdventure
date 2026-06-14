using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public string collectAnimationName = "Coin_Collect";
    public float destroyDelay = 0.35f;

    [Header("Audio")]
    public AudioSource audioSource;

    private Collider2D coinCollider;
    private SpriteRenderer spriteRenderer;
    private bool collected;

    void Awake()
    {
        coinCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected)
            return;

        if (!collision.CompareTag("Player"))
            return;

        collected = true;

        if (coinCollider != null)
            coinCollider.enabled = false;

        if (HUDManager.instance != null)
            HUDManager.instance.AddCoin();

        if (animator != null)
            animator.Play(collectAnimationName);

        if (audioSource != null)
            audioSource.Play();

        Destroy(gameObject, destroyDelay);
    }
}