using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    public Canvas DialogueCanvas;

    public AudioManager AudioManager;

    public DialogueSystem DialogueSystem;

    [SerializeField] private string _name1;
    [SerializeField] private string _name2;

    public string[] DialogueLines;

    public AudioClip[] AudioClips;

    [Tooltip("Volume between 0 and 1")]
    [Range(0, 1)]
    public float TestVolume1;
    [Tooltip("Volume between 0 and 1")]
    [Range(0, 1)]
    public float TestVolume2;

    [SerializeField] private AudioClip _clipToPlay;

    void Start()
    {
        GameObject camera = GameObject.Find("Main Camera");

        var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the test object 4 units in the x direction when you press W
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z);
        }

        // Use the PlayAudio function from the AudioManager script to play an audioclip at the location of the gameObject using the volume variables
        // if (Input.GetKeyDown(KeyCode.Space) && !DialogueSystem.PlayingAudio)
        // {
        //     DialogueCanvas.gameObject.SetActive(true);
        //     //AudioManager.PlayAudio(_clipToPlay, transform.position, TestVolume1);
        //     StartCoroutine(DialogueSystem.Dialogue(DialogueLines, AudioClips, 1f, transform.position, 1f, _name1, _name2));
        // }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AudioManager.PlayAudio(_clipToPlay, transform.position, TestVolume2);
        }
    }
}
