using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTesting : MonoBehaviour
{

    
    [SerializeField] private float _health;
    private HealthBar _healthBar;


    // Start is called before the first frame update
    void Start()
    {
        _healthBar = GetComponentInChildren<HealthBar>();
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
    public bool TakeDamage(float damage)
    {
        _health -= damage;
        _healthBar.UpdateHealthBar(- damage);
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
        Destroy(gameObject);
    }
}
