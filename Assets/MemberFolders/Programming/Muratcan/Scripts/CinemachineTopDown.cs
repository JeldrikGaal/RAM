using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTopDown : MonoBehaviour
{
    CinemachineVirtualCamera _virtualCamera;
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
        
    }
}
