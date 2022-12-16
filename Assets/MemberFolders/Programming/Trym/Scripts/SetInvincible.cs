using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInvinsebility : StateBlock
{
    [SerializeField] bool _changeOnStart;
    [SerializeField] bool _invincible;
    [SerializeField] bool _changeOnUpdate;
    [SerializeField] bool invincible;
    [SerializeField] bool _changeOnEnd;
    [SerializeField] bool Invincible;
    public override void OnEnd(EnemyController user, GameObject target)
    {
        throw new System.NotImplementedException();
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        throw new System.NotImplementedException();
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        throw new System.NotImplementedException();
    }

    
}
