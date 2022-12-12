using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkChargeAttack : MonoBehaviour
{
    [SerializeField] bool _doItOnceChase = false;
    [SerializeField] bool _doItOnceEscape = false;
    [SerializeField] AI_State _escape;
    [SerializeField] AI_State _chase;
    [SerializeField] GameObject[] _egg = new GameObject[3];
    [SerializeField] int _ammo = 3;
    [SerializeField] float _shootSpeed = 50f;
    public float damage = 1.5f;
    [SerializeField] Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyController>().MoveInput == Vector3.zero)
        {
            _animator.SetBool("IsMoving", false);
        }
        else
        {
            _animator.SetBool("IsMoving", true);
        }

        if (_ammo <= 0)
        {
            _animator.SetBool("canShoot", false);
        }
        else
        {
            _animator.SetBool("canShoot", true);
        }

        if (GetComponent<StateMachine>()._currentState == _escape)
        {
            GetComponent<Animator>().SetBool("PickUp", false);
            _animator.SetTrigger("RunAway");
            if (_ammo <= 0)
            {
                _ammo = 3;
                foreach (var item in _egg)
                {
                    item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    item.transform.parent = transform;
                    item.transform.localPosition = new Vector3(0f, 0.742f, 0.686f);
                    item.SetActive(false);
                }
            }
            _doItOnceEscape = true;
            _doItOnceChase = false;
        }
        else if (GetComponent<StateMachine>()._currentState == _chase && _doItOnceChase == false)
        {
            
            _animator.SetTrigger("RunToPlayer");
            _doItOnceChase = true;
            _doItOnceEscape = false;
        }
    }

    public void PickUpStart()
    {
        if (_ammo > 0)
        {
            print("pickup");
            GetComponent<Animator>().SetTrigger("AttackPickUp");
            GetComponent<Animator>().SetBool("PickUp", true);
        }
        
    }

    public void HawkChargeSetTrigger()
    {
        GetComponent<Animator>().SetTrigger("Attack");
    }
    public void HawkCharge()
    {
        _ammo--;
        _egg[_ammo].SetActive(true);
        _egg[_ammo].transform.parent = null;
        _egg[_ammo].GetComponent<Rigidbody>().AddForce(transform.forward * _shootSpeed);
    }
}
