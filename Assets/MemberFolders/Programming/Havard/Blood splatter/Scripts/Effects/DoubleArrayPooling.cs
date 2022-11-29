using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DoubleArrayPooling")]
public class DoubleArrayPooling : ScriptableObject
{
    public GameObject[] Array1;
    public GameObject[] Array2;
    public int CurrentArray1 = 10;
    public int CurrentArray2 = 5;

    // After "FullArray2" is full, stop spawning objects and reuse the NextToTake object instead.
    public bool FullArray1 = false;
    public bool FullArray2 = false;
    public GameObject NextToTake;

    /*private void Start()
    {
        // Sets the array sizes to be the ones set in the inspector
        Array1 = new GameObject[CurrentArray1];
        Array2 = new GameObject[CurrentArray2];
        // Then we reset those numbers so we can use them to count the arrays.
        CurrentArray1 = 0;
        CurrentArray2 = 0;
    }*/

    // When adding a new object to the pool, use this function. Also use this when reusing an asset to make it loop.
    public void AddPoint(GameObject newObject)
    {
        // If the current array number is higher than the length of the array, it resets and says that it has become full once.
        if (CurrentArray1 >= Array1.Length)
        {
            CurrentArray1 = 0;
            // When its full , this means we can start circling inputs to the other script.
            FullArray1 = true;
        }
        if (FullArray1)
        {
            // Start moving things to array 2
            SecondArrayFiller(Array1[CurrentArray1]);
            // Animate the object to fade away here:
            if (Array1[CurrentArray1].GetComponent<FadeOnTrigger>())
            {
                Array1[CurrentArray1].GetComponent<FadeOnTrigger>().TriggerFade();
            }
        }
        // Sets the input object to be the most recent one in the array
        Array1[CurrentArray1] = newObject;
        // Increases one so we can do the same to the next object.
        CurrentArray1++;
    }

    private void SecondArrayFiller(GameObject oldestArray1)
    {
        // If the current array number is higher than the length of the array, it resets and says that it has become full once.
        if (CurrentArray2 >= Array2.Length)
        {
            CurrentArray2 = 0;
            // When this is full, we can tell other scripts to stop making new objects and instead reuse the oldest one here.
            FullArray2 = true;
        }
        if (FullArray1)
        {
            // Tells the script that the next one it should use is the oldest one from the second array. It will then be sorted back into the main array by the script which generates the objects.
            NextToTake = Array2[CurrentArray2];
        }
        // Now that we've sent one away, we can use that number for the next object...
        Array2[CurrentArray2] = oldestArray1;
        // ... before doing it all again with the next int.
        CurrentArray2++;
    }


}
