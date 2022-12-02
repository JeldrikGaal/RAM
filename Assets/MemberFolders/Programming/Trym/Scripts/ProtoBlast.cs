using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoBlast : MonoBehaviour, IRammable
{
    private Dictionary<int,EnemyController> _enemiesInRange = new();
    [SerializeField] private float _timeToRun;
    [SerializeField] private float _damage;
    [Tooltip(" Range is based on the collider radius")]
    [SerializeField] private AnimationCurve _damageFalloffByDistance;
    [SerializeField] private SphereCollider _trigger;

    #region monitors enemies in range

    private void OnTriggerEnter(Collider other)
    {
        // registers that an enemy is in range
        if (other.gameObject.HasTag("enemy"))
        {
            _enemiesInRange.Add(other.GetInstanceID(), other.gameObject.GetComponent<EnemyController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // registers that an enemy is out of range
        if (other.gameObject.HasTag("enemy"))
        {
            int id = other.GetInstanceID();
            if (_enemiesInRange.ContainsKey(id))
            {
                _enemiesInRange.Remove(id);
            }
        }
    }

    #endregion


    // registers that rammy rammed the object.
    public bool Hit(GameObject g)
    {
        StartCoroutine(Explode());
        return false;
    }
    // explodes after _timeToRun
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(_timeToRun);

        // damages all the enemies in range
        foreach (var item in _enemiesInRange)
        {
            var enemy = item.Value;
            enemy.TakeDamage(_damage * _damageFalloffByDistance.Evaluate(Vector3.Distance(transform.position, enemy.transform.position) / _trigger.radius), (transform.position - enemy.transform.position).normalized);
        }
        Destroy(gameObject);
    }
}
