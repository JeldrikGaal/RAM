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
    private InputAction _charge;
    private InputAction _ability1;
    private InputAction _ability2;
    
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

    // Ability Inputs
    private float _ability1Key;
    private float _ability2Key;

    // Components
    [Header("Components")]
    private float _cameraDepth;
    private Rigidbody _rB;
    private MeshRenderer _mR;
    [SerializeField] private RammyFrontCheck _frontCheck;

    [Header("Character State")]
    // Bools describing playerstate
    [SerializeField] private bool Attacking;
    [SerializeField] private bool BasicAttacking;
    [SerializeField] private bool Dashing;
    [SerializeField] private bool Invincible;
    [SerializeField] private bool Walking;

    [Header("Player Stats")]
    // Player Values
    public float Health;
    // Speed Modifier 
    public float MovementSpeed;

    [Header("Basic Attack")]    
    // Attack Values
    [SerializeField] private float BasicAttackCoolDown;
    [SerializeField] private float BasicAttackDamage;
    [SerializeField] private float BasicAttackDuration;

    // Variables for basic attack
    private float _startTimeBasicAttack;
    private bool _basicAttackAllowed;

    [Header("Dash")]
    // Dash Values
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashDistance;
    [SerializeField] private float DashCoolDown;

    // Variables for Dashing
    private float _startTimeDash;
    private bool _dashingAllowed = true;
    private Vector3 _dashDestination;

    [SerializeField] private DashVisuals _dashVisuals;

    
    // Charge Attack Values
    [Header("Charge Attack")]
    [SerializeField] private float ChargeAttackDuration;
    [SerializeField] private float ChargeAttackDistance;
    [SerializeField] private float ChargeAttackCoolDown;
    [SerializeField] private float ChargeAttackDamage;
    [SerializeField] private float MaxChargeTime;

    // Variables for Charge Attack
    private float _startTimeChargeAttack;
    private bool _chargeAttackAllowed = true;
    private Vector3 _chargeAttackDestination;

    // Saving rotation to reset after attacking and Dashing
    private Quaternion _savedRotation;
    private Vector3 _savedPosition;

    [Header("Visual Effects")]
    // VFX
    [Range(0.0f, 2.0f)] [SerializeField] private float _bloodSpread = 0.5f;
    [Range(-0.5f, 10.0f)] [SerializeField] private float _bloodForceMin;
    [Range(-0.5f, 10.0f)] [SerializeField] private float _bloodForceMax;
    [SerializeField] private GameObject _bloodSpreadCalculator; // I'm dumb, so I'm letting Unity do my math -H�vard
    [SerializeField] private GameObject _bloodBomb;
    private Vector3 _bloodDir1;
    private Vector3 _bloodDir2;
    [SerializeField] private BloodySteps _stepScript;

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
        _charge = _playerControls.Player.ChargeAttack;
        _ability1 = _playerControls.Player.Ability1;
        _ability2 = _playerControls.Player.Ability2;

        _move.Enable();
        _look.Enable();
        _attack.Enable();
        _charge.Enable();
        _ability1.Enable();
        _ability2.Enable();
    }

    // Disabling PlayerControls when player gets disabled in the scene
    private void OnDisable()
    {
        _move.Disable();
        _look.Disable();
        _attack.Disable();
        _charge.Disable();
        _ability1.Disable();
        _ability2.Disable();
    }

    
    void Start()
    {
        _rB = GetComponent<Rigidbody>();
        _mR = GetComponent<MeshRenderer>();
        if(GetComponent<DashVisuals>()) _dashVisuals = GetComponent<DashVisuals>();
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
            _frameCounterLeftMouseButton += Time.deltaTime;
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
            _frameCounterRightMouseButton += Time.deltaTime;
            _lastFrameRightMouseButton = true;
        }

        // Reading Ability Key Inputs
        _ability1Key = _ability1.ReadValue<float>();
        _ability2Key = _ability2.ReadValue<float>();

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

        // Checking if player is allowed to move and if so adjust Rigidbody velocity according to input. Additionally turn the player in the direction its walking
        if (!_blockMovement)
        {
            _rB.velocity = vel;

            // Rotate player in the direction its walking
            if (_moveDirection.x < 0 && _moveDirection.y == 0) // looking left
            {
                transform.rotation = Quaternion.Euler(90, 0, 90);
            }
            else if (_moveDirection.x < 0 && _moveDirection.y < 0) // looking down left
            {
                transform.rotation = Quaternion.Euler(90, 0, 135);
            }
            else if (_moveDirection.x == 0 && _moveDirection.y < 0) // looking down
            {
                transform.rotation = Quaternion.Euler(90, 0, 180);
            }
            else if (_moveDirection.x > 0 && _moveDirection.y < 0) // looking down right
            {
                transform.rotation = Quaternion.Euler(90, 0, 225);
            }
            else if (_moveDirection.x > 0 && _moveDirection.y == 0) // looking right
            {
                transform.rotation = Quaternion.Euler(90, 0, 270);
            }
            else if (_moveDirection.x > 0 && _moveDirection.y > 0) // looking up right
            {
                transform.rotation = Quaternion.Euler(90, 0, 315);
            }
            else if (_moveDirection.x == 0 && _moveDirection.y > 0) // looking up 
            {
                transform.rotation = Quaternion.Euler(90, 0, 0);
            }
            else if (_moveDirection.x < 0 && _moveDirection.y > 0) // looking up left 
            {
                transform.rotation = Quaternion.Euler(90, 0, 45);
            }
        }

        #endregion

        #region Attacking

        // Trigger approriate Action if the player releases the Dash/ChargeDash Key
        if (_frameCounterRightMouseButton == 0 && _frameCounterRightMouseButtonSave > 0)
        {
            ReleaseChargeButton(_frameCounterRightMouseButtonSave);
        }

        // Logic while player is attacking
        if (Attacking)
        {
            // Lerping towards the target location
            transform.position = Vector3.Lerp(_savedPosition, _chargeAttackDestination, ((Time.time - _startTimeChargeAttack) / ChargeAttackDuration));
            // Checking if the Attacking time is over and resetting all needed variables if time is reached
            if (Time.time - _startTimeChargeAttack > ChargeAttackDuration)
            {
                EndChargeAttack();
            }     

        }
        if (BasicAttacking)
        {
            if (Time.time - _startTimeBasicAttack > BasicAttackDuration)
            {
                EndBasicAttack();
            }
        }

        // Trigger Basic attack if input is received and attack is possible
        if (_leftMouseButton == 1 && _basicAttackAllowed && !Attacking)
        {
            StartBasicAttack();
        }

        // Check if enough time since last charge attack has passed to attack again
        if (Time.time - _startTimeChargeAttack > ChargeAttackCoolDown && !Attacking)
        {
            _chargeAttackAllowed = true;
        }

        // Check if enough time since last basic attack has passed to attack again
        if (Time.time - _startTimeBasicAttack > BasicAttackCoolDown && !Attacking)
        {
            _basicAttackAllowed = true;
        }
        #endregion

        #region Dashing
        // Changing all needed variables to indiciate and calculate Dashing


        if (Dashing)
        {
            // Lerping towards the target location
            transform.position = Vector3.Lerp(_savedPosition, _dashDestination, ((Time.time - _startTimeDash) / DashDuration));
            // Checking if the Attacking time is over and resetting all needed variables if time is reached
            if (Time.time - _startTimeDash > DashDuration)
            {
                EndDash();
            }
        }

        // Check if enough time since last dash has passed to dash again
        if (Time.time - _startTimeDash > DashCoolDown && !Attacking && !Dashing)
        {
            _dashingAllowed = true;
        }

        #endregion

        // Showing in engine where the player is gonna dash towards
        directionIndicator.transform.forward = _lookingAtMouseRotation;

        // Resetting last frame bools ( needs to stay at the bottom of Update ! )
        _lastFrameLeftMouseButton = false;
        _lastFrameRightMouseButton = false;
    }

    /// <summary>
    /// Perform a Basic Attack
    /// </summary>
    private void StartBasicAttack()
    {
        if (!BasicAttacking && !Attacking && _basicAttackAllowed)
        {
            BasicAttacking = true;
            _startTimeBasicAttack = Time.time;
            _basicAttackAllowed = false;
            _blockMovement = true;
            _rB.velocity = Vector3.zero;

            foreach (GameObject g in _frontCheck._objectsInCollider)
            {
                if (TagManager.HasTag(g, "enemy"))
                {
                    g.GetComponent<EnemyTesting>().TakeDamage(BasicAttackDamage, transform.up);
                }
            }
        }
    }

    /// <summary>
    /// Ending the basic attack and resetting all needed variables 
    /// </summary>
    private void EndBasicAttack()
    {
        BasicAttacking = false;
        _blockMovement = false;
    }

    /// <summary>
    /// Handles the player releasing the charge attack button and triggering the appropriate attack type
    /// </summary>
    private void ReleaseChargeButton(float chargeTime)
    {
        if (chargeTime < 1)
        {
            StartDash();
        }
        else if (chargeTime > 1)
        {
            StartChargeAttack(chargeTime);
        }
    }
    /// <summary>
    /// Setting all variables to trigger the start of a charge attack
    /// </summary>
    private void StartChargeAttack(float chargingTime)
    {
        if (!Attacking && _chargeAttackAllowed)
        {
            Attacking = true;
            _startTimeChargeAttack = Time.time;
            _chargeAttackDestination = transform.position + _lookingAtMouseRotation * ChargeAttackDistance;
            _savedRotation = transform.rotation;
            _savedPosition = transform.position;
            transform.up = _lookingAtMouseRotation;
            _blockMovement = true;
            _mR.material = _mats[1];
            _rB.velocity = Vector3.zero;
            _chargeAttackAllowed = false;
        }
    }

    /// <summary>
    /// Reset all Variables, Material and Rotation to the state it was in before attacking
    /// </summary>
    private void EndChargeAttack()
    {
        Attacking = false;
        _blockMovement = false;
        transform.rotation = _savedRotation;
        _mR.material = _mats[0];
    }

    /// <summary>
    /// Setting all variables to trigger the start of a dash
    /// </summary>
    private void StartDash()
    {
        if (!Attacking && _dashingAllowed)
        {
            if (_dashVisuals != null) _dashVisuals.StartDash();
            Dashing = true;
            _startTimeDash = Time.time;
            _dashDestination = transform.position + _lookingAtMouseRotation * DashDistance;
            _savedRotation = transform.rotation;
            _savedPosition = transform.position;
            transform.up = _lookingAtMouseRotation;
            _blockMovement = true;
            _mR.material = _mats[2];
            _rB.velocity = Vector3.zero;
            _chargeAttackAllowed = false;
            _dashingAllowed = false;
            Invincible = true;
        }
    }

    /// <summary>
    /// Reset all Variables, Material and Rotation to the state it was in before Dashing
    /// </summary>
    private void EndDash()
    {
        Dashing = false;
        _blockMovement = false;
        transform.rotation = _savedRotation;
        _mR.material = _mats[0];

        if (_dashVisuals != null) _dashVisuals.EndDash();
    }

    // Checking for any collisions Rammy encouters and reacting accordingly
    private void OnCollisionEnter(Collision collision)
    {
        // Handle colliding with objects while attacking
        if (Attacking)
        {
            RamIntoObject(collision.gameObject);
            Attacking = false;
            EndChargeAttack();
        }
        // Handle colliding with objects while dashing
        if (Dashing)
        {
            //Dashing = false;
            //EndDash();
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
            // Calling Damage on the enemy script
            rammedObject.GetComponent<EnemyTesting>().TakeDamage(ChargeAttackDamage, transform.up);

            // VFX:

            var _bloodPrefab = Instantiate(_bloodBomb, rammedObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));

            // Vector3 _enemyDirection = rammedObject.transform.position - this.transform.position;
            _bloodSpreadCalculator.transform.rotation = this.transform.rotation;

            _bloodSpreadCalculator.transform.GetChild(0).transform.localScale = new Vector3(_bloodSpread, 1, 1);

            _bloodDir1 = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(0).transform.position - _bloodSpreadCalculator.transform.position;
            _bloodDir2 = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(1).transform.position - _bloodSpreadCalculator.transform.position;

            foreach (Transform child in _bloodPrefab.transform)
            {
                child.GetComponent<StickyBlood>()._bloodStepScript = _stepScript;
                child.GetComponent<InitVelocity>().CalcDirLeft = _bloodDir1;
                child.GetComponent<InitVelocity>().CalcDirRight = _bloodDir2;
                child.GetComponent<InitVelocity>().BloodForceMin = _bloodForceMin;
                child.GetComponent<InitVelocity>().BloodForceMax = _bloodForceMin;
            }

        }
        else if (TagManager.HasTag(rammedObject, "wall"))
        {
            Debug.Log(rammedObject.GetComponent<IRammable>().Hit(gameObject));
            if (rammedObject.GetComponent<IRammable>().Hit(gameObject))
            {
                Destroy(rammedObject);
            }
        }
        
    }
}
