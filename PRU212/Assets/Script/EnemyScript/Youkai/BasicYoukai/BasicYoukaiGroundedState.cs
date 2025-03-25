using UnityEngine;

public class BasicYoukaiGroundedState : YoukaiState
{
    protected BasicYoukai basicYoukai;
    protected Transform player;

    public BasicYoukaiGroundedState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, BasicYoukai basicYoukai) : base(Youkai, stateMachine, animBoolName)
    {
        this.basicYoukai = basicYoukai;
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

        if(basicYoukai.IsPlayerDetected() || Vector2.Distance(basicYoukai.transform.position, player.position) < 5)
        {
            stateMachine.ChangeState(basicYoukai.battleState);
        }
    }
}
