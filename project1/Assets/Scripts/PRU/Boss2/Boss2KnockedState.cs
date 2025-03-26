using UnityEngine;

public class Boss2KnockedState : Boss2State
{
    private Transform targetCheckpoint;
    bool reachCheckpoint;
    public Boss2KnockedState(Boss2StateMachine _stateMachine, Boss2 _boss2, string _animBoolName)
        : base(_stateMachine, _boss2, _animBoolName) { }

    public override void Enter()
    {
        boss2.rb.gravityScale = 3;
        boss2.rb.linearVelocity = new Vector2(boss2.facingDirection*-3f, 4f);
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (boss2.rb.linearVelocity.y < 0 )
        {
            stateMachine.ChangeState(boss2.airState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
