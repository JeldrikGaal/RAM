using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jonas_TempPlayer : MonoBehaviour
{
    public float MoveSpeed;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 move = new Vector3();
        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");

        _rb.velocity = move * MoveSpeed;
    }
}
