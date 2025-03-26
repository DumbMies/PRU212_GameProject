using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class RedYokaiState
{
    protected bool isAirbone;
    protected bool isMoving;
    protected bool isChasing;

    protected RedYokaiStateMachine stateMachine;
    protected RedYokai redYokai;

    protected Rigidbody2D rb;
    private Transform player;

    private string animBoolName;

    protected float stateTimer;
    protected int AttackCooldown = 5000;
    protected bool triggerCalled = false;

    public RedYokaiState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.redYokai = _redYokai;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        redYokai.anim.SetBool(animBoolName, true);
        rb = redYokai.rb;
        redYokai.canAttack = true;
    }


    public virtual void Update()
    {
        isMoving = rb.linearVelocity.x != 0;
        isChasing = IsPlayerDetected();
    }

    public virtual void Exit()
    {
        redYokai.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public async void Cooldown()
    {
        await Task.Delay(AttackCooldown);
        redYokai.canAttack = true;
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.BoxCast(redYokai.transform.position, new Vector2(10f, 4f), 0, Vector2.right * redYokai.facingDirection, 1, redYokai.whatIsPlayer);
}
