using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSpawner : MonoBehaviour
{
    private float _bloodStepCountdown = 0;
    [SerializeField] private bool _bloodStepsActive = false;

    [SerializeField] private GameObject _bloodStepPrefab;

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
            var _step = Instantiate(_bloodStepPrefab, new Vector3(point.x, 0.51f, point.z), this.transform.rotation);
            // Destroy(_step, 10f);
        }
        // do and "else if" here if there are any other steps that should happen
    }


}
