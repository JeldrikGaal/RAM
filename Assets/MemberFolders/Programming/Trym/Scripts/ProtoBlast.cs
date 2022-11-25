using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoBlast : MonoBehaviour, IRammable
{
    private Dictionary<int,EnemyTesting> _enemiesInRange = new();
    [SerializeField] private float _timeToRun;
    [SerializeField] private float _damage;
    [Tooltip(" Range is based on the collider radius")]
    [SerializeField] private AnimationCurve _damageFalloffByDistance;
    [SerializeField] private SphereCollider _trigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("enemy"))
        {
            _enemiesInRange.Add(other.GetInstanceID(), other.gameObject.GetComponent<EnemyTesting>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.HasTag("enemy"))
        {
            int id = other.GetInstanceID();
            if (_enemiesInRange.ContainsKey(id))
            {
                _enemiesInRange.Remove(id);
            }
        }
    }



    

    public bool Hit(GameObject g)
    {
        StartCoroutine(Explode());
        return false;
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(_timeToRun);
        foreach (var item in _enemiesInRange)
        {
            var enemy = item.Value;
            enemy.TakeDamage(_damage * _damageFalloffByDistance.Evaluate(Vector3.Distance(transform.position, enemy.transform.position) / _trigger.radius), (transform.position - enemy.transform.position).normalized);
        }
        Destroy(gameObject);
    }
}
