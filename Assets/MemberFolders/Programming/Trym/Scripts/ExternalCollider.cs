using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A script that enabeles external events for collisions.
/// Warning casting Collider to ExternalCollider might be expensive.
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ExternalCollider : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigid;
    private void Start() => _collider = GetComponent<Collider>();


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
    /// warning Expensive
    /// </summary>
    /// <param name="collider"></param>
    public static implicit operator ExternalCollider(Collider collider) => collider.TryGetComponent(out ExternalCollider external) ? external : collider.gameObject.AddComponent<ExternalCollider>();


    // Event handelers
    private void OnCollisionEnter(Collision collision) => CollisionEnter?.Invoke(collision);
    private void OnCollisionStay(Collision collision) => CollisionStay?.Invoke(collision);
    private void OnCollisionExit(Collision collision) => CollisionExit?.Invoke(collision);

}
