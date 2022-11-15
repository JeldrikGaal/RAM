using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateEffect : MonoBehaviour
{
    protected GameObject _user;
    protected GameObject _target;

    public abstract void OnStart(GameObject user, GameObject target);
    public abstract State OnUpdate();
    public abstract void OnEnd();
}
