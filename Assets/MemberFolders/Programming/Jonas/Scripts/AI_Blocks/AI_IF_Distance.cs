using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks distance between character and player
// Checks if actual distance is less than _distance when _close is true, and greater than if false
// If condition is not met skips next block
public class AI_IF_Distance : StateBlock
{
    [SerializeField] private bool _close;
    [SerializeField] private float _distance;

    public override void OnStart(Jonas_TempCharacter user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        if (Vector3.Distance(user.transform.position, target.transform.position) > _distance == _close)
        {
            List<float> temp = new List<float>();
            temp.Add((float)StateReturn.Skip);
            return (null, temp);
        }

        return (null, null);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target) { }
}
