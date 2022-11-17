using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MoveTowards : StateBlock
{
    public float Weight;

    public override void OnStart(Jonas_TempCharacter user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        user.MoveInput += (target.transform.position - user.transform.position).normalized * Weight;
        return (null, null);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target) { }
}
