using UnityEngine;

public class Boss2AirState : Boss2State
{
    private Transform targetCheckpoint;
    bool reachCheckpoint;
    public Boss2AirState(Boss2StateMachine _stateMachine, Boss2 _boss2, string _animBoolName)
        : base(_stateMachine, _boss2, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (boss2.distanceToGround < 1.5f)
        {
            stateMachine.ChangeState(boss2.groundHitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
