using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_RedYokai : Entity
{

    bool isAttacking;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;

    [Header("Player Detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("EntityState")]
    private RaycastHit2D isPlayerDetected;
    protected override void Start()
    {
        base.Start();


    }

    protected override void Update()
    {
        base.Update();
        DetectPlayer();

        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 0.9)
            {
                rb.linearVelocity = new Vector2(moveSpeed * 1.5f * facingDirection, rb.linearVelocity.y);

                Debug.Log("I see the player");
                isAttacking = false;
            }
            else
            {
                Debug.Log("Attack" + isPlayerDetected.collider.gameObject.name);
                isAttacking = true;
                Debug.Log(isPlayerDetected.distance);
            }
        }

        if (!IsGroundDetected())
        {
            Flip();
        }

        Movement();
        AnimatorControllers();
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

    private void DetectPlayer()
    {
        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, playerCheckDistance, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDirection, transform.position.y));
    }

    public override bool IsGroundDetected()
    {
        return base.IsGroundDetected();
    }

    public override bool IsWallDetected()
    {
        Debug.Log("wall detected");
        return base.IsWallDetected();
    }
}
