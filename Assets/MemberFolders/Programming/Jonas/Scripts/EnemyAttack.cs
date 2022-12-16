using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyAttack : MonoBehaviour
{
    public string AttackName;
    public int Area = 1;

    private float _damage;
    private bool _done = false;

    public float TimerValue = .05f;
    private float _timer = 0;

    [SerializeField] private float _waitTime = .5f;
    private float _lastHit = 0;

    private void OnEnable()
    {
        _done = false;
        _damage = transform.parent.parent.parent.GetComponent<EnemyController>().Stats.GetStats(AttackName).Damage(Area);
        _timer = TimerValue;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_done || Time.time - _lastHit < _waitTime) return;
        if (TagManager.HasTag(other.gameObject, "player"))
        {
            //print("i hit");
            _done = true;
            other.GetComponent<RammyController>().TakeDamageRammy(_damage);
            gameObject.SetActive(false);
            _lastHit = Time.time;
        }
    }
}
