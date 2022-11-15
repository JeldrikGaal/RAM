using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public Target TargetType;

    private Jonas_TempCharacter _c;
    private GameObject _target;

    private List<StateEffect> _stateEffects;

    public State StateUpdate()
    {
        State returnState = null;

        _c.MoveInput = new Vector3();

        foreach (StateEffect eff in _stateEffects)
        {
            returnState = eff.OnUpdate();
            if (returnState != null)
                break;
        }

        return returnState;
    }

    public void StateStart()
    {
        _c = transform.parent.parent.GetComponent<Jonas_TempCharacter>();

        _stateEffects = new List<StateEffect>(GetComponents<StateEffect>());

        foreach (StateEffect eff in _stateEffects)
        {
            GameObject target = null;
            if(TargetType == Target.Player) 
                target = GameObject.FindGameObjectWithTag("Player");

            eff.OnStart(transform.parent.parent.gameObject, target);
        }
    }

    public void StateEnd()
    {
        foreach (StateEffect eff in _stateEffects)
            eff.OnEnd();
    }
}

public enum Target
{
    None,
    Player
}
