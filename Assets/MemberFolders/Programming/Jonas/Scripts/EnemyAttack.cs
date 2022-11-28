using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _lingerTime = .2f;

    private float _timer;

    private void OnEnable()
    {
        _timer = _lingerTime;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
            gameObject.SetActive(false);
    }

    public void Init(float damage)
    {
        if (damage != -1) _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{_damage} dealt to {other.name}");
        gameObject.SetActive(false);
    }


}
