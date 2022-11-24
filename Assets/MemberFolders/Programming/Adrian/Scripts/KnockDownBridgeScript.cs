using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDownBridgeScript : MonoBehaviour
{
    [SerializeField] private float _degreesPerSecond;
    [SerializeField] private float _finalRotation;
    [SerializeField] private Transform _pivotPoint;


    public bool CanRotate;

    // Start is called before the first frame update
    void Start()
    {
        _pivotPoint = transform.GetChild(0);
    }


    // Update is called once per frame
    void Update()
    {
        if (CanRotate)
        {
            transform.RotateAround(_pivotPoint.position, _pivotPoint.right, Time.deltaTime * _degreesPerSecond);
            StartCoroutine(StopRotating());

            print(transform.eulerAngles.x);

            // if (transform.eulerAngles.x >= _finalRotation)
            // {
            //     print("too far dude");
            // }
        }
        // transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, _finalRotation), transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private IEnumerator StopRotating()
    {
        yield return new WaitForSeconds(_finalRotation / _degreesPerSecond);
        CanRotate = false;
        // Destroy(this);
    }
}
