using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public AI_State CurrentState;

    private Jonas_TempCharacter _user;
    private GameObject _target;

    private void Awake()
    {
        if (CurrentState == null) return;

        _user = GetComponent<Jonas_TempCharacter>();
        _target = GameObject.FindGameObjectWithTag("Player");
        CurrentState.StateStart(_user, _target);
    }

    private void Update()
    {
        AI_State nextState = CurrentState?.StateUpdate(_user, _target);

        if (nextState != null)
            NextState(nextState);
    }

    private void NextState(AI_State nextState)
    {
        CurrentState.StateEnd(_user, _target);
        CurrentState = nextState;
        CurrentState.StateStart(_user, _target);
    }
}
