using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    float _time;
    float _randomTime = 1.5f;
    Vector3 _currentPos;
    Vector3 _targetPos;
    RectTransform _rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _targetPos = _rectTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        if (_rectTransform.localPosition == _targetPos)
        {
            _randomTime = Random.Range(1.3f, 2f);
            _time = Time.time;
            print("p");
            _currentPos = _rectTransform.localPosition;
            _targetPos = new Vector3(Random.Range(449, 861), Random.Range(-115, 432), _currentPos.z);
        }
        _rectTransform.localPosition = Vector3.Lerp(_currentPos, _targetPos, (Time.time - _time) / _randomTime);
    }
}
