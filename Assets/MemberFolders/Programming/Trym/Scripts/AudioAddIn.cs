using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Sirenix.OdinInspector;

[System.Serializable]
public class AudioAddIn 
{
    [SerializeField] private EventReference _audio;
    [SerializeField][Range(0,1)] float _volume = 1;
    [SerializeField] private bool _attach;
    [ShowIf(nameof(_attach))]
    [SerializeField] private Transform _attachTo;
    
    [FoldoutGroup("Advanced")][SerializeField] private bool _stopBeforePlay = false;
    [ShowIf(nameof(_stopBeforePlay))]
    [FoldoutGroup("Advanced")][SerializeField] private bool _allowFadeout = true;
    [Tooltip("⚠ WARNING ⚠ can cause memory leak if it's false and Clear() does not get called. ")]
    [FoldoutGroup("Advanced")][SerializeField] bool _autoRelease = true;

    private List<EventInstance> _live = new();
    private Transform _transform;

    public void SetTransform(Transform transform)
    {
        _transform = transform;
    }

    /// <summary>
    /// Plays The audio in accordance with settings
    /// </summary>
    public void Play()
    {
        Play(null);
        
    }

    /// <summary>
    /// Plays The audio in accordance with settings and params
    /// </summary>
    /// <param name="paramRefs">Params</param>
    public void Play((string name, float value)[] paramRefs)
    {
        if (_stopBeforePlay)
        {
            Stop(_allowFadeout);
        }
        EventInstance instance = RuntimeManager.CreateInstance(_audio);
        if (_attach)
        {
            RuntimeManager.AttachInstanceToGameObject(instance, _attachTo);
        }
        else
        {
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(_transform));
        }

        instance.setVolume(_volume);

        if (paramRefs != null)
        {
            foreach (var paramRef in paramRefs)
            {
                instance.setParameterByName(paramRef.name, paramRef.value);
            }
        }
        
        instance.start();
        if (_autoRelease)
        {
            instance.release();
        }
        else
        {
            _live.Add(instance);
        }

        

    }


    public void ModifyParams((string name, float value)[] @params, bool restart)
    {
        foreach ((string name, float value) in @params)
        {
            foreach (var item in _live)
            {
                
                item.setParameterByName(name, value);
                if (restart)
                {
                    item.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    item.start();
                }
            }
        }
        
    }


    public void Stop(bool allowFaidout)
    {
        foreach (var item in _live)
        {
            item.stop(allowFaidout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }


    public void Clear()
    {


        int i = 0;
        while(_live.Count >0 && i < _live.Count)
        {
            try
            {
                if (_live[i].release() == FMOD.RESULT.OK)
                {
                    _live.RemoveAt(i);
                }
                
            }
            catch (System.Exception e)
            {
                i++;
                Debug.LogWarning(e + " | Could not release, try stop.");
                
            }
        }
        
    }

    



}

