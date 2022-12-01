using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 MoveInput;

    public float MoveSpeed;
    public float AttackDamage;
    public float Health;
    public float MaxHealth;

    private Rigidbody _rb;
    private Animator _anim;
    private int _animMoveHash;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _animMoveHash = Animator.StringToHash("MoveSpeed");
    }

    private void Update()
    {
        _rb.velocity = MoveInput * MoveSpeed;
        _anim.SetFloat(_animMoveHash, _rb.velocity.magnitude);

        if (MoveInput != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(new Vector3(MoveInput.x, 0, MoveInput.z));

        // For testing
        if (Input.GetKeyDown(KeyCode.N))
        {
            Health -= 5;
        }
    }

    #region Animation

    public void AnimSetFloat(string name, float value)
    {
        _anim.SetFloat(name, value);
    }

    public void AnimSetBool(string name, bool value)
    {
        _anim.SetBool(name, value);
    }

    public void AnimSetTrigger(string name)
    {
        _anim.SetTrigger(name);
    }

    #endregion
}
