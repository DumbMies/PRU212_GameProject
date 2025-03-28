using UnityEngine;
using System.Linq;

public class Boss2DashState : Boss2State
{
    private Transform targetCheckpoint;
    private Vector2 dashDirection;
    private float dashSpeed = 75f;
    private float dashDistance = 200f;
    private Vector2 startPosition;
    private int dashCount;
    public BossDashIndicator dashIndicator;

    // Arrays to categorize checkpoints
    private Transform[] leftSideCheckpoints;
    private Transform[] rightSideCheckpoints;
    private Transform previousCheckpoint;

    public Boss2DashState(Boss2StateMachine stateMachine, Boss2 boss2, string animBoolName)
        : base(stateMachine, boss2, animBoolName)
    {
        InitializeCheckpointArrays();
    }

    private void InitializeCheckpointArrays()
    {
        // Left side checkpoints (A, C, E)
        leftSideCheckpoints = new Transform[]
        {
            boss2.SCA,
            boss2.SCC,
            boss2.SCE
        };

        // Right side checkpoints (B, D, F)
        rightSideCheckpoints = new Transform[]
        {
            boss2.SCB,
            boss2.SCD,
            boss2.SCF
        };
    }

    private Transform SelectCheckpoint()
    {
        Transform[] availableCheckpoints;

        // Determine which side of checkpoints to choose from
        if (previousCheckpoint == null || leftSideCheckpoints.Contains(previousCheckpoint))
        {
            // If previous checkpoint was on left side or not set, choose from right side
            availableCheckpoints = rightSideCheckpoints
                .Where(cp => cp != previousCheckpoint)
                .ToArray();
        }
        else
        {
            // If previous checkpoint was on right side, choose from left side
            availableCheckpoints = leftSideCheckpoints
                .Where(cp => cp != previousCheckpoint)
                .ToArray();
        }

        // Randomly select from available checkpoints
        targetCheckpoint = availableCheckpoints[Random.Range(0, availableCheckpoints.Length)];
        previousCheckpoint = targetCheckpoint;

        return targetCheckpoint;
    }

    public override void Enter()
    {
        base.Enter();
        boss2.GetComponent<CapsuleCollider2D>().enabled = false;
        boss2.bossTrail.GetComponent<TrailRenderer>().enabled = true;
        // Reset dash count when entering frenzy mode
        dashCount = 0;

        // Disable gravity
        boss2.rb.gravityScale = 0;

        // Make boss invisible during teleports
        SpriteRenderer spriteRenderer = boss2.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // Disable ground collision
        Physics2D.IgnoreLayerCollision(boss2.gameObject.layer, LayerMask.NameToLayer("Ground"), true);

        // Perform first teleport and dash
        PerformTeleportAndDash();
    }

    private void PerformTeleportAndDash()
    {
        targetCheckpoint = SelectCheckpoint();

        // Teleport to checkpoint
        boss2.transform.position = targetCheckpoint.position;

        // Calculate dash direction and start position
        startPosition = boss2.transform.position;
        Vector2 playerPosition = boss2.GetPlayerPosition();
        dashDirection = playerPosition.x < startPosition.x ? new Vector2(-1, 0) : new Vector2(1, 0);

        // Flip if needed
        if (playerPosition.x > startPosition.x && boss2.facingDirection == 1 ||
            playerPosition.x < startPosition.x && boss2.facingDirection == -1)
        {
            boss2.Flip();
        }

        SpriteRenderer bossSpriteRenderer = boss2.GetComponent<SpriteRenderer>();
        float bossSpriteHeight = 200f;

        // Show dash indicator (with null check)
        if (boss2.dashIndicator != null)
        {
            boss2.dashIndicator.ShowDashIndicator(
                startPosition,
                dashDirection,
                bossSpriteHeight
            );
        }
        else
        {
            Debug.LogWarning("Dash Indicator is not assigned!");
        }

        // Trigger dash animation
        boss2.anim.SetTrigger("Dash");
    }

    //private Vector2 CalculateDashDirection()
    //{
    //    // Get player position or use a default direction
    //    Vector2 playerPosition = boss2.GetPlayerPosition();
    //    return (playerPosition - (Vector2)boss2.transform.position).normalized;
    //}

    public override void Update()
    {
        base.Update();

        // Perform dash movement
        boss2.rb.linearVelocity =  dashDirection * dashSpeed;

        // Check dash distance
        float distanceDashed = Vector2.Distance(startPosition, boss2.transform.position);
        if (distanceDashed >= dashDistance)
        {
            // Increment dash count
            dashCount++;

            if (dashCount >= 6)
            {
                // Return to float state after 6 dashes
                ReturnToFloatState();
            }
            else
            {
                // Perform next teleport and dash
                PerformTeleportAndDash();
            }
        }
    }

    private void ReturnToFloatState()
    {
        boss2.dashIndicator.HideDashIndicator();

        // Re-enable sprite
        SpriteRenderer spriteRenderer = boss2.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        // Re-enable ground collision
        Physics2D.IgnoreLayerCollision(boss2.gameObject.layer, LayerMask.NameToLayer("Ground"), false);

        // Reset gravity
        boss2.rb.gravityScale = 3.0f;
        boss2.rb.linearVelocity= Vector2.zero;

        // Change state
        boss2.anim.SetBool("Transform", false);
        stateMachine.ChangeState(boss2.floatState);
    }

    public override void Exit()
    {
        // Reset velocity
        boss2.bossTrail.GetComponent<TrailRenderer>().enabled = false;
        boss2.rb.linearVelocity = Vector2.zero;
        boss2.GetComponent<CapsuleCollider2D>().enabled = true;
        base.Exit();
    }
}