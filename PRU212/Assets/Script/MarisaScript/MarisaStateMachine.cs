using UnityEngine;

public class MarisaStateMachine
{
    public MarisaState currentState { get; private set; }

    public void Initialize(MarisaState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(MarisaState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
