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
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // trigger effect here
            var rammy = other.GetComponent<RammyController>();
            float range =  Vector3.Distance(other.ClosestPoint(other.transform.position), transform.position) / _collider.GetLossyRadius();
            rammy.TakeDamageRammy(_damage * _damageFalloff.Evaluate(range));
            Destroy(gameObject);
        }
    }
}


