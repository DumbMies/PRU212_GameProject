using UnityEngine;

public class WingedYoukaiMoveState : WingedYoukaiFlyState
{
    public WingedYoukaiMoveState(WingedYoukaiAI wingedYoukaiAI, WingedYoukaiStateMachine stateMachine, string animBoolName) : base(wingedYoukaiAI, stateMachine, animBoolName)
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

        //Youkai.SetVelocity(wingedYoukai.moveSpeed * wingedYoukai.facingDirection, rb.velocity.y);

        //if (!wingedYoukai.IsGroundDetected())
        //{
        //    wingedYoukai.Flip();
        //    stateMachine.ChangeState(wingedYoukai.idleState);
        //}
    }
}
