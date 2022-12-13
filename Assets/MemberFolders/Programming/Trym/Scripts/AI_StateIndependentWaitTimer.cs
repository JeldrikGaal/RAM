using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class AI_StateIndependentWaitTimer : StateBlock
{
    [SerializeField] string _tag;
    [SerializeField] bool _fromStats;
    [HideIf(nameof(_fromStats))]
    [SerializeField] float _time;
    [ShowIf(nameof(_fromStats))]
    [SerializeField] private string _attackName;
    [ShowIf(nameof(_fromStats))]
    [SerializeField] private AI_TIMER_Stat.TimeType _timeType;

    private readonly Dictionary<int, (float time, float start)> _times = new();

    public override void OnEnd(EnemyController user, GameObject target)
    {
        

        int id = user.GetInstanceID();
        
        (_, float start) = _times[id];
        

        _times[id] = (_time - (Time.time - start),start);

        //Debug.Log("OnEnd: " + _times[id].time.ToString());
    }

    private readonly Dictionary<int,List<float>> _returnLists = new();

    public override void OnStart(EnemyController user, GameObject target)
    {
        user.DoClean(this, Cleanup);
        if (_fromStats)
        {
            if (_fromStats)
            {
                switch (_timeType)
                {
                    case AI_TIMER_Stat.TimeType.Anticipation:
                        _time = user.Stats.GetStats(_attackName).AnticipationTime;
                        break;
                    case AI_TIMER_Stat.TimeType.Attack:
                        _time = user.Stats.GetStats(_attackName).AttackTime;
                        break;
                    case AI_TIMER_Stat.TimeType.Recovery:
                        _time = user.Stats.GetStats(_attackName).RecoveryTime;
                        break;
                    default:
                        break;
                }
            }
        }
        
        

        int id = user.GetInstanceID();
        if (!_returnLists.ContainsKey(id))
        {
            _returnLists.Add(id, new());
        }
        if (_times.ContainsKey(id))
        {
            if (_times[id].time <=0)
            {
                _times[id] = ( time: _time,start: Time.time);
            }
            //Debug.Log("OnStart: "+ _times[id].time.ToString());
            _returnLists[id] = new();
            _returnLists[id].Add((float)StateReturn.Timer);
            _returnLists[id].Add(_times[id].time);

        }
        else
        {
            _times.Add(id, (_time, Time.time));
            _returnLists[id] = new();
            _returnLists[id].Add((float)StateReturn.Timer);
            _returnLists[id].Add(_time);
        }
        
    }

    private void Cleanup(EnemyController obj)
    {
        _returnLists.Remove(obj.GetInstanceID());
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        
        return (null, _returnLists[user.GetInstanceID()]);
    }

    
}
