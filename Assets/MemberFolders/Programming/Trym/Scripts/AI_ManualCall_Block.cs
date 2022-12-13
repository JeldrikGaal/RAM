using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ManualCall_Block : StateBlock
{
    [SerializeField] private int _number;

    private readonly Dictionary<int, AI_ManualCallComponent> _manualCallComponents = new();
    public override void OnEnd(EnemyController user, GameObject target)
    {
        _manualCallComponents.Remove(user.GetInstanceID());
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        _manualCallComponents.Add(user.GetInstanceID(), user.GetComponent<AI_ManualCallComponent>());
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        _manualCallComponents[user.GetInstanceID()].Call(_number);
        return (null, null);
    }
}
