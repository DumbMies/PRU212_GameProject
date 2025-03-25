using UnityEngine;

public class ThunderYoukaiIdleState : ThunderYoukaiGroundedState
{
    public ThunderYoukaiIdleState(Youkai youkai, YoukaiStateMachine stateMachine, string animBoolName, ThunderYoukai thunderYoukai) : base(youkai, stateMachine, animBoolName, thunderYoukai)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = thunderYoukai.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(thunderYoukai.moveState);
        }
    }
}
