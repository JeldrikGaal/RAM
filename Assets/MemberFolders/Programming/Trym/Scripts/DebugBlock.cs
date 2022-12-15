using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBlock : StateBlock
{

    [SerializeField] string _logOnStart, _logOnUpdate, _logOnEnd;
    [SerializeField] bool _breakOnStart, _breakOnUpdate, _breakOnEnd , _returnStop;

    private bool _broke = false;
    public override void OnEnd(EnemyController user, GameObject target)
    {
        if (_logOnEnd != "")
        {
            Debug.Log(_logOnEnd);
        }
        if (_breakOnEnd)
        {
            Debug.Break();
        }
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        _broke = false;
        if (_logOnStart != "")
        {
            Debug.Log(_logOnStart);
        }
        if (_breakOnStart)
        {
            Debug.Break();
        }
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (_logOnStart != "")
        {
            Debug.Log(_logOnStart);
        }
        if (_breakOnUpdate && !_broke)
        {
            Debug.Break();
            _broke = true;
        }
        if (_returnStop)
        {
            return (null, new(new[] { (float)StateReturn.Stop }));
        }
        return (null, null);
    }

    
}
