using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DoubleArrayPooling")]
public class DoubleArrayPooling : ScriptableObject
{
    [SerializeField] private GameObject[] _array1;
    [SerializeField] private GameObject[] _array2;
    public int CurrentArray1 = 10;
    public int CurrentArray2 = 5;
    [SerializeField] private bool _fullArray1 = false;

    // After "FullArray2" is full, stop spawning objects and reuse the NextToTake object instead.
    public bool FullArray2 = false;
    public GameObject NextToTake;

    private void Start()
    {
        // Sets the array sizes to be the ones set in the inspector
        _array1 = new GameObject[CurrentArray1];
        _array2 = new GameObject[CurrentArray2];
        // Then we reset those numbers so we can use them to count the arrays.
        CurrentArray1 = 0;
        CurrentArray2 = 0;
    }

    // When adding a new object to the pool, use this function. Also use this when reusing an asset to make it loop.
    public void AddPoint(GameObject newObject)
    {
        // If the current array number is higher than the length of the array, it resets and says that it has become full once.
        if (CurrentArray1 >= _array1.Length)
        {
            CurrentArray1 = 0;
            // When its full , this means we can start circling inputs to the other script.
            _fullArray1 = true;
        }
        if (_fullArray1)
        {
            // Start moving things to array 2
            SecondArrayFiller(_array1[CurrentArray1]);
            // Animate the object to fade away here:
            // _array1[_currentArray1].fade
        }
        // Sets the input object to be the most recent one in the array
        _array1[CurrentArray1] = newObject;
        // Increases one so we can do the same to the next object.
        CurrentArray1++;
    }

    private void SecondArrayFiller(GameObject oldestArray1)
    {
        // If the current array number is higher than the length of the array, it resets and says that it has become full once.
        if (CurrentArray2 >= _array2.Length)
        {
            CurrentArray2 = 0;
            // When this is full, we can tell other scripts to stop making new objects and instead reuse the oldest one here.
            FullArray2 = true;
        }
        if (_fullArray1)
        {
            // Tells the script that the next one it should use is the oldest one from the second array. It will then be sorted back into the main array by the script which generates the objects.
            NextToTake = _array2[CurrentArray2];
        }
        // Now that we've sent one away, we can use that number for the next object...
        _array2[CurrentArray2] = oldestArray1;
        // ... before doing it all again with the next int.
        CurrentArray2++;
    }


}
