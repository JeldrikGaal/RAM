using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State CurrentState;

    private void Awake()
    {
        CurrentState.StateStart();
    }

    private void Update()
    {
        State nextState = CurrentState?.StateUpdate();

        if (nextState != null)
            NextState(nextState);
    }

    private void NextState(State nextState)
    {
        CurrentState.StateEnd();
        CurrentState = nextState;
        CurrentState.StateStart();
    }
}
