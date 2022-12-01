using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AttackManual : StateBlock
{
    [SerializeField] private string _attackName;

    private Dictionary<EnemyController, bool> _isDone;

    // Manually disables attack with the name _attackName in the Attacks child object of user
    public override void OnStart(EnemyController user, GameObject target)
    {
        if (_isDone == null) _isDone = new Dictionary<EnemyController, bool>();

        _isDone[user] = false;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_isDone[user])
        {
            GameObject atk = user.transform.Find($"Model/Attacks/{_attackName}").gameObject;
            atk.SetActive(false);
            _isDone[user] = true;
        }

        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {
        _isDone.Remove(user);
    }
}
