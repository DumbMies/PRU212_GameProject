using UnityEngine;

public class ThunderYoukaiGroundedState : YoukaiState
{
    protected ThunderYoukai thunderYoukai;
    protected Transform player;

    public ThunderYoukaiGroundedState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, ThunderYoukai thunderYoukai) : base(Youkai, stateMachine, animBoolName)
    {
        this.thunderYoukai = thunderYoukai;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Marisa").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (thunderYoukai.IsPlayerDetected() || Vector2.Distance(thunderYoukai.transform.position, player.position) < 5)
        {
            stateMachine.ChangeState(thunderYoukai.battleState);
        }
    }
}
