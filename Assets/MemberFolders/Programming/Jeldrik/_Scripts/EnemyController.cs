using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [HideInInspector]
    public Vector3 MoveInput;

    public float MoveSpeed;
    public float AttackDamage;
    public float MaxHealth;
    public float Health;

    [Header("Death Explosion Values")]
    [SerializeField] private GameObject[] _deathPieces;
    [SerializeField] private GameObject[] _deathPiecesOnce;
    [SerializeField] private float _deathPiecesSpreadingFactor;
    [SerializeField] private float _forceMultipier;
    [SerializeField] private float _pieceLiftime;
    [SerializeField] private int _pieceCount;

    [SerializeField] private float _defaultSpeed;

    [SerializeField] public bool _respawnAfterDeath;
    [SerializeField] public BuildSceneUtility _utilScript;

    [HideInInspector] public float StunDuration;

    [HideInInspector] public Vector3 PullPoint;
    public bool Pulled;

    // Visual Effects
    [SerializeField] private GameObject _bloodSmoke;
    [SerializeField] private float _bloodSize = 1;


    private HealthBar _healthBar;
    private PiecesManager _piecesManager;

    private Vector3 _lastIncomingHit;

    private Rigidbody _rb;
    private Animator _anim;
    private int _animMoveHash;

    private bool _doDie;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _animMoveHash = Animator.StringToHash("MoveSpeed");

        _healthBar = GetComponentInChildren<HealthBar>();
        _piecesManager = GetComponentInChildren<PiecesManager>();
        Health = MaxHealth;
        _defaultSpeed = MoveSpeed;
    }

    void Update()
    {
        _rb.velocity = MoveInput * MoveSpeed;
        if (_anim != null)
        {
            _anim.SetFloat(_animMoveHash, _rb.velocity.magnitude);
        }
        

        if (MoveInput != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(new Vector3(MoveInput.x, 0, MoveInput.z));

        if (_doDie)
        {
            Die();
        }

        if (Pulled)
        {
            Pull();
        }

    }

    public void Stun()
    {
        StartCoroutine(Stun(StunDuration));
    }

    public void Pull()
    {
        MoveToPullPoint(PullPoint);

        gameObject.layer = 23;

        if (Vector3.Distance(transform.position, PullPoint) < 0.5f)
        {
            gameObject.layer = 24;
            Pulled = false;
        }
    }

    /// <summary>
    /// Applies Damage to this enemie
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool TakeDamage(float damage, Vector3 hitDirection)
    {
        //FloatingDamageManager.DisplayDamage(_health < damage? _health:damage, transform.position + Vector3.up * .5f);
        Health -= damage;
        _healthBar.UpdateHealthBar(-(damage / MaxHealth));

        if (GetComponent<HawkBossManager>() != null)
        {
            GetComponent<HawkBossManager>().DamageTakenRecently += damage;

            if (!GetComponent<HawkBossManager>().PhaseThree)
            {
                Health = Mathf.Clamp(Health, 1, 100);
            }
        }

        _lastIncomingHit = hitDirection;
        if (_bloodSmoke != null)
        {
            var smokeBlood = Instantiate(_bloodSmoke, transform);
            smokeBlood.transform.localScale *= _bloodSize;
        }
        if (Health <= 0)
        {
            _doDie = true;
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
