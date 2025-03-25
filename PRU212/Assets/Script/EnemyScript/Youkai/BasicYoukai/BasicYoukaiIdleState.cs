using UnityEngine;

public class BasicYoukaiIdleState : BasicYoukaiGroundedState
{
    public BasicYoukaiIdleState(Youkai youkai, YoukaiStateMachine stateMachine, string animBoolName, BasicYoukai basicYoukai) : base(youkai, stateMachine, animBoolName, basicYoukai)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = basicYoukai.idleTime;
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
            stateMachine.ChangeState(basicYoukai.moveState);
        }
    }
}
