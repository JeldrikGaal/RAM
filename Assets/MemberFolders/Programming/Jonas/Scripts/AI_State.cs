using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State : ScriptableObject
{
    public List<StateBlock> AIBlocks;

    private Dictionary<Jonas_TempCharacter, int> _timerCount;
    private Dictionary<Jonas_TempCharacter, float> _timer;

    public void StateStart(Jonas_TempCharacter user, GameObject target)
    {
        if (_timerCount == null) _timerCount = new Dictionary<Jonas_TempCharacter, int>();
        if (_timer == null) _timer = new Dictionary<Jonas_TempCharacter, float>();

        _timerCount[user] = -1;
        _timer[user] = 0;

        foreach (StateBlock blk in AIBlocks)
        {
            blk.OnStart(user, target);
        }
    }

    public AI_State StateUpdate(Jonas_TempCharacter user, GameObject target)
    {
        int tempTimerCounter = -1;

        user.MoveInput = new Vector3();
        if (_timer[user] > 0) _timer[user] -= Time.deltaTime;

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

            switch ((StateReturn)stateVal.val[0])
            {
                case StateReturn.Skip:
                    skipNext = true;
                    break;

                case StateReturn.Timer:
                    tempTimerCounter++;
                    if (tempTimerCounter < _timerCount[user]) break;
                    if (_timer[user] > 0) return null;
                    if (tempTimerCounter == _timerCount[user]) break;

                    Debug.Log(blk.name);

                    _timer[user] = stateVal.val[1];
                    _timerCount[user] = tempTimerCounter;

                    return null;

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
    Timer = 1
}