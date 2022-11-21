using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In case it wasn't obvious this is just a temp script so it's possible to test other features. Don't actually use this for anything
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
