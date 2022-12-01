using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ANIM_SetFloat : StateBlock
{
    [SerializeField] private string _name;
    [SerializeField] private float _value;

    private Dictionary<EnemyController, bool> _isDone;

    public override void OnStart(EnemyController user, GameObject target)
    {
        if (_isDone == null) _isDone = new Dictionary<EnemyController, bool>();

        _isDone[user] = false;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_isDone[user])
        {
            user.AnimSetFloat(_name, _value);
        }

        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {
        _isDone.Remove(user);
    }
}
