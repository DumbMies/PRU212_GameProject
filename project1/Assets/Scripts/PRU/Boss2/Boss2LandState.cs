using UnityEngine;

public class Boss2LandState : Boss2State
{
    private Transform targetCheckpoint;
    private float switchTime = 3f;
    private float switchTimer;

    public Boss2LandState(Boss2StateMachine _stateMachine, Boss2 _boss2, string _animBoolName)
        : base(_stateMachine, _boss2, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            triggerCalled = false;
            stateMachine.ChangeState(boss2.attack1State);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
