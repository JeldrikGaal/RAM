using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTopDown : MonoBehaviour
{
    CinemachineVirtualCamera _virtualCamera;
    [SerializeField] AnimationCurve _curve;
    [SerializeField] bool _shakeStart;
    [SerializeField] float _shakeDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        _virtualCamera.m_Follow = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(transform.position.x + 7.13f, transform.position.y + 18.03f, transform.position.z + -6.26f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakeStart)
        {
            StartCoroutine(CameraShake());
            _shakeStart = false;
        }
    }
    IEnumerator CameraShake()
    {
        
        Vector3 startPos = transform.position;
        float _timePassed = 0f;

        while (_timePassed < _shakeDuration)
        {
            _timePassed += Time.deltaTime;
            float _strength = _curve.Evaluate(_timePassed / _shakeDuration);
            transform.position = startPos + Random.insideUnitSphere * _strength;
            yield return null;
        }
        transform.position = startPos;
    }

    public void ScreenShake()
    {
        _shakeStart = true;
    }
}
