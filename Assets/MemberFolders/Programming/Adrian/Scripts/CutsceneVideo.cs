using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneVideo : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        _videoPlayer.loopPointReached += EndReached;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _videoPlayer.frame += 40;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _videoPlayer.Play();
        }
    }

    // Destroy the game object when the cutscene is over
    void EndReached(VideoPlayer vp)
    {
        Destroy(gameObject);
    }
}
