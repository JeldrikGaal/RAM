using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class CutsceneVideo : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    private bool _canPlayVideo;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _hud;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _levelAudio;


    private float _defaultSpeed;

    private bool _hadSpeedBuffOnEntry;

    [SerializeField] private bool _deleteWhenDone;

    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();

        _videoPlayer.targetCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _videoPlayer.loopPointReached += EndReached;

        // Skips 100 frames in the video when the right arrow is pressed
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _videoPlayer.frame += 1000000;
        }


        // // Checks to see if the video can be played and the up arrow is pressed
        // if (Input.GetKeyDown(KeyCode.UpArrow) && _canPlayVideo)
        // {
        //     _videoPlayer.enabled = true;
        //     _videoPlayer.Play();

        //     // Sets the players movement speed to 0 so they don't move to the right when trying to skip the video
        //     _player.GetComponent<RammyController>().MovementSpeed = 0;

        //     // Stores if the player had a speedbuff active when the cutscene started
        //     _hadSpeedBuffOnEntry = _player.GetComponent<RammyController>().HasSpeedBuff;
        // }
    }

    // Destroy the game object when the cutscene is over
    void EndReached(VideoPlayer vp)
    {
        //Destroy(gameObject);
        // Disables the videoplayer to remove the overlay
        _videoPlayer.enabled = false;

        // Sets the players movement speed back to what they had when they entered the trigger
        _player.GetComponent<RammyController>().MovementSpeed = _defaultSpeed;

        // If the player had a speedbuff when the cutscene started, but not when it is over, halve the movementspeed
        if (_hadSpeedBuffOnEntry && !_player.GetComponent<RammyController>().HasSpeedBuff)
        {
            _player.GetComponent<RammyController>().MovementSpeed /= 2;
        }

        _audioSource.clip = _levelAudio;
        _audioSource.Play();

        // Enables the HUD
        _hud.SetActive(true);

        // Unblocks rammy
        _player.GetComponent<RammyController>().BLOCKEVERYTHINGRAMMY = false;

        if (_deleteWhenDone)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checks if it collided with the player
        if (other.tag == "Player")
        {
            // Disables the HUD
            _hud.SetActive(false);
            // Saves the player object
            _player = other.gameObject;

            // Saves the movement speed of the player
            _defaultSpeed = _player.GetComponent<RammyController>().MovementSpeed;

            // Block Rammy
            _player.GetComponent<RammyController>().BLOCKEVERYTHINGRAMMY = true;

            // Makes the videoplayer be able to play the video
            _canPlayVideo = true;

            // Play the cutscene
            _videoPlayer.enabled = true;
            _videoPlayer.Play();

            // Sets the players movement speed back to what they had when they entered the trigger
            _player.GetComponent<RammyController>().MovementSpeed = 0;

            // Stores if the player had a speedbuff active when the cutscene started
            _hadSpeedBuffOnEntry = _player.GetComponent<RammyController>().HasSpeedBuff;

            // Enables the canvas with the instructions to play the video
            // _instructions.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Disable the ability to play the video when the player exits the trigger
            _canPlayVideo = false;

            // Disables the canvas with the instructions
            // _instructions.SetActive(false);
        }
    }
}
