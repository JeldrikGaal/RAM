using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BearBombLouncher))]
public class AI_ThrowBomb : StateBlock
{
    [SerializeField] GenericBearBomb _bomb;
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _relativeSpeedOverDistance, _relativeTrajectory;

    private bool _launched = false;
    private BearBombLouncher _bombLouncher;
    public override void OnEnd(EnemyController user, GameObject target)
    {
        
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        _launched = false;
        _bombLouncher = user.GetComponent<BearBombLouncher>();
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_launched)
        {
            _bombLouncher.Lounch(_bomb,_relativeTrajectory,_relativeSpeedOverDistance,_speed,user.transform.position,target.transform.position);
            _launched = true;
        }

        return (null, null);
    }

    
}
