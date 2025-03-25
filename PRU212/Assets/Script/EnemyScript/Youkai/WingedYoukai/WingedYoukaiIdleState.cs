using UnityEngine;

public class WingedYoukaiIdleState : WingedYoukaiFlyState
{
    public WingedYoukaiIdleState(WingedYoukaiAI wingedYoukaiAI, WingedYoukaiStateMachine stateMachine, string animBoolName) : base(wingedYoukaiAI, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //stateTimer = wingedYoukai.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if (stateTimer < 0)
        //{
        //    stateMachine.ChangeState(wingedYoukai.moveState);
        //}
    }
}
