using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks distance between character and player
// Checks if actual distance is less than _distance when _close is true, and greater than if false
// If condition is not met skips next block
public class AI_IF_Random : StateBlock
{
    [SerializeField] private int _skipCount = 1;

    public override void OnStart(EnemyController user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {

        List<float> temp = new List<float>();
        temp.Add((float)StateReturn.Skip);
        temp.Add(_skipCount);

        var random = Random.Range(0, 2);
        if (random == 0)
        {
            return (null, temp);
        }
        else
        {
            return (null, null);
        }
    }

    public override void OnEnd(EnemyController user, GameObject target) { }
}
