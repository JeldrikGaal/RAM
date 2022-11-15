using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MoveCharge : StateEffect
{
    public float Weight;

    private Jonas_TempCharacter _c;
    private Vector3 _moveDir;

    public override void OnEnd() { }

    public override void OnStart(GameObject user, GameObject target)
    {
        _user = user;
        _target = target;

        _c = _user.GetComponent<Jonas_TempCharacter>();

        _moveDir = (_target.transform.position - _user.transform.position).normalized;
    }

    public override State OnUpdate()
    {
        _c.MoveInput += _moveDir * Weight;

        return null;
    }
}
