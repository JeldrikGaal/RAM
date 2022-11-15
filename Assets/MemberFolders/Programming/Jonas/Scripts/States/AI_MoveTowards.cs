using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MoveTowards : StateEffect
{
    public float Weight;

    private Jonas_TempCharacter _c;

    public override void OnStart(GameObject user, GameObject target)
    {
        _user = user;
        _target = target;

        _c = _user.GetComponent<Jonas_TempCharacter>();
    }

    public override void OnEnd() { }

    public override State OnUpdate()
    {
        _c.MoveInput += (_target.transform.position - _user.transform.position).normalized * Weight;

        return null;
    }
}
