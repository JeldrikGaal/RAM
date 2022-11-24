using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoSillyTree : MonoBehaviour, IRammable
{
    private Quaternion _modifiedRotation;
    private Quaternion _originalRotation;
    private Quaternion _targetRotation;
    private Vector3 _originalPosition;
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private float _fallTime;
    private float _hitTime = -999;
    [SerializeField] private float _height;
    [SerializeField] AnimationCurve curve;
    
    public bool Hit(GameObject g)
    {
        var fromPlayer = Quaternion.LookRotation(-(g.transform.position - transform.position));

        _originalRotation = transform.rotation;
        _modifiedRotation = Quaternion.Euler(fromPlayer.eulerAngles - transform.rotation.eulerAngles);

        _hitTime = Time.time;
        
        return true;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if ((Time.time - _hitTime) / _fallTime <= 1)
        {
            var rotation = Quaternion.Lerp(_originalRotation, _targetRotation, curve.Evaluate((Time.time - _hitTime) / _fallTime));
            _rigid.Move(PositionAroundThePoint(rotation.eulerAngles,_height/2),rotation);

            
        }
        
    }
    private Vector3 PositionAroundThePoint(Vector3 eulerAngles, float radius) => (_originalPosition - Vector3.up * (_height / 2)) + (Quaternion.Euler(_originalRotation.eulerAngles + eulerAngles) * (Vector3.up * radius));
}
