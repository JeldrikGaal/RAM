using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Heal : StateBlock
{
    [SerializeField] private bool _fullHeal;
    [SerializeField] private float _healAmount;
    public override void OnStart(EnemyController user, GameObject target)
    {

    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (_fullHeal)
        {
            user.Health = user.Stats.GetHealth(1); ;
        }
        else
        {
            user.Health += _healAmount;
        }
        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {

    }
}
