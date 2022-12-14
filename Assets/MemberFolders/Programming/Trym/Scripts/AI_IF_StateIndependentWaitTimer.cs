using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_IF_StateIndependentWaitTimer : StateBlock
{

    [SerializeField] string _tag;
    [SerializeField] bool _fromStats;
    [HideIf(nameof(_fromStats))]
    [SerializeField] float _time;
    [ShowIf(nameof(_fromStats))]
    [SerializeField] private string _attackName;
    [ShowIf(nameof(_fromStats))]
    [SerializeField] private AI_TIMER_Stat.TimeType _timeType;
    [SerializeField] int _skipCount = 1;
    [SerializeField] bool _invert = false;
    [SerializeField] private bool _resetOnStateStart = false;

    private readonly Dictionary<int,float> _startTimes = new();
    
    public override void OnEnd(EnemyController user, GameObject target)
    {

        
        

        
    }

    public override void OnStart(EnemyController user, GameObject target)
    {

        if (_resetOnStateStart && _startTimes.ContainsKey(user.GetInstanceID()))
        {
            _startTimes[user.GetInstanceID()] = Time.time;
        }

        user.DoOnDie(this, Cleanup);
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

    private void Cleanup(EnemyController obj)
    {
        _startTimes.Remove(obj.GetInstanceID());
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        
        int id = user.GetInstanceID();
        if (_startTimes.ContainsKey(id))
        {
            if ((_time - (Time.time - _startTimes[id]) <= 0) == !_invert)
            {
                if (!_invert)
                {
                    _startTimes[id] = Time.time;
                }
                
                return (null, null);
            }
            else
            {
                if (_invert)
                {
                    _startTimes[id] = Time.time;
                }
                return (null, new(new[] { (float)StateReturn.Skip, _skipCount }));
            }
        }
        else
        {
            _startTimes.Add(id, Time.time);
            return _invert?(null,null) : (null, new(new[] { (float)StateReturn.Skip, _skipCount }));
        }
        
    }

    /*
    [SerializeField] string _tag;
    [EnumToggleButtons] enum BlockAction { Set, Reset, Check }
    [SerializeField] BlockAction _action;

    [ShowIf(nameof(_action), BlockAction.Set)]
    [SerializeField] float _time;
    [ShowIf(nameof(_action), BlockAction.Set)]
    [Tooltip("the block wont do anything if a timer on the same enemy with the same tag is running and this is false.")]
    [SerializeField] bool _overrideExistingTimer = false;

    [ShowIf(nameof(_action), BlockAction.Reset)]
    [SerializeField] bool _overrideTime;
    [ShowIf(nameof(_overrideTime), true)]
    [SerializeField] float _timeOverride;

    [ShowIf(nameof(_action), BlockAction.Check)]
    [SerializeField] bool _resetTimer;

    private static readonly Dictionary<(int id, string tag), (float current, float target)> _Timers = new();

    private readonly Dictionary<int, List<float>> _argLists = new();
    private readonly Dictionary<int, bool> _setRan = new();
    public override void OnEnd(EnemyController user, GameObject target)
    {
        _argLists[user.GetInstanceID()] = null;
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        var id = user.GetInstanceID();
        if (!_setRan.ContainsKey(id))
        {
            _setRan.Add(id, false);
        }
        else
        {
            _setRan[id] = false;
        }
        _argLists[id] = null;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        switch (_action)
        {
            case BlockAction.Set:
                return Set(user, _time);

            case BlockAction.Reset:
                return Reset(user);

            case BlockAction.Check:
                return Check(user);

            default:
                return (null, null);
        }


        return (null, null);
    }

    private (AI_State state, List<float> val) Set(EnemyController user, float time)
    {

        var id = user.GetInstanceID();



        if (!_setRan[id])
        {
            if (_Timers.ContainsKey((id, _tag)))
            {
                if (_overrideExistingTimer || _Timers[(id, _tag)].target - _Timers[(id, _tag)].current <= 0)
                {
                    _Timers[(id, _tag)] = (0, time);
                    user.StartCoroutine(Timer(id, _tag, time));
                }

            }
            else
            {
                _Timers.Add((id, _tag), (0, time));
                user.StartCoroutine(Timer(id, _tag, time));
            }
            _setRan[id] = true;
        }

        return (null, null);
    }

    private (AI_State state, List<float> val) Check(EnemyController user)
    {
        int id = user.GetInstanceID();
        if (_Timers.ContainsKey((id, _tag)))
        {
            var returnTimer = _Timers[(id, _tag)];
            if (_argLists[id] == null)
            {

                var returnTime = returnTimer.target - returnTimer.current;

                if (returnTime > 0)
                {
                    _argLists[id] = new();
                    _argLists[id].Add((float)StateReturn.Timer);
                    _argLists[id].Add(returnTime);
                }
                if (_resetTimer && returnTimer.current >= returnTimer.target)
                {
                    _overrideExistingTimer = false;
                    Reset(user);

                }
            }

            Debug.Log(_argLists);

            return (null, _argLists[id]);
        }


        return (null, _argLists[id]);
    }

    private (AI_State state, List<float> val) Reset(EnemyController user)
    {
        int id = user.GetInstanceID();
        if (_Timers.ContainsKey((id, _tag)))
        {
            user.StopCoroutine(Timer(id, _tag, _Timers[(id, _tag)].target));
            _Timers[(id, _tag)] = (0, _overrideTime ? _timeOverride : _Timers[(id, _tag)].target);
            user.StartCoroutine(Timer(id, _tag, _Timers[(id, _tag)].target));
        }
        return (null, null);
    }


    IEnumerator Timer(int id, string tag, float time)
    {
        float startTime = Time.time;
        float curTime;
        while (_Timers.ContainsKey((id, tag)))
        {
            Debug.Log(curTime = Time.time - startTime);
            _Timers[(id, tag)] = (current: curTime, target: time);

            if (curTime >= time)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

    }
    */

}




