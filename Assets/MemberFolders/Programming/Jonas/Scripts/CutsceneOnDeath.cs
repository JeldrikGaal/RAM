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
    }

    private void OnDisable()
    {
        Debug.Log("on disable"); 
        StartCoroutine(_cutsceneVideo.RunCutscene(_waitTime));
    }
}
