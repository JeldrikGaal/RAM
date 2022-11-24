using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;

public class OverheadDialogue : MonoBehaviour
{
    public bool PanCamera;

    [SerializeField] private float _timeBetweenSpeaking;

    [SerializeField] private Canvas _character1Canvas;
    [SerializeField] private Canvas _character2Canvas;

    [SerializeField] private GameObject _character1Replacement;
    [SerializeField] private GameObject _character2Replacement;

    [SerializeField] private Sprite[] _dialogueSprites;

    [SerializeField] private Transform _lerpPoint;
    [SerializeField] private float _lerpSpeed;

    private Transform _cameraTransform;
    [SerializeField] private Transform _playerTransform;


    // Start is called before the first frame update
    void Start()
    {
        // Gets a reference to the main camera
        _cameraTransform = Camera.main.transform;

        // For testing purposes
        StartCoroutine(ShowDialogue(_timeBetweenSpeaking));
    }

    // Update is called once per frame
    void Update()
    {
        // Calls the move camera function
        MoveCamera();

        // Stole muratcans code again >:)
        _character1Canvas.transform.rotation = Quaternion.LookRotation(_cameraTransform.forward, _cameraTransform.up);
        _character2Canvas.transform.rotation = Quaternion.LookRotation(_cameraTransform.forward, _cameraTransform.up);
    }

    private void MoveCamera()
    {
        // Checks if the dialogue has started
        if (PanCamera)
        {
            // Lerp the camera position to a given point to show the dialogue
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, new Vector3(_lerpPoint.position.x, _cameraTransform.position.y, _lerpPoint.position.z), _lerpSpeed * Time.deltaTime);
        }
        else
        {
            // Lerp back to the position the cinemachine follows
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, new Vector3(_playerTransform.position.x + 7.13f, _cameraTransform.position.y, _playerTransform.position.z - 6.26f), _lerpSpeed * Time.deltaTime * 2f);
        }
    }

    private IEnumerator ShowDialogue(float duration)
    {
        // Wait for one second
        yield return new WaitForSeconds(1);

        // Sets the bool to true to start the camera move
        PanCamera = true;

        // Stops the cinemachine from controlling the camera
        _cameraTransform.GetComponent<CinemachineBrain>().enabled = false;

        // Waits for another second
        yield return new WaitForSeconds(1);

        // Enables the second canvas so when they flip later the first one is activated and not the second
        _character2Canvas.gameObject.SetActive(true);

        // Loop for each dialoguebox sprite
        for (int i = 0; i < _dialogueSprites.Length; i++)
        {
            // Sets the correct sprite in the canvases
            _character1Canvas.GetComponentInChildren<Image>().sprite = _dialogueSprites[i];
            _character2Canvas.GetComponentInChildren<Image>().sprite = _dialogueSprites[i];

            // Flips which canvas is active
            _character1Canvas.gameObject.SetActive(!_character1Canvas.gameObject.activeSelf);
            _character2Canvas.gameObject.SetActive(!_character2Canvas.gameObject.activeSelf);

            // Waits the given duration
            yield return new WaitForSeconds(duration);
        }

        // Disables both canvases
        _character1Canvas.gameObject.SetActive(false);
        _character2Canvas.gameObject.SetActive(false);

        // Stops the camera pan to the dialogue so it can move towards the player again
        PanCamera = false;

        // Waits for 4 seconds
        yield return new WaitForSeconds(4);

        // Instantiates a replacement enemy with all the enemy functionality for the first dialogue npc
        var char1 = Instantiate(_character1Replacement, _character1Canvas.transform.parent.position, Quaternion.identity);
        Destroy(_character1Canvas.transform.parent.gameObject);

        // Does the same for the second npc
        var char2 = Instantiate(_character2Replacement, _character2Canvas.transform.parent.position, Quaternion.identity);
        Destroy(_character2Canvas.transform.parent.gameObject);

        // Makes the enemies look at eachother like the npcs did
        char1.transform.LookAt(char2.transform);
        char2.transform.LookAt(char1.transform);

        // Enables the cinemachine brain again so the camera can follow the player again
        _cameraTransform.GetComponent<CinemachineBrain>().enabled = true;

        // Destroys this gameobject
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        PanCamera = true;

        _playerTransform = other.transform;
    }
}
