using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyAttack : MonoBehaviour
{
    public string AttackName;
    public int Area = 1;

    private float _damage;

    private void OnEnable()
    {
        _damage = transform.parent.parent.parent.GetComponent<EnemyController>().Stats.GetStats(AttackName).Damage(Area);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TagManager.HasTag(other.gameObject, "player"))
        {
            other.GetComponent<RammyController>().TakeDamageRammy(_damage);
            gameObject.SetActive(false);
        }
    }
}
