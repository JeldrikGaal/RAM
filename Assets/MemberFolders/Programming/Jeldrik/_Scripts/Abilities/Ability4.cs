using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability4 : Abilities
{
    [SerializeField] float _damage;
    [SerializeField] float _moveTime;
    float _startTime;
    bool _active = false;
    Vector3 _originalPosition;
    Quaternion _originalRotation;
    Rigidbody _rigid;
    public override void Start()
    {
        base.Start();
        _rigid = GetComponent<Rigidbody>();
    }
    override public void Update()
    {
        base.Update();

        if (_active && Time.time > _startTime + _moveTime)
        {
            _active = false;
        }
    }
    override public void Activate()
    {
        _active = true;
        _startTime = Time.time;
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.HasTag("enemy") && _active )
        {
            collision.gameObject.GetComponent<EnemyTesting>().TakeDamage(_damage, transform.up);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + (Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,30)) * (Vector3.up*2)),.5f);
    }

    private void MoveAroundThePoint(Vector3 eulerAngles, float radius)
    {

        
    }

}
