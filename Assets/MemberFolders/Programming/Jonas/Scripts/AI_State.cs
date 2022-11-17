using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State : ScriptableObject
{
    public List<StateBlock> AIBlocks;

    public void StateStart(Jonas_TempCharacter user, GameObject target)
    {
        foreach (StateBlock blk in AIBlocks)
        {
            blk.OnStart(user, target);
        }
    }

    public AI_State StateUpdate(Jonas_TempCharacter user, GameObject target)
    {
        user.MoveInput = new Vector3();

        bool skipNext = false;

        foreach (StateBlock blk in AIBlocks)
        {
            if (skipNext)
            {
                skipNext = false;
                continue;
            }

            (AI_State state, List<float> val) stateVal = blk.OnUpdate(user, target);

            if (stateVal.state != null)
                return stateVal.state;

            if (stateVal.val == null) continue;

            switch (stateVal.val[0])
            {
                case 0:
                    skipNext = true;
                    break;

                default:
                    break;
            }
        }

        return null;
    }

    public void StateEnd(Jonas_TempCharacter user, GameObject target)
    {
        foreach (StateBlock blk in AIBlocks)
            blk.OnEnd(user, target);
    }
}

public enum StateReturn
{
    Skip = 0,
    Wait = 1
}