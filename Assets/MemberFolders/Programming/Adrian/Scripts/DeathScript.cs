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
    [SerializeField] private GameObject pressB;

    private Image _deathImage;

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
        pressB.SetActive(true);
        _deathImage = GetComponentInChildren<Image>();
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.targetCamera = Camera.main;
        StartCoroutine(Show());
        _videoPlayer.Play();
        _hud.SetActive(false);
        
    }

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(0.1f);
        _deathImage.enabled = false;
    }
}
