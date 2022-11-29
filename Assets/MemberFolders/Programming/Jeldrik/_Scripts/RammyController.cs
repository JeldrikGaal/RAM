using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
//using UnityEngine.UIElements.Experimental;
//using static UnityEngine.EventSystems.EventTrigger;
//using static UnityEngine.Rendering.DebugUI.Table;

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
    private InputAction _ability3;
    private InputAction _ability4;
    private InputAction _ability5;


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
    private float _ability3Key;
    private float _ability4Key;
    private float _ability5Key;
    [SerializeField] private Ability1 _ability1Script;
    [SerializeField] private Ability2 _ability2Script;
    [SerializeField] private Ability3 _ability3Script;
    [SerializeField] private Ability4 _ability4Script;
    [SerializeField] private Ability5 _ability5Script;
    [SerializeField] private bool _startFullAbilities = true;

    private List<bool> _learnedAbilities = new List<bool>();


    // Components
    [Header("Components")]
    private float _cameraDepth;
    private Rigidbody _rB;
    private MeshRenderer _mR;
    private HealthBar _healthBar;
    [SerializeField] private RammyFrontCheck _frontCheck;
    [SerializeField] private CinemachineTopDown _cameraScript;
    [SerializeField] private StatManager _comboSystem;
    [SerializeField] private TimeStopper _timeStopper;


    [Header("Character State")]
    // Bools describing playerstate
    [SerializeField] private bool Attacking;
    [SerializeField] private bool BasicAttacking;
    [SerializeField] private bool Dashing;
    [SerializeField] private bool Invincible;
    [SerializeField] private bool Walking;
    [SerializeField] private bool UsingAbility;

    [Header("Player Stats")]
    // Player Values
    [SerializeField] private float Health;
    [SerializeField] private float MaxHealth;
    [SerializeField] private float HealPercentage;

    // Speed Modifier 
    [SerializeField] public float MovementSpeed;

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
    [SerializeField] private float DashAttackDamage;
    [SerializeField] private bool DashUpgraded;

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
    [SerializeField] private float MinChargeTime;
    [SerializeField] private float MaxChargeTime;


    // Variables for Charge Attack
    private float _startTimeChargeAttack;
    private bool _chargeAttackAllowed = true;
    private Vector3 _chargeAttackDestination;
    private GameObject _chargedEnemy;
    public Vector3 _chargedEnemyOffset;

    // Saving rotation to reset after attacking and Dashing
    private Quaternion _savedRotation;
    private Vector3 _savedPosition;

    // VFX
    [Header("Visual Effects")]
    [SerializeField] private RammyVFX _rammyVFX;
    [SerializeField] private float _freezeScaleRam;
    [SerializeField] private float _freezeTimeRam;

    [SerializeField] private float _freezeScaleHit;
    [SerializeField] private float _freezeTimeHit;

    // Help variables for various purposes
    private Plane _groundPlane = new Plane(Vector3.up, 0);
    private Vector3 _mouseWorldPosition;
    private Vector3 _lookingAtMouseRotation;
    private Vector3 _directionIndicatorScaleSave;
    private Vector3 _directionIndicatorPosSave;

    [Header("Buff Values")]
    [SerializeField] private bool _hasDamageBuff;
    public float DamageModifier;
    [HideInInspector] public float AppliedDamageModifier; // Multiply this by the damage in each ability
    public float DamageBuffDuration;
    private float _damageBuffTimer;
    public bool HasSpeedBuff;
    public float SpeedModifier;
    public float SpeedBuffDuration;
    private float _speedBuffTimer;
    private bool _setSpeed = true;
    [SerializeField] private bool _hasDamageReductionBuff;
    public float DamageReductionModifier;
    public float DamageReductionBuffDuration;
    [SerializeField] private float _damageReductionBuffTimer;
    public bool HasStunBuff;
    public float StunBuffModifier;
    public float StunBuffDuration;
    [SerializeField] private float _stunBuffTimer;


    // Debugging
    [Header("DEBUGGING STUFF")]
    [SerializeField] private List<Material> _mats = new List<Material>();
    [SerializeField] private GameObject directionIndicator;
    private GameObject _directionIndicatorTip;

    #region Startup and Disable
    // Setting Input Actions on Awake
    private void Awake()
    {
        _playerControls = new RammyInputActions();
        _cameraDepth = Camera.main.transform.position.z;
    }
    void Start()
    {
        _rB = GetComponent<Rigidbody>();
        _mR = GetComponent<MeshRenderer>();
        _healthBar = GetComponentInChildren<HealthBar>();
        if (GetComponent<DashVisuals>()) _dashVisuals = GetComponent<DashVisuals>();
        _directionIndicatorTip = directionIndicator.transform.GetChild(0).gameObject;
        _directionIndicatorScaleSave = _directionIndicatorTip.transform.localScale;
        _directionIndicatorPosSave = _directionIndicatorTip.transform.localPosition;
        for (int i = 0; i < 5; i++)
        {
            if (_startFullAbilities)
            {
                _learnedAbilities.Add(true);
            }
            else
            {
                _learnedAbilities.Add(false);
            }
        }
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
        _ability3 = _playerControls.Player.Ability3;
        _ability4 = _playerControls.Player.Ability4;
        _ability5 = _playerControls.Player.Ability5;

        _move.Enable();
        _look.Enable();
        _attack.Enable();
        _charge.Enable();
        _ability1.Enable();
        _ability2.Enable();
        _ability3.Enable();
        _ability4.Enable();
        _ability5.Enable();
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
        _ability3.Disable();
        _ability4.Disable();
        _ability5.Disable();
    }
    #endregion

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
        _ability3Key = _ability3.ReadValue<float>();
        _ability4Key = _ability4.ReadValue<float>();
        _ability5Key = _ability5.ReadValue<float>();

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
        //_moveDirection = _moveDirection * ( _cameraScript.transform.rotation.eulerAngles).normalized;
        Vector3 vel = new Vector3(_moveDirection.x, 0, _moveDirection.y);
        vel = Quaternion.AngleAxis(-45, Vector3.up) * vel;
        //vel = Quaternion.Euler(0, 0, 45) * vel;

        // Checking if player is allowed to move and if so adjust Rigidbody velocity according to input. Additionally turn the player in the direction its walking
        if (!_blockMovement)
        {
            _rB.velocity = vel;
            int baseRotation = 135;
            // Rotate player in the direction its walking
            if (_moveDirection.x < 0 && _moveDirection.y == 0) // looking left
            {
                transform.rotation = Quaternion.Euler(90, 0, baseRotation);
            }
            else if (_moveDirection.x < 0 && _moveDirection.y < 0) // looking down left
            {
                transform.rotation = Quaternion.Euler(90, 0, baseRotation + 45 * 1);
            }
            else if (_moveDirection.x == 0 && _moveDirection.y < 0) // looking down
            {
                transform.rotation = Quaternion.Euler(90, 0, baseRotation + 45 * 2);
            }
            else if (_moveDirection.x > 0 && _moveDirection.y < 0) // looking down right
            {
                transform.rotation = Quaternion.Euler(90, 0, baseRotation + 45 * 3);
            }
            else if (_moveDirection.x > 0 && _moveDirection.y == 0) // looking right
            {
                transform.rotation = Quaternion.Euler(90, 0, baseRotation + 45 * 4);
            }
            else if (_moveDirection.x > 0 && _moveDirection.y > 0) // looking up right
            {
                //transform.rotation = Quaternion.Euler(90, 0, 315);
                transform.rotation = Quaternion.Euler(90, 0, baseRotation + 45 * 5);
            }
            else if (_moveDirection.x == 0 && _moveDirection.y > 0) // looking up 
            {
                transform.rotation = Quaternion.Euler(90, 0, baseRotation + 45 * 6);
            }
            else if (_moveDirection.x < 0 && _moveDirection.y > 0) // looking up left 
            {
                transform.rotation = Quaternion.Euler(90, 0, baseRotation + 45 * 7);
            }

            #region saving
            /*
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
            */
            #endregion

        }

        #endregion

        #region Attacking

        // Trigger approriate Action if the player releases the Dash/ChargeDash Key
        if (_frameCounterRightMouseButton == 0 && _frameCounterRightMouseButtonSave > 0)
        {
            ReleaseChargeButton(_frameCounterRightMouseButtonSave);
            _frameCounterRightMouseButton = 0;
        }

        if (_frameCounterRightMouseButton > MaxChargeTime)
        {
            ReleaseChargeButton(MaxChargeTime);
            _frameCounterRightMouseButton = 0;
        }

        // Visualize the charging progress
        if (_frameCounterRightMouseButton > 0)
        {
            _directionIndicatorTip.transform.localScale = new Vector3(_directionIndicatorScaleSave.x, _directionIndicatorScaleSave.y, _directionIndicatorScaleSave.z + (_frameCounterRightMouseButton / MaxChargeTime));
            _directionIndicatorTip.transform.localPosition = new Vector3(_directionIndicatorPosSave.x, _directionIndicatorPosSave.y, _directionIndicatorPosSave.z + ((_frameCounterRightMouseButton / MaxChargeTime) * 0.5f));
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

        // If rammy picks up an enemy while charging drag it along
        if (_chargedEnemy != null)
        {
            _chargedEnemy.transform.position = transform.position + _chargedEnemyOffset;
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

        #region Abilities

        List<Abilities> l = new List<Abilities>();
        l.Add(_ability1Script);

        // Checking if the player is already using an ability and performing wanted ability if not
        if (!UsingAbility)
        {
            if (_ability1Key == 1)
            {
                if (_learnedAbilities[0]) l[0].CheckActivate();

            }
            else if (_ability2Key == 1)
            {
                if (_learnedAbilities[1]) _ability2Script.CheckActivate();
            }
            else if (_ability3Key == 1)
            {
                if (_learnedAbilities[2]) _ability3Script.CheckActivate();
            }
            else if (_ability4Key == 1)
            {
                if (_learnedAbilities[3]) _ability4Script.CheckActivate();
            }
            else if (_ability5Key == 1)
            {
                if (_learnedAbilities[4]) _ability5Script.CheckActivate();
            }
        }
        #endregion

        #region Buffs

        #region DamageBuff

        // Checks if the buff is active
        if (_hasDamageBuff)
        {
            // Timer counts down every second
            _damageBuffTimer -= Time.deltaTime;

            // The applied damage modifier is set to the wanted damage modifier
            AppliedDamageModifier = DamageModifier;
        }

        // If the timer is over
        if (_damageBuffTimer <= 0)
        {
            // "Turns off" the buff
            _hasDamageBuff = false;

            // Sets the damage back to normal
            AppliedDamageModifier = 1;
        }

        #endregion

        #region  SpeedBuff

        // Checks if the buff is active
        if (HasSpeedBuff)
        {
            // Timer counts down every second
            _speedBuffTimer -= Time.deltaTime;
        }

        // Checks to see if the timer is over, if we haven't already set the speed modifier, and if we have the buff
        if (_speedBuffTimer <= 0 && !_setSpeed && HasSpeedBuff)
        {
            // Turns off the buff
            HasSpeedBuff = false;

            // Reduces the speed of the player by the modifier again
            MovementSpeed /= SpeedModifier;

            // Says that we have already set the speed so that it doesn't reduce the speed every frame
            _setSpeed = true;
        }

        #endregion

        #region  DamageReductionBuff

        // Checks if the buff is active
        if (_hasDamageReductionBuff)
        {
            // Timer counts down every second
            _damageReductionBuffTimer -= Time.deltaTime;
        }

        // Checks to see if the timer is over
        if (_damageReductionBuffTimer <= 0)
        {
            // Turns off the buff
            _hasDamageReductionBuff = false;
        }

        #endregion

        #region StunBuff

        if (HasStunBuff)
        {
            _stunBuffTimer -= Time.deltaTime;
        }

        if (_stunBuffTimer <= 0)
        {
            HasStunBuff = false;
        }

        #endregion


        #endregion

        #region Debugging

        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamageRammy(5);
        }

        #endregion
        // Showing in engine where the player is gonna dash towards
        directionIndicator.transform.forward = _lookingAtMouseRotation;

        // Resetting last frame bools ( needs to stay at the bottom of Update ! )
        _lastFrameLeftMouseButton = false;
        _lastFrameRightMouseButton = false;

    }


    #region Basic Attack
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
                if (g != null) // Temporary fix
                {
                    if (TagManager.HasTag(g, "enemy"))
                    {
                        _rammyVFX.NormalAttack(g);
                        if (g.GetComponent<EnemyTesting>().TakeDamage(BasicAttackDamage * AppliedDamageModifier, transform.up))
                        {
                            Kill(g);
                        }
                    }
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
    #endregion

    #region Dash and ChargeAttack
    /// <summary>
    /// Handles the player releasing the charge attack button and triggering the appropriate attack type
    /// </summary>
    private void ReleaseChargeButton(float chargeTime)
    {
        if (chargeTime < MinChargeTime)
        {
            StartDash();
        }
        else if (chargeTime > MinChargeTime)
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

            RaycastHit hit;
            // calculating the destination for the charge by taking the direction from the player to the mouse, the max charging distance and how much of the max charging time has already passed
            _chargeAttackDestination = transform.position + _lookingAtMouseRotation * (ChargeAttackDistance * (chargingTime / MaxChargeTime));

            // Checking if player would end up in an object while charging and shortening charge if thats the case
            if (Physics.Raycast(transform.position, _lookingAtMouseRotation, out hit, (ChargeAttackDistance * (chargingTime / MaxChargeTime))))
            {
                RaycastHit hit2;
                if (Physics.Raycast(_chargeAttackDestination + Vector3.up * 100, Vector3.down, out hit2, 105) && hit.transform == hit2.transform)
                {
                    _chargeAttackDestination = hit.point;
                }

            }

            // Saving rotation and position
            _savedRotation = transform.rotation;
            _savedPosition = transform.position;
            // Setting rotation to make player look in charge direction
            transform.up = _lookingAtMouseRotation;
            transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            // Setting various variables to display that player is dashing in script and visually
            _blockMovement = true;
            _mR.material = _mats[1];
            _rB.velocity = Vector3.zero;
            _chargeAttackAllowed = false;
            _dashVisuals.StartDash(transform.rotation);
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
        _directionIndicatorTip.transform.localPosition = _directionIndicatorPosSave;
        _directionIndicatorTip.transform.localScale = _directionIndicatorScaleSave;
        if (_chargedEnemy != null)
        {
            _chargedEnemy = null;
        }
        _dashVisuals.EndDash();
    }

    /// <summary>
    /// Setting all variables to trigger the start of a dash
    /// </summary>
    private void StartDash()
    {
        if (!Attacking && _dashingAllowed)
        {

            Dashing = true;
            _startTimeDash = Time.time;
            RaycastHit hit;
            _dashDestination = transform.position + _lookingAtMouseRotation * DashDistance;

            // Checking if player would end up in an object while dashing and shortening dash if thats the case
            if (Physics.Raycast(transform.position, _lookingAtMouseRotation, out hit, DashDistance))
            {
                RaycastHit hit2;
                if (Physics.Raycast(_dashDestination + Vector3.up * 100, Vector3.down, out hit2, 105) && hit.transform == hit2.transform)
                {
                    _dashDestination = hit.point;
                }

            }

            _savedRotation = transform.rotation;
            _savedPosition = transform.position;
            transform.up = _lookingAtMouseRotation;
            transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            _blockMovement = true;
            _mR.material = _mats[2];
            _rB.velocity = Vector3.zero;
            _chargeAttackAllowed = false;
            _dashingAllowed = false;

            if (_dashVisuals != null) _dashVisuals.StartDash(transform.rotation);

            // Handle differences in dash if it has been upgraded already
            if (DashUpgraded)
            {
                Invincible = true;
            }

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
    #endregion

    /// <summary>
    /// Function that gets called when Rammy collides with any object while perfoming an attack action
    /// </summary>
    private void RamIntoObject(GameObject rammedObject)
    {
        Debug.Log(("Rammed into:", rammedObject.name));
        if (TagManager.HasTag(rammedObject, "enemy"))
        {
            if (_chargedEnemy == null)
            {
                _chargedEnemy = rammedObject;
                _chargedEnemyOffset = _chargedEnemy.transform.position - transform.position;
            }

            // Calling Damage on the enemy script
            if (rammedObject.GetComponent<EnemyTesting>().TakeDamage(ChargeAttackDamage * AppliedDamageModifier, transform.up))
            {
                Kill(rammedObject);
            }

            // VFX

            _rammyVFX.RamAttack(_chargedEnemy);

            // If time scale is not already slowed, slow down time on enemy hit
            if (Time.timeScale == 1) _timeStopper.PauseTime(_freezeScaleRam, _freezeTimeRam);

            _cameraScript.ScreenShake(0.5f);



            // Sorry for filling your lovely code up with my old commented out trash code.
            //  Then why not just delete it?  -JG

            /*
            var _bloodPrefab = Instantiate(_bloodBomb, rammedObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            _bloodPrefab.transform.localScale *= _bloodSize;


            // Vector3 _enemyDirection = rammedObject.transform.position - this.transform.position;
            _bloodSpreadCalculator.transform.rotation = this.transform.rotation;

            _bloodSpreadCalculator.transform.GetChild(0).transform.localScale = new Vector3(_bloodSpread, 1, 1);

            _bloodDir1 = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(0).transform.position - _bloodSpreadCalculator.transform.position;
            _bloodDir2 = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(1).transform.position - _bloodSpreadCalculator.transform.position;

            var i = 0;
            foreach (Transform child in _bloodPrefab.transform)
            {
                if(i >= _bloodAmount)
                {
                    Destroy(child.gameObject);
                }
                i++;
                child.GetComponent<StickyBlood>().BloodStepScript = _stepScript;
                child.GetComponent<StickyBlood>().BloodSize = _bloodSize;
                child.GetComponent<InitVelocity>().CalcDirLeft = _bloodDir1;
                child.GetComponent<InitVelocity>().CalcDirRight = _bloodDir2;
                child.GetComponent<InitVelocity>().BloodForceMin = _bloodForceMin;
                child.GetComponent<InitVelocity>().BloodForceMax = _bloodForceMin;
            }
            */

        }
        else if (TagManager.HasTag(rammedObject, "wall"))
        {
            Debug.Log(rammedObject.GetComponent<IRammable>().Hit(gameObject));
            if (rammedObject.GetComponent<IRammable>().Hit(gameObject))
            {
                Destroy(rammedObject);
            }
        }
        else if (TagManager.HasTag(rammedObject, "knockdownbridge"))
        {
            // Makes the bridge rotate when it is rammed
            rammedObject.GetComponent<KnockDownBridgeScript>().CanRotate = true;
        }
        else if (TagManager.HasTag(rammedObject, "objectfalltree"))
        {
            rammedObject.GetComponent<ObjectFallFromTree>().DropItem = true;
        }

    }

    // Checking for any collisions Rammy encouters and reacting accordingly
    private void OnCollisionEnter(Collision collision)
    {
        // Handle colliding with objects while attacking
        if (Attacking)
        {
            RamIntoObject(collision.gameObject);
        }
        // Handle colliding with objects while dashing
        if (Dashing)
        {
            if (TagManager.HasTag(collision.gameObject, "enemy"))
            {
                if (collision.gameObject.GetComponent<EnemyTesting>().TakeDamage(DashAttackDamage * AppliedDamageModifier, transform.up))
                {
                    Kill(collision.gameObject);
                }
            }
        }

        //Egg damage
        if (TagManager.HasTag(collision.gameObject, "egg"))
        {
            TakeDamageRammy(1.5f);
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checks to see if we collided with a damage powerup
        if (other.tag == "DamagePowerup")
        {
            // Turns on the buff
            _hasDamageBuff = true;

            // Adds time to the buff timer
            _damageBuffTimer = DamageBuffDuration;

            // Destroys the buff so it can't be picked up more than once
            Destroy(other.gameObject);
        }

        // Checks to see if we collided with a speed powerup
        if (other.tag == "SpeedPowerup")
        {
            if (!HasSpeedBuff)
            {
                // Modifies the speed of the player by the speed modifier
                MovementSpeed *= SpeedModifier;
            }

            // Turns on the speed buff
            HasSpeedBuff = true;

            // Adds time to the buff timer
            _speedBuffTimer = SpeedBuffDuration;


            // Sets a bool that helps with setting the speed when the buff is over
            _setSpeed = false;

            // Destroys the buff so it can't be picked up more than once
            Destroy(other.gameObject);
        }

        if (other.tag == "DamageReductionPowerup")
        {
            // Turns on the damage reduction buff
            _hasDamageReductionBuff = true;

            // Adds time to the buff timer
            _damageReductionBuffTimer = DamageReductionBuffDuration;

            // Destroys the buff so it can't be picked up more than once 
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// Heals Rammy by the given amount to a maxium of the max hp
    /// </summary>
    public void Heal(int healing)
    {
        Health = Math.Min(MaxHealth, Health + healing);
        _healthBar.UpdateHealthBar(healing / MaxHealth);
    }

    /// <summary>
    /// Function that should be called whenever rammy kills an enemy
    /// </summary>
    /// <param name="enemy"></param>
    public void Kill(GameObject enemy)
    {
        Debug.Log(MaxHealth * (HealPercentage / 100f));
        Heal((int)(MaxHealth * (HealPercentage / 100f)));
        if (_comboSystem) _comboSystem.AddKill();
    }

    #region Setter / Getter functions
    // Functions to start and end the usage of any ability
    public void StartUsingAbility()
    {
        UsingAbility = true;

    }
    public void EndUsingAbility()
    {
        UsingAbility = false;
    }

    // Blocking and unblocking player controlled movement
    public void BlockPlayerMovment()
    {
        _blockMovement = true;
    }
    public void UnBlockPlayerMovement()
    {
        _blockMovement = false;
    }

    public List<Abilities> GetAbilityScripts()
    {
        List<Abilities> abilities = new List<Abilities>();

        abilities.Add(_ability1Script);
        abilities.Add(_ability2Script);
        abilities.Add(_ability3Script);
        abilities.Add(_ability4Script);
        abilities.Add(_ability5Script);

        return abilities;
    }

    public bool GetUsingAbility()
    {
        return UsingAbility;
    }

    // Makes Info about charging available to the Ability UI
    public List<float> GetChargeInfo()
    {
        List<float> chargeInfo = new List<float>();
        chargeInfo.Add(_frameCounterRightMouseButton);
        chargeInfo.Add(_frameCounterRightMouseButtonSave);
        chargeInfo.Add(MaxChargeTime);
        return chargeInfo;
    }

    public bool GetAttacking()
    {
        return Attacking;
    }

    public bool GetDashing()
    {
        return Dashing;
    }

    public List<bool> GetAbilitiesLearned()
    {
        return _learnedAbilities;
    }

    public void LearnAbility(int index)
    {
        _learnedAbilities[index] = true;
    }
    #endregion

    // Triggerin ScreenShake with defined strength
    public void AddScreenShake(float strength)
    {
        _cameraScript.ScreenShake(strength);
    }

    // Function to call when Rammy takes any sort of damage
    public void TakeDamageRammy(float _damage)
    {
        // If Rammy is currently in an I frame dont take damage and dont show damage effects
        if (Invincible)
        {
            return;
        }
        // Short Time slow to emphazise taking damage
        _timeStopper.PauseTime(_freezeScaleHit, _freezeTimeHit);
        // Small ScreenShake
        AddScreenShake(0.3f);

        float appliedDamage;
        // If the player has the damage reduction buff
        if (_hasDamageReductionBuff)
        {
            // Take damage divided by the damage reduction modifier
            appliedDamage = (_damage / DamageReductionModifier);
        }
        else
        {
            // Checking if Rammy died from taking damage
            appliedDamage = _damage;
        }

        // Actually apply damage
        Health -= appliedDamage;

        // Apply damage to health bar
        _healthBar.UpdateHealthBar(-(appliedDamage / MaxHealth));

        // Stopping combo 
        if (_comboSystem != null)
        {
            _comboSystem.EndCombo();
        }


        // Cancel Charging Ram Attack 
        if (_frameCounterRightMouseButton > 0)
        {
            _frameCounterRightMouseButton = 0;
            _frameCounterRightMouseButtonSave = 0;
        }

        // Die if falls below 0 health
        if (Health <= 0)
        {
            Die();
        }
        // TODO: More VFX
    }

    // Rammy fell below 0 health and has now died. Deal with it in this function
    private void Die()
    {
        Debug.Log("RAMMY HAS DIED!!!!!");
        Destroy(gameObject);
    }

}
