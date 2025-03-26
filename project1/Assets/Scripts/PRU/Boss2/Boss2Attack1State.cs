using UnityEngine;

public class Boss2Attack1State : Boss2State
{
    private bool spearSpawned;
    public Boss2Attack1State(Boss2StateMachine _stateMachine, Boss2 _boss2, string _animBoolName)
        : base(_stateMachine, _boss2, _animBoolName) { }

    public override void Enter()
    {

        spearSpawned = false;
        spearThrown = false;
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            boss2.anim.speed = 1;
            boss2.rb.gravityScale = 1;
            triggerCalled = false;
            stateMachine.ChangeState(boss2.jumpState);
        }
    }


    public override void Exit()
    {
        base.Exit();
    }
}
