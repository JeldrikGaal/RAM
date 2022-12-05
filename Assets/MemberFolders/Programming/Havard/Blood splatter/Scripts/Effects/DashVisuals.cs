using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashVisuals : MonoBehaviour
{
    private bool _isDashing = false;
    [SerializeField] private GameObject _SmudgeEffect;
    private Vector3 _startSmudgeSpot;
    private GameObject _currentSmudge;
    private Quaternion _dashingDirection;
    [SerializeField] private Transform _directionObject;
    [SerializeField] private float _groundHeight = 0;

    // Tracker:
    [SerializeField] private int _maxItems = 50;
    [SerializeField] private int _maxItemsBackups = 50;
    [SerializeField] private GameObject[] _items;
    private int _completedItems = 0;
    private bool _deleting = false;

    [SerializeField] private DoubleArrayPooling _doubleArrayScript;

    private void Start()
    {
        _doubleArrayScript.Array1 = new GameObject[_maxItems];
        _doubleArrayScript.Array2 = new GameObject[_maxItemsBackups];
        _doubleArrayScript.FullArray1 = false;
        _doubleArrayScript.FullArray2 = false;
        _doubleArrayScript.CurrentArray1 = 0;
        _doubleArrayScript.CurrentArray2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentSmudge)
        {
            // Calculates the middle point and sets the scale to fit between.
            _currentSmudge.transform.position = Vector3.Lerp(new Vector3(_startSmudgeSpot.x, 0.51f + _groundHeight, _startSmudgeSpot.z), new Vector3(this.transform.position.x, 0.51f+ _groundHeight, this.transform.position.z), 0.5f);
            _currentSmudge.transform.localScale = new Vector3(Vector3.Distance(_startSmudgeSpot, _currentSmudge.transform.position), 1, 1);
        }
    }

    public void OverBlood()
    {
        if (_isDashing && !_currentSmudge)
        {
            if (!_doubleArrayScript.FullArray2)
            {
                // Sets the rotation to be same as player,
                _currentSmudge = Instantiate(_SmudgeEffect, transform.position, _dashingDirection);
                // Sets the first position so we can calculate the middle
                _startSmudgeSpot = this.transform.position;
                _doubleArrayScript.AddPoint(_currentSmudge);
            }
            else if (_doubleArrayScript.FullArray2)
            {
                _currentSmudge = _doubleArrayScript.NextToTake;
                _currentSmudge.transform.position = transform.position;
                _currentSmudge.transform.rotation = _dashingDirection;
                if (_currentSmudge.GetComponent<FadeOnTrigger>())
                {
                    _currentSmudge.GetComponent<FadeOnTrigger>().StopFade();
                }
                _doubleArrayScript.AddPoint(_currentSmudge);
                _startSmudgeSpot = this.transform.position;
            }
        }
    }

    public void StartDash(Quaternion direction)
    {
        // Gets the rotation
        _dashingDirection = direction *= Quaternion.Euler(0, 0, -90);
        _isDashing = true;
    }

    public void EndDash()
    {
        // Stops the dash and makess it so our smudge is unlinked
        _isDashing = false;
        _currentSmudge = null;
    }
}
