using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    [SerializeField] private GameObject _hud;

    private Image _loadingImage;

    [SerializeField] private bool _dontStartOnLoad;

    // Start is called before the first frame update
    void Start()
    {
        _loadingImage = GetComponent<Image>();

        _videoPlayer = GetComponent<VideoPlayer>();

        _videoPlayer.targetCamera = Camera.main;

        StartCoroutine(Show());
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
        _loadingImage.enabled = true;
        _hud.SetActive(false);
        _videoPlayer.Play();
        yield return new WaitForSeconds(0.1f);
        _loadingImage.enabled = false;
        yield return new WaitForSeconds(4);
        _hud.SetActive(true);
        _videoPlayer.Stop();
    }

    public IEnumerator NextLevel(int buildIndex)
    {
        StartCoroutine(Show());
        yield return new WaitForSeconds(3.9f);
        SceneManager.LoadScene(buildIndex);
    }
}
