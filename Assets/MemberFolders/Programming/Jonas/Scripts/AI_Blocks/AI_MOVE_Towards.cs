using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves the character towards the player
public class AI_MOVE_Towards : StateBlock
{
    [SerializeField] private float _weight;

    public override void OnStart(Jonas_TempCharacter user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        user.MoveInput += (target.transform.position - user.transform.position).normalized * _weight;
        return (null, null);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target) { }
}