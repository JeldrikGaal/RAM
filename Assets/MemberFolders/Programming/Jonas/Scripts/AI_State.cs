using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The brain behind each individual state
// Handles running and tracking each of the AI blocks on the characters
public class AI_State : ScriptableObject
{
    [SerializeField] private List<StateBlock> _aiBlocks;

    private Dictionary<EnemyController, int> _timerCount;
    private Dictionary<EnemyController, float> _timer;

    // Runs the starting function of all AI blocks and makes sure all variables are correct
    public void StateStart(EnemyController user, GameObject target)
    {
        if (_timerCount == null) _timerCount = new Dictionary<EnemyController, int>();
        if (_timer == null) _timer = new Dictionary<EnemyController, float>();

        _timerCount[user] = -1;
        _timer[user] = 0;

        foreach (StateBlock blk in _aiBlocks)
        {
            blk.OnStart(user, target);
        }
    }

    // Runs the update function of all AI blocks and handles any returned logic
    public AI_State StateUpdate(EnemyController user, GameObject target)
    {
        // Sets all variables for the start of update
        // MoveInput is reset to allow for stacking of multiple movement types in one state
        int skipNext = 0;
        int tempTimerCounter = -1;
        user.MoveInput = new Vector3();
        if (_timer[user] > 0) _timer[user] -= Time.deltaTime;

        // Loops through all AI blocks in the state, top to bottom
        // If any timers or other logic is encountered solves them before continuing
        // "IF" type blocks skips the next block if the conditions aren't met
        // "TIMER" type blocks stops execution of latter blocks until the timer is over
        foreach (StateBlock blk in _aiBlocks)
        {
            if (skipNext > 0)
            {
                skipNext--;
                continue;
            }

            // Get return values from each block
            // If null, just continue
            // If another state is returned switch to that state
            // If List is not null check and solve for any logic
            (AI_State state, List<float> val) stateVal = blk.OnUpdate(user, target);

            if (stateVal.state != null)
                return stateVal.state;

            if (stateVal.val == null) continue;

            switch ((StateReturn)stateVal.val[0])
            {
                case StateReturn.Skip:
                    if ((int)stateVal.val[1] < 0) return null;
                    skipNext = (int)stateVal.val[1];
                    break;

                case StateReturn.Timer:
                    tempTimerCounter++;
                    if (tempTimerCounter < _timerCount[user]) break;
                    if (_timer[user] > 0) return null;
                    if (tempTimerCounter == _timerCount[user]) break;

                    _timer[user] = stateVal.val[1];
                    _timerCount[user] = tempTimerCounter;

                    return null;

                case StateReturn.Stop:
                    return null;

                default:
                    break;
            }
        }

        return null;
    }

    // Runs the ending function of all the AI blocks
    public void StateEnd(EnemyController user, GameObject target)
    {
        foreach (StateBlock blk in _aiBlocks)
            blk.OnEnd(user, target);
    }
}

// Enum for the various logic behind the AI system
public enum StateReturn
{
    Skip = 0,
    Timer = 1,
    Stop = 2
}