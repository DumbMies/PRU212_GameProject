using UnityEngine;

public class RedYokaiHurtState : RedYokaiState
{
    private float hurtDuration = 0.5f;
    private float elapsedTime;

    public RedYokaiHurtState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName)
        : base(_stateMachine, _redYokai, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        elapsedTime = 0f;
    }

    public override void Update()
    {
        base.Update();

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= hurtDuration)
        {
            if (redYokai.IsGroundDetected()) {
                stateMachine.ChangeState(redYokai.moveState);
            }
            if (redYokai.currentHealth <= 0 && redYokai.IsGroundDetected())
            {
                stateMachine.ChangeState(redYokai.dieState);
            }
        }

    }
}
