using UnityEngine;

public class ThunderYoukaiMoveState : ThunderYoukaiGroundedState
{
    private float moveDuration = 2f;
    private float moveTimer;

    public ThunderYoukaiMoveState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, ThunderYoukai thunderYoukai) : base(Youkai, stateMachine, animBoolName, thunderYoukai)
    {
    }

    public override void Enter()
    {
        base.Enter();
        moveTimer = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Youkai.SetVelocity(thunderYoukai.moveSpeed * thunderYoukai.facingDirection, rb.linearVelocity.y);

        moveTimer += Time.deltaTime;

        if (moveTimer >= moveDuration || !thunderYoukai.IsGroundDetected())
        {
            thunderYoukai.Flip();
            stateMachine.ChangeState(thunderYoukai.idleState);
        }

        //if (!thunderYoukai.IsGroundDetected())
        //{
        //    thunderYoukai.Flip();
        //    stateMachine.ChangeState(thunderYoukai.idleState);
        //}
    }
}
