using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StageCheck : StateBlock
{
    [SerializeField] int _stage;

    private static int _Current;
    public static int Check() => _Current;
    public override void OnEnd(EnemyController user, GameObject target)
    {
        
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        _Current = _stage;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        return (null, null);
    }
}
