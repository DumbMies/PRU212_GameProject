using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedYokaiMoveState : RedYokaiGroundedState
{
    public RedYokaiMoveState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName) : base(_stateMachine, _redYokai, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        rb.linearVelocity = new Vector2(redYokai.moveSpeed * redYokai.facingDirection, rb.linearVelocity.y);

        if (!redYokai.IsGroundDetected() || redYokai.IsWallDetected())
        {
            redYokai.Flip();
        }

        if (isChasing)
        {
            stateMachine.ChangeState(redYokai.combatState);
        }
    }
}
