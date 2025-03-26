using System.Threading.Tasks;
using UnityEngine;

public class MarisaState
{
    protected MarisaStateMachine stateMachine;
    protected Marisa player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public MarisaState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }


    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        if (player.canMove)
        {
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");
            player.anim.SetFloat("yVelocity", rb.linearVelocity.y);
        }

    }
    public void TakeDamage()
    {
        if (stateMachine.currentState != player.hurtState)
        {
            Debug.Log("Currentstate = " + stateMachine.currentState);
            stateMachine.ChangeState(player.hurtState);
            Debug.Log("State after changing = " + stateMachine.currentState);
            AfterHurtState();
            Debug.Log("State after 1 second delay = " + stateMachine.currentState);

            //if (player.IsGroundDetected())
            //{
            //    stateMachine.ChangeState(player.moveState);
            //}
            //else
            //{
            //    stateMachine.ChangeState(player.airState);
            //}
        }
    }
    async void AfterHurtState()
    {
        await Task.Delay(1000);
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.moveState);
        }
        else
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
