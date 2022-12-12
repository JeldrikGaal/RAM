using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StateIndependentWaitTimer : StateBlock
{
    [SerializeField] string _tag;
    [SerializeField] float _time;

    private readonly Dictionary<int, (float time, float start)> _times = new();

    public override void OnEnd(EnemyController user, GameObject target)
    {
        int id = user.GetInstanceID();
        
        (_, float start) = _times[id];
        

        _times[id] = (_time - (Time.time - start),start);

        Debug.Log("OnEnd: " + _times[id].time.ToString());
    }

    private readonly Dictionary<int,List<float>> _returnLists = new();

    public override void OnStart(EnemyController user, GameObject target)
    {
        
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
            Debug.Log("OnStart: "+ _times[id].time.ToString());
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

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        Debug.Log(user.GetInstanceID());
        return (null, _returnLists[user.GetInstanceID()]);
    }

    
}
