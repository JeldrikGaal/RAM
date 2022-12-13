using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TIMER_Stat : StateBlock
{
    private List<float> _returnList;

    [SerializeField] private string _attackName;
    [SerializeField] private TimeType _timeType;

    public override void OnStart(EnemyController user, GameObject target)
    {
        _returnList = new List<float>();
        _returnList.Add((float)StateReturn.Timer);

        switch (_timeType)
        {
            case TimeType.Anticipation:
                Debug.Log($"{user.gameObject.name}/{_attackName}:{user.Stats.GetStats(_attackName).AnticipationTime}");
                _returnList.Add(user.Stats.GetStats(_attackName).AnticipationTime);
                break;
            case TimeType.Attack:
                Debug.Log($"{user.gameObject.name}/{_attackName}:{user.Stats.GetStats(_attackName).AttackTime}");
                _returnList.Add(user.Stats.GetStats(_attackName).AttackTime);
                break;
            case TimeType.Recovery:
                Debug.Log($"{user.gameObject.name}/{_attackName}:{user.Stats.GetStats(_attackName).RecoveryTime}");
                _returnList.Add(user.Stats.GetStats(_attackName).RecoveryTime);
                break;
            default:
                break;
        }
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        return (null, _returnList);
    }

    public override void OnEnd(EnemyController user, GameObject target) { }

    public enum TimeType
    {
        Anticipation,
        Attack,
        Recovery
    }
}