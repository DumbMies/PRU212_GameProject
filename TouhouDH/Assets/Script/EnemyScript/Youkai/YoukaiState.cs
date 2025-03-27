using UnityEngine;

public class YoukaiState
{
    protected Youkai Youkai;
    protected YoukaiStateMachine stateMachine;
    protected Rigidbody2D rb;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public YoukaiState(Youkai youkai, YoukaiStateMachine stateMachine, string animBoolName)
    {
        Youkai = youkai;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = Youkai.rb;
        Youkai.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        Youkai.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
