using UnityEngine;

public class Boss2JumpState : Boss2State
{
    private Transform targetCheckpoint;
    bool reachCheckpoint;
    public Boss2JumpState(Boss2StateMachine _stateMachine, Boss2 _boss2, string _animBoolName)
        : base(_stateMachine, _boss2, _animBoolName) { }

    public override void Enter()
    {
        boss2.rb.linearVelocity = new Vector2(0, 4f);
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (boss2.rb.linearVelocityY <=0)
        {
            stateMachine.ChangeState(boss2.floatState);
        }
    }

    public override void Exit()
    {
        isAirbone = false;
        base.Exit();
    }
}
