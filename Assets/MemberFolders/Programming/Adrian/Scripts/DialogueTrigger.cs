using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    private InteractController _interactController;

    private InputAction _interact;

    public DialogueSystem DialogueSystem;

    [SerializeField] private Canvas DialogueCanvas;

    private Canvas _pressECanvas;

    [SerializeField] private string _name1;
    [SerializeField] private string _name2;

    public string[] DialogueLines;

    public AudioClip[] AudioClips;

    private bool _startedPlaying;
    private bool _canDisplayDialogue;

    private Transform _cameraTransform;

    private void Awake()
    {
        _interactController = new InteractController();
    }

    private void OnEnable()
    {
        _interact = _interactController.Player.Interact;

        _interact.Enable();

        _interact.performed += Interact;
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _pressECanvas = transform.GetChild(0).GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        // If this NPC has started and the dialogue is over
        if (_startedPlaying && DialogueSystem.Finished)
        {
            // Reset the started dialogue bool
            _startedPlaying = false;
        }

        // If the player is within range of the NPC
        if (_canDisplayDialogue)
        {
            // If we have not started the dialogue yet
            if (!_startedPlaying)
            {
                // Display the "E button" indicator
                _pressECanvas.gameObject.SetActive(true);
            }
            else
            {
                // Disable the "E button" indicator if the dialogue has started
                _pressECanvas.gameObject.SetActive(false);
            }
        }

    }


    void LateUpdate()
    {
        LookAtCamera();
    }

    void LookAtCamera()
    {
        // Stole Muratcans code to rotate the interact button indicator towards the camera :)
        _pressECanvas.transform.rotation = Quaternion.LookRotation(-_cameraTransform.forward, _cameraTransform.up);
    }

    private void Interact(InputAction.CallbackContext context)
    {
        // If the player interacts with the NPC and has not already started the dialogue
        if (_canDisplayDialogue && !_startedPlaying)
        {
            // Enable the dialogue canvas
            DialogueCanvas.gameObject.SetActive(true);

            // Enable a bool to show that the dialogue has started
            _startedPlaying = true;

            // Start the dialogue coroutine
            StartCoroutine(DialogueSystem.Dialogue(DialogueLines, AudioClips, 1f, transform.position, 1, _name1, _name2));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // If the player is inside the trigger and the dialogue has not started yet
        if (other.tag == "Player" && !_startedPlaying)
        {
            // Enable a bool so the dialogue stuff can be activated
            _canDisplayDialogue = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the player leaves
        if (other.tag == "Player")
        {
            // No longer can display dialogue stuff
            _canDisplayDialogue = false;

            // Disable the "E button" indicator
            _pressECanvas.gameObject.SetActive(false);
        }
    }
}
