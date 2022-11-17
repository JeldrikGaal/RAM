using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{
    [SerializeField] UnityEvent<Vector3> _eventToInvoke;
    [SerializeField] private int _hitLayer = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == _hitLayer)
            _eventToInvoke.Invoke(this.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _hitLayer)
            _eventToInvoke.Invoke(this.transform.position);
    }
}
