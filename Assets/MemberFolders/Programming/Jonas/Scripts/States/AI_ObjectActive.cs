using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ObjectActive : StateEffect
{
    public GameObject Object;
    public bool Activate;

    public override void OnStart(GameObject user, GameObject target)
    {
        if (Activate)
            Object.SetActive(true);
    }

    public override void OnEnd()
    {
        if (!Activate)
            Object.SetActive(false);
    }

    public override State OnUpdate() { return null; }
}
