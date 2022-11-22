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

    // Tracker:
    [SerializeField] private int maxItems = 50;
    [SerializeField] private GameObject[] _items;
    private int _completedItems = 0;
    private bool _deleting = false;

    // Start is called before the first frame update
    void Start()
    {
        _items = new GameObject[maxItems];
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentSmudge)
        {
            // Calculates the middle point and sets the scale to fit between.
            _currentSmudge.transform.position = Vector3.Lerp(new Vector3(_startSmudgeSpot.x, 0.51f, _startSmudgeSpot.z), new Vector3(this.transform.position.x, 0.51f, this.transform.position.z), 0.5f);
            _currentSmudge.transform.localScale = new Vector3(Vector3.Distance(_startSmudgeSpot, _currentSmudge.transform.position), 1, 1);
        }
    }

    public void OverBlood()
    {
        if (_isDashing && !_currentSmudge)
        {
            // Sets the rotation to be same as player,
            _currentSmudge = Instantiate(_SmudgeEffect, transform.position, _dashingDirection);
            // Sets the first position so we can calculate the middle
            _startSmudgeSpot = this.transform.position;
            DeleteOldest(_currentSmudge);
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

    public void DeleteOldest(GameObject footprint)
    {
        if (_completedItems >= _items.Length)
        {
            _completedItems = 0;
            _deleting = true;
        }
        if (_deleting == true)
        {
            _items[_completedItems].GetComponent<FadeOnTrigger>().Fade = true;
            Destroy(_items[_completedItems].gameObject, 1);
        }
        _items[_completedItems] = footprint;
        _completedItems++;
    }
}
