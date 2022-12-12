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
    private bool _finishedLeaping;
    [SerializeField] private bool _striking;
    private SphereCollider _damageCollider;
    private EnemyController _controller;

    [SerializeField] private bool _rammyInRange;
    [SerializeField] private RammyController _rammyController;

    [SerializeField] private AI_State  _toChaseState;
    [SerializeField] private AI_State _toStrikeState;

    private IEnumerator _strikeRoutine;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<EnemyController>();
        _damageCollider = GetComponent<SphereCollider>();
        _damageCollider.enabled = false;
        _lastState = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_striking && _rammyInRange)
        {
            transform.LookAt(_rammyController.transform, Vector3.up);
        }

        // ONLY STATE LOGIC
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


        if (_lastState != _stateMachine._currentState && _stateMachine._currentState.name == "Strike_MeleeWolf")
        {
            _damageCollider.enabled = true;
            _strikeRoutine = Strike();
            StartCoroutine(_strikeRoutine);
            
        }

        if (_lastState != _stateMachine._currentState && _stateMachine._currentState.name == "Leap_MeleeWolf")
        {
            //_animator.SetTrigger("Leap");
            //StartCoroutine(LeapAnim());
            _animator.SetTrigger("Attack2");
            Leap();
            
        }
        if (_leaping && _stateMachine._currentState.name != "Leap_MeleeWolf")
        {
            _leaping = false;
            _damageCollider.enabled = false;
            _rammyInRange = false;
            _finishedLeaping = true;

            //_animator.SetBool("Leaping", false);

        }
        if (_lastState != null)
        {
            if (_finishedLeaping)
            {
                //_stateMachine._currentState = _toStrikeState;
                _finishedLeaping = false;
            }

        }

        _lastState = _stateMachine._currentState;
    }

    public void Leap()
    {
        _leaping = true;
        _damageCollider.enabled = true;
    }

    private IEnumerator Strike()
    {
        _striking = true;
        _animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(_controller.Stats.GetStats("Axe Strike 1").AnticipationTime);
        if (_rammyInRange) _rammyController.TakeDamageRammy(_controller.Stats.GetStats("Axe Strike 1").Damage(1));
        yield return new WaitForSeconds(_controller.Stats.GetStats("Axe Strike 1").RecoveryTime);
        _animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(_controller.Stats.GetStats("Axe Strike 2").AnticipationTime);
        if (_rammyInRange) _rammyController.TakeDamageRammy(_controller.Stats.GetStats("Axe Strike 2").Damage(1));
        yield return new WaitForSeconds(_controller.Stats.GetStats("Axe Strike 2").RecoveryTime);
        _animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(_controller.Stats.GetStats("Axe Strike 3").AnticipationTime);
        if (_rammyInRange) _rammyController.TakeDamageRammy(_controller.Stats.GetStats("Axe Strike 3").Damage(1));
        yield return new WaitForSeconds(_controller.Stats.GetStats("Axe Strike 3").RecoveryTime);
        _damageCollider.enabled = false;
        _striking = false;

    }

    private IEnumerator LeapAnim()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("Leaping", true);
        yield return new WaitForSeconds(0.5f);
        _animator.SetTrigger("Leap2");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player"))
        {
            _rammyInRange = true;
            _rammyController = other.GetComponent<RammyController>();
            if (_leaping)
            {
                _rammyController.TakeDamageRammy(_controller.Stats.GetStats("Leap").Damage(1));
                _damageCollider.enabled = false;
            }
            
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.HasTag("player"))
        {
            _rammyInRange = false;
            if (_striking)
            {
                _striking = false;
                StopCoroutine(_strikeRoutine);
                _stateMachine._currentState = _toChaseState;
            }
        }
    }
}
