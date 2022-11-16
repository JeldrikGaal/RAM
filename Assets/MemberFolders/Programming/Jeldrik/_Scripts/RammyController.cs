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
    private InputAction _dodge;
    private InputAction _charge;
    
    // Vector in which the character is currently moving according to player input
    private Vector2 _moveDirection;
    // Mouseposition in world space
    private Vector3 _mousePosition;
    // blocking movement 
    private bool _blockMovement;

    // Mouse input
    private float _leftMouseButton;
    private float _rightMouseButton;
    private bool _lastFrameLeftMouseButton;
    private float _frameCounterLeftMouseButton;
    private float _frameCounterLeftMouseButtonSave;
    private bool _lastFrameRightMouseButton;
    private float _frameCounterRightMouseButton;
    private float _frameCounterRightMouseButtonSave;

    // Action inputs
    private float _dodgingKey;

    private float _cameraDepth;
    private Rigidbody _rB;
    private MeshRenderer _mR;

    [Header("Character State")]
    // Bools describing playerstate
    [SerializeField] private bool Attacking;
    [SerializeField] private bool Dodging;
    [SerializeField] private bool Invincible;
    [SerializeField] private bool Walking;

    [Header("Player Stats")]
    // Player Values
    public float Health;
    // Speed Modifier 
    public float MovementSpeed;

    [Header("Attack")]
    // Attack Values
    [SerializeField] private float AttackDuration;
    [SerializeField] private float AttackDistance;
    [SerializeField] private float AttackCoolDown;
    [SerializeField] private float AttackDamage;

    // Variables for Attacking
    private float _startTimeAttack;
    private bool _attackingAllowed = true;
    private Vector3 _attackDestination;
    

    [Header("Dodge")]
    // Dodge Values
    [SerializeField] private float DodgeDuration;
    [SerializeField] private float DodgeDistance;
    [SerializeField] private float DodgeCoolDown;

    // Variables for Dodging
    private float _startTimeDodge;
    private bool _dodgingAllowed = true;
    private Vector3 _dodgeDestination;

    [Header ("Charg Attack")]
    // Charge Attack Values
    float MaxChargingDuration;
    [SerializeField] private float ChargeAttackDuration;
    [SerializeField] private float ChargeAttackDistance;
    [SerializeField] private float ChargeAttackCoolDown;
    [SerializeField] private float ChargeAttackDamage;

    // Variables for Charge Attack
    private float _startTimeCharging;
    private float _startTimeChargeAttack;
    private bool _chargeAttackAllowed = true;
    private Vector3 _chargeAttackDestination;


    // Saving rotation to reset after attacking and Dodging
    private Quaternion _savedRotation;
    private Vector3 _savedPosition;

    // Help variables for various purposes
    private Plane _groundPlane = new Plane(Vector3.up, 0);
    private Vector3 _mouseWorldPosition;
    private Vector3 _lookingAtMouseRotation;

    // Debugging
    [Header("DEBUGGING STUFF")]
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
        _dodge = _playerControls.Player.Dodge;
        _charge = _playerControls.Player.ChargeAttack;

        _move.Enable();
        _look.Enable();
        _attack.Enable();
        _dodge.Enable();
        _charge.Enable();
    }

    // Disabling PlayerControls when player gets disabled in the scene
    private void OnDisable()
    {
        _move.Disable();
        _look.Disable();
        _attack.Disable();
        _dodge.Disable();
        _charge.Disable();
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
        _rightMouseButton = _charge.ReadValue<float>();

        // Counting and resetting input frame counters
        if (_leftMouseButton == 0)
        {
            _frameCounterLeftMouseButtonSave = _frameCounterLeftMouseButton;
            _frameCounterLeftMouseButton = 0;
        }
        else
        {
            _frameCounterLeftMouseButtonSave = 0;
            _frameCounterLeftMouseButton += 1;
            _lastFrameLeftMouseButton = true;
        }
        if (_rightMouseButton == 0)
        {
            _frameCounterRightMouseButtonSave = _frameCounterRightMouseButton;
            _frameCounterRightMouseButton = 0;
        }
        else
        {
            _frameCounterRightMouseButtonSave = 0;
            _frameCounterRightMouseButton += 1;
            _lastFrameRightMouseButton = true;
        }

        //if (_frameCounterLeftMouseButton == 0)

        //Debug.Log( (_frameCounterLeftMouseButton, _frameCounterRightMouseButton) );

        

        // Reading Dodge input
        _dodgingKey = _dodge.ReadValue<float>();

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

        if (_frameCounterRightMouseButton == 0 && _frameCounterRightMouseButtonSave > 50)
        {
            Debug.Log("Short Relase");
        }

        else if (_frameCounterRightMouseButton == 0 && _frameCounterRightMouseButtonSave > 500)
        {
            Debug.Log("Long Relase");
        }

        // Changing all needed variables to indiciate and calculate attacking
        if (_rightMouseButton == 1 && !Attacking && _attackingAllowed)
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

        #region Dodging
        // Changing all needed variables to indiciate and calculate dodging
        if (_dodgingKey == 1 && !Attacking && _dodgingAllowed)
        {
            Dodging = true;
            _startTimeDodge = Time.time;
            _dodgeDestination = transform.position + _lookingAtMouseRotation * DodgeDistance;
            _savedRotation = transform.rotation;
            _savedPosition = transform.position;
            transform.up = _lookingAtMouseRotation;
            _blockMovement = true;
            _mR.material = _mats[2];
            _rB.velocity = Vector3.zero;
            _attackingAllowed = false;
            _dodgingAllowed = false;
            Invincible = true;
        }

        if (Dodging)
        {
            // Lerping towards the target location
            transform.position = Vector3.Lerp(_savedPosition, _dodgeDestination, ((Time.time - _startTimeDodge) / DodgeDuration));
            // Checking if the Attacking time is over and resetting all needed variables if time is reached
            if (Time.time - _startTimeDodge > DodgeDuration)
            {
                EndDodge();
            }
        }

        // Check if enough time since last dodge has passed to dodge again
        if (Time.time - _startTimeDodge > DodgeCoolDown && !Attacking && !Dodging)
        {
            _dodgingAllowed = true;
        }

        #endregion

        // Showing in engine where the player is gonna dash towards
        directionIndicator.transform.forward = _lookingAtMouseRotation;

        // Resetting last frame bools ( needs to stay at the bottom of Update ! )
        _lastFrameLeftMouseButton = false;
        _lastFrameRightMouseButton = false;
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

    /// <summary>
    /// Reset all Variables, Material and Rotation to the state it was in before dodging
    /// </summary>
    private void EndDodge()
    {
        Dodging = false;
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
        if (Dodging)
        {
            Dodging = false;
            EndDodge();
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
            Debug.Log(rammedObject.GetComponent<DestructibleWall>().Hit(gameObject));
            if (rammedObject.GetComponent<DestructibleWall>().Hit(gameObject))
            {
                Destroy(rammedObject);
            }
        }
        
    }
}
