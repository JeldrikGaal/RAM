using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_IF_StateRepeatCount : StateBlock
{
    [SerializeField] int _count;
    //[SerializeField] bool _invert;
    [SerializeField] int _scipCount;
    private readonly Dictionary<int, int> _tallys = new();
    public override void OnEnd(EnemyController user, GameObject target)
    {
        _tallys[user.GetInstanceID()]++;
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        if (!_tallys.ContainsKey(user.GetInstanceID()))
        {
            _tallys.Add(user.GetInstanceID(), 0);
        }
        user.DoOnDie(this, OnDie);

    }

    private void OnDie(EnemyController obj)
    {
        _tallys.Remove(obj.GetInstanceID());
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (_tallys[user.GetInstanceID()] >= _count )
        {
            _tallys[user.GetInstanceID()] = 0;
            return (null, null);
        }
        return (null, new(new[] { (float)StateReturn.Skip, _scipCount }));
    }
}
