using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_CheckDistance : StateBlock
{
    public bool Close;
    public float Distance;

    public override void OnStart(Jonas_TempCharacter user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        if (Vector3.Distance(user.transform.position, target.transform.position) > Distance == Close)
        {
            List<float> temp = new List<float>();
            temp.Add((float)StateReturn.Skip);
            return (null, temp);
        }

        return (null, null);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target) { }
}
