using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MovePack : StateEffect
{
    public float Weight;
    public float Distance;
    public float RotationTime;
    public AIController PackGroup;
    public float Deadzone;

    private Jonas_TempCharacter _c;
    private AIController_MovePack _controller;

    public override void OnStart(GameObject user, GameObject target)
    {
        _user = user;
        _target = target;

        _c = _user.GetComponent<Jonas_TempCharacter>();

        // Connect to controller, create if none
        GameObject controllerGameObject = GameObject.Find($"AIController_MovePack{PackGroup.ToString()}");
        if (controllerGameObject == null)
        {
            controllerGameObject = new GameObject($"AIController_MovePack{PackGroup.ToString()}");
            controllerGameObject.AddComponent<AIController_MovePack>();
            controllerGameObject.GetComponent<AIController_MovePack>().Init(_target, Distance, RotationTime);
        }

        _controller = controllerGameObject.GetComponent<AIController_MovePack>();
        controllerGameObject.GetComponent<AIController_MovePack>().AddMember(this);
    }

    public override void OnEnd()
    {
        _controller.RemoveMember(this);
    }

    public override State OnUpdate()
    {
        Vector3 followPos = (_controller.GetPoint(this));

        if(Vector3.Distance(followPos, transform.position) > Deadzone)
            _c.MoveInput += (followPos - _user.transform.position).normalized * Weight;
        else
            _c.MoveInput += (followPos - _user.transform.position).normalized * Weight/2;

        return null;
    }
}

public enum AIController
{
    WolfPack
}
