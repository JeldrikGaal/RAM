using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RammyController : MonoBehaviour
{
    // Input Action Asset for Rammy Player Controls
    private RammyInputActions _playerControls;
    private InputAction _move;
    private InputAction _attack;
    private InputAction _look;
    
    // Vector in which the character is currently moving according to player input
    private Vector2 _moveDirection;
    // Mouseposition in world space
    private Vector3 _mousePosition;
    // blocking movement 
    private bool _blockMovement;

    // Mouse input
    private float _leftMouseButton;
    private float _rightMouseButton;

    private float _cameraDepth;
    private Rigidbody _rB;
    private MeshRenderer _mR;

    // Bools describing playerstate
    public bool Attacking;
    public bool Invincible;
    public bool Dashing;
    public bool Walking;

    // Player Values
    public float Health;
    // Speed Modifier 
    public float MovementSpeed;
    public float AttackDuration;
    public float AttackDistance;
    public float AttackCoolDown;
    public float AttackDamage;

    // Variables for Attacking
    private float _startTimeAttack;
    private bool _attackingAllowed = true;
    
    // Saving rotation to reset after attacking
    private Quaternion _savedRotation;
    private Vector3 _savedPosition;
    private Vector3 _attackDestination;


    private Plane _groundPlane = new Plane(Vector3.up, 0);
    private Vector3 _mouseWorldPosition;
    private Vector3 _lookingAtMouseRotation;

    // Debugging
    [SerializeField] private List<Material> _mats = new List<Material>();
    [SerializeField] private GameObject directionIndicator;

    // Setting Input Actions on Awake
    private void Awake()
    {
        _playerControls = new RammyInputActions();
        _cameraDepth = Camera.main.transform.position.z;
    }

    // Enabling PlayerControls when player gets enabled in the scene
    private void OnEnable()
    {
        _move = _playerControls.Player.Move;
        _look = _playerControls.Player.Look;
        _attack = _playerControls.Player.Attack;
        _move.Enable();
        _look.Enable();
        _attack.Enable();
    }

    // Disabling PlayerControls when player gets disabled in the scene
    private void OnDisable()
    {
        _move.Disable();
    }

    
    void Start()
    {
        _rB = GetComponent<Rigidbody>();
        _mR = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        #region Reading Input
        // Reading the mouse position on screen
        _mousePosition = _look.ReadValue<Vector2>();

        // Reading mouse click input 
        _leftMouseButton = _attack.ReadValue<float>();

        // Calculating the world position where the mouse is currently pointing at and needed help vectors
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        if (_groundPlane.Raycast(ray, out distance))
        {
            _mouseWorldPosition = ray.GetPoint(distance);
        }
        _mouseWorldPosition = new Vector3(_mouseWorldPosition.x, transform.position.y, _mouseWorldPosition.z);
        _lookingAtMouseRotation = _mouseWorldPosition - transform.position;
        _lookingAtMouseRotation = _lookingAtMouseRotation.normalized;
        #endregion

        #region Movement




        // Applying input from the 'Move' Vector from the InputAction to the velocity of the rigidbody and multiplying by the Movement 
        _moveDirection = _move.ReadValue<Vector2>() * MovementSpeed;
        Vector3 vel = new Vector3(_moveDirection.x, 0, _moveDirection.y);

        // Checking if player is allowed to move and if so adjust Rigidbody velocity according to input
        if (!_blockMovement)
        {
            _rB.velocity = vel;
        }

        #endregion


        #region Attacking

        // Changing all needed variables to indiciate and calculate attacking
        if (_leftMouseButton == 1 && !Attacking && _attackingAllowed)
        {
            Attacking = true;
            _startTimeAttack = Time.time;
            _attackDestination = transform.position + _lookingAtMouseRotation * AttackDistance;
            _savedRotation = transform.rotation;
            _savedPosition = transform.position;
            transform.up = _lookingAtMouseRotation;
            _blockMovement = true;
            _mR.material = _mats[1];
            _rB.velocity = Vector3.zero;
            _attackingAllowed = false;
        }   

        // Logic while player is attacking
        if (Attacking)
        {
            // Lerping towards the target location
            transform.position = Vector3.Lerp(_savedPosition, _attackDestination, ((Time.time - _startTimeAttack) / AttackDuration));
            // Checking if the Attacking time is over and resetting all needed variables if time is reached
            if (Time.time - _startTimeAttack > AttackDuration)
            {
                EndAttack();
            }         
        }

        // Check if enough time since last attack has passed to attack again
        if (Time.time - _startTimeAttack > AttackCoolDown && !Attacking)
        {
            _attackingAllowed = true;
        }
        #endregion

        // Showing in engine where the player is gonna dash towards
        directionIndicator.transform.forward = _lookingAtMouseRotation;
    }

    /// <summary>
    /// Reset all Variables, Material and Rotation to the state it was in before attacking
    /// </summary>
    private void EndAttack()
    {
        Attacking = false;
        _blockMovement = false;
        transform.rotation = _savedRotation;
        _mR.material = _mats[0];
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle colliding with objects while attacking
        if (Attacking)
        {
            RamIntoObject(collision.gameObject);
            Attacking = false;
            EndAttack();
            
        }
    }

    /// <summary>
    /// Function that gets called when Rammy collides with any object while perfoming an attack action
    /// </summary>
    private void RamIntoObject(GameObject rammedObject)
    {
        Debug.Log( ("Rammed into:", rammedObject.name) );
        if (TagManager.HasTag(rammedObject, "enemy"))
        {
            rammedObject.GetComponent<EnemyTesting>().TakeDamage(AttackDamage);
        }
        else if (TagManager.HasTag(rammedObject, "wall"))
        {
            if (rammedObject.GetComponent<DestructibleWall>().Hit(gameObject))
            {
                Destroy(rammedObject);
            }
        }
        
    }
}
