using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneOnDeath : MonoBehaviour
{
    private CutsceneVideo _cutsceneVideo;

    private void Start()
    {
        _cutsceneVideo = GameObject.FindObjectOfType<CutsceneVideo>();
    }

    private void OnDisable()
    {
        _cutsceneVideo.RunCutscene();
    }
}
