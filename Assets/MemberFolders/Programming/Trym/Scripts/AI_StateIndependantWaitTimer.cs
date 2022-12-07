using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AI_StateIndependantWaitTimer : StateBlock
{
    [SerializeField] string _tag;
    enum BlockAction {Set,Reset,Check}
    [SerializeField] BlockAction _action;
    
    [ShowIf(nameof(_action),BlockAction.Set)]
    [SerializeField] float _time;
    [ShowIf(nameof(_action), BlockAction.Set)]
    [Tooltip("the block wont do anything if a timer on the same enemy with the same tag is running and this is false.")]
    [SerializeField] bool _overrideExistingTimer = false;

    [ShowIf(nameof(_action), BlockAction.Reset)]
    [SerializeField] bool _overrideTime;
    [ShowIf(nameof(_overrideTime), true)]
    [SerializeField] float _timeOverride;

    [ShowIf(nameof(_action),BlockAction.Check)]
    [SerializeField] bool _resetTimer;

    private static readonly Dictionary<(int id, string tag), (float current, float target)> _Timers = new();

    private Dictionary<int,List<float>> _argLists;

    public override void OnEnd(EnemyController user, GameObject target)
    {
        _argLists[user.GetInstanceID()] = null;
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        


        return (null, null);
    }

    private (AI_State state, List<float> val) Set(EnemyController user, float time)
    {
        
        if (_Timers.ContainsKey((user.GetInstanceID(), _tag)) && _overrideExistingTimer)
        {
            _Timers[(user.GetInstanceID(), _tag)] = (0, time);
        }
        else
        {
            _Timers.Add((user.GetInstanceID(), _tag), (0 ,time));
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
                    _argLists[id] = new List<float>();
                    _argLists[id].Add((int)StateReturn.Timer);
                    _argLists[id].Add(returnTime);
                }
                
            }

            if (_resetTimer && returnTimer.current> returnTimer.target)
            {
                Set(user, returnTimer.target);
            }

            return (null, _argLists[id]);
        }


        return (null, null);
    }

    private (AI_State state, List<float> val) Reset(EnemyController user, int id, float time)
    {
        if (_Timers.ContainsKey((id,_tag)))
        {
            user.StopCoroutine(Timer(id, _tag, _Timers[(id, _tag)].target));
            _Timers.Remove((id, _tag));
        }
        

        return (null, null);
    }


    IEnumerator Timer(int id,string tag,float time)
    {
        float startTime = Time.time;
        while (_Timers.ContainsKey((id,tag)))
        {
            
            _Timers[(id, tag)] = (Time.time - startTime, time);

            if (_Timers[(id, tag)].current >= time)
            {
                _Timers.Remove((id, tag));
            }
            yield return new WaitForEndOfFrame();
        }
        
    }


}

