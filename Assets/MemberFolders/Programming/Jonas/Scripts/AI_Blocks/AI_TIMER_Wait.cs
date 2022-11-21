using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TIMER_Wait : StateBlock
{
    [SerializeField] private float _timeSec;

    private List<float> _returnList;

    public override void OnStart(Jonas_TempCharacter user, GameObject target)
    {
        _returnList = new List<float>();
        _returnList.Add((float)StateReturn.Timer);
        _returnList.Add(_timeSec);
    }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        return (null, _returnList);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target) { }
}