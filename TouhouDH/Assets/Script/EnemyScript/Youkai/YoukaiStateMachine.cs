using UnityEngine;

public class YoukaiStateMachine
{
    public YoukaiState currentState { get; private set; }

    public void Initialize(YoukaiState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(YoukaiState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
