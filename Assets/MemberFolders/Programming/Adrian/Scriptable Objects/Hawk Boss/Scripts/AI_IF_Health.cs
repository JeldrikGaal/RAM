using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_IF_Health : StateBlock
{
    [SerializeField] private bool _over;
    [SerializeField] private float _health;
    [SerializeField] private int _skipCount;


    public override void OnStart(EnemyController user, GameObject target)
    {

    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (user.Health > _health == _over)
        {
            return (null, null);
        }

        List<float> temp = new List<float>();
        temp.Add((float)StateReturn.Skip);
        temp.Add(_skipCount);
        return (null, temp);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {

    }

}
