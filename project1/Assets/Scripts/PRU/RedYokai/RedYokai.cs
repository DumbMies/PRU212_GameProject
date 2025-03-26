using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RedYokai : Entity
{
    bool isAttacking;
    bool isPursuing;
    bool canJump;
    bool canFlip = true;
    public bool canAttack = true;
    public RaycastHit2D groundHit;
    public float distanceToGround;

    [SerializeField] private float knockbackForce = 5f;

    [SerializeField] float distanceBetweenPillar;
    [SerializeField] float timeBetweenPillar;
    
    float distanceToPlayer;

    [SerializeField] public GameObject lavaPillarPrefab;

    public bool isBusy { get; private set; }

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Player Detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] public LayerMask whatIsPlayer;

    [Header("EntityState")]

    private RaycastHit2D playerDetected;
    private float GroundSlamCooldown;

    #region States
    public RedYokaiStateMachine stateMachine { get; private set; }
    public RedYokaiMoveState moveState { get; private set; }
    public RedYokaiCombatState combatState { get; private set; }
    public RedYokaiJumpState jumpState { get; private set; }
    public RedYokaiAirState airState { get; private set; }
    public RedYokaiGroundSlamState groundSlamState { get; private set; }
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
        stateMachine = new RedYokaiStateMachine();

        moveState = new RedYokaiMoveState(stateMachine, this, "Move");
        combatState = new RedYokaiCombatState(stateMachine, this, "Combat");
        jumpState = new RedYokaiJumpState(stateMachine, this, "Jump");
        airState = new RedYokaiAirState(stateMachine, this, "Air");
        groundSlamState = new RedYokaiGroundSlamState(stateMachine, this, "GroundSlam");
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 1;
        stateMachine.Initialize(moveState);
    }

    //private void Movement()
    //{
    //    rb.linearVelocity = new Vector2(moveSpeed * facingDirection, rb.linearVelocity.y);
    //}
    public RaycastHit2D getGroundHit()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, Mathf.Infinity, whatIsGround);
    }
    public RaycastHit2D getDistanceToGround()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, whatIsGround);
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        groundHit = getGroundHit();
        distanceToGround = groundHit.distance;

        #region old spaghetti code
        //Debug.Log("canJump : " + canJump);
        //Debug.Log("groundDetect : " + IsGroundDetected());

        //if (!IsGroundDetected() || IsWallDetected())
        //{
        //   //Debug.Log("IsPursuing : " + isPursuing);
        //   if (isPursuing)
        //   {
        //       wallCheckDistance = 0.2f;
        //       if (canJump && isAttacking == false)
        //       {
        //           float leapForceX = 5f * facingDirection;
        //           float leapForceY = 5f;
        //           stateMachine.ChangeState(airState);
        //           rb.linearVelocity = new Vector2(leapForceX, leapForceY);
        //           canJump = false;
        //       }
        //   }
        //   else
        //   {
        //       //Debug.Log("Flip");
        //       Flip();
        //   }
        //}

        //if (stateMachine.currentState == dashState && IsWallDetected()) //hit the wall
        //{
        //    Debug.Log("IsAttacking = " + isAttacking);
        //    anim.speed = 1f;
        //    rb.linearVelocity = Vector2.zero;
        //    rb.linearVelocity = new Vector2(knockbackForce * -facingDirection, knockbackForce);
        //    Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);

        //    Debug.Log("facingDir = " + facingDirection);
        //    stateMachine.ChangeState(moveState);
        //    canFlip = true;
        //}

        isPlayerDetected();
        if (IsGroundDetected())
        {
           canJump = true;
        }
        //Debug.Log("Ground below Y level = " + MathF.Round(groundHit.point.y + 1.3f));
        //Debug.Log("Player ground Y level = " + MathF.Round(playerDetected.point.y));
        //if (groundHit.collider != null && groundHit.distance > 1f && stateMachine.currentState == airState && canSlam && MathF.Round(groundHit.point.y + 1.3f) == MathF.Round(playerDetected.point.y)) //GroundSlamCondition
        //{
        //   stateMachine.ChangeState(groundSlamState);
        //   canFlip = false;
        //   canSlam = false;
        //   canDash = false;
        //}

        //if (groundHit.distance < 1.5f && isPursuing && canDash && distanceToPlayer < 3f) //DashCondition
        //                                                                                 //if (groundHit.distance < 1.5f && isPursuing && canDash && distanceToPlayer < 3f && transform.position.x == isPlayerDetected.collider.transform.position.x) //DashCondition

        //{
        //    stateMachine.ChangeState(dashState);
        //    Debug.Log("Is Dashing");
        //    DashAttack();
        //    canDash = false;
        //    canSlam = false;
        //}
        //Debug.Log(canFlip);
        //Debug.Log(canSlam);
        //Debug.Log(canDash);
        //Debug.Log(distanceToPlayer);
        #endregion
    }


    private void FacingPlayer()
    {
        if (playerDetected.collider != null)
        {
            //Debug.Log(isPlayerDetected.collider);
            moveSpeed = 2;
            isPursuing = true;
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
            isPursuing = false;
        }
    }

    #region GroundSlamAttack
    //public async void AttackCooldownStart()
    //{
    //    await Task.Delay(5000);
    //    canAttack = true;
    //}

    private void DetectAndSpawnLavaPillars()
    {
        RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, new Vector2(0.4f, 0.1f), 0, Vector2.down, Mathf.Infinity, whatIsGround);

        if (groundHit.collider == null) return;

        float yLevel = groundHit.point.y;
        float startX = transform.position.x;

        StartCoroutine(SpawnLavaPillarsWithDelay(startX, yLevel));
    }

    private IEnumerator SpawnLavaPillarsWithDelay(float startX, float yLevel)
    {
        int spawnAmout = 0;

        float x = startX;
        int faceDirectionWhenSkillCast = facingDirection;

        while (true)
        {
            GameObject newPillar = Instantiate(lavaPillarPrefab, new Vector3(x, yLevel, 0), Quaternion.identity);
            newPillar.GetComponent<LavaPillar>().enabled = true;
            newPillar.GetComponent<BoxCollider2D>().enabled = true;
            float newPillarXPos = newPillar.transform.position.x;
            float newPillarYPos = newPillar.transform.position.y;
            Vector2 direction = faceDirectionWhenSkillCast == 1 ? Vector2.right : Vector2.left;
            RaycastHit2D groundCheck = Physics2D.BoxCast(newPillar.transform.position, new Vector2(0.4f, 0.1f), 0, Vector2.down, 0.1f, whatIsGround);
            RaycastHit2D wallCheck = Physics2D.Raycast(newPillar.transform.position, direction, 0.1f, whatIsGround);
            Debug.Log(wallCheck.collider);
            if (groundCheck.collider == null || wallCheck.collider != null)
            {
                Destroy(newPillar);
                break;
            }
            if (faceDirectionWhenSkillCast == 1)
            {
                x += distanceBetweenPillar;
            }
            else
            {
                x -= distanceBetweenPillar;
            }
            spawnAmout++;
            yield return new WaitForSeconds(timeBetweenPillar);
        }
    }
    #endregion

    public IEnumerator BusyFor(float _second)
    {
        isBusy = true;
        yield return new WaitForSeconds(_second);
        isBusy = false;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.speed = 1f;
            Marisa marisa = collision.gameObject.GetComponent<Marisa>();
            stateMachine.ChangeState(airState);

            if (!marisa.isHurt)
            {
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);
                //Debug.Log("Collidable after switch = " + Physics2D.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player")));


                canFlip = true;
                isAttacking = false;

                Animator playerAnimator = collision.gameObject.GetComponentInChildren<Animator>();
                playerAnimator.SetBool("Hurt", true);
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Jump", false);
                playerAnimator.SetBool("Move", false);

                collision.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                PlayerTakeDamage(playerAnimator, collision.gameObject);
                Debug.Log("CodeReachedHere");
                marisa.stateMachine.currentState.TakeDamage();
                marisa.TriggerInvincibleFrame();
                if (collision.transform.position.x < transform.position.x)
                {
                    collision.gameObject.GetComponent<Marisa>().rb.linearVelocity = new Vector2(-knockbackForce, knockbackForce);
                    rb.linearVelocity = new Vector2(knockbackForce, knockbackForce);
                    if (marisa.facingDirection == -1)
                    {
                        marisa.Flip();
                    }
                }
                else
                {
                    collision.gameObject.GetComponent<Marisa>().rb.linearVelocity = new Vector2(knockbackForce, knockbackForce);
                    rb.linearVelocity = new Vector2(-knockbackForce, knockbackForce);
                    if (marisa.facingDirection == 1)
                    {
                        marisa.Flip();
                    }
                }
            }

        }
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
}