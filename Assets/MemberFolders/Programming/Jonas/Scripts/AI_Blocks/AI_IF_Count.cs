using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks the numbe of enemies of the same type
// If _more is true returns true of there are more than _count enemies, less if _more is false

public class AI_IF_Count : StateBlock
{
    [SerializeField] private int _skipCount = 1;
    [SerializeField] private int _count;
    [SerializeField] private bool _more;

    [SerializeField] private AIController _packGroup;

    private AIController_IfCount _controller;

    // Creates a controller if there is none
    // Adds the current character to the controller
    public override void OnStart(EnemyController user, GameObject target)
    {
        // Connect to controller, create if none
        if (_controller == null)
        {
            GameObject controllerGameObject = new GameObject($"AIController_IfCount{_packGroup.ToString()}");
            controllerGameObject.AddComponent<AIController_IfCount>();
            _controller = controllerGameObject.GetComponent<AIController_IfCount>();
            _controller.Init();
        }

        _controller.AddMember(user);
    }

    // Removes itself from the controller
    public override void OnEnd(EnemyController user, GameObject target)
    {
        _controller.RemoveMember(user);
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (_controller.GetCount() > _count == _more)
            return (null, null);

        List<float> temp = new List<float>();
        temp.Add((float)StateReturn.Skip);
        temp.Add(_skipCount);
        return (null, temp);
    }
}
