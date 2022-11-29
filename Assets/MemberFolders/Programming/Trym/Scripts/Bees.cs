using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bees : MonoBehaviour
{
    private bool _inRange = false;
    private RammyController _rammy;
    [SerializeField] private float _damageTiming,_damage , _lifeTime;
    private float _startTime, _stungTime;


    private void Start()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        if (_inRange && Time.time - _stungTime >= _damageTiming)
        {
            _rammy.TakeDamageRammy(_damage);
            _stungTime = Time.time;
        }

        if (Time.time - _startTime >= _lifeTime)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (_rammy == null)
            {
                _rammy = other.GetComponent<RammyController>();
            }
            
            _inRange = true;
        }
    }

    public void SetProperties(float damage,float timing, float lifeTime)
    {
        _damage = damage;
        _damageTiming = timing;
        _lifeTime = lifeTime;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
        }
    }

   
}
