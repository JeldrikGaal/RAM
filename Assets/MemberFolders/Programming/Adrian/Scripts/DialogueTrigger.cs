using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSystem DialogueSystem;

    public Canvas DialogueCanvas;

    [SerializeField] private string _name1;
    [SerializeField] private string _name2;

    public string[] DialogueLines;

    public AudioClip[] AudioClips;

    private bool _startedPlaying;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_startedPlaying && !DialogueSystem.PlayingAudio)
        {
            // If the dialogue is over destroy the gameobject
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // If the dialogue is not already started
        if (!DialogueSystem.PlayingAudio && !_startedPlaying)
        {
            // Enable the Dialogue canvas
            DialogueCanvas.gameObject.SetActive(true);

            // Start the dialogue Coroutine
            StartCoroutine(DialogueSystem.Dialogue(DialogueLines, AudioClips, 1f, transform.position, 1, _name1, _name2));

            // Enable the bool to indicate the dialogue has started
            _startedPlaying = true;
        }
    }
}
