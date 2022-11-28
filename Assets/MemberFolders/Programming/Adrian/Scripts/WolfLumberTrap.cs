using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WolfLumberTrap : MonoBehaviour
{
    private InteractController _interactController;
    private InputAction _interact;


    [SerializeField] private GameObject _wall;


    private Transform _wallRotatePoint;

    [SerializeField] private bool _canRotate;

    [SerializeField] private float _degreesPerSecond;
    [SerializeField] private float _finalRotation;

    [SerializeField] private GameObject[] _logs;

    private void Awake()
    {
        _interactController = new InteractController();
    }

    private void OnEnable()
    {
        _interact = _interactController.Player.Interact;
        _interact.Enable();

        // _interact.performed += Interact;
    }

    // Start is called before the first frame update
    void Start()
    {
        _wallRotatePoint = _wall.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_canRotate)
        {
            _wall.transform.RotateAround(_wallRotatePoint.position, _wallRotatePoint.right, Time.deltaTime * _degreesPerSecond);
            StartCoroutine(StopRotating());
        }
    }

    private IEnumerator StopRotating()
    {
        foreach (GameObject log in _logs)
        {
            Destroy(log, 40);
        }
        yield return new WaitForSeconds(_finalRotation / _degreesPerSecond);
        _canRotate = false;
    }

    // private void Interact(InputAction.CallbackContext context)
    // {
    //     _canRotate = true;
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _canRotate = true;
        }
    }
}
