using UnityEngine;
using System.Linq;

public class Boss2FloatState : Boss2State
{
    private Transform targetCheckpoint;
    private bool reachCheckpoint;
    private Transform previousCheckpoint;
    private bool goneFrenzy =false;

    // Arrays to categorize checkpoints
    private Transform[] leftSideCheckpoints;
    private Transform[] rightSideCheckpoints;

    public Boss2FloatState(Boss2StateMachine stateMachine, Boss2 boss2, string animBoolName)
        : base(stateMachine, boss2, animBoolName)
    {
        InitializeCheckpointArrays();
    }

    private void InitializeCheckpointArrays()
    {
        // Left side checkpoints (A, C, E)
        leftSideCheckpoints = new Transform[]
        {
            boss2.checkpointA,
            boss2.checkpointC,
            boss2.checkpointE
        };

        // Right side checkpoints (B, D, F)
        rightSideCheckpoints = new Transform[]
        {
            boss2.checkpointB,
            boss2.checkpointD,
            boss2.checkpointF
        };
    }

    public override void Enter()
    {
        base.Enter();
        boss2.rb.gravityScale = 0;
        reachCheckpoint = false;

        SelectNextCheckpoint();
    }

    private void SelectNextCheckpoint()
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
    }

    public override void Update()
    {
        base.Update();
        //if (goneFrenzy == false && boss2.currentHealth <= 20)
        //{
        //    goneFrenzy = true;
        //    boss2.rb.gravityScale = 0;
        //    boss2.rb.linearVelocity = Vector2.zero;
        //    stateMachine.ChangeState(boss2.transformState);
        //}

        if (!reachCheckpoint)
        {
            boss2.transform.position = Vector3.MoveTowards(
                boss2.transform.position,
                targetCheckpoint.position,
                boss2.moveSpeed * Time.deltaTime
            );
        }

        #region Face Player
        Vector2 playerPosition = IsPlayerDetected().point;
        if (playerPosition.x < boss2.transform.position.x && boss2.facingDirection < 0)
        {
            boss2.Flip();
        }
        else if (playerPosition.x > boss2.transform.position.x && boss2.facingDirection > 0)
        {
            boss2.Flip();
        }
        #endregion

        // Check if reached checkpoint
        if (Vector3.Distance(boss2.transform.position, targetCheckpoint.position) < 0.1f)
        {
            reachCheckpoint = true;
            boss2.rb.linearVelocity = Vector2.zero;

            // Change state based on which checkpoint was reached
            if (targetCheckpoint == boss2.checkpointC || targetCheckpoint == boss2.checkpointD)
            {
                stateMachine.ChangeState(boss2.attack3WallState);
            }
            else
            {
                boss2.rb.gravityScale = 3.0f;
                stateMachine.ChangeState(boss2.landState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}