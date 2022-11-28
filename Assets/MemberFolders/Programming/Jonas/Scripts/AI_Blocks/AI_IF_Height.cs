using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks height distance between character and player
// If condition is not met skips next block
public class AI_IF_Height : StateBlock
{
    [SerializeField] private int _skipCount = 1;
    [SerializeField] private float _height;
    [SerializeField] private bool _over;

    public override void OnStart(EnemyController user, GameObject target) { }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (user.transform.position.y > _height == _over)
            return (null, null);

        List<float> temp = new List<float>();
        temp.Add((float)StateReturn.Skip);
        temp.Add(_skipCount);
        return (null, temp);

    }

    public override void OnEnd(EnemyController user, GameObject target) { }
}
