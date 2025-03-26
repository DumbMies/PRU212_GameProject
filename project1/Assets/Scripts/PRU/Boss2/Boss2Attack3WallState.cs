using UnityEngine;

public class Boss2Attack3WallState : Boss2State
{
    private bool spearSpawned;
    private Transform currentCheckpoint;

    public Boss2Attack3WallState(Boss2StateMachine stateMachine, Boss2 boss2, string animBoolName)
        : base(stateMachine, boss2, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        currentCheckpoint = null;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            triggerCalled = false;
            stateMachine.ChangeState(boss2.attack3LaunchState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}