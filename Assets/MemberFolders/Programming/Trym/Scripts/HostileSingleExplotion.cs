using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NorrunLib;
[RequireComponent(typeof(SphereCollider))]
public class HostileSingleExplotion : MonoBehaviour
{
    [SerializeField] float _damage;
    [SerializeField] AnimationCurve _damageFalloff;
    private SphereCollider _collider;
    private bool _hit = false;
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        StartCoroutine(SelfDetonateOnMiss());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _hit = true;
            // trigger effect here
            var rammy = other.GetComponent<RammyController>();
            float range =  Vector3.Distance(other.ClosestPoint(other.transform.position), transform.position) / _collider.GetLossyRadius();
            rammy.TakeDamageRammy(_damage * _damageFalloff.Evaluate(range));
            Destroy(gameObject);
        }
    }
    // Enshures that it is destroied after missing rammy.
    private IEnumerator SelfDetonateOnMiss()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        if (!_hit)
        {
            Destroy(gameObject);
        }



    }

}


