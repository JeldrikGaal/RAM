using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In case it wasn't obvious this is just a temp script so it's possible to test other features. Don't actually use this for anything
public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 MoveInput;

    public float MoveSpeed;
    public float AttackDamage;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rb.velocity = MoveInput * MoveSpeed;

        if (MoveInput != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(MoveInput);
    }
}
