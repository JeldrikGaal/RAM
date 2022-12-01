using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ObjectEnable : StateBlock
{
    [SerializeField] private bool _enable;
    [SerializeField] private string _childName;

    private Dictionary<EnemyController, bool> _isDone;

    // Enables or disables child of user object with name _childName
    public override void OnStart(EnemyController user, GameObject target)
    {
        if (_isDone == null) _isDone = new Dictionary<EnemyController, bool>();

        _isDone[user] = false;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_isDone[user])
        {
            user.transform.Find("Model/"+_childName).gameObject.SetActive(_enable);
        }

        return (null, null);
    }

    public override void OnEnd(EnemyController user, GameObject target)
    {
        _isDone.Remove(user);
    }
}
