using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodySteps : MonoBehaviour
{
    [SerializeField]private Vector2[] _locationPoints;
    [SerializeField] private LayerMask _playerLayer;
    private float _height = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        _locationPoints = new Vector2[500];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _locationPoints.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(_locationPoints[i].x, _height, _locationPoints[i].y), transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, _playerLayer))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
                Debug.Log("Hit player");
                // Function to give player bloody shoes here
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 1000, Color.white);
            }
        }
    }

    public void AddPoint(Vector2 point)
    {
        for (int i = 0; i < _locationPoints.Length; i++)
        {
            if(_locationPoints[i] == new Vector2(0,0))
            {
                _locationPoints[i] = point;
                return;
            }
        }
    }

}
