using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HawkBossAttackPhaseOne : StateBlock
{
    [SerializeField] private int _weight1;
    [SerializeField] private int _weight2;
    private Dictionary<string, int> _weightTable = new Dictionary<string, int>()
    {
        {"Basic Attack", 1},
        {"Horizontal Spray", 1}
    };


    private Dictionary<EnemyController, bool> _isDone;
    public override void OnStart(EnemyController user, GameObject target)
    {
        _weightTable["Basic Attack"] = _weight1;
        _weightTable["Horizontal Spray"] = _weight2;

        if (_isDone == null)
        {
            _isDone = new Dictionary<EnemyController, bool>();
        }

        _isDone[user] = false;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_isDone[user])
        {
            string[] keys = _weightTable.Keys.ToArray();

            switch (keys[GetWeightedValue()])
            {
                case "Basic Attack":
                    BasicAttack();
                    break;
                case "Horizontal Spray":
                    HorizontalSpray();
                    break;
                default:
                    break;
            }

            _isDone[user] = true;
        }

        return (null, null);
    }
    public override void OnEnd(EnemyController user, GameObject target)
    {
        _isDone.Remove(user);
    }

    void BasicAttack()
    {
        Debug.Log("basic attack " + _weightTable["Basic Attack"]);
    }

    void HorizontalSpray()
    {
        Debug.Log("Horizontal Spray " + _weightTable["Horizontal Spray"]);
    }

    private int GetWeightedValue()
    {
        // Gets all the different weights and adds them to a sepparate array
        int[] weights = _weightTable.Values.ToArray();

        // Gets a random weight
        int randomWeight = Random.Range(0, weights.Sum());

        // Makes an int for the accumulated weights
        int accumulation = 0;

        // Makes a new array for the running total of the weights
        int[] runningTotal = new int[weights.Length];

        // Loops through and adds together all the weights, adding the current running total to the runningtotal array
        for (int i = 0; i < weights.Length; i++)
        {
            // Adds the current weight to the total
            accumulation += weights[i];

            // Adds the current running total to the array
            runningTotal[i] = accumulation;
        }

        // Makes new values for the min and max value
        var low = 0;
        var high = weights.Length;

        // As long as low is less than high
        while (low < high)
        {
            // Mid is the halfway point between low and high
            var mid = (int)((low + high) / 2);

            // Sets distance to the halfway point
            var distance = runningTotal[mid];

            // If the mid point value is lower than the weight we want
            if (distance < randomWeight)
            {
                // Set low to a point over mid
                low = mid + 1;
            }
            else if (distance > randomWeight)
            {
                // If the midpoint value is higher than the point we want
                high = mid;
            }
            else
            {
                // Return mid if it is neither higher nor lower than the mid point
                return mid;
            }
        }
        // Return low when low no longer is less than high
        return low;
    }
}
