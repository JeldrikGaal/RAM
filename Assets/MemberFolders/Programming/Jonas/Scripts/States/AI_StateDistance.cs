using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StateDistance : StateEffect
{
    public State NextState;

    public bool Close;
    public float Distance;

    private Jonas_TempCharacter _c;

    public override void OnEnd() { }

    public override void OnStart(GameObject user, GameObject target)
    {
        _user = user;
        _target = target;

        _c = _user.GetComponent<Jonas_TempCharacter>();
    }

    public override State OnUpdate()
    {
        if (Vector3.Distance(_user.transform.position, _target.transform.position) < Distance == Close) return NextState;

        return null;
    }
}
