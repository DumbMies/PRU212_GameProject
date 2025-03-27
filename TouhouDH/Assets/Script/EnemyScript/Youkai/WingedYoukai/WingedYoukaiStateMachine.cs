using UnityEngine;

public class WingedYoukaiStateMachine
{
    public WingedYoukaiState currentState { get; private set; }

    public void Initialize(WingedYoukaiState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(WingedYoukaiState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
