using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBossRangedAttack : MonoBehaviour
{
    GameObject _player;
    int _rnd;

    bool _onTheWay = false;
    bool _onTheWay2 = false;
    bool _onTheWay3 = false;

    bool _onTheWayBack = false;
    bool _onTheWayBack2 = false;
    bool _onTheWayBack3 = false;

    Vector3 _startPos;
    Vector3 _startPos2;
    Vector3 _startPos3;

    Vector3 _targetPos;
    Vector3 _targetPos2;
    Vector3 _targetPos3;

    float _startTimeThrow;
    float _startTimeThrow2;
    float _startTimeThrow3;

    [SerializeField] Animator _animator;
    public float throwDuration = 5f;

    [SerializeField] GameObject _boomerang;
    [SerializeField] GameObject _boomerang2;
    [SerializeField] GameObject _boomerang3;

    [SerializeField] GameObject _target;
    [SerializeField] float _shootSpeed = 50f;
    public float damage = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyController>().MoveInput == Vector3.zero)
        {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isRunning", false);
        }
        else
        {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isRunning", true);
        }
        //Boomerang
        if (_onTheWay == true && ((Time.time - _startTimeThrow) / throwDuration) < 1f)
        {
            if (_boomerang.activeSelf)
                _boomerang.transform.position = Vector3.Lerp(_startPos, _targetPos, ((Time.time - _startTimeThrow) / throwDuration));
        }
        else if (_onTheWay == true && ((Time.time - _startTimeThrow) / throwDuration) >= 1f)
        {
            _onTheWay = false;
            _onTheWayBack = true;
            _startTimeThrow = Time.time;
        }
        else if (_onTheWayBack == true && ((Time.time - _startTimeThrow) / throwDuration) < 1f)
        {
            if (_boomerang.activeSelf)
                _boomerang.transform.position = Vector3.Lerp(_targetPos, _startPos, ((Time.time - _startTimeThrow) / throwDuration));
        }
        else if (_onTheWayBack == true && ((Time.time - _startTimeThrow) / throwDuration) >= 1f)
        {
            _onTheWayBack = false;

            _boomerang.transform.SetParent(transform);


            _boomerang.transform.localPosition = new Vector3(0f, 0f, 0f);

            _boomerang.SetActive(false);
        }


        if (_onTheWay2 == true && ((Time.time - _startTimeThrow2) / throwDuration) < 1f)
        {
            if (_boomerang2.activeSelf)
                _boomerang2.transform.position = Vector3.Lerp(_startPos2, _targetPos2, ((Time.time - _startTimeThrow2) / throwDuration));
        }
        else if (_onTheWay2 == true && ((Time.time - _startTimeThrow2) / throwDuration) >= 1f)
        {
            _onTheWay2 = false;
            _onTheWayBack2 = true;
            _startTimeThrow2 = Time.time;
        }
        else if (_onTheWayBack2 == true && ((Time.time - _startTimeThrow2) / throwDuration) < 1f)
        {
            if (_boomerang2.activeSelf)
                _boomerang2.transform.position = Vector3.Lerp(_targetPos2, _startPos2, ((Time.time - _startTimeThrow2) / throwDuration));
        }
        else if (_onTheWayBack2 == true && ((Time.time - _startTimeThrow2) / throwDuration) >= 1f)
        {
            _onTheWayBack2 = false;

            _boomerang2.transform.SetParent(transform);

            _boomerang2.transform.localPosition = new Vector3(0f, 0f, 0f);

            _boomerang2.SetActive(false);
        }


        if (_onTheWay3 == true && ((Time.time - _startTimeThrow3) / throwDuration) < 1f)
        {
            if (_boomerang3.activeSelf)
                _boomerang3.transform.position = Vector3.Lerp(_startPos3, _targetPos3, ((Time.time - _startTimeThrow3) / throwDuration));
        }
        else if (_onTheWay3 == true && ((Time.time - _startTimeThrow3) / throwDuration) >= 1f)
        {
            _onTheWay3 = false;
            _onTheWayBack3 = true;
            _startTimeThrow3 = Time.time;
        }
        else if (_onTheWayBack3 == true && ((Time.time - _startTimeThrow3) / throwDuration) < 1f)
        {
            if (_boomerang3.activeSelf)
                _boomerang3.transform.position = Vector3.Lerp(_targetPos3, _startPos3, ((Time.time - _startTimeThrow3) / throwDuration));
        }
        else if (_onTheWayBack3 == true && ((Time.time - _startTimeThrow3) / throwDuration) >= 1f)
        {
            _onTheWayBack3 = false;

            _boomerang3.transform.SetParent(transform);

            _boomerang3.transform.localPosition = new Vector3(0f, 0f, 0f);

            _boomerang3.SetActive(false);
        }

        //if (_inProgress && ((Time.time - _startTimeThrow) / throwDuration) > 0f)
        //{

        //    _wolfMeleeColliders[_rnd].gameObject.transform.position = Vector3.Lerp(_startPos, _targetPos, ((Time.time - _startTimeThrow) / throwDuration));
        //}
    }

    public void ThrowBoomerang()
    {
        _startPos = _boomerang.transform.position;
        _boomerang.SetActive(true);
        _boomerang.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        _boomerang.transform.SetParent(null);

        _startTimeThrow = Time.time;
        _onTheWay = true;
    }

    public void ThrowBoomerang2()
    {
        _startPos2 = _boomerang2.transform.position;
        _boomerang2.SetActive(true);
        _boomerang2.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        _boomerang2.transform.SetParent(null);
        _targetPos2 = RotateAround(_targetPos, transform.position, new Vector3(0, -30, 0));

        _startTimeThrow2 = Time.time;
        _onTheWay2 = true;
    }

    public void ThrowBoomerang3()
    {
        _targetPos = _target.transform.position;
        _startPos3 = _boomerang3.transform.position;
        _boomerang3.SetActive(true);
        _boomerang3.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        _boomerang3.transform.SetParent(null);
        _targetPos3 = RotateAround(_targetPos, transform.position, new Vector3(0, 30, 0));

        _startTimeThrow3 = Time.time;
        _onTheWay3 = true;
    }

    private Vector3 RotateAround(Vector3 point, Vector3 pivot, Vector3 angle)
    {
        return Quaternion.Euler(angle) * (point - pivot) + pivot;
    }

    public void ThrowBoomerangAnimEvent(GameObject player)
    {
        _player = player;
        _animator.SetTrigger("Attack1");
    }

    public void MeleeAttackAnimEvent(GameObject player)
    {
        _player = player;
        _animator.SetTrigger("Attack2");
    }

    public void LeapAttackAnimEvent(GameObject player)
    {
        _player = player;
        _animator.SetTrigger("Attack3");
    }
}
