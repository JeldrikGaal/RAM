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


    // Start is called before the first frame update
    void Start()
    {
        
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

        }
    }

    public void StartDash()
    {
        // Gets the rotation
        _dashingDirection = transform.GetChild(1).transform.rotation *= Quaternion.Euler(0, 90, 0);
        _isDashing = true;
    }

    public void EndDash()
    {
        // Stops the dash and makess it so our smudge is unlinked
        _isDashing = false;
        _currentSmudge = null;
    }
}
