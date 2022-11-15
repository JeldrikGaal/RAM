using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Player Reference
    [SerializeField] private GameObject _rammy;
    private Vector3 _startPosition;



    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _startPosition + _rammy.transform.position;
    }
}
