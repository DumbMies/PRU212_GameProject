using UnityEngine;
using Pathfinding;

public class WingedYoukaiAI : MonoBehaviour
{
    public Transform target;
    public Collider2D detectionZone;
    public Collider2D hitbox;

    [Header("Move info")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    [Header("Attack info")]
    public float battleTime;
    public float attackRange;
    public float attackCooldown;
    public Transform attackPoint;
    [HideInInspector] public float lastTimeAttack;

    [Header("Bullet info")]
    public GameObject bulletPrefab;

    [Header("Character stats")]
    public int maxHealth;
    public int currentHealth;

    public Animator anim { get; private set; }

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool playerInZone = false;

    Seeker seeker;
    public Rigidbody2D rb { get; private set; }

    #region States
    public WingedYoukaiIdleState idleState { get; private set; }
    public WingedYoukaiMoveState moveState { get; private set; }
    public WingedYoukaiBattleState battleState { get; private set; }
    public WingedYoukaiAttackState attackState { get; private set; }
    public WingedYoukaiHurtState hurtState { get; private set; }
    public WingedYoukaiStateMachine stateMachine { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new WingedYoukaiStateMachine();

        idleState = new WingedYoukaiIdleState(this, stateMachine, "Idle");
        moveState = new WingedYoukaiMoveState(this, stateMachine, "Move");
        battleState = new WingedYoukaiBattleState(this, stateMachine, "Move");
        attackState = new WingedYoukaiAttackState(this, stateMachine, "Attack");
        hurtState = new WingedYoukaiHurtState(this, stateMachine, "Hurt");
    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    public void UpdatePath()
    {
        if (playerInZone && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected virtual void Update()
    {
        stateMachine.currentState.Update();
    }

    protected virtual void FixedUpdate()
    {
        if (path == null || !playerInZone)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        FlipCharacterBasedOnDirection();
    }

    public void SetZeroVelocity() => rb.velocity = Vector2.zero;

    public void FireBullet()
    {
        if (bulletPrefab != null && attackPoint != null)
        {
            Vector2 direction = (target.position - attackPoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<WingedYoukaiBullet>().Initialize(direction);
        }
    }

    public void TakeDamage()
    {
        stateMachine.ChangeState(hurtState);
    }

    #region Flip
    public void FlipCharacterBasedOnDirection()
    {
        if (rb.velocity.x > 0.01f && !IsFacingRight())
        {
            Flip();
        }
        else if (rb.velocity.x < -0.01f && IsFacingRight())
        {
            Flip();
        }
    }

    public bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    public void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    #endregion

    #region Trigger target
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            playerInZone = true;
            UpdatePath();
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other == detectionZone && other.transform == target)
        {
            playerInZone = false;
            path = null;
            rb.velocity = Vector2.zero;
        }
    }
    #endregion
}
