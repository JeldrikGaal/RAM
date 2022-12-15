//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Warning only for bosses
/// </summary>
[Tooltip("Warning Only For Bosses")]
public class AI_BodySlam : StateBlock
{
    [Header("Only For Single Bosses")]
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _relativeSpeedOverDistance, _relativeTrajectory;
    [SerializeField] float _range = float.PositiveInfinity;
    [SerializeField] Vector3 _ralativePosMod;
    [SerializeField] bool _manualCall;
    [SerializeField] EnemyStats _stats;
     EnemyAttackStats _attackStats1;
     EnemyAttackStats _attackStats2;
    [SerializeField] string _enemyName;
    [SerializeField] string _attackName1;
    [SerializeField] string _attackName2;

    private bool _jumped;
    private static bool _landed;
    private ExternalCollider _eCollider;
    private GameObject _target;
    private EnemyController _user;
    private RammyController _rammy;
    private float _initiaRamlHeight;
    private float _initiaEnemylHeight;
    
    public override void OnEnd(EnemyController user, GameObject target)
    {
        _eCollider.CollisionEnterEvent -= OnCollisionEnter;
        _eCollider.CollisionStayEvent -= OnCollisionStay;
        _eCollider.CollisionExitEvent -= OnCollisionExit;
    }

    public override void OnStart(EnemyController user, GameObject target)
    {

        _stats = ImportManager.GetEnemyStats(_enemyName);
        _attackStats1 = _stats.Attacks[_attackName1];
        _attackStats1 = _stats.Attacks[_attackName2];

        _initiaEnemylHeight = user.transform.position.y;
        _eCollider = user.GetComponent<ExternalCollider>();
        _user = user;
        _target = target;
        _jumped = _landed = false;
        _eCollider.CollisionEnterEvent += OnCollisionEnter;
        _eCollider.CollisionStayEvent += OnCollisionStay;
        _eCollider.CollisionExitEvent += OnCollisionExit;
        _target = target;
        
    }

    private void OnCollisionExit(Collision obj)
    {
        if (!obj.gameObject.HasTag("player"))
        {
            return;
        }
        GetRammy(obj);
        if (_jumped && !_landed)
        {
            var rigid = _rammy.GetComponent<Rigidbody>();
            rigid.velocity *= Mathf.Epsilon;
        }
    }

    private void OnCollisionStay(Collision obj)
    {
        if (!obj.gameObject.HasTag("player"))
        {
            return;
        }
        GetRammy(obj);
        if (_jumped && !_landed)
        {
            Rigidbody rigid = _rammy.GetComponent<Rigidbody>();
            rigid.AddForce((_rammy.transform.position - _user.transform.position).normalized * _speed);
            _eCollider.GetRigidbody().AddForce((_user.transform.position - _rammy.transform.position).normalized * _speed);
            if (Mathf.Abs( _rammy.transform.position.y - _initiaRamlHeight) > Mathf.Epsilon)
            {
                rigid.velocity += (Vector3.up * (_initiaRamlHeight - _rammy.transform.position.y)).normalized;
            }
            
        }
        if (_landed)
        {
            if (Mathf.Abs(_eCollider.transform.position.y - _initiaEnemylHeight) > Mathf.Epsilon)
            {
                _eCollider.GetRigidbody().velocity += (Vector3.up * (_initiaEnemylHeight - _eCollider.transform.position.y)).normalized;
            }
        }
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_manualCall)
        {
            Throw(user, target);
        }
        if (!_landed)
        {
            return (null, new(new[] { (float)StateReturn.Stop}));
        }
        return (null, null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_jumped && !_landed)
        {
            if (collision.gameObject.layer == 10)
            {
                _landed = true;
            }
            if (!collision.gameObject.HasTag("player"))
            {
                return;
            }
            GetRammy(collision);

            switch (AI_StageCheck.Check())
            {
                case 1:
                    _rammy.TakeDamageRammy(_attackStats1.Damage(1));
                    break;
                case 2:
                    _rammy.TakeDamageRammy(_attackStats1.Damage(1));
                    break;
                case 3:
                    _rammy.TakeDamageRammy(_attackStats2.Damage(1));
                    break;
                default:
                    break;
            }
        }
    }

    private void GetRammy(Collision collision)
    {
        if (_rammy == null)
        {
            _rammy = collision.gameObject.GetComponent<RammyController>();
        }
    }

    private void Throw(EnemyController user, GameObject target)
    {
        int iD = user.GetInstanceID();

        if (!_jumped)
        {
            Debug.Log(name);
            var origin = user.transform.position + (user.transform.rotation * _ralativePosMod);
            var targetPos = target.transform.position;

            Vector2 origin2D = new(origin.x, origin.z);

            Vector2 target2D = new(target.transform.position.x, target.transform.position.z);

            if (Vector2.Distance(origin2D, target2D) > _range)
            {
                Vector2 targetDir = (target2D - origin2D).normalized;
                targetPos = origin + (new Vector3(targetDir.x, 0, targetDir.y) * _range);
            }
            
            // Instantiates the bomb and starts sends it on it's journey.
            user.StartCoroutine(ManageTrajectory(user.GetComponent<Rigidbody>(), _relativeTrajectory, _speed, _relativeSpeedOverDistance, origin, targetPos));
            _jumped = true;
        }
    }

    #region Moving the enemy

    /// <summary>
    /// Takes the Instantiated bomb and animates it so it follows the relativeTrajectory from the origin to the target, at speed times relativeSpeed over relative distance.
    /// </summary>
    
    /// <param name="relativeTrajectory">trajectory relative to the distance</param>
    /// <param name="relativeSpeed"> animation curve modifying speed over relative distance </param>
    /// <param name="speed"></param>
    /// <param name="origin">Where it comes from</param>
    /// <param name="target">Where it's going</param>
    /// <returns></returns>
    private static IEnumerator ManageTrajectory(Rigidbody rigid, AnimationCurve relativeTrajectory, float speed, AnimationCurve relativeSpeed, Vector3 origin, Vector3 target)
    {
        Vector2 target2D = new Vector2(target.x, target.z);
        Vector2 origin2D = new Vector2(origin.x, origin.z);
        Vector2 currentPosition2D;
        float originHeight = origin.y;
        Vector2 dir = (target2D - origin2D).normalized;
        
        float distance = Vector2.Distance(target2D, origin2D);

        float relativePositionInSequence;


        // this while loop is basically a FixedUpdate().
        while (!_landed)
        {
            currentPosition2D = new(rigid.position.x, rigid.position.z);



            relativePositionInSequence = (Vector2.Distance(origin2D, currentPosition2D) / distance);

            if (relativePositionInSequence <= 1)
            {

                // calculates the next position and moves the bomb to it
                Vector2 newPos = currentPosition2D + dir * (relativeSpeed.Evaluate(relativePositionInSequence) * speed * Time.fixedDeltaTime);
                float newHeight = originHeight + (relativeTrajectory.Evaluate(relativePositionInSequence) * distance);
                rigid.MovePosition(new Vector3(newPos.x, newHeight, newPos.y));
            }
            else
            {
                // for preventing it from crashing trough the ground.
                rigid.isKinematic = false;
                if (rigid.velocity.magnitude > 20)
                {
                    rigid.velocity /= 1.2f;
                }

            }



            if (rigid.transform.position.y <= -10)
            {
                Destroy(rigid.gameObject);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        //rigid.velocity = Vector3.zero;
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
    }

    #endregion

}
