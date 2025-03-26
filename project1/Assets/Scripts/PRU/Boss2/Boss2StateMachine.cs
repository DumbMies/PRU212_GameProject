using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Boss2StateMachine
{
    public Boss2State currentState { get; private set; }

    public void Initialize(Boss2State startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(Boss2State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
