using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves the character towards the player
public class AI_MOVE_Towards : StateBlock
{
    [SerializeField] private float _weight;

    public override void OnStart(EnemyController user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        Vector3 moveInput = (target.transform.position - user.transform.position).normalized;
        user.MoveInput += new Vector3(moveInput.x, 0, moveInput.z) * _weight;
        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target) { }
}
