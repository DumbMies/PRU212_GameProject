using UnityEngine;

public class MarisaJumpState : MarisaState
{
    public MarisaJumpState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}