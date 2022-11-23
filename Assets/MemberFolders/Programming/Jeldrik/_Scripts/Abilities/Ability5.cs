using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability5 : Abilities
{
    Rigidbody _rb;
    Vector3 _dashDestination;
    bool _inProgress = false;
    float _startTimeDash;
    [SerializeField] float timer = 0f;
    [SerializeField] float _pushForce = 50f;
    [SerializeField] float _pushDistance = 8f;
    [SerializeField] float _moveDuration = 1.5f;
    public override void Start()
    {
        _rb = GetComponent<Rigidbody>();
        base.Start();
    }
    override public void Update()
    {
        
        base.Update();
        if (_inProgress)
        {
            timer += Time.deltaTime;
            print("p");
            transform.position = Vector3.Lerp(transform.position, _dashDestination, ((Time.time - _startTimeDash) / _moveDuration));
            print((Time.time - _startTimeDash) / _moveDuration);
        }
    }
    override public void Activate()
    {
        StartCoroutine(WaitForJumpAnim());
    }

    IEnumerator WaitForJumpAnim()
    {
        yield return new WaitForSeconds(0.5f);
        // Checking if player would end up in an object while dashing and shortening dash if thats the case
        _dashDestination = transform.position + transform.up * _pushDistance;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, _pushDistance))
        {
            RaycastHit hit2;
            if (Physics.Raycast(_dashDestination + Vector3.up * 100, Vector3.down, out hit2, 105) && hit.transform == hit2.transform)
            {
                _dashDestination = hit.point;
            }
        }
        _startTimeDash = Time.time;
        timer = 0f;
        _inProgress = true;
        yield return new WaitForSeconds(0.6f);
        _inProgress = false;
        
    }
}
