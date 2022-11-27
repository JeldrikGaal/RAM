using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Changes state to NextState
// Make sure to use timers with this one to avoid instant swaps between states
public class AI_StateChange : StateBlock
{
    [SerializeField] private AI_State _nextState;

    public override void OnStart(EnemyController user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        return (_nextState, null);
    }

    public override void OnEnd(EnemyController user, GameObject target) { }
}
