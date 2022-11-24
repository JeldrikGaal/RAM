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
            _pivotPoint.eulerAngles = new Vector3(90, 0, 0);
        }
    }


    private IEnumerator StopRotating()
    {
        yield return new WaitForSeconds(_finalRotation / _degreesPerSecond);
        CanRotate = false;
        // Destroy(this);
    }
}
