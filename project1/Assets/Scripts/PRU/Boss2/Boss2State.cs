using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Boss2State
{
    protected bool isAirbone;
    protected bool isMoving;
    protected bool isChasing;
    protected bool spearThrown;

    protected Boss2StateMachine stateMachine;
    protected Boss2 boss2;

    protected Rigidbody2D rb;
    private Transform player;

    private string animBoolName;

    protected float stateTimer;
    protected int AttackCooldown = 5000;
    protected bool triggerCalled = false;

    public Boss2State(Boss2StateMachine _stateMachine, Boss2 _boss2, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.boss2 = _boss2;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        boss2.anim.SetBool(animBoolName, true);
        rb = boss2.rb;
        boss2.canAttack = true;
    }


    public virtual void Update()
    {
        isMoving = rb.linearVelocity.x != 0;
        isChasing = IsPlayerDetected();
    }

    public virtual void Exit()
    {
        boss2.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public async void Cooldown()
    {
        await Task.Delay(AttackCooldown);
        boss2.canAttack = true;
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.BoxCast(boss2.transform.position, new Vector2(50f, 20f), 0, Vector2.right * boss2.facingDirection, 1, boss2.whatIsPlayer);
}
