using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScreen : MonoBehaviour
{
    private Canvas _canvas;

    private VideoPlayer _videoPlayer;

    [SerializeField] private GameObject _hud;



    [SerializeField] private bool _dontStartOnLoad;
    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();

        _videoPlayer = GetComponent<VideoPlayer>();

        _videoPlayer.targetCamera = Camera.main;

        if (!_dontStartOnLoad)
        {
            StartCoroutine(Show());
        }
    }

    // Update is called once per frame
    void Update()
    {
        _videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        // _hud.SetActive(true);
    }

    public IEnumerator Show()
    {
        _videoPlayer.Play();
        _canvas.enabled = !_canvas.enabled;
        _hud.SetActive(false);
        yield return new WaitForSeconds(4);
        _hud.SetActive(true);
        _canvas.enabled = !_canvas.enabled;
        _videoPlayer.Stop();
    }

    public IEnumerator NextLevel(int buildIndex)
    {
        StartCoroutine(Show());
        yield return new WaitForSeconds(3.9f);
        SceneManager.LoadScene(buildIndex);
    }
}
