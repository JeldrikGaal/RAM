using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MovePack : StateBlock
{
    public float Weight;
    public float Distance;
    public float RotationTime;
    public float Deadzone;
    public AIController PackGroup;

    private AIController_MovePack _controller;

    public override void OnStart(Jonas_TempCharacter user, GameObject target)
    {
        // Connect to controller, create if none
        if (_controller == null)
        {
            GameObject controllerGameObject = new GameObject($"AIController_MovePack{PackGroup.ToString()}");
            controllerGameObject.AddComponent<AIController_MovePack>();
            controllerGameObject.GetComponent<AIController_MovePack>().Init(target, Distance, RotationTime);
            _controller = controllerGameObject.GetComponent<AIController_MovePack>();
        }

        _controller.AddMember(user);
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target)
    {
        _controller.RemoveMember(user);
    }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        Vector3 followPos = (_controller.GetPoint(user));

        if(Vector3.Distance(followPos, user.transform.position) > Deadzone)
            user.MoveInput += (followPos - user.transform.position).normalized * Weight;
        else
            user.MoveInput += (followPos - user.transform.position).normalized * Weight/2;

        return (null, null);
    }
}

public enum AIController
{
    WolfPack
}
