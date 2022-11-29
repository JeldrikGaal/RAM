using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSpawner : MonoBehaviour
{
    private float _bloodStepCountdown = 0;
    [SerializeField] private bool _bloodStepsActive = false;

    [SerializeField] private GameObject _bloodStepPrefab;

    // Tracker:
    [SerializeField] private int _maxFootprints = 50;
    [SerializeField] private int _maxFootprintBackups = 50;
    [SerializeField] private GameObject[] _footprints;
    private int _completedFootprints = 0;
    private bool _deleting = false;

    [SerializeField] private DoubleArrayPooling _doubleArrayScript;

    private void Start()
    {
        _doubleArrayScript.Array1 = new GameObject[_maxFootprints];
        _doubleArrayScript.Array2 = new GameObject[_maxFootprintBackups];
        _doubleArrayScript.FullArray1 = false;
        _doubleArrayScript.FullArray2 = false;
        _doubleArrayScript.CurrentArray1 = 0;
        _doubleArrayScript.CurrentArray2 = 0;
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

            if (!_doubleArrayScript.FullArray2)
            {
                _step = Instantiate(_bloodStepPrefab, new Vector3(point.x, 0.51f, point.z), this.transform.rotation);
                _doubleArrayScript.AddPoint(_step);
            } else if (_doubleArrayScript.FullArray2)
            {
                _step = _doubleArrayScript.NextToTake;
                _step.transform.position = new Vector3(point.x, 0.51f, point.z);
                _step.transform.rotation = this.transform.rotation;
                if (_step.GetComponent<FadeOnTrigger>())
                {
                    _step.GetComponent<FadeOnTrigger>().StopFade();
                }
                _doubleArrayScript.AddPoint(_step);
            }
        }
        // do and "else if" here if there are any other steps that should happen
    }




    // Deletes the oldest one if one is added when created
    /*public void DeleteOldest(GameObject footprint)
    {
        if (_completedFootprints >= _footprints.Length)
        {
            _completedFootprints = 0;
            _deleting = true;
        }
        if (_deleting == true)
        {
            _footprints[_completedFootprints].GetComponent<FadeOnTrigger>().Fade = true;
            Destroy(_footprints[_completedFootprints].gameObject, 1f);
        }
        _footprints[_completedFootprints] = footprint;
        _completedFootprints++;        
    }*/

}
