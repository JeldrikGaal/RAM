using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MoveCharge : StateBlock
{
    public float Weight;

    private Dictionary<Jonas_TempCharacter, Vector3> _moveDir;

    public override void OnStart(Jonas_TempCharacter user, GameObject target)
    {
        _moveDir[user] = (target.transform.position - user.transform.position).normalized;
    }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        user.MoveInput += _moveDir[user] * Weight;
        return (null, null);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target)
    {
        _moveDir.Remove(user);
    }
}