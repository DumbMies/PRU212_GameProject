using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss2 : Entity
{
    float distanceToPlayer;
    public RaycastHit2D groundHit;
    public float distanceToGround;
    bool canFlip = true;
    public bool canAttack;
    public BoxCollider2D boxCollider2D;
    public CapsuleCollider2D capsuleCollider2D;
    public Collider2D hitbox;
    public Boss2Health healthDisplay;

    public int currentHealth;
    [SerializeField] public GameObject Spear;
    [SerializeField] public GameObject DemonCircle;
    [SerializeField] public GameObject DemonCircleAnimator;
    public Animator DemonCircleAnim;
    public BossDashIndicator dashIndicator;
    public GameObject bossTrail;


    #region Normal Checkpoints
    [Header("Normal Checkpoint")]
    [SerializeField] public Transform checkpointA;
    [SerializeField] public Transform checkpointB;
    [SerializeField] public Transform checkpointC;
    [SerializeField] public Transform checkpointD;
    [SerializeField] public Transform checkpointE;
    [SerializeField] public Transform checkpointF;
    #endregion

    #region Side Checkpoints
    [Header("Side Checkpoint")]
    [SerializeField] public Transform SCA;
    [SerializeField] public Transform SCB;
    [SerializeField] public Transform SCC;
    [SerializeField] public Transform SCD;
    [SerializeField] public Transform SCE;
    [SerializeField] public Transform SCF;
    #endregion

    [Header("Move info")]
    public float moveSpeed = 8f;

    [Header("Player Detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] public LayerMask whatIsPlayer;

    [Header("EntityState")]

    private RaycastHit2D playerDetected;

    #region States
    public Boss2StateMachine stateMachine { get; private set; }
    public Boss2FloatState floatState { get; private set; }
    public Boss2LandState landState { get; private set; }
    public Boss2Attack1State attack1State { get; private set; }
    public Boss2JumpState jumpState { get; private set; }
    public Boss2Attack3WallState attack3WallState { get; private set; }
    public Boss2Attack3LaunchState attack3LaunchState { get; private set; }
    public Boss2KnockedState knockedState { get; private set; }
    public Boss2AirState airState { get; private set; }
    public Boss2GroundHitState groundHitState { get; private set; }
    public Boss2TransformState transformState { get; private set; }
    public Boss2DashState dashState { get; private set; }
    #endregion
    public override bool IsGroundDetected()
    {
        return base.IsGroundDetected();
    }

    public override bool IsWallDetected()
    {
        return base.IsWallDetected();
    }

    protected override void Awake()
    {
        base.Awake();
        boxCollider2D = GetComponent<BoxCollider2D>();
        stateMachine = new Boss2StateMachine();
        bossTrail = GameObject.Find("BossTrail") ;

        floatState = new Boss2FloatState(stateMachine, this, "Float");
        landState = new Boss2LandState(stateMachine, this, "Land");
        attack1State = new Boss2Attack1State(stateMachine, this, "Attack1");
        jumpState = new Boss2JumpState(stateMachine, this, "Float");
        attack3WallState = new Boss2Attack3WallState(stateMachine, this, "Attack3Wall");
        attack3LaunchState = new Boss2Attack3LaunchState(stateMachine, this, "Attack3Launch");
        knockedState = new Boss2KnockedState(stateMachine, this, "Knocked");
        airState = new Boss2AirState(stateMachine, this, "Air");
        groundHitState = new Boss2GroundHitState(stateMachine, this, "GroundHit");
        transformState = new Boss2TransformState(stateMachine, this, "Transform");
        dashState = new Boss2DashState(stateMachine, this, "Dash");
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public void TakeDamage()
    {
        stateMachine.ChangeState(knockedState);
    }

    public void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }



    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(floatState);
        dashIndicator = FindFirstObjectByType<BossDashIndicator>();
        if (dashIndicator == null)
        {
            Debug.LogWarning("No BossDashIndicator found in the scene!");
        }
    }

    public RaycastHit2D getGroundHit()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, Mathf.Infinity, whatIsGround);
    }
    public RaycastHit2D getDistanceToGround()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 10f, whatIsGround);
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        groundHit = getGroundHit();
        distanceToGround = groundHit.distance;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }

    public Vector3 GetPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            return player.transform.position;
        }
        return Vector3.zero;
    }

    public void PhaseThroughGround()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ground"), true);
    }
    public void UnPhase()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ground"), false);
    }

    public void TakeDamage(int damage)
    {
        if (stateMachine.currentState.CanTakeDamage())
        {
            currentHealth -= damage;
            if (currentHealth > 0)
            {
                stateMachine.ChangeState(knockedState);
            }
            else
            {
                //stateMachine.ChangeState(deathState);
            }
            //healthDisplay.TakeDamage(damage);
        }
    }
    private void FacingPlayer()
    {
        if (playerDetected.collider != null)
        {
            moveSpeed = 2;
            Vector2 playerPosition = playerDetected.collider.transform.position;
            if ((playerPosition.x > transform.position.x && facingDirection < 0) ||
                (playerPosition.x < transform.position.x && facingDirection > 0))
            {
                if (canFlip == true)
                {
                    Flip();
                }
            }
        }
        else if (playerDetected.collider == null)
        {
            moveSpeed = 1;
        }
    }

    public IEnumerator BusyFor(float _second)
    {
        yield return new WaitForSeconds(_second);
    }


    private void isPlayerDetected()
    {
        playerDetected = Physics2D.BoxCast(wallCheck.position, new Vector2(20f, 4f), 0, Vector2.right * facingDirection, 0, whatIsPlayer);
        distanceToPlayer = Vector2.Distance(wallCheck.position, playerDetected.point);
    }

    //async void DashAttack()
    //{
    //    isAttacking = true;
    //    rb.linearVelocity = Vector2.zero;
    //    canJump = false;
    //    canFlip = false;
    //    await Task.Delay(3000);
    //    AttackCooldownStart();
    //}

    public void Dash()
    {
        anim.speed = 0f;
        rb.linearVelocity = new Vector2(20 * facingDirection, 5f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), false);

    }
    async void PlayerTakeDamage(Animator playerAnimator, GameObject playerSprite)
    {
        ColorTween(playerSprite.GetComponentInChildren<SpriteRenderer>(), Color.red, Color.white, 1f);

        await Task.Delay(1000);

        //playerSprite.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, 0.2f);
        playerAnimator.SetBool("Hurt", false);
    }

    async void ColorTween(SpriteRenderer sprite, Color StartColor, Color EndColor, float Duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / Duration;
            Color newColor = Color.Lerp(StartColor, EndColor, t);
            sprite.color = newColor;
            await Task.Yield();
        }
    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        GameObject player = GameObject.FindGameObjectWithTag("Player");

    //        if (player != null)
    //        {
    //            player.GetComponent<Marisa>().TakeDamage(1);
    //        }
    //    }
    //}
}