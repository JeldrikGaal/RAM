using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MovePack : StateEffect
{
    public float Weight;
    public AIController PackGroup;

    private Jonas_TempCharacter _c;
    private AIController_MovePack _controller;

    public override void OnStart(GameObject user, GameObject target)
    {
        _user = user;
        _target = target;

        _c = _user.GetComponent<Jonas_TempCharacter>();

        GameObject controllerGameObject = GameObject.Find($"AIController_MovePack{PackGroup.ToString()}");
        if (controllerGameObject == null)
        {
            controllerGameObject = new GameObject($"AIController_MovePack{PackGroup.ToString()}");
        }
    }

    public override void OnEnd()
    {
        // Remove self from controller, destroy controller if last
    }

    public override State OnUpdate()
    {
        // Get

        return null;
    }
}

public enum AIController
{
    WolfPack
}
