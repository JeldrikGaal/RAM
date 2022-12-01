using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HawkBossAttackPhaseOne : StateBlock
{
    public Dictionary<string, int> WeightTable = new Dictionary<string, int>()
    {
        {"Basic Attack", 90},
        {"Horizontal Spray", 10},
        {"Minion Swarm", 0},
        {"Super Claw Melee", 0}
    };

    private bool _attacking;

    [HideInInspector] public GameObject Egg;
    [HideInInspector] public float ShootSpeed;
    [HideInInspector] public Transform ShootPoint;

    private float _eggTimer;
    private int _eggsShot;

    private Dictionary<EnemyController, bool> _isDone;

    // [SerializeField] private List<WeightedAttacks> _weightedAttacks;

    // [System.Serializable]
    // public struct WeightedAttacks
    // {
    //     public string Attack;
    //     public int Weight;
    // }

    public override void OnStart(EnemyController user, GameObject target)
    {
        // Adds the specified attacks and their weights to the dictionary
        // for (int i = 0; i < _weightedAttacks.Count; i++)
        // {
        //     if (WeightTable.ContainsKey(_weightedAttacks[i].Attack))
        //     {
        //         WeightTable[_weightedAttacks[i].Attack] = _weightedAttacks[i].Weight;
        //     }
        //     else
        //     {
        //         WeightTable.Add(_weightedAttacks[i].Attack, _weightedAttacks[i].Weight);
        //     }
        // }

        if (_isDone == null) _isDone = new Dictionary<EnemyController, bool>();

        _isDone[user] = false;
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        if (!_isDone[user])
        {
            // Makes an array of all the key values
            string[] keys = WeightTable.Keys.ToArray();

            // Switch statement for the different Values
            switch (keys[GetWeightedValue()])
            {
                case "Basic Attack":
                    BasicAttack(user);
                    break;
                case "Horizontal Spray":
                    HorizontalSpray();
                    break;
                case "Minion Swarm":
                    MinionSwarm();
                    break;
                case "Super Claw Melee":
                    SuperClawMelee();
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

    private void BasicAttack(EnemyController user)
    {
        Debug.Log("Basic attack " + WeightTable["Basic Attack"]);

        for (int i = 0; i < 3; i++)
        {
            var egg = Instantiate(Egg, ShootPoint.position, Quaternion.identity);
            egg.GetComponent<Rigidbody>().AddForce(user.transform.forward * ShootSpeed);
            Destroy(egg, 6);
        }
        _attacking = false;
    }

    void HorizontalSpray()
    {
        Debug.Log("Horizontal Spray " + WeightTable["Horizontal Spray"]);
    }

    private void MinionSwarm()
    {
        Debug.Log("Minion Swarm " + WeightTable["Minion Swarm"]);
    }

    private void SuperClawMelee()
    {
        Debug.Log("Super Claw Melee " + WeightTable["Super Claw Melee"]);
    }

    private int GetWeightedValue()
    {
        // Gets all the different weights and adds them to a sepparate array
        int[] weights = WeightTable.Values.ToArray();

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
