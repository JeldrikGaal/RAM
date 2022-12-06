using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AttackShootWolf : StateBlock
{
    [SerializeField] private float _damage;
    [SerializeField] private float _dmgWeight;

    private Dictionary<EnemyController, bool> _isDone;

    // Enables or disables attack with the name _attackName in the Attacks child object of user
    public override void OnStart(EnemyController user, GameObject target)
    {
        if (_isDone == null) _isDone = new Dictionary<EnemyController, bool>();

        _isDone[user] = false;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_isDone[user])
        {
            //atk.GetComponent<EnemyAttack>().Init(_damage != 0 ? _damage : (user.AttackDamage * _dmgWeight));
            if (user.GetComponent<WolfRangedAttack>().CheckNearbyWolfs())
            {
                user.GetComponent<WolfRangedAttack>().ThrowWolf(target);
            }
            else
            {
                user.GetComponent<WolfRangedAttack>().ThrowBoomerang(target);
            }

            _isDone[user] = true;
        }
        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {
        _isDone.Remove(user);
    }
}
