using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneVideo : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _hud;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _levelAudio;

    [SerializeField] private PauseGame _pauseGame;

    private float _defaultSpeed;

    private bool _hadSpeedBuffOnEntry;
    [SerializeField] private bool _nextSceneOnEnd = false;

    [SerializeField] private bool _deleteWhenDone = false;
    private bool _loadOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();

        _videoPlayer.targetCamera = Camera.main;
        _videoPlayer.loopPointReached += EndReached;

        _player = GameObject.FindObjectOfType<RammyController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Skips 100 frames in the video when the right arrow is pressed
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _videoPlayer.frame += 1000000;
        }

        if (_videoPlayer.isPlaying)
        {
            _hud.SetActive(false);
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
        if (!_loadOnce && _nextSceneOnEnd)
        {
            _loadOnce = true;
            StartCoroutine(GameObject.FindObjectOfType<LoadingScreen>().NextLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

        //Destroy(gameObject);
        // Disables the videoplayer to remove the overlay
        _videoPlayer.enabled = false;

        // Sets the players movement speed back to what they had when they entered the trigger
        // _player.GetComponent<RammyController>().MovementSpeed = _defaultSpeed;

        // // If the player had a speedbuff when the cutscene started, but not when it is over, halve the movementspeed
        // if (_hadSpeedBuffOnEntry && !_player.GetComponent<RammyController>().HasSpeedBuff)
        // {
        //     _player.GetComponent<RammyController>().MovementSpeed /= 2;
        // }

        _audioSource.clip = _levelAudio;
        _audioSource.Play();

        // Enables the HUD
        _hud.SetActive(true);

        // Enable pausing after cutscene
        if (_pauseGame != null)
        {
            // _pauseGame.AllowPause = true;
            // _pauseGame.EnablePause();
        }

        // Unblocks rammy
        _player.GetComponent<RammyController>().BLOCKEVERYTHINGRAMMY = false;
        _player.GetComponent<RammyController>().UnBlockPlayerMovement();
        // _deleteWhenDone = true;

        if (_deleteWhenDone)
        {
            Debug.Log("Pausing allowed:" + _pauseGame.AllowPause);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checks if it collided with the player
        if (other.tag == "Player")
        {
            RunCutscene();
        }
    }

    public void RunCutscene()
    {
        // Play the cutscene
        _videoPlayer.enabled = true;
        _videoPlayer.Play();
        // Disallows pausing during cutscene
        if (_pauseGame != null) _pauseGame.AllowPause = false;
        // Disables the HUD
        _hud.SetActive(false);

        // Saves the movement speed of the player
        // _defaultSpeed = _player.GetComponent<RammyController>().MovementSpeed;

        // Block Rammy
        _player.GetComponent<RammyController>().BLOCKEVERYTHINGRAMMY = true;


        // Sets the players movement speed back to what they had when they entered the trigger
        // _player.GetComponent<RammyController>().MovementSpeed = 0;

        // Stores if the player had a speedbuff active when the cutscene started
        // _hadSpeedBuffOnEntry = _player.GetComponent<RammyController>().HasSpeedBuff;
    }
}
