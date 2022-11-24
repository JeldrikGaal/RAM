using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A script that enabeles external events for collisions.
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
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
    public event System.Action<Collision> CollisionEnter;
    /// <summary>
    /// OnCollisionStay
    /// </summary>
    public event System.Action<Collision> CollisionStay;
    /// <summary>
    /// OnCollisionExit
    /// </summary>
    public event System.Action<Collision> CollisionExit;

    /// <summary>
    /// Gets the collider
    /// </summary>
    /// <returns></returns>
    public Collider GetCollider() => _collider;

    /// <summary>
    /// Gets the rigidbody of the externalCollider.
    /// </summary>
    /// <returns></returns>
    public Rigidbody GetRigidbody() => _rigid;

    // Event handelers
    private void OnCollisionEnter(Collision collision) => CollisionEnter?.Invoke(collision);
    private void OnCollisionStay(Collision collision) => CollisionStay?.Invoke(collision);
    private void OnCollisionExit(Collision collision) => CollisionExit?.Invoke(collision);
    

}
