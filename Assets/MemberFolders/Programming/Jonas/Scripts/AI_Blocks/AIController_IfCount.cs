using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_IfCount : MonoBehaviour
{
    private List<Jonas_TempCharacter> _packMembers;

    public void Init()
    {
        _packMembers = new List<Jonas_TempCharacter>();
    }

    public void AddMember(Jonas_TempCharacter member)
    {
        _packMembers.Add(member);
    }

    public void RemoveMember(Jonas_TempCharacter member)
    {
        _packMembers.Remove(member);

        if (_packMembers.Count == 0)
            Destroy(gameObject);
    }

    public int GetPos(Jonas_TempCharacter member)
    {
        return _packMembers.IndexOf(member);
    }

    public int GetCount()
    {
        return _packMembers.Count;
    }
}