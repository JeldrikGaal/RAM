using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_CheckTime : StateBlock
{
    public float TimeSec;

    private Dictionary<Jonas_TempCharacter, float> _timer;

    public override void OnStart(Jonas_TempCharacter user, GameObject target)
    {
        if (_timer == null) _timer = new Dictionary<Jonas_TempCharacter, float>();

        _timer[user] = TimeSec;
    }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        _timer[user] -= Time.deltaTime;

        if (_timer[user] > 0)
        {
            List<float> temp = new List<float>();
            temp.Add((float)StateReturn.Skip);
            return (null, temp);
        }

        return (null, null);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target)
    {
        _timer.Remove(user);
    }
}
