using System.Collections;
using UnityEngine;

public class WingedYoukaiAttackState : WingedYoukaiState
{
    private WingedYoukaiAI _wingedYoukaiAI;
    private float _fireDelay = .68f;

    public WingedYoukaiAttackState(WingedYoukaiAI wingedYoukaiAI, WingedYoukaiStateMachine stateMachine, string animBoolName) : base(wingedYoukaiAI, stateMachine, animBoolName)
    {
        this._wingedYoukaiAI = wingedYoukaiAI;
    }

    //public WingedYoukaiAttackState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, WingedYoukai wingedYoukai) : base(Youkai, stateMachine, animBoolName)
    //{
    //    this.wingedYoukai = wingedYoukai;
    //}

    public override void Enter()
    {
        base.Enter();
        _wingedYoukaiAI.StartCoroutine(FireBulletAfterDelay());
    }

    public override void Exit()
    {
        base.Exit();

        _wingedYoukaiAI.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        _wingedYoukaiAI.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(_wingedYoukaiAI.battleState);
        }
    }
    private IEnumerator FireBulletAfterDelay()
    {
        yield return new WaitForSeconds(_fireDelay);
        _wingedYoukaiAI.FireBullet();
    }
}
