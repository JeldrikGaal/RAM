using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSpawner : MonoBehaviour
{
    private float _bloodStepCountdown = 0;
    [SerializeField] private bool _bloodStepsActive = false;

    [SerializeField] private GameObject _bloodStepPrefab;

    // Tracker:
    [SerializeField] private int _maxFootprint = 50;
    [SerializeField] private GameObject[] _footprints;
    private int _completedFootprints = 0;
    private bool _deleting = false;

    private void Start()
    {
        _footprints = new GameObject[_maxFootprint];
    }

    void Update()
    {
        // Lowers countdown
        _bloodStepCountdown -= Time.deltaTime;

        // If countdown is less than 0, steps can't spawn \
        if(_bloodStepCountdown > 0)
        {
            _bloodStepsActive = true;
        } else if(_bloodStepCountdown <= 0)
        {
            _bloodStepsActive = false;
        }
    }

    // Function to resets steps
    public void RenewBloodSteps()
    {
        _bloodStepCountdown = 2.5f;
    }
    // Function to spawn the steps.
    public void SpawnSteps(Vector3 point)
    {
        if (_bloodStepsActive)
        {
            GameObject _step;
            //print(this.transform.rotation.eulerAngles.y);
            _step = Instantiate(_bloodStepPrefab, new Vector3(point.x, 0.51f, point.z), this.transform.rotation); // Issue here where the rotation sometimes is not (90, 0, correctRotation) so it shows as a bit of white.
            DeleteOldest(_step);
        }
        // do and "else if" here if there are any other steps that should happen
    }

    // Deletes the oldest one if one is added when created
    public void DeleteOldest(GameObject footprint)
    {
        if (_completedFootprints >= _footprints.Length)
        {
            _completedFootprints = 0;
            _deleting = true;
        }
        if (_deleting == true)
        {
            Destroy(_footprints[_completedFootprints].gameObject);
        }
        _footprints[_completedFootprints] = footprint;
        _completedFootprints++;        
    }

}
