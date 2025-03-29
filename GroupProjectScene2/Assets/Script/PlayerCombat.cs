using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking = false;
    public float attackCooldown = 0.5f; 

    // Add reference to PlayerController
    private PlayerController playerController;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.7f;
    public LayerMask enemyLayers;
    public int attackDamage = 15;

    public GameObject attackHitbox;

    public bool showAttackRange = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        if (attackPoint == null)
        {
            GameObject newAttackPoint = new GameObject("AttackPoint");
            newAttackPoint.transform.parent = transform;
            newAttackPoint.transform.localPosition = new Vector3(1f, 0, 0); 
            attackPoint = newAttackPoint.transform;
        }

        if (attackHitbox == null)
        {
            GameObject newHitbox = new GameObject("PlayerAttackHitbox");
            newHitbox.transform.parent = attackPoint;
            newHitbox.transform.localPosition = Vector3.zero;

            BoxCollider2D boxCollider = newHitbox.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(attackRange * 2f, 1.5f);
            boxCollider.isTrigger = true;
            boxCollider.enabled = false;

            AttackHitbox hitboxComponent = newHitbox.AddComponent<AttackHitbox>();
            hitboxComponent.damage = attackDamage;
            hitboxComponent.targetTag = "Enemy";
            hitboxComponent.autoDisable = true;
            hitboxComponent.activeTime = 0.2f;

            attackHitbox = newHitbox;
            attackHitbox.SetActive(false);
        }
        else
        {
            AttackHitbox hitboxComponent = attackHitbox.GetComponent<AttackHitbox>();
            if (hitboxComponent == null)
            {
                hitboxComponent = attackHitbox.AddComponent<AttackHitbox>();
            }

            hitboxComponent.damage = attackDamage;
            hitboxComponent.targetTag = "Enemy";

            Collider2D hitboxCollider = attackHitbox.GetComponent<Collider2D>();
            if (hitboxCollider == null)
            {
                BoxCollider2D boxCollider = attackHitbox.AddComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(attackRange * 2f, 1.5f);
                boxCollider.isTrigger = true;
            }

            attackHitbox.SetActive(false);
        }
    }

    void Update()
    {
        HandleAttack();
        UpdateAttackPointPosition();
    }

    void UpdateAttackPointPosition()
    {
        if (attackPoint != null)
        {
            float xDirection = transform.localScale.x > 0 ? 1f : -1f;
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x) * xDirection,
                                                   attackPoint.localPosition.y,
                                                   attackPoint.localPosition.z);
        }
    }

    void HandleAttack()
    {
        if (isAttacking) return; 

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartAttack("Attack1");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartAttack("Attack2");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            StartAttack("Attack3");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartAttack("Attack4");
        }
    }

    void StartAttack(string attackTrigger)
    {
        isAttacking = true; 
        Debug.Log($"Triggering: {attackTrigger}"); 

        // Fire the corresponding attack animation
        animator.SetTrigger(attackTrigger);
        animator.SetTrigger("isFighting");

        if (playerController != null)
            playerController.Attack();

        PerformAttack();

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void PerformAttack()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);

            AttackHitbox hitboxComponent = attackHitbox.GetComponent<AttackHitbox>();
            if (hitboxComponent != null)
            {
                hitboxComponent.Activate();
            }
        }
        else
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Hit " + enemy.name);

                DamageHandler enemyDamage = enemy.GetComponent<DamageHandler>();
                if (enemyDamage != null)
                {
                    enemyDamage.TakeDamage(attackDamage);
                }

                BossAI bossAI = enemy.GetComponent<BossAI>();
                if (bossAI != null)
                {
                    bossAI.TakeDamage(attackDamage);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showAttackRange && attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("Attack4");
        animator.ResetTrigger("isFighting");
    }

    public void TakeDamage()
    {
        animator.SetTrigger("Hurt"); 
    }
}