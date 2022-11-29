using System.Runtime.InteropServices;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedRandomness : MonoBehaviour
{
    private Dictionary<string, int> weightTable = new Dictionary<string, int>()
    {
        {"Wolf", 45},
        {"Ram", 50},
        {"Powerup", 125},
        {"Tree", 50},
        {"Hawk", 30},
        {"Bear", 100},
    };

    private string[] stringList;
    // Start is called before the first frame update
    void Start()
    {
        stringList = weightTable.Keys.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            var weight = GetWeightedString();
            print(stringList[weight]);

            switch (weight)
            {
                case 0:
                    Test0();
                    break;
                case 1:
                    Test1();
                    break;
                case 2:
                    Test2();
                    break;
                case 3:
                    Test3();
                    break;
                case 4:
                    Test4();
                    break;
                case 5:
                    Test5();
                    break;
                default:
                    print("It broke");
                    break;
            }
        }
    }

    private int GetWeightedString()
    {
        // Gets all the different weights and adds them to a sepparate array
        int[] weights = weightTable.Values.ToArray();

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

    private void Test0()
    {
        print("wow test 0");
    }

    private void Test1()
    {
        print("wow test 1");
    }

    private void Test2()
    {
        print("wow test 2");
    }
    private void Test3()
    {
        print("wow test 3");
    }
    private void Test4()
    {
        print("wow test 4");
    }
    private void Test5()
    {
        print("wow test 5");
    }
}
