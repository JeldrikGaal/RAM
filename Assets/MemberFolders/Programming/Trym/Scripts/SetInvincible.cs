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
    private readonly Dictionary<int, bool> _updateds = new();
    public override void OnEnd(EnemyController user, GameObject target)
    {
        if (_changeOnEnd)
        {
            user.SetInviciblity(Invincible);
        }
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        if (_changeOnStart)
        {
            user.SetInviciblity(_invincible)
        }
        if (_changeOnUpdate)
        {
            _updateds.Add(user.GetInstanceID(), false);
        }
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        int id = user.GetInstanceID();
        if (_changeOnUpdate && !_updateds[id])
        {
            user.SetInviciblity(invincible);
        }
        return (null, null);
    }

    
}
