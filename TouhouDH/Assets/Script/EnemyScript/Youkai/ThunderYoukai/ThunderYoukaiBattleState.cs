using UnityEngine;

public class ThunderYoukaiBattleState : YoukaiState
{
    private Transform player;
    private ThunderYoukai thunderYoukai;
    private int moveDirection;

    public ThunderYoukaiBattleState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, ThunderYoukai thunderYoukai) : base(Youkai, stateMachine, animBoolName)
    {
        this.thunderYoukai = thunderYoukai;
    }
    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Marisa").transform;
    }

    public override void Update()
    {
        base.Update();

        if (thunderYoukai.IsPlayerDetected())
        {
            stateTimer = thunderYoukai.battleTime;

            if (thunderYoukai.IsPlayerDetected().distance < thunderYoukai.attackRange)
            {
                if (canAttack())
                    stateMachine.ChangeState(thunderYoukai.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, thunderYoukai.transform.position) > 15)
                stateMachine.ChangeState(thunderYoukai.idleState);
        }

        if (player.position.x > thunderYoukai.transform.position.x)
        {
            moveDirection = 1;
        }
        else if (player.position.x < thunderYoukai.transform.position.x)
        {
            moveDirection = -1;
        }

        thunderYoukai.SetVelocity(thunderYoukai.moveSpeed * moveDirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool canAttack()
    {
        if (Time.time >= thunderYoukai.lastTimeAttack + thunderYoukai.attackCooldown)
        {
            thunderYoukai.lastTimeAttack = Time.time;
            return true;
        }

        return false;
    }
}