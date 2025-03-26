using UnityEngine;

public class RedYokaiCombatState : RedYokaiState
{
    public RedYokaiCombatState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName) : base(_stateMachine, _redYokai, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        redYokai.DistanceWallCheck(2f);
        isChasing = true;
    }

    public override void Update()
    {
        base.Update();
        rb.linearVelocity = new Vector2(redYokai.moveSpeed * 3f * redYokai.facingDirection, rb.linearVelocity.y); 

        if (IsPlayerDetected())
        {
            Vector2 playerPosition = IsPlayerDetected().point; 
            if (playerPosition.x < redYokai.transform.position.x && redYokai.facingDirection > 0)
            {
                redYokai.Flip(); 
            }
            else if (playerPosition.x > redYokai.transform.position.x && redYokai.facingDirection < 0)
            {
                redYokai.Flip(); 
            }
        }

        if (!IsPlayerDetected())
        {
            stateMachine.ChangeState(redYokai.moveState);
            isChasing = false;

        }
        else if (redYokai.DistanceWallCheck(1f) || !redYokai.IsGroundDetected())
        {
            redYokai.rb.linearVelocity = new Vector2(3f * redYokai.facingDirection, 7f);
            stateMachine.ChangeState(redYokai.jumpState);
        }
    }
}
