using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class DeathScript : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _hud;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnEnable()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.targetCamera = Camera.main;
        _videoPlayer.Play();
        _hud.SetActive(false);
    }
}
