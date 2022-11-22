using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability4 : Abilities
{
    public bool Upgraded = false;

    [SerializeField] float _effectRadius;
    [SerializeField] float _effectUpgradedRadius;
    [SerializeField] float _moveRadius;
    [SerializeField] float _damage;
    [SerializeField] float _moveTime;
    [SerializeField] Vector3 _aulerAngleArea = new(0,30,0);
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
        if (_active)
        {
            var currentRot = CalculateEulerRotationOverTime(-(_aulerAngleArea / 2), _aulerAngleArea / 2);

            if (Time.time > _startTime + _moveTime)
            {
                _active = false;
            }
        }
        
    }
    override public void Activate()
    {
        _active = true;
        _startTime = Time.time;
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.HasTag("enemy") && _active )
        {
            collision.gameObject.GetComponent<EnemyTesting>().TakeDamage(_damage, transform.up);
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + (Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,30)) * (Vector3.up*2)),.5f);
    }

    private void UpdatePositionAndRotation(Vector3 eulerAngles, float radius)
    {
        _rigid.MovePosition(PositionAroundThePoint(eulerAngles, radius));
        transform.rotation = Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles);
    }

    private Vector3 PositionAroundThePoint(Vector3 eulerAngles, float radius) => _originalPosition + (Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles) * (Vector3.forward * radius));

    Vector3 CalculateEulerRotationOverTime(Vector3 start, Vector3 end) => Vector3.Lerp(start, end, (Time.time - _startTime) / _moveTime);
    
    // for the area of effect
    void ExtendedAreaOfEffect(float radius, Vector3 rot)
    {
        var rotat = Quaternion.Euler(rot);
        float boxRadius = .5f;
        if (Physics.BoxCast(_originalPosition, new Vector3(boxRadius, boxRadius, boxRadius), rotat * Vector3.forward, out RaycastHit hit, rotat, radius - boxRadius))
        { if (true)
            {

            } }
    }
}
