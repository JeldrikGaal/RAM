using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ThrowBomb : StateBlock
{
    [SerializeField] GenericBearBomb _bomb;
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _relativeSpeedOverDistance, _relativeTrajectory;

    private bool _ran = false;
    private BearBombLouncher bombLouncher;
    public override void OnEnd(EnemyController user, GameObject target)
    {
        
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        _ran = false;
        bombLouncher = user.GetComponent<BearBombLouncher>();
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_ran)
        {
            bombLouncher.Lounch(_bomb,_relativeTrajectory,_relativeSpeedOverDistance,_speed,user.transform.position,target.transform.position);
            _ran = true;
        }

        return (null, null);
    }

    
}
