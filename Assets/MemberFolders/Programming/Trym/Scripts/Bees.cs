using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bees : MonoBehaviour
{
    private bool _inRange = false;
    private RammyController _rammy;
    [SerializeField] private float _damageTiming,_damage , _lifeTime;
    private float _startTime, _stungTime;
    private bool _isActive = true;
    [SerializeField] ParticleSystem _bees;
    

    private void Start()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        // dameges rammy over time if in range
        if (_inRange && Time.time - _stungTime >= _damageTiming && _isActive)
        {
            _rammy.TakeDamageRammy(_damage);
            _stungTime = Time.time;
        }

        // destroys the bees after lifetime.
        if (Time.time - _startTime >= _lifeTime)
        {
            
            _bees.Stop();
            _isActive = false;
            //print(_bees.particleCount);
            if (_bees.particleCount == 1)
            {
                Destroy(gameObject);
            }
        }
    }

    #region checks that rammy is in range
    private void OnTriggerEnter(Collider other)
    {
        // chacks taht the player is in range
        if (other.CompareTag("Player"))
        {
            // assigns rammy unless it's already assignd.
            if (_rammy == null)
            {
                _rammy = other.GetComponent<RammyController>();
            }
            
            _inRange = true;
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        // checks that the player is out of range
        if (other.CompareTag("Player"))
        {
            _inRange = false;
        }
    }
    #endregion

    /// <summary>
    /// overrides the properties of the bees
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="timing"></param>
    /// <param name="lifeTime"></param>
    public void SetProperties(float damage, float timing, float lifeTime)
    {
        _damage = damage;
        _damageTiming = timing;
        _lifeTime = lifeTime;
    }
}
