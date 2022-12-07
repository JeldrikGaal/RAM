using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SimplifiedStateIndependentWaitTimer : StateBlock
{
    [SerializeField] string _tag;
    [SerializeField] float _time;

    private Dictionary<int, (float time, float start)> _times = new();

    public override void OnEnd(EnemyController user, GameObject target)
    {
        int id = user.GetInstanceID();
        Debug.Log(id);
        (_, float start) = _times[id];
        Debug.Log(_time);
        Debug.Log(_times[id]);

        _times[id] = (_time - (Time.time - start),start);
        Debug.Log(_times[id]);

    }

    private Dictionary<int,List<float>> _returnLists = new();

    public override void OnStart(EnemyController user, GameObject target)
    {
        
        int id = user.GetInstanceID();
        Debug.Log(id);
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
