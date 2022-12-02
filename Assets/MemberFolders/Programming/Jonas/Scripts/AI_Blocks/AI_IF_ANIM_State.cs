using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks if animator is in _state
// If condition is not met skips next block
public class AI_IF_ANIM_State : StateBlock
{
    [SerializeField] private int _skipCount = 1;
    [SerializeField] private string _state;

    public override void OnStart(EnemyController user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (user.AnimGetState(_state))
            return (null, null);

        List<float> temp = new List<float>();
        temp.Add((float)StateReturn.Skip);
        temp.Add(_skipCount);
        return (null, temp);

    }

    public override void OnEnd(EnemyController user, GameObject target) { }
}
