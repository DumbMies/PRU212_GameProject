using UnityEngine;

public class WingedYoukaiState
{
    protected WingedYoukaiAI WingedYoukaiAI;
    protected WingedYoukaiStateMachine stateMachine;
    protected string animBoolName;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;

    protected WingedYoukaiState(WingedYoukaiAI wingedYoukaiAI, WingedYoukaiStateMachine stateMachine, string animBoolName)
    {
        WingedYoukaiAI = wingedYoukaiAI;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() 
    {
        triggerCalled = false;
        rb = WingedYoukaiAI.rb;
        WingedYoukaiAI.anim.SetBool(animBoolName, true);
    }
    public virtual void Update() 
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit() 
    {
        WingedYoukaiAI.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
