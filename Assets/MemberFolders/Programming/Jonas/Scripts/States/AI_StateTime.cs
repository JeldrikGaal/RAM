using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StateTime : StateEffect
{
    public State NextState;

    public float TimeSec;

    private float _timer;

    public override void OnStart(GameObject user, GameObject target)
    {
        _user = user;
        _target = target;

        _timer = TimeSec;
    }

    public override void OnEnd() { }

    public override State OnUpdate()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0) return NextState;
        return null;
    }
}
