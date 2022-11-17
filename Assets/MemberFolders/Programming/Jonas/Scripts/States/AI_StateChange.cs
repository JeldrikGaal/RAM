using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StateChange : StateBlock
{
    public AI_State NextState;

    public override void OnStart(Jonas_TempCharacter user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        return (NextState, null);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target) { }
}
