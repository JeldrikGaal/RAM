using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBlock : ScriptableObject
{
    public abstract void OnStart(Jonas_TempCharacter user, GameObject target);
    public abstract (AI_State state, List<float> val) OnUpdate(Jonas_TempCharacter user, GameObject target);
    public abstract void OnEnd(Jonas_TempCharacter user, GameObject target);
}
