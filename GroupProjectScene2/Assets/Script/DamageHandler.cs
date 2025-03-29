using System.Collections;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Animator animator;
    private BossAI bossAI;
    private PlayerController playerController; 
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    [Header("UI")]
    public bool useHealthBar = true;
    public HealthBar healthBar; 

    [Header("Hit Effect")]
    public float flashDuration = 0.1f;
    public Color damageFlashColor = new Color(1f, 0.3f, 0.3f, 1f);
    private Color originalColor;
    private bool isFlashing = false;

    private bool isImmune = true;
    private float immunityDuration = 1.5f; 
    private float hitImmunityDuration = 0.5f; 
    public float damageReductionFactor = 1.0f;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        bossAI = GetComponent<BossAI>();
        playerController = GetComponent<PlayerController>();

        if (useHealthBar && healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            if (CompareTag("Player"))
            {
                GameObject playerHealthBar = GameObject.Find("PlayerHealthBar");
                if (playerHealthBar != null)
                {
                    healthBar = playerHealthBar.GetComponent<HealthBar>();
                    if (healthBar != null)
                    {
                        useHealthBar = true;
                        healthBar.SetMaxHealth(maxHealth);
                    }
                }
            }
            else if (CompareTag("Enemy"))
            {
                GameObject bossHealthBar = GameObject.Find("BossHealthBar");
                if (bossHealthBar != null)
                {
                    healthBar = bossHealthBar.GetComponent<HealthBar>();
                    if (healthBar != null)
                    {
                        useHealthBar = true;
                        healthBar.SetMaxHealth(maxHealth);
                    }
                }
            }
        }

        StartCoroutine(DisableInitialImmunity());
    }

    IEnumerator DisableInitialImmunity()
    {
        Debug.Log($"Initial immunity active for {gameObject.name}");
        yield return new WaitForSeconds(immunityDuration);
        isImmune = false;
        Debug.Log($"Initial immunity ended for {gameObject.name}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {
            Debug.Log($"{gameObject.name} detected hit from {other.gameObject.name}");

            int damage = 10; 

            AttackHitbox hitbox = other.GetComponent<AttackHitbox>();
            if (hitbox != null)
            {
                // Verify target tag matches
                if (!CompareTag(hitbox.targetTag))
                {
                    Debug.Log($"Attack ignored: target mismatch. Expected {hitbox.targetTag}, got {tag}");
                    return;
                }
                damage = hitbox.damage;
            }

            TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; 

        if (isImmune)
        {
            Debug.Log($"{gameObject.name} is immune, damage ignored");
            return;
        }

        isImmune = true;
        StartCoroutine(DisableHitImmunity());

        Debug.Log($"{gameObject.name} took {damage} damage!");

        if (bossAI != null)
        {
            damage = Mathf.RoundToInt(damage * damageReductionFactor);
        }

        currentHealth -= damage;

        currentHealth = Mathf.Max(0, currentHealth);

        if (useHealthBar && healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        StartCoroutine(FlashEffect());

        if (bossAI != null)
        {
            bossAI.TakeDamage(damage);
        }
        else if (playerController != null)
        {
            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DisableHitImmunity()
    {
        yield return new WaitForSeconds(hitImmunityDuration);
        isImmune = false;
    }

    System.Collections.IEnumerator FlashEffect()
    {
        if (isFlashing || spriteRenderer == null) yield break;

        isFlashing = true;

        spriteRenderer.color = damageFlashColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;

        isFlashing = false;
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (animator != null)
        {
            animator.ResetTrigger("Hurt");
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");

            animator.SetBool("isAlive", false);
            animator.SetTrigger("Die");

            Debug.Log("Death animation triggered");
        }

        if (bossAI != null)
        {
            bossAI.enabled = false;

            if (GameManager.instance != null)
            {
                StartCoroutine(DelayedSceneTransition(GameManager.instance.PlayerWins, 2f));
            }
        }
        else if (playerController != null)
        {
            playerController.enabled = false;

            if (GameManager.instance != null)
            {
                StartCoroutine(DelayedSceneTransition(GameManager.instance.GameOver, 2f));
            }
        }

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    IEnumerator DelayedSceneTransition(System.Action sceneTransition, float delay)
    {
        yield return new WaitForSeconds(delay);
        sceneTransition();
    }
}