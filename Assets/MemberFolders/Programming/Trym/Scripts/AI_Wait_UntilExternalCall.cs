using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Wait_UntilExternalCall : StateBlock
{
    Dictionary<int, bool> _trigger = new();
    public override void OnEnd(EnemyController user, GameObject target)
    {
        
    }
    
    public void ExternalCall(EnemyController user)
    {
        _trigger[user.GetInstanceID()] = true;
    }
    public override void OnStart(EnemyController user, GameObject target)
    {
        user.DoOnDie(this, OnDie);
        
    }

    private void OnDie(EnemyController obj)
    {
        _trigger.Remove(obj.GetInstanceID());
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (_trigger[user.GetInstanceID()])
        {
            _trigger[user.GetInstanceID()] = false;
            return (null, null);
        }
        return (null, new(new[] { (float)StateReturn.Timer, float.PositiveInfinity }));
    }

    
}
