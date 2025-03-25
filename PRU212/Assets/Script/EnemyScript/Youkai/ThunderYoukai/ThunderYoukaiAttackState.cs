using UnityEngine;

public class ThunderYoukaiAttackState : YoukaiState
{
    private ThunderYoukai thunderYoukai;

    public ThunderYoukaiAttackState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, ThunderYoukai thunderYoukai) : base(Youkai, stateMachine, animBoolName)
    {
        this.thunderYoukai = thunderYoukai;
    }

    public override void Enter()
    {
        base.Enter();
        thunderYoukai.TriggerLightningStrike();
    }

    public override void Exit()
    {
        base.Exit();

        Youkai.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        thunderYoukai.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(thunderYoukai.battleState);
        }
    }
}
