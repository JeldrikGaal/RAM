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
        instance.release();
        
    }





}
