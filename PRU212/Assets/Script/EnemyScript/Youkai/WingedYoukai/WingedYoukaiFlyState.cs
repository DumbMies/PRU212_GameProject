using UnityEngine;

public class WingedYoukaiFlyState : WingedYoukaiState
{
    private WingedYoukaiAI _wingedYoukaiAI;
    public WingedYoukaiFlyState(WingedYoukaiAI wingedYoukaiAI, WingedYoukaiStateMachine stateMachine, string animBoolName) : base(wingedYoukaiAI, stateMachine, animBoolName)
    {
        this._wingedYoukaiAI = wingedYoukaiAI;
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
        if (_wingedYoukaiAI.detectionZone)
        {
            stateMachine.ChangeState(_wingedYoukaiAI.battleState);
        }
    }
}
