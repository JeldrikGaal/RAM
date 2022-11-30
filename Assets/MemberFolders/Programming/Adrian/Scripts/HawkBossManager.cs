using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HawkBossManager : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;

    private Dictionary<string, int> _weightedAttacks = new Dictionary<string, int>()
    {
        {"Basic Attack", 90},
        {"Horizontal Spray", 10},
        {"Minion Swarm", 0},
        {"Super Claw Melee", 0}
    };

    // [SerializeField] private List<WeightedAttacks> _weightedAttacks;

    // [System.Serializable]
    // public struct WeightedAttacks
    // {
    //     public string Attack;
    //     public int Weight;
    // }

    [SerializeField] private bool _phaseOne;
    [SerializeField] private bool _phaseTwo;
    [SerializeField] private bool _phaseThree;

    [SerializeField] private bool _stageOne;
    [SerializeField] private bool _stageTwo;
    [SerializeField] private bool _stageThree;

    [SerializeField] private bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        _phaseOne = true;
        _stageOne = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_phaseOne)
        {
            #region PhaseOne
            if (_stageOne)
            {
                _weightedAttacks["Basic Attack"] = 90;
                _weightedAttacks["Horizontal Spray"] = 10;

                ChangeToStageTwo();

                Attack();
            }
            else if (_stageTwo)
            {
                _weightedAttacks["Basic Attack"] = 65;
                _weightedAttacks["Horizontal Spray"] = 35;

                ChangeToStageThree();

                Attack();
            }
            else if (_stageThree)
            {
                _weightedAttacks["Basic Attack"] = 35;
                _weightedAttacks["Horizontal Spray"] = 65;

                ChangeToStageOne();

                Attack();
            }
            #endregion
        }
        else if (_phaseTwo)
        {
            #region PhaseTwo
            if (_stageOne)
            {
                _weightedAttacks["Basic Attack"] = 70;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 20;

                ChangeToStageTwo();

                Attack();
            }
            else if (_stageTwo)
            {
                _weightedAttacks["Basic Attack"] = 45;
                _weightedAttacks["Horizontal Spray"] = 15;
                _weightedAttacks["Minion Swarm"] = 40;

                ChangeToStageThree();

                Attack();
            }
            else if (_stageThree)
            {
                _weightedAttacks["Basic Attack"] = 25;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 65;

                ChangeToStageOne();

                Attack();
            }
            #endregion
        }
        else if (_phaseThree)
        {
            #region PhaseThree
            if (_stageOne)
            {
                _weightedAttacks["Basic Attack"] = 55;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 10;
                _weightedAttacks["Super Claw Melee"] = 25;

                ChangeToStageTwo();

                Attack();
            }
            else if (_stageTwo)
            {
                _weightedAttacks["Basic Attack"] = 25;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 30;
                _weightedAttacks["Super Claw Melee"] = 35;

                ChangeToStageThree();

                Attack();
            }
            else if (_stageThree)
            {
                _weightedAttacks["Basic Attack"] = 10;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 25;
                _weightedAttacks["Super Claw Melee"] = 65;

                Attack();
            }
            #endregion
        }
    }

    private void ChangeToStageOne()
    {
        if (_controller.Health < 10)
        {
            _controller.Health = _controller.MaxHealth;
            _stageThree = false;
            _stageOne = true;

            if (_phaseOne)
            {
                _phaseOne = false;
                _phaseTwo = true;
            }
            else if (_phaseTwo)
            {
                _phaseTwo = false;
                _phaseThree = true;
            }
        }
    }

    private void ChangeToStageTwo()
    {
        if (_controller.Health < 90)
        {
            _stageOne = false;
            _stageTwo = true;
        }
    }

    private void ChangeToStageThree()
    {
        if (_controller.Health < 40)
        {
            _stageTwo = false;
            _stageThree = true;
        }
    }

    private int GetWeightedValue()
    {
        // Gets all the different weights and adds them to a sepparate array
        int[] weights = _weightedAttacks.Values.ToArray();

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

    private void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            string[] attacks = _weightedAttacks.Keys.ToArray();

            var attack = attacks[GetWeightedValue()];

            switch (attack)
            {
                case "Basic Attack":
                    StartCoroutine(BasicAttack());
                    break;
                case "Horizontal Spray":
                    StartCoroutine(HorizontalSpray());
                    break;
                case "Minion Swarm":
                    StartCoroutine(MinionSwarm());
                    break;
                case "Super Claw Melee":
                    StartCoroutine(SuperClawMelee());
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator BasicAttack()
    {
        print("Basic attack");
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    private IEnumerator HorizontalSpray()
    {
        print("Horizontal Spray");
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    private IEnumerator MinionSwarm()
    {
        print("Minion Swarm");
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    private IEnumerator SuperClawMelee()
    {
        print("Super Claw Melee");
        yield return new WaitForSeconds(1);
        attacking = false;
    }
}
