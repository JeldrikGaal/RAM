using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 MoveInput;

    public string EnemyName;
    public EnemyStats Stats;

    public float MoveSpeed;
    public float AttackDamage;
    public float Health;

    public bool AlwaysFace = false;

    public int _area = 1;

    [Header("Death Explosion Values")]
    [SerializeField] private GameObject[] _deathPieces;
    [SerializeField] private GameObject[] _deathPiecesOnce;
    [SerializeField] private float _deathPiecesSpreadingFactor;
    [SerializeField] private float _forceMultipier;
    [SerializeField] private float _pieceLiftime;
    [SerializeField] private int _pieceCount;

    [SerializeField] GameObject _player;

    [SerializeField] private float _defaultSpeed;

    [SerializeField] public bool _respawnAfterDeath;
    [SerializeField] public BuildSceneUtility _utilScript;

    [HideInInspector] public float StunDuration;

    [HideInInspector] public Vector3 PullPoint;
    public bool Pulled;

    private float _pulledTimer;

    // Visual Effects
    [SerializeField] private GameObject _bloodSmoke;
    [SerializeField] private float _bloodSize = 1;
    [SerializeField] private GameObject _vfxParticle;

    // Sound Effects
    [SerializeField] private AudioAddIn _hurtSound, _deathSound;

    private HealthBar _healthBar;
    private PiecesManager _piecesManager;

    private Vector3 _lastIncomingHit;

    private Rigidbody _rb;
    private Animator _anim;
    private int _animMoveHash;

    public bool DoDie { get; private set; }
    public bool IsBoss = false;
    private bool _doMove;

    private bool _invincible = false;

    Cleanup cleanup = new();

    public void DoOnDie(StateBlock block, System.Action<EnemyController> cleaner) => cleanup.DoClean(block, cleaner);

    void Start()
    {
        Stats = ImportManager.GetEnemyStats(EnemyName);

        _rb = GetComponent<Rigidbody>();
        // Temporary, hopefully

        _anim = GetComponentInChildren<Animator>();
        _animMoveHash = Animator.StringToHash("MoveSpeed");
        _player = FindObjectOfType<RammyController>().gameObject;

        _healthBar = GetComponentInChildren<HealthBar>();
        _piecesManager = GetComponentInChildren<PiecesManager>();

        _defaultSpeed = MoveSpeed;

        if (IsBoss)
        {
            _area = 4;
        }
        else if (SceneManager.GetActiveScene().buildIndex < 4)
        {
            _area = 1;
        }
        else if (SceneManager.GetActiveScene().buildIndex < 7)
        {
            _area = 2;
        }
        else
        {
            _area = 3;
        }

        if (GetComponent<HawkBossManager>() != null)
        {
            Health = GetComponent<HawkBossManager>().MaxHealth;
        }
        else
        {
            Health = Stats.GetHealth(_area);
        }


    }

    public void SetInviciblity(bool newValue)
    {
        _invincible = newValue;
    }

    void Update()
    {
        Vector3 moveVelocity = MoveInput * MoveSpeed;
        moveVelocity.y = _rb.velocity.y;
        _rb.velocity = moveVelocity;

        if (_anim != null)
        {
            _anim.SetFloat(_animMoveHash, _rb.velocity.magnitude);
        }


        if (MoveInput != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(new Vector3(MoveInput.x, 0, MoveInput.z));

        if (AlwaysFace)
            transform.LookAt(_player.transform);

        if (Pulled)
        {
            _pulledTimer += Time.deltaTime;
            Pull();
        }

    }

    private void LateUpdate()
    {
        if (DoDie)
        {
            Die();
        }
    }

    public void Stun()
    {
        StartCoroutine(Stun(StunDuration));
    }

    public void Pull()
    {
        MoveToPullPoint(PullPoint);

        // gameObject.layer = 23;

        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;

        if (_pulledTimer > 1f)
        {
            // gameObject.layer = 24;

            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;


            Pulled = false;
            _pulledTimer = 0;
        }
    }

    /// <summary>
    /// Applies Damage to this enemy
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool TakeDamage(float damage, Vector3 hitDirection)
    {
        if (_invincible)
        {
            return true;
        }
        Debug.Log(damage);

        //FloatingDamageManager.DisplayDamage(_health < damage? _health:damage, transform.position + Vector3.up * .5f);
        Health -= damage;
        _anim.SetTrigger("TakeDamage");
        transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
        _healthBar.UpdateHealthBar(-(damage / Stats.GetHealth(_area)));

        _hurtSound.Play();

        if (GetComponent<HawkBossManager>() != null)
        {
            GetComponent<HawkBossManager>().DamageTakenRecently += damage;
            GetComponent<HawkBossManager>().DamageTakenRecentlyStageChange += damage;
        }

        _lastIncomingHit = hitDirection;

        if (_vfxParticle)
        {
            var _hitParticle = Instantiate(_vfxParticle, transform.position, Quaternion.Euler(0, 0, 0));
            _hitParticle.transform.parent = null;
        }
        if (_bloodSmoke != null)
        {
            var smokeBlood = Instantiate(_bloodSmoke, transform);
            smokeBlood.transform.localScale *= _bloodSize;
        }
        if (Health <= 0)
        {
            DoDie = true;
            GetComponent<StateMachine>().EndStates();
            return true;
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<RammyController>().HasStunBuff)
        {
            Stun(GameObject.FindGameObjectWithTag("Player").GetComponent<RammyController>().StunBuffModifier);
        }

        return false;
    }

    /// <summary>
    /// Handles game logic of enemy dying
    /// </summary>
    private void Die()
    {
        /*
        _piecesManager.SpawnPieces(_deathPieces, _deathPiecesOnce, transform.position,
                                   new Vector2(_lastIncomingHit.x - _deathPiecesSpreadingFactor, _lastIncomingHit.x + _deathPiecesSpreadingFactor),
                                   new Vector2(_lastIncomingHit.y - _deathPiecesSpreadingFactor, _lastIncomingHit.y + _deathPiecesSpreadingFactor),
                                   new Vector2(_lastIncomingHit.z - _deathPiecesSpreadingFactor, _lastIncomingHit.z + _deathPiecesSpreadingFactor),
                                   _forceMultipier, _pieceCount, _pieceLiftime); // force multiplier, amount, lifespan
        */

        cleanup.Clean(this);
        _deathSound.Play();
        if (!_respawnAfterDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            //_utilScript.Respawn(gameObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }

    public IEnumerator Stun(float duration)
    {
        // Sets the movespeed to 0 to fake the enemy being stunned
        MoveSpeed = 0;
        // Wait for stun duration
        yield return new WaitForSeconds(duration);

        // Sets the movespeed to the default speed again
        MoveSpeed = _defaultSpeed;
    }

    public void MoveToPullPoint(Vector3 point)
    {
        transform.position = Vector3.Lerp(transform.position, point, 2 * Time.deltaTime);
    }

    #region Animation

    public void AnimSetFloat(string name, float value)
    {
        _anim.SetFloat(name, value);
    }
    public void AnimSetInteger(string name, int value)
    {
        _anim.SetInteger(name, value);
    }

    public void AnimSetBool(string name, bool value)
    {
        _anim.SetBool(name, value);
    }

    public void AnimSetTrigger(string name)
    {
        _anim.SetTrigger(name);
    }

    public bool AnimGetState(string name)
    {
        return _anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    #endregion
}
