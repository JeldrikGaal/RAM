using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Wait : StateBlock
{
    public float TimeSec;

    private List<float> _returnList;

    public override void OnStart(Jonas_TempCharacter user, GameObject target)
    {
        _returnList = new List<float>();
        _returnList.Add((float)StateReturn.Timer);
        _returnList.Add(TimeSec);
    }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        return (null, _returnList);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target) { }
}