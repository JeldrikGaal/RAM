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

    private void OnEnable()
    {
        _done = false;
        _damage = transform.parent.parent.parent.GetComponent<EnemyController>().Stats.GetStats(AttackName).Damage(Area);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_done) return;
        if (TagManager.HasTag(other.gameObject, "player"))
        {
            _done = true;
            other.GetComponent<RammyController>().TakeDamageRammy(_damage);
            gameObject.SetActive(false);
        }
    }
}
