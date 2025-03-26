using UnityEngine;

public class RedYokaiDashState : RedYokaiState
{
    private float lastTimeAttacked;

    public RedYokaiDashState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName) : base(_stateMachine, _redYokai, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(redYokai.airState);
        }

        if (triggerCalled)
        {
            Debug.Log("triggereCalled");
            stateMachine.ChangeState(redYokai.moveState);
            triggerCalled = false;
        }
    }
}
