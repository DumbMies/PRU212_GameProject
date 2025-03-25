using UnityEngine;

public class BasicYoukaiAttackState : YoukaiState
{
    private BasicYoukai basicYoukai;

    public BasicYoukaiAttackState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, BasicYoukai basicYoukai) : base(Youkai, stateMachine, animBoolName)
    {
        this.basicYoukai = basicYoukai;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        basicYoukai.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        basicYoukai.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(basicYoukai.battleState);
        }
    }
}
