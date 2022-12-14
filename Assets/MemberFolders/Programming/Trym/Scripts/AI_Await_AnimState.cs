using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Await_AnimState : StateBlock
{
    [SerializeField] private string _state;
    [SerializeField] private bool _equal;

    public override void OnStart(EnemyController user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (user.AnimGetState(_state) == _equal)
            return (null, null);

        
        return (null, new(new[] {(float)StateReturn.Stop }));

    }

    public override void OnEnd(EnemyController user, GameObject target) { }
}
