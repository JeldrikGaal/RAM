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
    [SerializeField] Vector3 _cameraDistanceVector;
    [SerializeField] Vector3 _cameraDistanceVectorCurrent;
    [SerializeField] float _cameraDistance;
    [SerializeField] float _cameraDistanceCurrent;
    // Start is called before the first frame update
    void Start()
    {
        //Necessary component for screen shake
        _impulseSource = GetComponent<CinemachineImpulseSource>();

        //Necessary component for camera follow
        _virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        _virtualCamera.m_Follow = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(_virtualCamera.m_Follow.transform.position.x + 7.13f, _virtualCamera.m_Follow.transform.position.y + 18.03f, _virtualCamera.m_Follow.transform.position.z + -6.26f);
        _cameraDistanceVector = new Vector3(7.13f, 18.03f, -6.26f);
        _cameraDistance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        _cameraDistanceCurrent = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        _cameraDistanceVectorCurrent = transform.position - _virtualCamera.m_Follow.transform.position;
        if (_shakeStart)
        {
            _impulseSource.GenerateImpulseWithForce(screenShakeForce);
            _shakeStart = false;
        }
        if (_cameraDistanceCurrent > _cameraDistance * 1.05f || _cameraDistanceCurrent < _cameraDistance * 0.95f)
        {
            //transform.position = transform.position + _cameraDistanceVector;
        }
    }

    /// <summary>
    /// Use this to create a screen shake effect. Do not use this in continuous methods like Update(). Call this once every time you need it. Default force value is 0.1f. Can be tested in inspector with "Shake Start" bool.
    /// </summary>
    public void ScreenShake()
    {
        _shakeStart = true;
    }
    /// <summary>
    /// Use this to update the camera follow after you have changed the player position from the code and it messes up the camera. Hopefully we won't need it.
    /// </summary>
    public void UpdateVCam()
    {
        _virtualCamera.enabled = false;
        transform.position = new Vector3(_virtualCamera.m_Follow.transform.position.x + 7.13f, _virtualCamera.m_Follow.transform.position.y + 18.03f, _virtualCamera.m_Follow.transform.position.z + -6.26f);
        _virtualCamera.enabled = true;
    }
}
