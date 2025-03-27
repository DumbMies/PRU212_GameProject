using UnityEngine;

public class BasicYoukaiBattleState : YoukaiState
{
    private Transform player;
    private BasicYoukai basicYoukai;
    private int moveDirection;

    public BasicYoukaiBattleState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, BasicYoukai basicYoukai) : base(Youkai, stateMachine, animBoolName)
    {
        this.basicYoukai = basicYoukai;
    }
    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Marisa").transform;
    }

    public override void Update()
    {
        base.Update();

        if(basicYoukai.IsPlayerDetected())
        {
            stateTimer = basicYoukai.battleTime;

            if (basicYoukai.IsPlayerDetected().distance < basicYoukai.attackRange)
            {
                if (canAttack())
                    stateMachine.ChangeState(basicYoukai.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, basicYoukai.transform.position) > 15)
                stateMachine.ChangeState(basicYoukai.idleState);
        }

        if (player.position.x > basicYoukai.transform.position.x)
        {
            moveDirection = 1;
        }
        else if (player.position.x < basicYoukai.transform.position.x)
        {
            moveDirection = -1;
        }

        basicYoukai.SetVelocity(basicYoukai.moveSpeed * moveDirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool canAttack()
    {
        if (Time.time >= basicYoukai.lastTimeAttack + basicYoukai.attackCooldown)
        {
            basicYoukai.lastTimeAttack = Time.time;
            return true;
        }

        return false;
    }
}