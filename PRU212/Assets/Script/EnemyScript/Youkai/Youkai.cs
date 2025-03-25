using UnityEngine;

public class Youkai : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")]
    public float attackRange;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttack;

    public YoukaiStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new YoukaiStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.BoxCast(transform.position, new Vector2(3, 0.1f), 0, Vector2.right * facingDirection, 1, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        //base.OnDrawGizmos();

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.position, new Vector3(transform.position.x + attackRange * facingDirection, transform.position.y));

        // Show the player detection box
        //Gizmos.color = Color.red;
        //Vector2 boxCastSize = new Vector2(5, 0f);
        //Vector2 boxCastOrigin = transform.position;
        //Vector2 boxCastDirection = Vector2.right * facingDirection;
        //Gizmos.DrawWireCube(boxCastOrigin + boxCastDirection * 0.5f, boxCastSize);
    }
}
