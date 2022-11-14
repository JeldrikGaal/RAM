using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class RammyController : MonoBehaviour
{
    // Input Action Asset for Rammy Player Controls
    private RammyInputActions _playerControls;
    private InputAction _move;
    private InputAction _attack;
    
    // Vector in which the character is currently moving according to player input
    private Vector2 _moveDirection;

    // Rigidbody
    private Rigidbody _rB;

    // Speed Modifier 
    public float MovementSpeed;

    // Setting Input Actions on Awake
    private void Awake()
    {
        _playerControls = new RammyInputActions();
    }

    // Enabling PlayerControls when player gets enabled in the scene
    private void OnEnable()
    {
        _move = _playerControls.Player.Move;
        _move.Enable();
    }

    // Disabling PlayerControls when player gets disabled in the scene
    private void OnDisable()
    {
        _move.Disable();
    }

    void Start()
    {
        _rB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _moveDirection = _move.ReadValue<Vector2>() * MovementSpeed;
        Debug.Log(_moveDirection);

        _rB.velocity = new Vector3(_moveDirection.x, 0 , _moveDirection.y);
    }
}
