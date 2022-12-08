using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public class AI_Audio : StateBlock
{
    enum StartIn {Update, End, ExecuteManually}
    [SerializeField] EventReference _audioEvent;
    [SerializeField] Vector3 _positionMod;
    [SerializeField] StartIn _startIn;
    [HideIf(nameof(_startIn),StartIn.End)]
    [SerializeField] bool _repeat;
    [ShowIf(nameof(_startIn), StartIn.Update)]
    [SerializeField] bool _runOnlyOnce;
    
    enum RepeatMode {Movement,Time}
    [ShowIf(nameof(_repeat), true)]
    [SerializeField] RepeatMode _repeatMode;
    [ShowIf(nameof(_repeat), true)]
    [ShowIf(nameof(_repeatMode), RepeatMode.Time)]
    [SerializeField] float _intervalInSeconds;
    [ShowIf(nameof(_repeat), true)]
    [ShowIf(nameof(_repeatMode), RepeatMode.Movement)]
    [SerializeField] float _movementInterval;

    private readonly Dictionary<(int id, int instance),bool> _usersWithInstances = new();
    private readonly Dictionary<int, List<int>> _instancesByUsers = new();
    public override void OnEnd(EnemyController user, GameObject target)
    {
        
        var id = user.GetInstanceID();
        var instance = _instancesByUsers[id][0];
        _usersWithInstances[(id,instance)] = false;
        _usersWithInstances.Remove((id, instance));
        _instancesByUsers[id].RemoveAt(0);
        if (_startIn == StartIn.End)
        {
            Play(user);
        }
        

    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        if (_startIn == StartIn.End)
        {
            _repeat = false;
        }
        if (_repeat && _instancesByUsers[user.GetInstanceID()] == null)
        {
            _instancesByUsers[user.GetInstanceID()] = new();
        }

       
        
        
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        
        if (_startIn == StartIn.Update)
        {
            int id = user.GetInstanceID();
            if (!_runOnlyOnce || _instancesByUsers[id].Count > 0)
            {
                Run(user);
            }
        }
       
        return (null, null);
    }
    private void Play(EnemyController user)
    {
        
        RuntimeManager.PlayOneShot(_audioEvent, user.transform.position + _positionMod);
    }

    public void PlayAudio(EnemyController user)
    {
        if (_startIn == StartIn.ExecuteManually)
        {
            Run(user);
        }
    }
    private void Run(EnemyController user)
    {
        int id = user.GetInstanceID();
        int count = _instancesByUsers[id].Count;
        if (_repeat)
        {
            Repeating(user,count);
        }
        Play(user);

        _usersWithInstances.Add((id, count), true);
        _instancesByUsers[id].Add(count);
    }

    private IEnumerator RunRepeatingTime(EnemyController user,int instance)
    {

        while (_usersWithInstances[(user.GetInstanceID(),instance)])
        {
            Play(user);
            yield return new WaitForSeconds(_intervalInSeconds);
        }
        

    }
    
    private IEnumerator RunRepeatingMovement(EnemyController user,int instance)
    {
        float lastEntry = 0;
        while (_usersWithInstances[(user.GetInstanceID(), instance)])
        {

            lastEntry += user.MoveInput.magnitude * user.MoveSpeed;
            if (lastEntry >= _movementInterval)
            {
                Play(user);
                lastEntry = 0;
            }

            yield return new WaitForEndOfFrame();
        }
        

    }


    private void Repeating(EnemyController user,int count)
    {
        int id = user.GetInstanceID();
        
        switch (_repeatMode)
        {
            case RepeatMode.Movement:
                user.StartCoroutine(RunRepeatingMovement(user, count));
                
                break;
            case RepeatMode.Time:
                user.StartCoroutine(RunRepeatingTime(user, count));

                break;
            default:throw new System.Exception("?????");
                break;
        }
        
    }






}
