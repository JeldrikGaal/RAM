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

    [SerializeField] private GameObject _player;
    [SerializeField] private float _defaultSpeed;

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
            _player.GetComponent<RammyController>().MovementSpeed = 0;
        }
    }

    // Destroy the game object when the cutscene is over
    void EndReached(VideoPlayer vp)
    {
        //Destroy(gameObject);
        _videoPlayer.enabled = false;
        _player.GetComponent<RammyController>().MovementSpeed = _defaultSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _player = other.gameObject;

            _defaultSpeed = _player.GetComponent<RammyController>().MovementSpeed;

            _canPlayVideo = true;

            _instructions.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _canPlayVideo = false;

        _instructions.SetActive(false);
    }
}
