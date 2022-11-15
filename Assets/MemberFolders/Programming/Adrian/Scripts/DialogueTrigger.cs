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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!DialogueSystem.PlayingAudio)
        {
            DialogueCanvas.gameObject.SetActive(true);
            StartCoroutine(DialogueSystem.Dialogue(DialogueLines, AudioClips, 1f, transform.position, 1, _name1, _name2));
            _startedPlaying = true;
        }
    }
}
