using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class RedYokaiStateMachine
{
    public RedYokaiState currentState { get; private set; }

    public void Initialize(RedYokaiState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(RedYokaiState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
