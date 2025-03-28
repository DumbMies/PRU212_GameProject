using UnityEngine;

public class MarisaDashState : MarisaState
{
    public MarisaDashState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.audioManager.PlaySFX(player.audioManager.dashSound);
        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override bool CanTakeDamage()
    {
        return false;
    }
}
