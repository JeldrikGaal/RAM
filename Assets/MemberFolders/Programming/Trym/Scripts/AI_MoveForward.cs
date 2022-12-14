using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MoveForward : StateBlock
{
    enum Dir {Forward,Right,Up}
    [SerializeField] float _weight = 1;
    [SerializeField] Dir _forvardDirDebug;
    public override void OnEnd(EnemyController user, GameObject target)
    {
        
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        switch (_forvardDirDebug)
        {
            case Dir.Forward:
                user.MoveInput += user.transform.forward * _weight;
                break;
            case Dir.Right:
                user.MoveInput += user.transform.right * _weight;
                break;
            case Dir.Up:
                user.MoveInput += user.transform.up * _weight;
                break;
            default:
                break;
        }
        return (null, null);
    }

    
}
