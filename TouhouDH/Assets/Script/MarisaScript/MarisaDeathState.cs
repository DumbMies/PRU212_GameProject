using UnityEditor.Sequences;
using UnityEngine;

public class MarisaDeathState : MarisaState
{
    public MarisaDeathState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.fx.StartCoroutine(player.fx.FlashFx());
    }

    //public override void Exit()
    //{
    //    base.Exit();
    //}

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    public override bool CanTakeDamage()
    {
        return false;
    }
}
