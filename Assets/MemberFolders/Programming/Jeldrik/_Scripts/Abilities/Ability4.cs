using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability4 : Abilities
{
    [SerializeField] ExternalCollider _eCollider;
    [SerializeField] float _prepTime;
    [SerializeField] float _moveTime;
    [SerializeField] float _resetTime;
    private Vector3 _aulerAngleArea = new(0, 30, 0);
    [SerializeField] float _moveRadius = 0.5f;
    [SerializeField] AnimationCurve movementCurve;
    float _startTime;
    int _stage = 0;
    Vector3 _originalPosition;
    Quaternion _originalRotation;
    Rigidbody _rigid;
    private readonly List<int> _hurt = new();

    // Setup
    public override void Start()
    {
        base.Start();
        _rigid = GetComponent<Rigidbody>();

        // Adds an action to external collider for hitting the enemy.
        _eCollider.CollisionEnter += (Collision collision) =>
        {

            if (collision.gameObject.HasTag("enemy") && !_hurt.Contains(collision.gameObject.GetInstanceID()) && _stage > 0)
            {

                var enemy = collision.gameObject.GetComponent<EnemyController>();
                var damage = _upgraded ? Stats.UDmg : Stats.Dmg;

                if (enemy.TakeDamage(damage, transform.up))
                {
                    _controller.Kill(enemy.gameObject);
                }
                //FloatingDamageManager.DisplayDamage(damage, collision.transform.position);
                _hurt.Add(collision.gameObject.GetInstanceID());
                _controller.AddScreenShake(1.2f);

                // VFX:
                GetComponent<RammyVFX>().Ab4Attack(enemy.gameObject, collision.contacts[0].normal);


            }
        };
    }

    // managing stages and animating.
    override public void Update()
    {
        base.Update();

        _aulerAngleArea = new(0, _upgraded ? Stats.UAttackDegrees : Stats.AttackDegrees, 0);

        // Controlling the attack process
        switch (_stage)
        {
            // Windup
            case 1:
                {
                    var currentRot = CalculateEulerRotationOverTime(Vector3.zero, -(_aulerAngleArea / 2), _prepTime);

                    UpdatePositionAndRotation(currentRot, _moveRadius);

                    if (Time.time > _startTime + _prepTime)
                    {
                        _stage = 2;
                        _eCollider.gameObject.SetActive(true);
                        _startTime = Time.time;
                    }
                    break;
                }
            // Attacking
            case 2:
                {
                    var currentRot = CalculateEulerRotationOverTime(-(_aulerAngleArea / 2), _aulerAngleArea / 2, _moveTime);
                    UpdatePositionAndRotation(currentRot, _moveRadius);

                    ExtendedAreaOfEffect(currentRot, _upgraded ? Stats.USplashRadius : Stats.SplashRadius);

                    if (Time.time > _startTime + _moveTime)
                    {
                        _stage = 3;
                        _eCollider.gameObject.SetActive(false);
                        _startTime = Time.time;
                        _hurt.Clear();
                    }
                    break;
                }
            // Resetting
            case 3:
                {
                    var currentRot = CalculateEulerRotationOverTime(_aulerAngleArea / 2, Vector3.zero, _resetTime);

                    UpdatePositionAndRotation(currentRot, _moveRadius);

                    if (Time.time > _startTime + _resetTime)
                    {
                        _stage = 0;

                    }
                    break;
                }
            default:
                break;
        }



    }
    override public void Activate()
    {
        _audio.Play();
        if (_stage < 1)
        {
            _startTime = Time.time;
            _originalPosition = transform.position;

            Vector3 worldPosition = Vector3.zero;
            Plane plane = new Plane(Vector3.up, -20);

            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance);
            }

            transform.LookAt(worldPosition);

            transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            _originalRotation = transform.rotation;
            _stage = 1;
        }
    }

    // May be reimplemented if neccesary.
    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.HasTag("enemy") && _active )
        {
            collision.gameObject.GetComponent<EnemyTesting>().TakeDamage(_damage, transform.up);
        }*/
    }

    #region calculating and updating rotation and Positions

    // Updates the positio and rotation of rammy during the move
    private void UpdatePositionAndRotation(Vector3 eulerAngles, float radius) => _rigid.Move(PositionAroundThePoint(eulerAngles, radius), Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles));

    // Gets the position with the radius and the angle away from the start point.
    private Vector3 PositionAroundThePoint(Vector3 eulerAngles, float radius) => _originalPosition + (Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles) * (Vector3.up * radius));

    // Calculates rotation over time
    private Vector3 CalculateEulerRotationOverTime(Vector3 start, Vector3 end, float time) => Vector3.Lerp(start, end, movementCurve.Evaluate((Time.time - _startTime) / time));

    // Updates the position of the externalCollider
    private void ExtendedAreaOfEffect(Vector3 rot, float radius)
    {
        var rotat = Quaternion.Euler(_originalRotation.eulerAngles + rot);
        _eCollider.GetRigidbody().Move(PositionAroundThePoint(rot, radius - _eCollider.GetCollider().bounds.size.z / 2), rotat);
    }
    #endregion

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position + (Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,30)) * (Vector3.up*2)),.5f);


        Gizmos.DrawRay(_originalPosition, transform.up);
    }

}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Ability4 : Abilities
//{
//    public bool Upgraded = false;
//    [SerializeField] ExternalCollider _eCollider;
//    [SerializeField] float _effectRadius;
//    [SerializeField] float _effectUpgradedRadius;
//    [SerializeField] float _moveRadius;
//    [SerializeField] float _initialDamage;
//    [SerializeField] float _upgradedDamage;
//    [SerializeField] float _prepTime;
//    [SerializeField] float _moveTime;
//    [SerializeField] float _resetTime;
//    [SerializeField] Vector3 _aulerAngleArea = new(0, 30, 0);
//    [SerializeField] AnimationCurve movementCurve;
//    float _startTime;
//    int _stage = 0;
//    Vector3 _originalPosition;
//    Quaternion _originalRotation;
//    Rigidbody _rigid;
//    private readonly List<int> _hurt = new();

