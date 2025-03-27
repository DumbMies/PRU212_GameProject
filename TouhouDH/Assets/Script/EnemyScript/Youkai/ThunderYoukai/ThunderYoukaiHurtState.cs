using UnityEngine;

public class ThunderYoukaiHurtState : YoukaiState
{
    private ThunderYoukai thunderYoukai;
    private float knockbackForceX = 4f;
    private float knockbackForceY = 7f;
    private float hurtDuration = 0.5f;

    public ThunderYoukaiHurtState(Youkai youkai, YoukaiStateMachine stateMachine, string animBoolName, ThunderYoukai thunderYoukai) : base(youkai, stateMachine, animBoolName)
    {
        this.thunderYoukai = thunderYoukai;
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
            GameObject.Destroy(thunderYoukai.gameObject);
        }
    }

    private void ApplyKnockback()
    {
        Vector2 knockbackDirection = new Vector2(-thunderYoukai.facingDirection * knockbackForceX, knockbackForceY);
        thunderYoukai.rb.velocity = knockbackDirection;
    }
}
