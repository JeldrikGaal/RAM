using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSystem DialogueSystem;

    [SerializeField] private Canvas DialogueCanvas;

    [SerializeField] private Canvas _pressECanvas;

    [SerializeField] private string _name1;
    [SerializeField] private string _name2;

    public string[] DialogueLines;

    public AudioClip[] AudioClips;

    private bool _startedPlaying;

    private Transform _cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_startedPlaying && DialogueSystem.Finished)
        {
            _startedPlaying = false;
            // If the dialogue is over destroy the gameobject
            //Destroy(gameObject);
        }
    }


    void LateUpdate()
    {
        LookAtCamera();
    }

    void LookAtCamera()
    {
        _pressECanvas.transform.rotation = Quaternion.LookRotation(-_cameraTransform.forward, _cameraTransform.up);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E) && !_startedPlaying)
            {
                DialogueCanvas.gameObject.SetActive(true);
                _startedPlaying = true;
                StartCoroutine(DialogueSystem.Dialogue(DialogueLines, AudioClips, 1f, transform.position, 1, _name1, _name2));
            }

            if (!_startedPlaying)
            {
                _pressECanvas.gameObject.SetActive(true);
            }
            else
            {
                _pressECanvas.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _pressECanvas.gameObject.SetActive(false);
        }
    }
}
