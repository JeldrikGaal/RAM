using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyAttack : MonoBehaviour
{
    public string AttackName;

    private float _damage;

    private void OnEnable()
    {
        _damage = transform.parent.parent.parent.GetComponent<EnemyController>().Stats.GetStats(AttackName).Damage(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (TagManager.HasTag(other.gameObject, "player"))
        {
            other.GetComponent<RammyController>().TakeDamageRammy(_damage);
            gameObject.SetActive(false);
        }
        Debug.Log($"{_damage} dealt to {other.name}");
    }
}
