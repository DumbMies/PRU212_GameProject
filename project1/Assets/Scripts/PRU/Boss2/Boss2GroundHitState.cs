using UnityEngine;

public class Boss2GroundHitState : Boss2State
{
    private Transform targetCheckpoint;
    private bool stunned;
    private float stunDuration = 2f;
    private float stunTimer;

    public Boss2GroundHitState(Boss2StateMachine stateMachine, Boss2 boss2, string animBoolName)
        : base(stateMachine, boss2, animBoolName) { }

    public override void Enter()
    {

        base.Enter();
        stunned = true;
        stunTimer = stunDuration;
    }

    public override void Update()
    {
        base.Update();

        if (stunned)
        {
            stunTimer -= Time.deltaTime;
            
            if (stunTimer <= 0)
            {
                stunned = false;
            }
        }
        if (!stunned && boss2.currentHealth <= 20)
        {
            boss2.rb.gravityScale = 0;
            boss2.rb.linearVelocity = new Vector2(0, 10f);
            stateMachine.ChangeState(boss2.jumpState);
        }

        if (!stunned && boss2.currentHealth > 0)
        {
            stateMachine.ChangeState(boss2.jumpState);
        }
        if (boss2.currentHealth == 0)
        {
            Debug.Log("Die");
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}