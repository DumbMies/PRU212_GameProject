using UnityEngine;

public class WingedYoukaiBattleState : WingedYoukaiState
{
    //private Transform player;
    private WingedYoukaiAI _wingedYoukaiAI;
    //private int moveDirection;

    public WingedYoukaiBattleState(WingedYoukaiAI wingedYoukaiAI, WingedYoukaiStateMachine stateMachine, string animBoolName) : base(wingedYoukaiAI, stateMachine, animBoolName)
    {
        this._wingedYoukaiAI = wingedYoukaiAI;
    }

    //public WingedYoukaiBattleState(Youkai Youkai, YoukaiStateMachine stateMachine, string animBoolName, WingedYoukai wingedYoukai) : base(Youkai, stateMachine, animBoolName)
    //{
    //    this.wingedYoukai = wingedYoukai;
    //}
    public override void Enter()
    {
        base.Enter();

        //player = GameObject.Find("Marisa").transform;
    }

    public override void Update()
    {
        base.Update();

        if (_wingedYoukaiAI.detectionZone)
        {
            stateTimer = _wingedYoukaiAI.battleTime;

            float distanceToTarget = Vector2.Distance(_wingedYoukaiAI.transform.position, _wingedYoukaiAI.target.position);

            if (distanceToTarget < _wingedYoukaiAI.attackRange)
            {
                if (canAttack())
                    stateMachine.ChangeState(_wingedYoukaiAI.attackState);
            }
        }
        else
        {
            if (!_wingedYoukaiAI.detectionZone)
                stateMachine.ChangeState(_wingedYoukaiAI.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool canAttack()
    {
        if (Time.time >= _wingedYoukaiAI.lastTimeAttack + _wingedYoukaiAI.attackCooldown)
        {
            _wingedYoukaiAI.lastTimeAttack = Time.time;
            return true;
        }

        return false;
    }
}