using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StateDistance : StateEffect
{
    public State NextState;

    public bool Close;
    public float Distance;

    public override void OnStart(GameObject user, GameObject target)
    {
        _user = user;
        _target = target;
    }

    public override void OnEnd() { }

    public override State OnUpdate()
    {
        if (Vector3.Distance(_user.transform.position, _target.transform.position) < Distance == Close) return NextState;

        return null;
    }
}
