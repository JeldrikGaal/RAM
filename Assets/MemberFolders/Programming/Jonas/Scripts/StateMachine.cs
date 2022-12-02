using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base state machine class, should be put on everything that uses the AI system
// Takes in the characters starting state, CurrentState
public class StateMachine : MonoBehaviour
{
    [SerializeField] private AI_State _startState;

    [SerializeField] private AI_State _currentState;

    private EnemyController _user;
    private GameObject _target;

    private bool _reset = false;

    private void Awake()
    {
        if (_startState == null) return;
        _currentState = _startState;

        _user = GetComponent<EnemyController>();
        _target = GameObject.FindGameObjectWithTag("Player");
        _currentState.StateStart(_user, _target);
    }

    private void Update()
    {
        AI_State nextState = _currentState?.StateUpdate(_user, _target);

        if (_reset)
        {
            _reset = false;
            NextState(_startState);
            return;
        }

        if (nextState != null)
            NextState(nextState);
    }

    // If the next state is "Reset" return to starting state
    private void NextState(AI_State nextState)
    {
        _currentState.StateEnd(_user, _target);

        if (nextState.name == "Reset")
            _currentState = _startState;
        else
            _currentState = nextState;

        _currentState.StateStart(_user, _target);
    }

    public void ResetState()
    {
        _reset = true;
    }
}
