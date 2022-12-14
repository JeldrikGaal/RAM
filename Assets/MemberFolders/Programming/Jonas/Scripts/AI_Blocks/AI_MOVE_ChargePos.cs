using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Saves player position on start and moves in that direction for a specified distance
public class AI_MOVE_ChargePos : StateBlock
{
    [SerializeField] private float _weight;
    [SerializeField] private float _distance;

    [SerializeField] private bool _asTimer;

    private Dictionary<EnemyController, Vector3> _moveTarget;
    private Dictionary<EnemyController, float> _backupTimer;

    private List<float> _returnList;

    public override void OnStart(EnemyController user, GameObject target)
    {
        _returnList = new List<float>();
        _returnList.Add((float)StateReturn.Stop);

        if (_moveTarget == null) _moveTarget = new Dictionary<EnemyController, Vector3>();
        if (_backupTimer == null) _backupTimer = new Dictionary<EnemyController, float>();

        _moveTarget[user] = target.transform.position;
        _backupTimer[user] = 2f;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (Vector3.Distance(user.transform.position, _moveTarget[user]) < _distance) return (null, null);

        Debug.Log("Still going");

        user.MoveInput += (_moveTarget[user] - user.transform.position).normalized * _weight;

        _backupTimer[user] -= Time.deltaTime;
        if (_asTimer && _backupTimer[user] > 0)
            return (null, _returnList);

        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {
        _moveTarget.Remove(user);
        _backupTimer.Remove(user);
    }
}