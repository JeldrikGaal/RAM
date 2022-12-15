using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks distance between character and player
// Checks if actual distance is less than _distance when _close is true, and greater than if false
// If condition is not met skips next block
public class AI_IF_Distance : StateBlock
{
    [SerializeField] private int _skipCount = 1;
    [SerializeField] private bool _close;
    [SerializeField] private float _distance;

    public override void OnStart(EnemyController user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (user == null) return (null, null);
        if (Vector3.Distance(user.transform.position, target.transform.position) < _distance == _close)
            return (null, null);

        List<float> temp = new List<float>();
        temp.Add((float)StateReturn.Skip);
        temp.Add(_skipCount);
        return (null, temp);

    }

    public override void OnEnd(EnemyController user, GameObject target) { }
}
