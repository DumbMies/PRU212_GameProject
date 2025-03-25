using UnityEngine;

public class MarisaHurtState : MarisaState
{
    private float knockbackForceX = 4f;
    private float knockbackForceY = 7f;
    private float hurtDuration = 0.5f;

    public MarisaHurtState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.audioManager.PlaySFX(player.audioManager.hurtSound);
        ApplyKnockback();
        stateTimer = hurtDuration;
        player.fx.StartCoroutine(player.fx.FlashFx());
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override bool CanTakeDamage()
    {
        return false;
    }

    private void ApplyKnockback()
    {
        Vector2 knockbackDirection = player.isFacingRight ? Vector2.left : Vector2.right;
        player.rb.linearVelocity = new Vector2(knockbackDirection.x * knockbackForceX, knockbackForceY);
    }
}
