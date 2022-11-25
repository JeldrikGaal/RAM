using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class CutsceneVideo : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    private bool _canPlayVideo;

    [SerializeField] private GameObject _instructions;

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
            _videoPlayer.frame += 100;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && _canPlayVideo)
        {
            // Play the cutscene
            _videoPlayer.enabled = true;
            _videoPlayer.Play();
        }
    }

    // Destroy the game object when the cutscene is over
    void EndReached(VideoPlayer vp)
    {
        //Destroy(gameObject);
        _videoPlayer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _canPlayVideo = true;

        _instructions.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        _canPlayVideo = false;

        _instructions.SetActive(false);
    }
}
