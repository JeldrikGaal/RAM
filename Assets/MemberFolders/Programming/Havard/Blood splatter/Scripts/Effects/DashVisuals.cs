using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashVisuals : MonoBehaviour
{
    private bool _isDashing = false;
    private Vector3 _startSmudgeSpot;
    [SerializeField] private GameObject _SmudgeEffect;
    private GameObject _currentSmudge;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentSmudge)
        {
            _currentSmudge.transform.position = Vector3.Lerp(new Vector3(_startSmudgeSpot.x, 0.2f, _startSmudgeSpot.z), new Vector3(this.transform.position.x, 0.51f, this.transform.position.z), 0.5f);
            _currentSmudge.transform.localScale = new Vector3(Vector3.Distance(_startSmudgeSpot, _currentSmudge.transform.position), 1, 1);
        }
    }

    public void OverBlood()
    {
        if (_isDashing && !_currentSmudge)
        {
            // Smudge it from the point
            _currentSmudge = Instantiate(_SmudgeEffect, transform.position, Quaternion.Euler(new Vector3(0, transform.rotation.y, 0)));
            _startSmudgeSpot = this.transform.position;

        }
    }

    public void StartDash()
    {
        _isDashing = true;
    }

    public void EndDash()
    {
        _isDashing = false;
        _currentSmudge = null;
    }
}
