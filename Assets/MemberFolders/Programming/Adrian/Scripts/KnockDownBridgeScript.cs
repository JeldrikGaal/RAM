using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDownBridgeScript : MonoBehaviour
{
    [SerializeField] private float _degreesPerSecond;
    [SerializeField] private float _finalRotation;
    [SerializeField] private Transform _pivotPoint;

    private float traverse;
    private float currentBearing;
    private float newBearing;

    public bool CanRotate;

    // Start is called before the first frame update
    void Start()
    {
        _pivotPoint = transform.parent;
    }


    // Update is called once per frame
    void Update()
    {
        if (CanRotate)
        {
            traverse = _degreesPerSecond * Time.deltaTime;
            _pivotPoint.Rotate(new Vector3(traverse, 0, 0));
            // transform.RotateAround(_pivotPoint.position, _pivotPoint.right, Time.deltaTime * _degreesPerSecond);
            // StartCoroutine(StopRotating());

            print(_pivotPoint.eulerAngles.x);

            newBearing = currentBearing + traverse;
            SetCurrentBearing(newBearing);

            // if (transform.eulerAngles.x >= _finalRotation)
            // {
            //     print("too far dude");
            // }
        }
        // transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, _finalRotation), transform.eulerAngles.y, transform.eulerAngles.z);
    }


    void SetCurrentBearing(float rot)
    {
        currentBearing = Mathf.Clamp(rot, 0, 90);
        _pivotPoint.transform.rotation = Quaternion.Euler(rot, 0, 0);
    }

    private IEnumerator StopRotating()
    {
        yield return new WaitForSeconds(_finalRotation / _degreesPerSecond);
        CanRotate = false;
        // Destroy(this);
    }
}
