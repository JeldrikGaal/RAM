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
    [SerializeField] ParamRef param;
    
    
    [SerializeField] Vector3 _positionMod;
    [Tooltip("follows the AI")]
    [SerializeField] bool _attached;
    [SerializeField] StartIn _startIn;
    [HideIf(nameof(_startIn),StartIn.End)]
    [SerializeField] bool _repeat;
    [ShowIf(nameof(_startIn), StartIn.Update)]
    [SerializeField] bool _runOnlyOnce;
    
    enum RepeatMode {Movement,Time}
    [ShowIf(nameof(_repeat), true)]
    [SerializeField] RepeatMode _repeatMode;
    
    [ShowIf("@this._repeatMode == RepeatMode.Time && this._repeat")]
    [SerializeField] float _intervalInSeconds;
    
    [ShowIf("@this._repeat && this._repeatMode == RepeatMode.Movement")]
    [SerializeField] float _movementInterval;

    FMOD.Studio.EventDescription eventDescription;

    private readonly Dictionary<(int id, int instance),bool> _usersWithInstances = new();
    private readonly Dictionary<int, List<int>> _instancesByUsers = new();
    public override void OnEnd(EnemyController user, GameObject target)
    {
        var id = user.GetInstanceID();
        if (_instancesByUsers[id].Count>0)
        {
            var instance = _instancesByUsers[id][0];
            _usersWithInstances[(id, instance)] = false;
            _usersWithInstances.Remove((id, instance));
            _instancesByUsers[id].RemoveAt(0); 
        }


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
            if (!_runOnlyOnce || _instancesByUsers[id].Count < 1)
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
        if (_repeat)
        {
            Repeating(user,count);
        }
        Play(user);

        _usersWithInstances.Add((id, count), true);
        _instancesByUsers[id].Add(count);
        Debug.Log("Run Ran");
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
