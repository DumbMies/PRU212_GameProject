using UnityEngine;

public class Boss2JumpState : Boss2State
{
    private Transform targetCheckpoint;
    bool reachCheckpoint;

    public Boss2JumpState(Boss2StateMachine stateMachine, Boss2 boss2, string animBoolName)
        : base(stateMachine, boss2, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        boss2.rb.gravityScale = 1;
        boss2.rb.linearVelocity = new Vector2(0, 4f);

    }

    public override void Update()
    {
        base.Update();
        if (boss2.rb.linearVelocityY < 0)
        {
            stateMachine.ChangeState(boss2.floatState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}