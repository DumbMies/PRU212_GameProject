using UnityEngine;

public class RedYokaiJumpState : RedYokaiState
{
    public RedYokaiJumpState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName) : base(_stateMachine, _redYokai, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(redYokai.airState);
        }
    }
} 