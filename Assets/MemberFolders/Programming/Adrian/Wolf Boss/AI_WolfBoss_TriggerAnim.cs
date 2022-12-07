using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_WolfBoss_TriggerAnim : StateBlock
{
    public override void OnStart(EnemyController user, GameObject target)
    {
        user.GetComponent<WolfBossRangedAttack>().LeapAttackAnimEvent(target);
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {

        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {

    }

}
