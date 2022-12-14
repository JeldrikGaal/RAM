using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ANIM_SetTrigger : StateBlock
{
    [SerializeField] private string _name;

    private Dictionary<EnemyController, bool> _isDone;

    public override void OnStart(EnemyController user, GameObject target)
    {
        if (_isDone == null) _isDone = new Dictionary<EnemyController, bool>();

        _isDone[user] = false;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_isDone.ContainsKey(user)) return (null, null);
        if (!_isDone[user])
        {
            _isDone[user] = true;            user.AnimSetTrigger(_name);
        }

        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {
        _isDone.Remove(user);
    }
}
