using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{

    /// <summary>
    /// This script was made for the simple action of detecting when the feet are hitting the ground, and sending a message with the position to the steps spawner script.
    /// </summary>

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
