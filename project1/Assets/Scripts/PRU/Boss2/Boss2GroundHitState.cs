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

        if (!stunned)
        {
            stateMachine.ChangeState(boss2.floatState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}