using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneOnDeath : MonoBehaviour
{
    private CutsceneVideo _cutsceneVideo;
    [SerializeField] private float _waitTime;

    private void Start()
    {
        _cutsceneVideo = GameObject.FindObjectOfType<CutsceneVideo>();
        Debug.Log(_cutsceneVideo);
    }

    private void OnDisable()
    {
        _cutsceneVideo.StartCutscene(_waitTime);
    }
}
