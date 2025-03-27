using UnityEngine;

public class MarisaIdleState : MarisaGroundedState
{
    public MarisaIdleState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
        
    }
}
