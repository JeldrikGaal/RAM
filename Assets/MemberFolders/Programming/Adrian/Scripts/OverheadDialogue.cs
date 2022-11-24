using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class OverheadDialogue : MonoBehaviour
{
    public bool PanCamera;

    [SerializeField] private float _timeBetweenSpeaking;
    [SerializeField] private int _dialogueCount;

    [SerializeField] private Canvas _character1Canvas;
    [SerializeField] private Canvas _character2Canvas;

    [SerializeField] private GameObject _character1Replacement;
    [SerializeField] private GameObject _character2Replacement;



    [SerializeField] private Transform _lerpPoint;
    [SerializeField] private float _lerpSpeed;

    private Transform _cameraTransform;
    [SerializeField] private Transform _playerTransform;


    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        StartCoroutine(ShowDialogue(_timeBetweenSpeaking));
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();

        _character1Canvas.transform.rotation = Quaternion.LookRotation(-_cameraTransform.forward, _cameraTransform.up);
        _character2Canvas.transform.rotation = Quaternion.LookRotation(-_cameraTransform.forward, _cameraTransform.up);
    }

    private void MoveCamera()
    {
        if (PanCamera)
        {
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, new Vector3(_lerpPoint.position.x, _cameraTransform.position.y, _lerpPoint.position.z), _lerpSpeed * Time.deltaTime);
        }
        else
        {
            // Follow player again
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, new Vector3(_playerTransform.position.x + 7.13f, _cameraTransform.position.y, _playerTransform.position.z - 6.26f), _lerpSpeed * Time.deltaTime * 2f);
        }
    }

    private IEnumerator ShowDialogue(float duration)
    {
        yield return new WaitForSeconds(1);

        PanCamera = true;

        _cameraTransform.GetComponent<CinemachineBrain>().enabled = false;

        yield return new WaitForSeconds(1);

        _character2Canvas.gameObject.SetActive(true);

        for (int i = 0; i < _dialogueCount; i++)
        {
            _character1Canvas.gameObject.SetActive(!_character1Canvas.gameObject.activeSelf);
            _character2Canvas.gameObject.SetActive(!_character2Canvas.gameObject.activeSelf);
            yield return new WaitForSeconds(duration);
        }

        _character1Canvas.gameObject.SetActive(false);
        _character2Canvas.gameObject.SetActive(false);

        PanCamera = false;

        yield return new WaitForSeconds(4);

        var char1 = Instantiate(_character1Replacement, _character1Canvas.transform.parent.position, Quaternion.identity);
        Destroy(_character1Canvas.transform.parent.gameObject);

        var char2 = Instantiate(_character2Replacement, _character2Canvas.transform.parent.position, Quaternion.identity);
        Destroy(_character2Canvas.transform.parent.gameObject);

        char1.transform.LookAt(char2.transform);
        char2.transform.LookAt(char1.transform);

        _cameraTransform.GetComponent<CinemachineBrain>().enabled = true;

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        PanCamera = true;

        _playerTransform = other.transform;
    }
}
