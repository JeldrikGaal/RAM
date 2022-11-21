using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Runs movement of a circling pack of the same characters
public class AI_MOVE_Pack : StateBlock
{
    [SerializeField] private float _weight;
    [SerializeField] private float _distance;
    [SerializeField] private float _rotationTime;
    [SerializeField] private float _deadzone;
    [SerializeField] private AIController _packGroup;

    private AIController_MovePack _controller;

    // Creates a controller if there is none
    // Adds the current character to the controller
    public override void OnStart(Jonas_TempCharacter user, GameObject target)
    {
        // Connect to controller, create if none
        if (_controller == null)
        {
            GameObject controllerGameObject = new GameObject($"AIController_MovePack{_packGroup.ToString()}");
            controllerGameObject.AddComponent<AIController_MovePack>();
            controllerGameObject.GetComponent<AIController_MovePack>().Init(target, _distance, _rotationTime);
            _controller = controllerGameObject.GetComponent<AIController_MovePack>();
        }

        _controller.AddMember(user);
    }

    // Removes itself from the controller
    public override void OnEnd(Jonas_TempCharacter user, GameObject target)
    {
        _controller.RemoveMember(user);
    }

    // Gets its target position from the controller and moves towards it
    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        Vector3 followPos = (_controller.GetPoint(user));

        if(Vector3.Distance(followPos, user.transform.position) > _deadzone)
            user.MoveInput += (followPos - user.transform.position).normalized * _weight;
        else
            user.MoveInput += (followPos - user.transform.position).normalized * _weight/2;

        return (null, null);
    }
}

public enum AIController
{
    WolfPack
}
