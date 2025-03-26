using UnityEngine;

public class MarisaHurtState : MarisaState
{
    //private float hurtDuration = 0.5f;
    //private float startTime;

    public MarisaHurtState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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
        //Debug.Log("time = " + Time.time + "\n" + "startTime = " + startTime + "hurtDuration = " + hurtDuration);
        //if (Time.time - startTime >= hurtDuration)
        //{
        //    if (player.IsGroundDetected())
        //    {
        //        stateMachine.ChangeState(player.moveState);
        //    }
        //    else
        //    {
        //        stateMachine.ChangeState(player.airState);
        //    }
        //}
    }
}