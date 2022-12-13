using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SlowHoney : MonoBehaviour
{
    [SerializeField] private float _speedMod;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _lingerTime;
    [SerializeField] private float _hight;
    [SerializeField] Animator _animator;
    private bool _entered = false;
    private RammyController _rammy;
    private float _startTime;

    private bool _init = false;

    private void Start()
    {
        _animator.enabled = true;
        _startTime = Time.time;
       
    }


    private void Update()
    {
        if (!_init)
        {
            //var tempPos = transform.position;
            //transform.position = new(tempPos.x, _hight, tempPos.z);
            _init = true;
        }
        if (Time.time - _startTime >= _lingerTime)
        {
            _animator.SetTrigger("DISAPPEAR");

            if (_animator.IsInTransition(0))
            {
                if (_entered)
                {
                    _rammy.MovementSpeed /= _speedMod;
                }
                Destroy(gameObject);
            }
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
            if (_rammy.MovementSpeed * _speedMod > _minSpeed)
            {
                
                _rammy.MovementSpeed *= _speedMod;
                _entered = true; 
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _rammy.MovementSpeed /= _speedMod;
            _entered = false;
        }
    }

    

}
