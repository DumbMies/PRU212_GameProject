using UnityEngine;

public class RedYokaiAirState : RedYokaiState
{
    public RedYokaiAirState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName) : base(_stateMachine, _redYokai, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        isAirbone = false;
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (redYokai.IsGroundDetected())
        {
            stateMachine.ChangeState(redYokai.moveState);
        }
        //Debug.Log("Distance To Ground = " + redYokai.distanceToGround);
        //Debug.Log("Is Chasing = " + isChasing);
        //Debug.Log("Can Attack = " + canAttack);
        if (isChasing && redYokai.canAttack && redYokai.distanceToGround > 3f)
        {
            stateMachine.ChangeState(redYokai.groundSlamState);
        }
    }
}