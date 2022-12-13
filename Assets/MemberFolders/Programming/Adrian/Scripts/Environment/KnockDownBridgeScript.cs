using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDownBridgeScript : MonoBehaviour
{
    [SerializeField] private float _degreesPerSecond;
    [SerializeField] private float _finalRotation;
    [SerializeField] private Transform _pivotPoint;
    [SerializeField] private bool _testBool;

    [HideInInspector] public bool CanRotate;

    [SerializeField] private AnimationCurve _animCurve;

    private float _fallTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Gets a reference to the parent / pivot point
        _pivotPoint = transform.parent;
    }


    // Update is called once per frame
    void Update()
    {
        if(_testBool)
        {
            CanRotate = true;
            _testBool = false;
        }
        // Checks to see if it can rotate
        if (CanRotate)
        {
            _fallTimer += Time.deltaTime;
            // Starts the coroutine that stops the rotation after a while
            //StartCoroutine(StopRotating());

            // Rotates the pivot point

            if(_fallTimer > _animCurve[1].time)
            {
                Destroy(this);
            }

            //_pivotPoint.Rotate(new Vector3(_degreesPerSecond * Time.deltaTime, 0, 0));

            // Clamps the rotation to 90 degrees
            _pivotPoint.eulerAngles = new Vector3(_animCurve.Evaluate(_fallTimer), _pivotPoint.eulerAngles.y, _pivotPoint.eulerAngles.z);
        }
    }


    private IEnumerator StopRotating()
    {
        // Waits the final rotation degrees divided by the degrees per second amount of seconds
        yield return new WaitForSeconds(_finalRotation / _degreesPerSecond);

        // Stops the rotation
        CanRotate = false;

        // Destroys the script so it can't be activated again
        // Destroy(this);
    }
}
