using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    public Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private DamageHandler damageHandler;

    public float hoverSpeed = 2f;
    public float hoverHeight = 0.5f;
    public float moveSpeed = 20f; 
    public float retreatSpeed = 10f; 
    public float retreatDistance = 3f;
    public float minDistanceToPlayer = 1.5f;
    public float approachSpeed = 200f; 
    public float lungeForce = 25f; 

    public float attackRange = 8f; 
    public float attackCooldown = 0.2f;
    private float nextAttackTime = 0f;
    public float aggressiveness = 0.9f; 
    public float lungeChance = 0.6f; 

    private float originalY;
    private bool isAttacking = false;
    private bool isRetreating = false;
    private bool isTakingDamage = false;
    private bool isLunging = false;
    private Vector3 retreatTarget;
    private float lastMoveDecisionTime = 0f;
    private float moveDecisionInterval = 0.1f; 

    public GameObject attackHitbox;
    public int attackDamage = 20;

    public float knockbackForce = 5f;
    public float knockbackDuration = 0.3f; 

    public bool showDebugInfo = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageHandler = GetComponent<DamageHandler>();
        originalY = transform.position.y;
        rb.gravityScale = 0; 

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.mass = 2.0f;

        SetupAttackHitbox();

        nextAttackTime = Time.time;
    }

    void SetupAttackHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);

            attackHitbox.transform.localPosition = new Vector3(1f, 0f, 0f);

            AttackHitbox hitboxComponent = attackHitbox.GetComponent<AttackHitbox>();
            if (hitboxComponent == null)
            {
                hitboxComponent = attackHitbox.AddComponent<AttackHitbox>();
            }

            hitboxComponent.damage = attackDamage;
            hitboxComponent.targetTag = "Player";

            Collider2D hitboxCollider = attackHitbox.GetComponent<Collider2D>();
            if (hitboxCollider == null)
            {
                BoxCollider2D boxCollider = attackHitbox.AddComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(1.5f, 1.5f);
                boxCollider.isTrigger = true;
            }
        }
        else
        {
            Debug.LogWarning("Attack hitbox not assigned to Boss AI! Creating one...");

            GameObject newHitbox = new GameObject("BossAttackHitbox");
            newHitbox.transform.parent = transform;
            newHitbox.transform.localPosition = new Vector3(1f, 0f, 0f);

            BoxCollider2D boxCollider = newHitbox.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(1.5f, 1.5f);
            boxCollider.isTrigger = true;

            AttackHitbox hitboxComponent = newHitbox.AddComponent<AttackHitbox>();
            hitboxComponent.damage = attackDamage;
            hitboxComponent.targetTag = "Player";

            attackHitbox = newHitbox;
            attackHitbox.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference not set in Boss AI!");
            return;
        }

        if (isTakingDamage)
            return; 

        Hover();

        UpdateFacingDirection();
        PositionAttackHitbox();

        UpdateStateMachine();
    }

    void UpdateStateMachine()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (showDebugInfo && Time.frameCount % 60 == 0)
        {
            Debug.Log($"Boss state: " +
                $"attacking={isAttacking}, " +
                $"retreating={isRetreating}, " +
                $"distance={distanceToPlayer:F2}, " +
                $"can attack={Time.time >= nextAttackTime}");
        }

        if (isAttacking)
        {
            if (isLunging)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * lungeForce;
            }
            return; 
        }

        if (isRetreating)
        {
            Retreat();
            return;
        }

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            if (Random.value <= aggressiveness)
            {
                StartAttack();
                return;
            }
        }

        ApproachPlayer();
    }

    void Hover()
    {
        float newY = originalY + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(currentPos.x, newY, currentPos.z);
    }

    void PositionAttackHitbox()
    {
        if (attackHitbox != null)
        {
            float xOffset = transform.localScale.x > 0 ? 1.5f : -1.5f;
            float yOffset = -0.5f;
            attackHitbox.transform.localPosition = new Vector3(xOffset, yOffset, 0);
        }
    }

    void UpdateFacingDirection()
    {
        bool shouldFaceRight = player.position.x < transform.position.x;

        Vector3 scale = transform.localScale;
        if (shouldFaceRight)
        {
            scale.x = Mathf.Abs(scale.x); 
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    void ApproachPlayer()
    {
        if (showDebugInfo && Time.frameCount % 60 == 0)
        {
            Debug.Log("Boss is pursuing player aggressively");
        }

        Vector2 direction = (player.position - transform.position).normalized;

        Vector2 targetVelocity = direction * approachSpeed;

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * 20f);

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == "IsMoving" && param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool("IsMoving", true);
                break;
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        isLunging = Random.value < lungeChance;

        if (!isLunging)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            Vector2 lungeDirection = (player.position - transform.position).normalized;
            rb.linearVelocity = lungeDirection * lungeForce;
        }

        int attackType = Random.Range(1, 5);

        if (showDebugInfo)
        {
            Debug.Log($"Boss is attacking with attack type {attackType}, lunging: {isLunging}");
        }

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == "Attack" + attackType && param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.SetTrigger("Attack" + attackType);
                break;
            }
        }

        if (attackHitbox != null)
        {
            StartCoroutine(ActivateAttackHitbox(0.2f));
        }
    }

    IEnumerator ActivateAttackHitbox(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);

            Collider2D collider = attackHitbox.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            yield return new WaitForSeconds(0.2f);

            attackHitbox.SetActive(false);
                
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }

    public void OnAttackFinished()
    {
        isAttacking = false;
        isLunging = false;

        if (Random.value < 0.05f)
        {
            StartRetreat();
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && Random.value < aggressiveness)
            {
                StartAttack();
            }
            else
            {
                ApproachPlayer();
            }
        }
    }

    void StartRetreat()
    {
        isRetreating = true;

        if (showDebugInfo)
        {
            Debug.Log("Boss is retreating briefly");
        }

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == "Retreating" && param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool("Retreating", true);
                break;
            }
        }

        Vector2 retreatDirection = (transform.position - player.position).normalized;
        retreatTarget = transform.position + (Vector3)(retreatDirection * retreatDistance);
    }

    void Retreat()
    {
        Vector2 direction = (retreatTarget - transform.position).normalized;
        Vector2 targetVelocity = direction * retreatSpeed;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * 10f);

        float distanceToTarget = Vector2.Distance(transform.position, retreatTarget);
        if (distanceToTarget < 0.5f) 
        {
            isRetreating = false;

            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == "Retreating" && param.type == AnimatorControllerParameterType.Bool)
                {
                    animator.SetBool("Retreating", false);
                    break;
                }
            }

            nextAttackTime = Time.time;

            ApproachPlayer();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isTakingDamage)
            return; 

        isTakingDamage = true;

        if (showDebugInfo)
        {
            Debug.Log($"Boss took {damage} damage");
        }

        isAttacking = false;
        isRetreating = false;
        isLunging = false;
        rb.linearVelocity = Vector2.zero;

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == "Hurt" && param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.SetTrigger("Hurt");
                break;
            }
        }

        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        StartCoroutine(ApplyKnockback(knockbackDirection));

        StartCoroutine(ResetHurtTrigger());
    }

    IEnumerator ResetHurtTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        animator.ResetTrigger("Hurt");
    }

    IEnumerator ApplyKnockback(Vector2 direction)
    {
        rb.linearVelocity = direction * knockbackForce;
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
        isTakingDamage = false;

        nextAttackTime = Time.time;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            StartAttack();
        }
        else
        {
            ApproachPlayer();
        }
    }

    public void OnHurtFinished()
    {
        isTakingDamage = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!showDebugInfo) return;

        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, minDistanceToPlayer);
        }
    }
}