using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks if animator is in _state
// If condition is not met skips next block
public class AI_IF_ANIM_State : StateBlock
{
    [SerializeField] private int _skipCount = 1;
    [SerializeField] private string _state;
<<<<<<< HEAD
    [SerializeField] private bool _equal;
=======
>>>>>>> b7becb6... All Local Changes _ delete Intended

    public override void OnStart(EnemyController user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
<<<<<<< HEAD
        if (user.AnimGetState(_state) == _equal)
=======
        if (user.AnimGetState(_state))
>>>>>>> b7becb6... All Local Changes _ delete Intended
            return (null, null);

        List<float> temp = new List<float>();
        temp.Add((float)StateReturn.Skip);
        temp.Add(_skipCount);
        return (null, temp);

    }

    public override void OnEnd(EnemyController user, GameObject target) { }
}
