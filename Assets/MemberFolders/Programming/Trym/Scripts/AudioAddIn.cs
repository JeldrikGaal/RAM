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
    [SerializeField] private bool _attach;
    [ShowIf(nameof(_attach))]
    [SerializeField] private Transform _attachTo;

    /// <summary>
    /// Plays The audio in accordance with settings
    /// </summary>
    public void Play()
    {
        if (_attach)
        {
            RuntimeManager.PlayOneShotAttached(_audio, _attachTo.gameObject);
        }
        else
        {
            RuntimeManager.PlayOneShot(_audio);
        }
    }

    /// <summary>
    /// Plays The audio in accordance with settings and params
    /// </summary>
    /// <param name="paramRefs">Params</param>
    public void Play(ParamRef[] paramRefs)
    {
        EventInstance instance = RuntimeManager.CreateInstance(_audio);
        if (_attach)
        {
            RuntimeManager.AttachInstanceToGameObject(instance, _attachTo);
        }

        foreach (var paramRef in paramRefs)
        {
            instance.setParameterByName(paramRef.Name, paramRef.Value);
        }
        instance.start();
        instance.release();
        
    }





}

