using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability1 : Abilities
{
    [SerializeField] private bool _jumping;

    private LayerMask enemyMask;

    [SerializeField] private float _stunDuration;

    private AnimationCurve _yPosCurve;

    [SerializeField] private GameObject _damageArea;
    [SerializeField] private GameObject _upgradedArea;

    [HideInInspector] public RammyController PlayerController;
    public RammyVFX VFXScript;

    private Vector3 _startPos;
    private Vector3 _landingPos;

    private float _jumpTimer;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpDuration;

    private Rigidbody _rb;

    [SerializeField] private SpawnRocks _rockSpawner;

    public override void Start()
    {
        base.Start();
        enemyMask |= 1 << LayerMask.NameToLayer("Enemy");
        _rb = GetComponent<Rigidbody>();
    }
    override public void Update()
    {
        base.Update();

        // If the ability is activated
        if (_jumping)
        {
            _jumpTimer += Time.deltaTime;
            //transform.position = new Vector3(transform.position.x, _startPos.y + _yPosCurve.Evaluate(_jumpTimer), transform.position.z);
            _controller.BlockPlayerMovment();

            // If the timer has passed the last keyframe in the animation
            if (_jumpTimer > _yPosCurve.keys[_yPosCurve.keys.Length - 1].time)
            {
                if (_rockSpawner)
                {
                    _audio.Play();
                    _rockSpawner.InitiateRocks();
                }

                _controller.AddScreenShake(1.2f);
                _landingPos = transform.position;
                _jumping = false;
                _controller.UnBlockPlayerMovement();

                Collider[] enemies = Physics.OverlapSphere(transform.position, _upgraded ? Stats.USplashRadius : Stats.SplashRadius, enemyMask);

                List<Transform> enemyHolder = new List<Transform>();

                foreach (Collider col in enemies)
                {
                    if (!enemyHolder.Contains(col.transform))
                    {
                        enemyHolder.Add(col.transform);
                    }
                }

                foreach (Transform c in enemyHolder)
                {
                    if (c.gameObject.GetComponent<EnemyController>().Health > 0)
                    {// If the damage kills the enemy, do cool stuff
                        if (c.gameObject.GetComponent<EnemyController>().TakeDamage((_upgraded ? Stats.UDmg : Stats.Dmg) * _controller.Damage, transform.up))
                            PlayerController.Kill(c.gameObject);
                    }

                    // Stuns enemy
                    c.GetComponent<EnemyController>().StunDuration = _stunDuration;
                    c.GetComponent<EnemyController>().Stun();

                    VFXScript.Ab1Attack(c.gameObject);
                }
            }
        }
    }

    override public void Activate()
    {
        _jumping = true;
        _jumpTimer = 0f;
        _startPos = transform.position;

        _controller.SetAnimationTrigger("Ability1");

        #region Animation
        Keyframe[] keyframes = new Keyframe[3];
        keyframes[0] = new Keyframe(0, 0);
        keyframes[1] = new Keyframe((_upgraded ? Stats.UAttackTime : Stats.AttackTime) / 2, _jumpHeight);
        keyframes[2] = new Keyframe(_upgraded ? Stats.UAttackTime : Stats.AttackTime, 0);
        _yPosCurve = new AnimationCurve(keyframes);
        #endregion
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Ability1 : Abilities
//{
//    [SerializeField] private RammyAttack _stats;

//    [SerializeField] private bool _jumping;

//    /*[HideInInspector]*/
//    public bool Upgraded;

//    private AnimationCurve _yPosCurve;

//    [SerializeField] private GameObject _damageArea;
//    [SerializeField] private GameObject _upgradedArea;

//    private Vector3 _startPos;
//    private Vector3 _landingPos;

//    private float _jumpTimer;
//    [SerializeField] private float _defaultStunDuration;
//    [SerializeField] private float _upgradetStunDuration;
//    [SerializeField] private float _defaultDamage;
//    [SerializeField] private float _upgradedDamage;
//    [SerializeField] private float _jumpHeight;
//    [SerializeField] private float _jumpDuration;

//    private Rigidbody _rb;


//    public override void Start()
//    {
//        base.Start();
//        _rb = GetComponent<Rigidbody>();
//    }
//    override public void Update()
//    {
//        base.Update();

//        // If the ability is activated
//        if (_jumping)
//        {
//            // Increased the jump timer
//            _jumpTimer += Time.deltaTime;

//            // Sets the y position based on the animation curve
//            transform.position = new Vector3(transform.position.x, _startPos.y + _yPosCurve.Evaluate(_jumpTimer), transform.position.z);

//            // Blocks the character from receiving movement inputs
//            _controller.BlockPlayerMovment();

//            // If the timer has passed the last keyframe in the animation
//            if (_jumpTimer > _yPosCurve.keys[_yPosCurve.keys.Length - 1].time)
//            {
//                // Add Screen Shake
//                _controller.AddScreenShake(1.2f);

//                // Records the landing position
//                _landingPos = transform.position;

//                // Sets jumping to false
//                _jumping = false;

//                // Unblocks the movement inputs
//                _controller.UnBlockPlayerMovement();

//                // Spawns the damage area at the players landing location
//                var jumpArea = Instantiate(_damageArea, new Vector3(_landingPos.x, _landingPos.y - 0.5f, _landingPos.z), Quaternion.identity);

//                // Sets the stun duration
//                jumpArea.GetComponent<JumpAttackArea>().StunDuration = _defaultStunDuration;

//                // Sets the damage
//                jumpArea.GetComponent<JumpAttackArea>().Damage = _defaultDamage * _controller.AppliedDamageModifier;

//                // Sets the VFX script
//                jumpArea.GetComponent<JumpAttackArea>().VFXScript = GetComponent<RammyVFX>();

//                // Sets the controller script
//                jumpArea.GetComponent<JumpAttackArea>().PlayerController = _controller;

//                // Destroy the damage area after 0.5 seconds
//                Destroy(jumpArea, 0.5f);

//                // Checks if the ability is upgraded or not
//                if (Upgraded)
//                {
//                    // Starts the coroutine to spawn the upgraded damage area
//                    StartCoroutine(SpawnUpgradedArea());
//                }
//            }
//        }
//    }

//    override public void Activate()
//    {
//        // Activates the ability
//        _jumping = true;

//        // Resets the jumping timer
//        _jumpTimer = 0f;

//        // Saves the startpos of the jump
//        _startPos = transform.position;

//        // Creates a local keyframe array with 3 values
//        Keyframe[] keyframes = new Keyframe[3];

//        // Sets the first keyfram at 0 seconds and 0 value
//        keyframes[0] = new Keyframe(0, 0);

//        // Sets the second keyframe to be at half the duration of the jump and at max height
//        keyframes[1] = new Keyframe(_jumpDuration / 2, _jumpHeight);

//        // Sets the last keyframe at the full duration of the jump and the value to 0
//        keyframes[2] = new Keyframe(_jumpDuration, 0);

//        // Sets the keyframe values to the animation curve
//        _yPosCurve = new AnimationCurve(keyframes);

//        // Debug.Log(_controller.transform.position);
//        // Debug.Log("Ability 1");
//    }

//    private IEnumerator SpawnUpgradedArea()
//    {
//        // Waits for 0.5 seconds
//        yield return new WaitForSeconds(0.25f);

//        // Spawns the upgraded damage area at the players feet
//        var upgradedArea = Instantiate(_upgradedArea, new Vector3(_landingPos.x, _landingPos.y - 0.5f, _landingPos.z), Quaternion.identity);

//        // Sets the stun duration
//        upgradedArea.GetComponent<JumpAttackArea>().StunDuration = _upgradetStunDuration;

//        // Sets the damage
//        upgradedArea.GetComponent<JumpAttackArea>().Damage = _upgradedDamage * _controller.AppliedDamageModifier;

//        // Sets the VFX script
//        upgradedArea.GetComponent<JumpAttackArea>().VFXScript = GetComponent<RammyVFX>();

//        // Sets the controller script
//        upgradedArea.GetComponent<JumpAttackArea>().PlayerController = _controller;

//        // Destroys the area after 0.5 seconds
//        Destroy(upgradedArea, 0.5f);
//    }
//}
