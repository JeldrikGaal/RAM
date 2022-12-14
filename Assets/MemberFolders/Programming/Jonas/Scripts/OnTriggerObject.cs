using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerObject : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private float _waitTimer;
    [SerializeField] private bool _setTo;

    private float _timer;
    private bool _timerActive = false;

    private void Start()
    {
        _timer = _waitTimer;
    }

    private void Update()
    {
        if (_timerActive)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
                _object.SetActive(_setTo);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player"))
        {
            if(_timer == 0)
            {
                _object.SetActive(_setTo);
                return;
            }
            _timerActive = true;
        }
    }
}
