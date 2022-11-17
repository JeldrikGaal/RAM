using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_MovePack : MonoBehaviour
{
    private GameObject _target;
    private float _distance;
    private float _rotationTime;

    private Vector3 _point;

    private AIController _packGroup;
    private List<Jonas_TempCharacter> _packMembers;

    public void Init(GameObject target, float distance, float rotationTime)
    {
        _target = target;
        _packMembers = new List<Jonas_TempCharacter>();
        _distance = distance;
        _rotationTime = rotationTime;

        _point = new Vector3(_distance, 0, 0);
    }

    private void Update()
    {
        _point = RotateAround(_point, Vector3.zero, new Vector3(0, 360 / _rotationTime * Time.deltaTime, 0));
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

    public Vector3 GetPoint(Jonas_TempCharacter member)
    {
        int memberPos = _packMembers.IndexOf(member);

        float rotAngle = memberPos * (360 / _packMembers.Count);
        Vector3 point = RotateAround(_point, Vector3.zero, new Vector3(0, rotAngle, 0));

        return _target.transform.position + point;
    }

    private Vector3 RotateAround(Vector3 point, Vector3 pivot, Vector3 angle)
    {
        return Quaternion.Euler(angle) * (point - pivot) + pivot;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_target.transform.position + _point, .1f);
    }
}