//    // Setup
//    public override void Start()
//    {
//        base.Start();
//        _rigid = GetComponent<Rigidbody>();

//        // Adds an action to external collider for hitting the enemy.
//        _eCollider.CollisionEnter += (Collision collision) =>
//        {

//            if (collision.gameObject.HasTag("enemy") && !_hurt.Contains(collision.gameObject.GetInstanceID()) && _stage > 0)
//            {

//                var enemy = collision.gameObject.GetComponent<EnemyController>();
//                var damage = this.Upgraded ? _upgradedDamage : _initialDamage;

//                enemy.TakeDamage(damage, transform.up);
//                //FloatingDamageManager.DisplayDamage(damage, collision.transform.position);
//                _hurt.Add(collision.gameObject.GetInstanceID());
//                _controller.AddScreenShake(1.2f);

//                // VFX:
//                GetComponent<RammyVFX>().Ab4Attack(enemy.gameObject, collision.contacts[0].normal);


//            }
//        };
//    }

//    // managing stages and animating.
//    override public void Update()
//    {
//        base.Update();

//        // Controlling the attack process
//        switch (_stage)
//        {
//            // Windup
//            case 1:
//                {
//                    var currentRot = CalculateEulerRotationOverTime(Vector3.zero, -(_aulerAngleArea / 2), _prepTime);

//                    UpdatePositionAndRotation(currentRot, _moveRadius);

//                    if (Time.time > _startTime + _prepTime)
//                    {
//                        _stage = 2;
//                        _eCollider.gameObject.SetActive(true);
//                        _startTime = Time.time;
//                    }
//                    break;
//                }
//            // Attacking
//            case 2:
//                {
//                    var currentRot = CalculateEulerRotationOverTime(-(_aulerAngleArea / 2), _aulerAngleArea / 2, _moveTime);
//                    UpdatePositionAndRotation(currentRot, _moveRadius);

//                    ExtendedAreaOfEffect(currentRot, Upgraded ? _effectUpgradedRadius : _effectRadius);

//                    if (Time.time > _startTime + _moveTime)
//                    {
//                        _stage = 3;
//                        _eCollider.gameObject.SetActive(false);
//                        _startTime = Time.time;
//                        _hurt.Clear();
//                    }
//                    break;
//                }
//            // Resetting
//            case 3:
//                {
//                    var currentRot = CalculateEulerRotationOverTime(_aulerAngleArea / 2, Vector3.zero, _resetTime);

//                    UpdatePositionAndRotation(currentRot, _moveRadius);

//                    if (Time.time > _startTime + _resetTime)
//                    {
//                        _stage = 0;

//                    }
//                    break;
//                }
//            default:
//                break;
//        }



//    }
//    override public void Activate()
//    {
//        if (_stage < 1)
//        {
//            _startTime = Time.time;
//            _originalPosition = transform.position;
//            _originalRotation = transform.rotation;
//            _stage = 1;
//        }
//    }

//    // May be reimplemented if neccesary.
//    private void OnCollisionEnter(Collision collision)
//    {
//        /*if (collision.gameObject.HasTag("enemy") && _active )
//        {
//            collision.gameObject.GetComponent<EnemyTesting>().TakeDamage(_damage, transform.up);
//        }*/
//    }

//    #region calculating and updating rotation and Positions

//    // Updates the positio and rotation of rammy during the move
//    private void UpdatePositionAndRotation(Vector3 eulerAngles, float radius) => _rigid.Move(PositionAroundThePoint(eulerAngles, radius), Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles));

//    // Gets the position with the radius and the angle away from the start point.
//    private Vector3 PositionAroundThePoint(Vector3 eulerAngles, float radius) => _originalPosition + (Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles) * (Vector3.up * radius));

//    // Calculates rotation over time
//    private Vector3 CalculateEulerRotationOverTime(Vector3 start, Vector3 end, float time) => Vector3.Lerp(start, end, movementCurve.Evaluate((Time.time - _startTime) / time));

//    // Updates the position of the externalCollider
//    private void ExtendedAreaOfEffect(Vector3 rot, float radius)
//    {
//        var rotat = Quaternion.Euler(_originalRotation.eulerAngles + rot);
//        _eCollider.GetRigidbody().Move(PositionAroundThePoint(rot, radius - _eCollider.GetCollider().bounds.size.z / 2), rotat);
//    }
//    #endregion

//    private void OnDrawGizmos()
//    {
//        //Gizmos.DrawSphere(transform.position + (Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,30)) * (Vector3.up*2)),.5f);


//        Gizmos.DrawRay(_originalPosition, transform.up);
//    }

//}
