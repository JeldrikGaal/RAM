using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    public AudioManager AudioManager;

    public float TestVolume1;
    public float TestVolume2;

    [SerializeField] private AudioClip clipToPlay;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Move the test object 4 units in the x direction when you press W
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z);
        }

        // Use the PlayAudio function from the AudioManager script to play an audioclip at the location of the gameObject
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.PlayAudio(clipToPlay, transform.position, TestVolume1);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AudioManager.PlayAudio(clipToPlay, transform.position, TestVolume2);
        }
    }
}
