using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ObjectActive : StateBlock
{
    public bool Activate;

    public override void OnStart(Jonas_TempCharacter user, GameObject target)
    {
        if (Activate)
        {
            GameObject obj = user.transform.Find("Active").gameObject;
            obj.SetActive(true);
        }
    }

    public override (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target)
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnd(Jonas_TempCharacter user, GameObject target)
    {
        if (!Activate)
        {
            GameObject obj = user.transform.Find("Active").gameObject;
            obj.SetActive(false);
        }
    }
}
