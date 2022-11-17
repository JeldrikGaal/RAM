using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTesting : MonoBehaviour, IRammable
{

    
    [SerializeField] private float _health;

    [Header("Death Explosion Values")]
    [SerializeField] private GameObject[]  _deathPieces;
    [SerializeField] private float _deathPiecesSpreadingFactor;
    [SerializeField] private float _forceMultipier;
    [SerializeField] private float _pieceLiftime;
    [SerializeField] private int _pieceCount;
    


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
    }

    // Update is called once per frame
    void Update()
    {
        if (_health <= 0)
        {
            Die();
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
        _healthBar.UpdateHealthBar(- (damage/_startingHealth));
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
        _piecesManager.SpawnPieces(_deathPieces, transform.position, 
                                   new Vector2(_lastIncomingHit.x - _deathPiecesSpreadingFactor, _lastIncomingHit.x + _deathPiecesSpreadingFactor), 
                                   new Vector2(_lastIncomingHit.y - _deathPiecesSpreadingFactor, _lastIncomingHit.y + _deathPiecesSpreadingFactor),
                                   new Vector2(_lastIncomingHit.z - _deathPiecesSpreadingFactor, _lastIncomingHit.z + _deathPiecesSpreadingFactor), 
                                   _forceMultipier, _pieceCount, _pieceLiftime); // force multiplier, amount, lifespan
        Destroy(gameObject);
    }
}
