using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTesting : MonoBehaviour
{


    [SerializeField] private float _health;

    [Header("Death Explosion Values")]
    [SerializeField] private GameObject[] _deathPieces;
    [SerializeField] private GameObject[] _deathPiecesOnce;
    [SerializeField] private float _deathPiecesSpreadingFactor;
    [SerializeField] private float _forceMultipier;
    [SerializeField] private float _pieceLiftime;
    [SerializeField] private int _pieceCount;

    [SerializeField] private float _defaultSpeed;

    [SerializeField] private bool _respawnAfterDeath;
    [SerializeField] private BuildSceneUtility _utilScript;

    [HideInInspector] public float StunDuration;
    [HideInInspector] public bool Stunned;


    private float _startingHealth;
    private HealthBar _healthBar;
    private PiecesManager _piecesManager;

    private Vector3 _lastIncomingHit;





    // Start is called before the first frame update
    void Start()
    {
        _healthBar = GetComponentInChildren<HealthBar>();
        _piecesManager = GetComponentInChildren<PiecesManager>();
        _startingHealth = _health;

        // Records the default speed
        if(GetComponent<Jonas_TempCharacter>()) _defaultSpeed = GetComponent<Jonas_TempCharacter>().MoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_health <= 0)
        {
            Die();
        }

        if (Stunned)
        {
            // Starts the coroutine to stun the enemy
            StartCoroutine(Stun(StunDuration));

            // Sets the bool to false so the enemy only gets stunned once
            Stunned = false;
        }

    }

    /// <summary>
    /// Applies Damage to this enemie
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool TakeDamage(float damage, Vector3 hitDirection)
    {
        _health -= damage;
        _healthBar.UpdateHealthBar(-(damage / _startingHealth));
        _lastIncomingHit = hitDirection;
        if (_health <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles game logic of enemy dying
    /// </summary>
    private void Die()
    {
        _piecesManager.SpawnPieces(_deathPieces, _deathPiecesOnce,transform.position,
                                   new Vector2(_lastIncomingHit.x - _deathPiecesSpreadingFactor, _lastIncomingHit.x + _deathPiecesSpreadingFactor),
                                   new Vector2(_lastIncomingHit.y - _deathPiecesSpreadingFactor, _lastIncomingHit.y + _deathPiecesSpreadingFactor),
                                   new Vector2(_lastIncomingHit.z - _deathPiecesSpreadingFactor, _lastIncomingHit.z + _deathPiecesSpreadingFactor),
                                   _forceMultipier, _pieceCount, _pieceLiftime); // force multiplier, amount, lifespan
        if (!_respawnAfterDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            _utilScript.Respawn(gameObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
    }

    public IEnumerator Stun(float duration)
    {
        // Sets the movespeed to 0 to fake the enemy being stunned
        GetComponent<Jonas_TempCharacter>().MoveSpeed = 0;

        // Wait for stun duration
        yield return new WaitForSeconds(duration);

        // Sets the movespeed to the default speed again
        GetComponent<Jonas_TempCharacter>().MoveSpeed = _defaultSpeed;
    }
}
