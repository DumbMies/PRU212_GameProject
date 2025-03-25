using UnityEngine;

public class MarisaGroundedState : MarisaState
{
    public MarisaGroundedState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.K) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }
}
