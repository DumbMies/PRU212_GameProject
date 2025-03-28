using UnityEngine;
using System.Linq;

public class Boss2TransformState : Boss2State
{

    public Boss2TransformState(Boss2StateMachine stateMachine, Boss2 boss2, string animBoolName)
        : base(stateMachine, boss2, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        boss2.DemonCircleAnim = boss2.DemonCircleAnimator.GetComponent<Animator>();

        // Disable gravity and movement
        boss2.rb.gravityScale = 0;
        boss2.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            triggerCalled = false;
            boss2.anim.SetBool("Transform", false);
            stateMachine.ChangeState(boss2.dashState);
        }
    }

    public override void Exit()
    {

    }
}
