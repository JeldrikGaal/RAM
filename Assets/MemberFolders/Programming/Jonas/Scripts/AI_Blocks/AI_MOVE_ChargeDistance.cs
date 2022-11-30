using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Saves player position on start and moves in that direction for a specified distance
public class AI_MOVE_ChargeDistance : StateBlock
{
    [SerializeField] private float _weight;
    [SerializeField] private float _distance;

    [SerializeField] private bool _asTimer;

    private Dictionary<EnemyController, Vector3> _moveDir;
    private Dictionary<EnemyController, Vector3> _startPos;

    private List<float> _returnList;

    public override void OnStart(EnemyController user, GameObject target)
    {
        _returnList = new List<float>();
        _returnList.Add((float)StateReturn.Stop);

        if (_moveDir == null) _moveDir = new Dictionary<EnemyController, Vector3>();
        if (_startPos == null) _startPos = new Dictionary<EnemyController, Vector3>();

        Vector3 moveDir = (target.transform.position - user.transform.position).normalized;
        _moveDir[user] = new Vector3(moveDir.x, 0, moveDir.z);
        _startPos[user] = user.transform.position;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (Vector3.Distance(user.transform.position, _startPos[user]) > _distance) return (null, null);

        user.MoveInput += _moveDir[user] * _weight;

        if (_asTimer)
            return (null, _returnList);

        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {
        _moveDir.Remove(user);
        _startPos.Remove(user);
    }
}