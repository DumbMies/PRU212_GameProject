using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedYokaiGroundedState : RedYokaiState
{
    public RedYokaiGroundedState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName) : base(_stateMachine, _redYokai, _animBoolName)
    {
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
    }
}
