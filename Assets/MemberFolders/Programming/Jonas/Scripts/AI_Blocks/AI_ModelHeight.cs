using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ModelHeight : StateBlock
{
    [SerializeField] private float _weight;
    [SerializeField] private float _height;

    [SerializeField] private bool _asTimer;

    private Dictionary<EnemyController, Transform> _models;

    private List<float> _returnList;

    public override void OnStart(EnemyController user, GameObject target)
    {
        if (_models == null) _models = new Dictionary<EnemyController, Transform>();

        _models[user] = user.transform.Find("Model");

        _returnList = new List<float>();
        _returnList.Add((float)StateReturn.Stop);
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (Mathf.Abs(_height - _models[user].position.y) < 1f) return (null, null);

        _models[user].position += new Vector3(0, Mathf.Sign(_height -_models[user].position.y) * user.MoveSpeed * _weight * Time.deltaTime, 0);

        if (_asTimer)
            return (null, _returnList);


        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {
        _models.Remove(user);
    }
}
