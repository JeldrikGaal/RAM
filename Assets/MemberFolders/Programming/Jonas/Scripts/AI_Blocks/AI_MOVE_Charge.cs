using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Saves player position on start and moves in that direction
public class AI_MOVE_Charge : StateBlock
{
    [SerializeField] private float _weight;

    private Dictionary<Jonas_TempCharacter, Vector3> _moveDir;

    public override void OnStart(Jonas_TempCharacter user, GameObject target)
    {
        if (_moveDir == null) _moveDir = new Dictionary<Jonas_TempCharacter, Vector3>();

        _moveDir[user] = (target.transform.position - user.transform.position).normalized;
    }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        user.MoveInput += _moveDir[user] * _weight;
        return (null, null);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target)
    {
        _moveDir.Remove(user);
    }
}