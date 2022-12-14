using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MoveTowardWMod : StateBlock
{
    [SerializeField] Vector3 _offset;
    [SerializeField] float _weight = 1;
    public override void OnStart(EnemyController user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        Vector3 moveInput = (target.transform.position - (user.transform.position + (user.transform.rotation * _offset))).normalized;
        user.MoveInput += new Vector3(moveInput.x, 0, moveInput.z) * _weight;
        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target) { }
}
