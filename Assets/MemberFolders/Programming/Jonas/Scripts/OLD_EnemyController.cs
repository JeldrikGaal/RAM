using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class OLD_EnemyController : MonoBehaviour
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

        Health = MaxHealth;
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

    public void TakeDamage(int dmg)
    {
        Health -= dmg;

        if(Health <= 0)
        {
            Destroy(gameObject);
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
        Debug.Log("Set Trigger");
        _anim.SetTrigger(name);
    }

    public bool AnimGetState(string name)
    {
        return _anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    #endregion
}
