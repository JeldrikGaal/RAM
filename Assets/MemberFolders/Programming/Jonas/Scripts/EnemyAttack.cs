using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _dmgWeight;

    private float _damage;

    private void OnEnable()
    {
        _damage = transform.parent.parent.parent.GetComponent<EnemyController>().AttackDamage * _dmgWeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{_damage} dealt to {other.name}");
        gameObject.SetActive(false);
    }


}
