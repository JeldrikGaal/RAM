using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    public AudioManager AudioManager;

    public DialogueSystem DialogueSystem;

    public AudioClip[] AudioClips;

    public float TestVolume1;
    public float TestVolume2;

    [SerializeField] private AudioClip _clipToPlay;

    // Update is called once per frame
    void Update()
    {
        // Move the test object 4 units in the x direction when you press W
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z);
        }

        // Use the PlayAudio function from the AudioManager script to play an audioclip at the location of the gameObject using the volume variables
        if (Input.GetKeyDown(KeyCode.Space) && !DialogueSystem.PlayingAudio)
        {
            //AudioManager.PlayAudio(_clipToPlay, transform.position, TestVolume1);
            StartCoroutine(DialogueSystem.Dialogue(AudioClips, 1.5f, transform.position, 1f));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AudioManager.PlayAudio(_clipToPlay, transform.position, TestVolume2);
        }
    }
}
