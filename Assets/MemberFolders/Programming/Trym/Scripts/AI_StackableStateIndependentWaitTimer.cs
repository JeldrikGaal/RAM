using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StackableStateIndependentWaitTimer : StateBlock
{
    [SerializeField] string _context;
    [SerializeField] float _time;
    [SerializeField] bool _autoStart;
    private static readonly Dictionary<(string context, int id),(int current , List<(float time, float start)> blocks)> _Times = new();
    private readonly Dictionary<int, bool> _didUpdate;
    public override void OnEnd(EnemyController user, GameObject target)
    {
        if (user.DoDie)
        {
            return;
        }

        int id = user.GetInstanceID();
        
        (_, float start) = _Times[id];
        

        _Times[id] = (_time - (Time.time - start),start);

        //Debug.Log("OnEnd: " + _times[id].time.ToString());
    }

    private readonly Dictionary<int,List<float>> _returnLists = new();

    public override void OnStart(EnemyController user, GameObject target)
    {
       
        int id = user.GetInstanceID();

        

        if (!_returnLists.ContainsKey(id))
        {
            _returnLists.Add(id, new());
        }

        if (_Times.ContainsKey(id))
        {
            if (_Times[id].time <=0)
            {
                _Times[id] = ( time: _time,start: Time.time);
            }
            //Debug.Log("OnStart: "+ _times[id].time.ToString());
            _returnLists[id] = new();
            _returnLists[id].Add((float)StateReturn.Timer);
            _returnLists[id].Add(_Times[id].time);

        }
        else
        {
            _Times.Add(id, (_time, Time.time));
            _returnLists[id] = new();
            _returnLists[id].Add((float)StateReturn.Timer);
            _returnLists[id].Add(_time);
        }
        
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (user.DoDie)
        {
            _returnLists.Remove(user.GetInstanceID());
            return (new(), null);
        }

        if (_autoStart)
        {
            
            int id = user.GetInstanceID();
            (_, float start) = _Times[id];
            _Times[id] = (_time - (Time.time - start),start);
            _returnLists[id] = new();
            _returnLists[id].Add((float)StateReturn.Timer);
            _returnLists[id].Add(_Times[id].time);
            
        }

        return (null, _returnLists[user.GetInstanceID()]);
    }

    
}
