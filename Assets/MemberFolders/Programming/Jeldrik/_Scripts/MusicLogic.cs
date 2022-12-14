using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicLogic : MonoBehaviour
{
    [SerializeField] private AudioClip _area1Chill;
    [SerializeField] private AudioClip _area1Combat;
    [SerializeField] private AudioClip _area2Chill;
    [SerializeField] private AudioClip _area2Combat;
    [SerializeField] private AudioClip _area3Chill;
    [SerializeField] private AudioClip _area3Combat;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Loading the correct music file into the audio source
        if (SceneManager.GetActiveScene().buildIndex < 4 )
        {
            _audioSource.clip = _area1Combat;
        }
        else if (SceneManager.GetActiveScene().buildIndex < 7)
        {
            _audioSource.clip = _area2Combat;
        }
        else
        {
            _audioSource.clip = _area3Combat;
        }
    }

    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }


}
