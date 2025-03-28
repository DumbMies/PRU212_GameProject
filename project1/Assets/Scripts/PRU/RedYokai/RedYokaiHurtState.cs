using UnityEngine;

public class RedYokaiHurtState : RedYokaiState
{
    public RedYokaiHurtState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName) : base(_stateMachine, _redYokai, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
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
    }
}
