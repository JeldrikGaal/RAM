using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Wait_UntilExternalCall : StateBlock
{
    private readonly Dictionary<int, bool> _trigger = new();
    public override void OnEnd(EnemyController user, GameObject target)
    {
        _trigger[user.GetInstanceID()] = false;
    }
    
    public void ExternalCall(EnemyController user)
    {
        _trigger[user.GetInstanceID()] = true;
    }
    public override void OnStart(EnemyController user, GameObject target)
    {
        user.DoOnDie(this, OnDie);
        if (!_trigger.ContainsKey(user.GetInstanceID()))
        {
            _trigger.Add(user.GetInstanceID(), false);
        }
        
    }

    private void OnDie(EnemyController obj)
    {
        _trigger.Remove(obj.GetInstanceID());
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (_trigger[user.GetInstanceID()])
        {
            
            return (null, null);
        }
        return (null, new(new[] { (float)StateReturn.Stop}));
    }

    
}
