using UnityEngine;

public class WingedYoukaiHurtState : WingedYoukaiState
{
    private float knockbackForceX = 4f;
    private float knockbackForceY = 7f;
    private float hurtDuration = 0.5f;

    public WingedYoukaiHurtState(WingedYoukaiAI wingedYoukai, WingedYoukaiStateMachine stateMachine, string animBoolName) : base(wingedYoukai, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ApplyKnockback();
        stateTimer = hurtDuration;
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
            GameObject.Destroy(WingedYoukaiAI.gameObject);
        }
    }

    private void ApplyKnockback()
    {
        Vector2 knockbackDirection = WingedYoukaiAI.IsFacingRight() ? Vector2.left : Vector2.right;
        WingedYoukaiAI.rb.velocity = new Vector2(knockbackDirection.x * knockbackForceX, knockbackForceY);
    }
}
