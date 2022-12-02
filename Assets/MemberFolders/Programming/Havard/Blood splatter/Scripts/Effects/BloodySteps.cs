using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodySteps : MonoBehaviour
{

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _detectRadius = 0.5f;

    [SerializeField] private StepsSpawner _stepScript;
    [SerializeField] private DashVisuals _dashScript;

    [SerializeField] private GameObject[] _array1;
    [SerializeField] private GameObject[] _array2;
    [SerializeField] private int _currentArray1 = 10;
    [SerializeField] private int _currentArray2 = 5;
    [SerializeField] private bool _fullArray1 = false;
    // After "FullArray2" is full, stop spawning objects and reuse the NextToTake object instead.
    public bool FullArray2 = false;
    public GameObject NextToTake;

    // Start is called before the first frame update
    void Start()
    {
        _array1 = new GameObject[_currentArray1];
        _array2 = new GameObject[_currentArray2];
        _currentArray1 = 0;
        _currentArray2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _array1.Length; i++)
        {
            if(_array1[i] != null)
            {
                var locationPoint = _array1[i].transform.position;

                Collider[] hitColliders = Physics.OverlapSphere(locationPoint, _array1[i].transform.localScale.x * _detectRadius, _playerLayer);
                foreach (var hitCollider in hitColliders)
                {
                    // Function to give player bloody shoes here
                    _stepScript.RenewBloodSteps();

                    // Telling the dash script that we're above a blood spot
                    if (_dashScript)
                    {
                        _dashScript.OverBlood();
                    }
                }

                /*RaycastHit hit;
                if (Physics.Raycast(new Vector3(locationPoint.x, _height, locationPoint.z), (Vector3.up), out hit, 1000, _playerLayer))
                {
                    // Function to give player bloody shoes here
                    _stepScript.RenewBloodSteps();
                    
                    // Telling the dash script that we're above a blood spot
                    if (_dashScript)
                    {
                        _dashScript.OverBlood();
                    }
                }
                else
                {
                    Debug.DrawRay(new Vector3(locationPoint.x, _height, locationPoint.z), (Vector3.up) * 1000, Color.white);
                }*/
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _array1.Length; i++)
        {
            if(_array1[i] != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_array1[i].transform.position, _array1[i].transform.localScale.x * _detectRadius);
            }
        }

    }

    public void AddPoint(GameObject newObject)
    {
        // If the current array number is higher than the length of the array, it resets and says that it has become full once.
        if (_currentArray1 >= _array1.Length)
        {
            _currentArray1 = 0;
            // When its full , this means we can start circling inputs to the other script.
            _fullArray1 = true;
        }
        if (_fullArray1)
        {
            // Start moving things to array 2
            SecondArrayFiller(_array1[_currentArray1]);
            // Animate the object to fade away here:
            if (_array1[_currentArray1].GetComponent<FadeOnTrigger>())
            {
                _array1[_currentArray1].GetComponent<FadeOnTrigger>().TriggerFade();
            }
        }
        // Sets the input object to be the most recent one in the array
        _array1[_currentArray1] = newObject;
        // Increases one so we can do the same to the next object.
        _currentArray1++;
    }

    private void SecondArrayFiller(GameObject oldestArray1)
    {
        // If the current array number is higher than the length of the array, it resets and says that it has become full once.
        if (_currentArray2 >= _array2.Length)
        {
            Debug.Log(_currentArray2);
            Debug.Log(_array2.Length);
            _currentArray2 = 0;
            // When this is full, we can tell other scripts to stop making new objects and instead reuse the oldest one here.
            FullArray2 = true;
        }
        if (_fullArray1)
        {
            // Tells the script that the next one it should use is the oldest one from the second array. It will then be sorted back into the main array by the script which generates the objects.
            NextToTake = _array2[_currentArray2];
        }
        // Now that we've sent one away, we can use that number for the next object...
        _array2[_currentArray2] = oldestArray1;
        // ... before doing it all again with the next int.
        _currentArray2++;
    }


}
