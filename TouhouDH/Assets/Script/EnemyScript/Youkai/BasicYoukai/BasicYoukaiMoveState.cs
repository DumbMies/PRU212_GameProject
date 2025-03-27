using UnityEngine;

public class BasicYoukaiMoveState : BasicYoukaiGroundedState
{
    public BasicYoukaiMoveState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, BasicYoukai basicYoukai) : base(Youkai, stateMachine, animBoolName, basicYoukai)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Youkai.SetVelocity(basicYoukai.moveSpeed * basicYoukai.facingDirection, rb.velocity.y);

        if (!basicYoukai.IsGroundDetected())
        {
            basicYoukai.Flip();
            stateMachine.ChangeState(basicYoukai.idleState);
        }
    }
}
