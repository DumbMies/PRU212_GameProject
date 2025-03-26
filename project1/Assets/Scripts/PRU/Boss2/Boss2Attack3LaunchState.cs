using UnityEngine;

public class Boss2Attack3LaunchState : Boss2State
{
    private bool spearSpawned;
    private Transform currentCheckpoint;

    public Boss2Attack3LaunchState(Boss2StateMachine stateMachine, Boss2 boss2, string animBoolName)
        : base(stateMachine, boss2, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (boss2.IsWallDetected())
        {
            stateMachine.ChangeState(boss2.floatState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}