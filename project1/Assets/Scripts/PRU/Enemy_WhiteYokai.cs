using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_WhiteYokai : Entity
{
    private Transform player;
    private Transform enemy;
    bool isAttacking;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] float leapAnimationTime;

    [Header("Player Detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    private RaycastHit2D isPlayerDetected;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldownTime = 2f;
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Marisa").transform;
        enemy = transform;
    }

    protected override void Update()
    {
        base.Update();

        if (IsWallDetected())
        {
            Flip();
        }

        DetectPlayer();

        if (isPlayerDetected)
        {
            if (enemy.position.x < player.position.x && facingDirection == 1 || enemy.position.x > player.position.x && facingDirection == -1)
            {
            }
            else if (enemy.position.x < player.position.x && facingDirection == -1 || enemy.position.x > player.position.x && facingDirection == 1)
            {
                Flip();
            }
            if (playerCheckDistance > 1.5)
            {
                rb.linearVelocity = new Vector2(moveSpeed * 1.5f * facingDirection, rb.linearVelocity.y);

                isAttacking = false;
            }
            else if (playerCheckDistance < 1.5 &&  playerCheckDistance >0.5)
            {
                if (!isAttacking && Time.time >= lastAttackTime + attackCooldownTime)
                {
                    StartCoroutine(LeapForward());
                }

            }
        }

        if (!IsGroundDetected())
        {
            Flip();
        }

        Movement();
        AnimatorControllers();
    }
    public override bool IsWallDetected()
    {
        return base.IsWallDetected();
    }

    private IEnumerator LeapForward()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        float leapForceX = 7f * facingDirection;
        float leapForceY = 1f;

        rb.linearVelocity = new Vector2(leapForceX, leapForceY);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);

        yield return new WaitForSeconds(leapAnimationTime);

        EndLeap();
    }

    private void EndLeap()
    {
        isAttacking = false;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), false);
    }

    private void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1, 0.1f), 0, Vector2.right * facingDirection, 0, whatIsPlayer);

        if (hit.collider != null)
        {
            isPlayerDetected = hit;
            float playerDistance = Vector2.Distance(transform.position, hit.collider.transform.position);

            Debug.Log("Player detected! Distance: " + playerDistance);
        }
        else
        {
            isPlayerDetected = new RaycastHit2D();
        }
    }

    private void EnableCollision()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), false);
    }

    private void AnimatorControllers()
    {
        anim.SetBool("isPlayerDetected", isPlayerDetected);
        anim.SetBool("isAttacking", isAttacking);
    }

    private void Movement()
    {
        if(!isAttacking)
        {
            rb.linearVelocity = new Vector2(moveSpeed * facingDirection, rb.linearVelocity.y);
        }

    }



    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDirection, transform.position.y));

        Gizmos.color = Color.red;
        Vector3 direction = Vector2.right * facingDirection;
        Vector3 boxSize = new Vector3(10, 0.1f, 0);

        Gizmos.DrawWireCube(transform.position + direction, boxSize);
    }

    public override bool IsGroundDetected()
    {
        return base.IsGroundDetected();
    }

    //public override RaycastHit2D IsPlayerDetected()
    //{
    //    RaycastHit2D hit = base.IsPlayerDetected();

    //    if (hit.collider != null)
    //    {
    //        facingDir = hit.collider.transform.position.x > transform.position.x ? 1 : -1;
    //    }

    //    return hit;
    //}
}
