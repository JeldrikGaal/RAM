using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWolfScript : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] StateMachine _stateMachine;

    AI_State _lastState;

    private bool _leaping;
    private SphereCollider _damageCollider;


    // Start is called before the first frame update
    void Start()
    {
        _damageCollider = GetComponent<SphereCollider>();
        _damageCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastState == _stateMachine._currentState) return;

        if (_stateMachine._currentState.name == "Circle_MeleeWolf")
        {
            _animator.SetBool("running", true);
        }
        if (_stateMachine._currentState.name == "Chase_MeleeWolf")
        {
            _animator.SetBool("running", true);
        }
        if (_stateMachine._currentState.name == "Idle_MeleeWolf")
        {
            _animator.SetBool("running", false);
        }

        if (_lastState != _stateMachine._currentState && _stateMachine._currentState.name == "Leap_MeleeWolf")
        {
            _animator.SetTrigger("Attack2");
            Leap();
        }
        if (_leaping && _stateMachine._currentState.name != "Leap_MeleeWolf")
        {
            _leaping = false;
            _damageCollider.enabled = false;
        }

        _lastState = _stateMachine._currentState;
    }

    public void Leap()
    {
        _leaping = true;
        _damageCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player"))
        {
            other.GetComponent<RammyController>().TakeDamageRammy(5);
            _damageCollider.enabled = false;
        }
    }
}
