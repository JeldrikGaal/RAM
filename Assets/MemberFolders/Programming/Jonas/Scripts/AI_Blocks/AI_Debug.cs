using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks height distance between character and player
// If condition is not met skips next block
public class AI_Debug : StateBlock
{
    [SerializeField] private string _msg;
    [SerializeField] private bool _inUpdate;

    public override void OnStart(EnemyController user, GameObject target)
    {
        Debug.Log($"Start: {_msg}");
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (_inUpdate)
            Debug.Log($"Update: {_msg}");

        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {
        Debug.Log($"End: {_msg}");
    }
}
