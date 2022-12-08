using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability5 : Abilities
{
    Rigidbody _rb;
    Vector3 _dashDestination;
    Vector3 _dashStart;
    Quaternion _dashStartRot;
    bool _inProgress = false;
    float _startTimeDash;
    [SerializeField] float _damage = 30f;
    [SerializeField] float _pushDistance = 8f;
    [SerializeField] float _moveDuration = 1.5f;
    [SerializeField] GameObject _externalCollider;

    [SerializeField] private SpawnRocks _rockSpawner;
    public override void Start()
    {
        _rb = GetComponent<Rigidbody>();
        base.Start();

        //Accesses collider's OnColiisonEnter and deals the damage to the enemies
        _externalCollider.GetComponent<ExternalCollider>().CollisionEnter += (Collision collision) =>
        {
            if (collision.gameObject.GetComponent<EnemyController>() && _inProgress)
            {
                if (collision.gameObject.GetComponent<EnemyController>().TakeDamage(_damage, transform.up))
                {
                    _controller.Kill(collision.gameObject);
                }
                GetComponent<RammyVFX>().Ab5Attack(collision.gameObject, (_dashDestination - _dashStart).normalized);
                _controller.AddScreenShake(1f);
            }
        };
    }
    override public void Update()
    {
        base.Update();

        //Moves the collider forward so it can deal damage
        if (_inProgress)
        {
            _externalCollider.transform.position = Vector3.Lerp(_dashStart, _dashDestination, ((Time.time - _startTimeDash) / _moveDuration));
        }
    }
    override public void Activate()
    {
        StartCoroutine(WaitForJumpAnim());
    }

    IEnumerator WaitForJumpAnim()
    {
        //Waits half a second for the jump animation
        yield return new WaitForSeconds(0.5f);
        //To-do: Shake the camera in animation events so it would shake just when it lands

        Vector3 worldPosition = Vector3.zero;
        Plane plane = new Plane(Vector3.up, -20);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }

        transform.LookAt(worldPosition);

        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        _controller.BlockPlayerMovment();

        //Sets the external collider active, sets its position to be just in front of Rammy
        _externalCollider.SetActive(true);
        _externalCollider.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        _externalCollider.transform.SetParent(null);

        // Spawns rocks!
        if (_rockSpawner)
        {
            //_rockSpawner.gameObject.transform.parent = null;
            // _rockSpawner.transform.rotation = Quaternion.LookRotation(this.transform.rotation.eulerAngles, Vector3.forward);
            _rockSpawner.InitiateRocks();
            //_rockSpawner.gameObject.transform.parent = this.transform;
        }

        // Checking if the force field would end up in an object while dashing and shortening dash if thats the case
        _dashDestination = _externalCollider.transform.position + transform.up * _pushDistance;
        RaycastHit hit;
        if (Physics.Raycast(_externalCollider.transform.position, transform.up, out hit, _pushDistance))
        {
            RaycastHit hit2;
            if (Physics.Raycast(_dashDestination + Vector3.up * 100, Vector3.down, out hit2, 105) && hit.transform == hit2.transform)
            {
                _dashDestination = hit.point;
            }
        }

        //Sets start position and time for the lerp calculations
        _dashStart = _externalCollider.transform.position;
        _dashStartRot = _externalCollider.transform.rotation;
        _startTimeDash = Time.time;
        _inProgress = true;

        _controller.UnBlockPlayerMovement();
        yield return new WaitForSeconds(_moveDuration);

        //Stops the abiliy
        _inProgress = false;
        _externalCollider.transform.SetParent(transform);
        _externalCollider.transform.localPosition = new Vector3(0f, 0f, 0f);
        _externalCollider.SetActive(false);
    }
}
