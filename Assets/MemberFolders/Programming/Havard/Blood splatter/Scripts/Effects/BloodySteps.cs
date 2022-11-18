using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodySteps : MonoBehaviour
{
    [SerializeField]private Vector2[] _locationPoints;
    private int _completedPoint;
    [SerializeField] private int _maxPoints = 500;
    [SerializeField] private LayerMask _playerLayer;
    private float _height = 0;

    [SerializeField] private StepsSpawner _stepScript;
    [SerializeField] private DashVisuals _dashScript;

    // Start is called before the first frame update
    void Start()
    {
        _locationPoints = new Vector2[_maxPoints];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _locationPoints.Length; i++)
        {
            if(_locationPoints[i] != new Vector2(0, 0))
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(_locationPoints[i].x, _height, _locationPoints[i].y), (Vector3.up), out hit, 5, _playerLayer))
                {
                    Debug.DrawRay(new Vector3(_locationPoints[i].x, _height, _locationPoints[i].y), (Vector3.up) * hit.distance, Color.yellow);

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
                    Debug.DrawRay(new Vector3(_locationPoints[i].x, _height, _locationPoints[i].y), (Vector3.up) * 5, Color.white);
                }
            }
        }
    }
    // Function to add new point. Latest ones gets deleted if it exceeds the max.
    public void AddPoint(Vector2 point)
    {
        if(_completedPoint >= _locationPoints.Length)
        {
            _completedPoint = 0;
        }
        _locationPoints[_completedPoint] = point;
        _completedPoint++;
        return;
    }

}
