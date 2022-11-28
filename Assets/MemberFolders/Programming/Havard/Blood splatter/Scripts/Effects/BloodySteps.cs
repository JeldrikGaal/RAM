using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodySteps : MonoBehaviour
{
    [SerializeField] private int _maxPoints = 500;
    [SerializeField] private int _backupPoints = 50;
    private int _completedPoint;
    private int _completedBackup;
    private GameObject[] _bloodSplats;
    private Vector2[] _locationPoints;
    private GameObject[] _bloodSplatsBackup;
    private bool _deleting = false;
    public bool _backupDeleting = false;

    [SerializeField] private LayerMask _playerLayer;
    private float _height = 0;

    [SerializeField] private StepsSpawner _stepScript;
    [SerializeField] private DashVisuals _dashScript;

    // Start is called before the first frame update
    void Start()
    {
        _locationPoints = new Vector2[_maxPoints];
        _bloodSplats = new GameObject[_maxPoints];
        _bloodSplatsBackup = new GameObject[_backupPoints];
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
    public void AddPoint(Vector2 point, GameObject ob)
    {
        if(_completedPoint >= _locationPoints.Length)
        {
            _completedPoint = 0;
            _deleting = true;
        }
        if (_deleting == true)
        {
            _bloodSplats[_completedPoint].GetComponent<FadeOnTrigger>().Fade = true;
            // AddBackupPoint(_bloodSplats[_completedPoint]);
            Destroy(_bloodSplats[_completedPoint].gameObject, 5);
        }
        _bloodSplats[_completedPoint] = ob;
        _locationPoints[_completedPoint] = point;
        _completedPoint++;
        return;
    }

    public GameObject GetSplat()
    {
        return _bloodSplatsBackup[_completedBackup];
    }

    private void AddBackupPoint(GameObject splat)
    {
        print(_completedBackup);
        print(_bloodSplatsBackup.Length);
        if(_completedBackup >= _bloodSplatsBackup.Length)
        {
            _completedBackup = 0;
            _backupDeleting = true;
        }
        if (_backupDeleting)
        {

        }

        

        _bloodSplatsBackup[_completedBackup] = splat;

        _completedBackup++;
    }


}
