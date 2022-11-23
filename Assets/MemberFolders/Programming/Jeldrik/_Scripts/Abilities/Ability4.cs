using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability4 : Abilities
{
    public bool Upgraded = false;
    [SerializeField] ExternalCollider _eCollider;
    [SerializeField] float _effectRadius;
    [SerializeField] float _effectUpgradedRadius;
    [SerializeField] float _moveRadius;
    [SerializeField] float _initialDamage;
    [SerializeField] float _upgradedDamage;
    [SerializeField] float _moveTime;
    [SerializeField] Vector3 _aulerAngleArea = new(0,30,0);
    [SerializeField] AnimationCurve movementCurve;
    float _startTime;
    bool _active = false;
    Vector3 _originalPosition;
    Quaternion _originalRotation;
    Rigidbody _rigid;
    public override void Start()
    {
        base.Start();
        _rigid = GetComponent<Rigidbody>();

        // Adds an action to external collider
        _eCollider.CollisionEnter += (Collision collision) => { if (collision.gameObject.HasTag("enemy")) GetComponent<EnemyTesting>().TakeDamage( this.Upgraded ? _upgradedDamage : _initialDamage, transform.up); };
    }
    override public void Update()
    {
        base.Update();
        if (_active)
        {
            var currentRot = CalculateEulerRotationOverTime(-(_aulerAngleArea / 2), _aulerAngleArea / 2);

            UpdatePositionAndRotation(currentRot, _moveRadius);
            ExtendedAreaOfEffect(currentRot, Upgraded ? _effectUpgradedRadius : _effectRadius);

            if (Time.time > _startTime + _moveTime)
            {
                _active = false;
                _eCollider.gameObject.SetActive(false);
            }
        }
        
    }
    override public void Activate()
    {
        _active = true;
        _startTime = Time.time;
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _eCollider.gameObject.SetActive(true);
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
        
        
        Gizmos.DrawRay(_originalPosition, transform.up);
    }

    private void UpdatePositionAndRotation(Vector3 eulerAngles, float radius)
    {
        //_rigid.MovePosition(PositionAroundThePoint(eulerAngles, radius));
        _rigid.Move(PositionAroundThePoint(eulerAngles, radius), Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles));
        //transform.rotation = Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles);
    }

    private Vector3 PositionAroundThePoint(Vector3 eulerAngles, float radius) => _originalPosition + (Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles) * (Vector3.up * radius));

    Vector3 CalculateEulerRotationOverTime(Vector3 start, Vector3 end) => Vector3.Lerp(start, end, movementCurve.Evaluate((Time.time - _startTime) / _moveTime));
    
    // for the area of effect
    void ExtendedAreaOfEffect(Vector3 rot, float radius)
    {
        var rotat = Quaternion.Euler(_originalRotation.eulerAngles + rot);
        float boxRadius = .5f;
        _eCollider.GetRigidbody();
    }
}
