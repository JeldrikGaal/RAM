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
    //[ParamRef]
    //[SerializeField] ParamRef param;

    [SerializeField] bool _attached;
    [HideIf(nameof(_attached))]
    [SerializeField] Vector3 _positionMod;
    [Tooltip("follows the AI")]
    
    [SerializeField] StartIn _startIn;
    [ShowIf(nameof(_startIn), StartIn.Update)]
    [SerializeField] bool _startOnlyOnce;
    [HideIf(nameof(_startIn),StartIn.End)]
    [SerializeField] bool _repeat;
    
    
    enum RepeatMode {Movement,Time}
    [ShowIf(nameof(_repeat), true)]
    [SerializeField] RepeatMode _repeatMode;

    [ShowIf(nameof(_repeat))]
    [SerializeField] bool _randomize;
    [ShowIf("@this._repeatMode == RepeatMode.Time && this._repeat && !this._randomize")]
    [SerializeField] float _intervalInSeconds;
    [ShowIf("@this._repeatMode == RepeatMode.Time && this._repeat && this._randomize")]
    [SerializeField] float _maxTime, _minTime;


    [ShowIf("@this._repeat && this._repeatMode == RepeatMode.Movement && !this._randomize")]
    [SerializeField] float _movementInterval;
    [ShowIf("@this._repeat && this._repeatMode == RepeatMode.Movement && this._randomize")]
    [SerializeField] float _maxDistance, _minDistance;
    //FMOD.Studio.EventDescription eventDescription;

    private readonly Dictionary<(int id, int instance),bool> _usersWithInstances = new();
    private readonly Dictionary<int, List<int>> _instancesByUsers = new();
    public override void OnEnd(EnemyController user, GameObject target)
    {
        Cleanup(user);

        if (_startIn == StartIn.End)
        {
            Play(user);
        }


    }

    private void Cleanup(EnemyController user)
    {
        var id = user.GetInstanceID();
        if (_instancesByUsers[id].Count > 0)
        {
            var instance = _instancesByUsers[id][0];
            _usersWithInstances[(id, instance)] = false;
            _usersWithInstances.Remove((id, instance));
            _instancesByUsers[id].RemoveAt(0);
        }
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        if (_startIn == StartIn.End)
        {
            _repeat = false;
        }
        if (!_instancesByUsers.ContainsKey(user.GetInstanceID()))
        {
            _instancesByUsers.Add(user.GetInstanceID(), new List<int>());
        }

        Debug.Log(user.GetInstanceID());
        Debug.Log("also: " + GetInstanceID());
        


    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        
        if (_startIn == StartIn.Update)
        {
            Debug.Log("Update Running");
            int id = user.GetInstanceID();
            if (!_startOnlyOnce || _instancesByUsers[id].Count < 1)
            {
                Debug.Log("One Instance");
                Run(user);
            }
            Debug.Log("Instances: " + _instancesByUsers[id].Count);
        }
       
        return (null, null);
    }
    private void Play(EnemyController user)
    {
        if (_attached)
        {
            RuntimeManager.PlayOneShotAttached(_audioEvent, user.gameObject);
        }
        else
        {
            RuntimeManager.PlayOneShot(_audioEvent, user.transform.position + (user.transform.rotation * _positionMod));
        }
        
        
        Debug.Log("Played");
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
        _usersWithInstances.Add((id, count), true);
        _instancesByUsers[id].Add(count);
        if (_repeat)
        {
            Repeating(user,count);
        }
        Play(user);

        
        Debug.Log("Run Ran");
    }

    private IEnumerator RunRepeatingTime(EnemyController user,int instance)
    {

        while (IsRunning(user, instance))
        {
            Play(user);
            yield return new WaitForSeconds(_randomize ? Random.Range(_minTime, _maxTime) : _intervalInSeconds);
        }


    }

    private bool IsRunning(EnemyController user, int instance) => _usersWithInstances.ContainsKey((user.GetInstanceID(), instance))?  _usersWithInstances[(user.GetInstanceID(), instance)]:false;

    private IEnumerator RunRepeatingMovement(EnemyController user,int instance)
    {
        float lastEntry = 0;
        while (IsRunning(user,instance))
        {

            lastEntry += user.MoveInput.magnitude * user.MoveSpeed;
            if (lastEntry >= (_randomize? Random.Range(_minDistance,_maxDistance) :_movementInterval))
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
