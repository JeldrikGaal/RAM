using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_IfCount : MonoBehaviour
{
    private List<EnemyController> _packMembers;

    public void Init()
    {
        _packMembers = new List<EnemyController>();
    }

    public void AddMember(EnemyController member)
    {
        _packMembers.Add(member);
    }

    public void RemoveMember(EnemyController member)
    {
        _packMembers.Remove(member);

        if (_packMembers.Count == 0)
            Destroy(gameObject);
    }

    public int GetPos(EnemyController member)
    {
        return _packMembers.IndexOf(member);
    }

    public int GetCount()
    {
        return _packMembers.Count;
    }
}