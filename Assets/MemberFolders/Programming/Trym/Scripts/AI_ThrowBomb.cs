using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class AI_ThrowBomb : StateBlock, IStateBlockGizmo
{
    [SerializeField] bool _random = false;
    [SerializeField] GenericBearBomb _bomb;
    [ShowIf(nameof(_random))]
    [SerializeField] GenericBearBomb _bomb2;
    [ShowIf(nameof(_random))]
    [SerializeField] float _bomb2Percent;
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _relativeSpeedOverDistance, _relativeTrajectory;
    [SerializeField] float _range = float.PositiveInfinity;
    [SerializeField] Vector3 _ralativePosMod;
    [SerializeField] float _inacuracy;
    [SerializeField] bool _manualCall;
    [SerializeField] float _fuse;
    [ShowIf(nameof(_random))]
    [SerializeField] float _fuse2;



    private readonly Dictionary<int, bool> _launched = new();
    private GameObject _target;

    public Vector3 GizPos => _ralativePosMod;

    public float GizRad => .45f;

    public override void OnEnd(EnemyController user, GameObject target)
    {
        
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        user.DoOnDie(this, OnDie);

        // stores the boolean information for the specifig gameObject the function got called from.
        int iD = user.GetInstanceID();
        if (_launched.ContainsKey(iD))
        {
            _launched[iD] = false;
        }
        else
        {
            _launched.Add(iD, false);
        }

        _target = target;
    }

    private void OnDie(EnemyController obj)
    {
        _launched.Remove(obj.GetInstanceID());
    }

    public void ManualThrow(EnemyController user)
    {
        if (_manualCall)
        {
            Throw(user, _target);
        }
        
    }


    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_manualCall)
        {
            Throw(user, target);
        }
        
        return (null, null);
    }
    
    private void Throw(EnemyController user, GameObject target)
    {
        int iD = user.GetInstanceID();

        if (!_launched[iD])
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            Vector3 randomAssembled = new Vector3(randomX,0, randomY).normalized * Random.Range(0.5f, _inacuracy);


            Debug.Log(name);
            var origin = user.transform.position + (user.transform.rotation * _ralativePosMod);
            var targetPos = target.transform.position;
            if (_inacuracy >0)
            {
                targetPos = target.transform.position + randomAssembled;
            }
            

            Vector2 origin2D = new(origin.x, origin.z);

            Vector2 target2D = new(target.transform.position.x, target.transform.position.z);

            if (Vector2.Distance(origin2D, target2D) > _range)
            {
                Vector2 targetDir = (target2D - origin2D).normalized;
                targetPos = origin + (new Vector3(targetDir.x, 0, targetDir.y) * _range);
            }
            _bomb.SetProperties(_fuse);
            if (_random)
            {
                _bomb2.SetProperties(_fuse2);
            }
            
            
            // Instantiates the bomb and starts sends it on it's journey.
            GameManager.HandleCoroutine(ManageTrajectory(Instantiate(_random?Random.Range(1,101)>=_bomb2Percent?_bomb:_bomb2  : _bomb, origin, user.transform.rotation, null), _relativeTrajectory, _speed, _relativeSpeedOverDistance, origin, targetPos));
            _launched[iD] = true;
        }
    }

    #region Moving tha bomb

    /// <summary>
    /// Takes the Instantiated bomb and animates it so it follows the relativeTrajectory from the origin to the target, at speed times relativeSpeed over relative distance.
    /// </summary>
    /// <param name="bomb"></param>
    /// <param name="relativeTrajectory">trajectory relative to the distance</param>
    /// <param name="relativeSpeed"> animation curve modifying speed over relative distance </param>
    /// <param name="speed"></param>
    /// <param name="origin">Where it comes from</param>
    /// <param name="target">Where it's going</param>
    /// <returns></returns>
    private static IEnumerator ManageTrajectory(GenericBearBomb bomb, AnimationCurve relativeTrajectory, float speed, AnimationCurve relativeSpeed, Vector3 origin, Vector3 target)
    {
        Vector2 target2D = new Vector2(target.x, target.z);
        Vector2 origin2D = new Vector2(origin.x, origin.z);
        Vector2 currentPosition2D;
        float originHeight = origin.y;
        Vector2 dir = (target2D - origin2D).normalized;
        var rigid = bomb.Rb;
        float distance = Vector2.Distance(target2D, origin2D);
       
        float relativePositionInSequence;


        // this while loop is basically a FixedUpdate().
        while (!bomb.HitCheck)
        {
            currentPosition2D = new(bomb.Rb.position.x, bomb.Rb.position.z);



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



            if (bomb.transform.position.y <= -10)
            {
                Destroy(bomb.gameObject);
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
