using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTopDown : MonoBehaviour
{
    CinemachineVirtualCamera _virtualCamera;
    CinemachineImpulseSource _impulseSource;
    [SerializeField] float screenShakeForce = 0.1f;
    [SerializeField] bool _shakeStart;
    // Start is called before the first frame update
    void Start()
    {
        //Necessary component for screen shake
        _impulseSource = GetComponent<CinemachineImpulseSource>();

        //Necessary component for camera follow
        _virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        _virtualCamera.m_Follow = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(transform.position.x + 7.13f, transform.position.y + 18.03f, transform.position.z + -6.26f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakeStart)
        {
            _impulseSource.GenerateImpulseWithForce(screenShakeForce);
            _shakeStart = false;
        }
    }

    /// <summary>
    /// Use this to create a screen shake effect. Do not use this in continuous methods like Update(). Call this once every time you need it. Default force value is 0.1f. Can be tested in inspector with "Shake Start" bool.
    /// </summary>
    public void ScreenShake()
    {
        _shakeStart = true;
    }
}
