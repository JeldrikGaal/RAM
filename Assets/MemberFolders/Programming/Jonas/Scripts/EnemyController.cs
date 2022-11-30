using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 MoveInput;

    public float MoveSpeed;
    public float AttackDamage;
    public float Health;
    public float MaxHealth;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rb.velocity = MoveInput * MoveSpeed;

        if (MoveInput != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(MoveInput.x, 0, MoveInput.z));
        }

        // For testing
        if (Input.GetKeyDown(KeyCode.N))
        {
            Health -= 5;
        }
    }
}
