using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A script that enabeles external events for collisions.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ExternalCollider : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigid;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigid = GetComponent<Rigidbody>();
    }
    


    /// <summary>
    /// OnCollisionEnter
    /// </summary>
    public event System.Action<Collision> CollisionEnterEvent;
    /// <summary>
    /// OnCollisionStay
    /// </summary>
    public event System.Action<Collision> CollisionStayEvent;
    /// <summary>
    /// OnCollisionExit
    /// </summary>
    public event System.Action<Collision> CollisionExitEvent;
    /// <summary>
    /// OnTriggerEnter
    /// </summary>
    public event System.Action<Collider> TriggerEnterEvent;
    /// <summary>
    /// OnTriggerStay
    /// </summary>
    public event System.Action<Collider> TriggerStayEvent;
    /// <summary>
    /// OnTriggerExit
    /// </summary>
    public event System.Action<Collider> TriggerExitEvent;
    



    /// <summary>
    /// Gets the collider
    /// </summary>
    /// <returns></returns>
    public Collider GetCollider() => _collider;

    /// <summary>
    /// Gets the rigidbody of the externalCollider.
    /// </summary>
    /// <returns> null or rigidbody </returns>
    public Rigidbody GetRigidbody() => _rigid;

    // Event handelers
    private void OnCollisionEnter(Collision collision) => CollisionEnterEvent?.Invoke(collision);
    private void OnCollisionStay(Collision collision) => CollisionStayEvent?.Invoke(collision);
    private void OnCollisionExit(Collision collision) => CollisionExitEvent?.Invoke(collision);

   
    private void OnTriggerEnter(Collider other) => TriggerEnterEvent?.Invoke(other);
    private void OnTriggerStay(Collider other) => TriggerStayEvent?.Invoke(other);
    private void OnTriggerExit(Collider other) => TriggerExitEvent?.Invoke(other);
    
}
