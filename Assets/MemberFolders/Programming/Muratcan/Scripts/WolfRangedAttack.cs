using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfRangedAttack : MonoBehaviour
{
    int _rnd;
    bool _onTheWay = false;
    bool _onTheWayBack = false;
    bool _inProgress = false;
    Vector3 _startPos;
    Vector3 _targetPos;
    float _startTimeThrow;
    public float throwDuration = 5f;
    [SerializeField] Collider[] _hitColliders = new Collider[50];
    [SerializeField] List<Collider> _wolfMeleeColliders = new List<Collider>();
    [SerializeField] float _range = 3f;
    [SerializeField] GameObject _boomerang;
    [SerializeField] GameObject _target;
    [SerializeField] int _ammo = 1;
    [SerializeField] float _shootSpeed = 50f;
    public float damage = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_onTheWay == true && ((Time.time - _startTimeThrow) / throwDuration) < 1f)
        {
            print("p1");
            _boomerang.transform.position = Vector3.Lerp(_startPos, _targetPos, ((Time.time - _startTimeThrow) / throwDuration));
        }
        else if (_onTheWay == true && ((Time.time - _startTimeThrow) / throwDuration) >= 1f)
        {
            print("p2");
            _onTheWay = false;
            _onTheWayBack = true;
            _startTimeThrow = Time.time;
        }
        else if (_onTheWayBack == true && ((Time.time - _startTimeThrow) / throwDuration) < 1f)
        {
            print("p3");
            _boomerang.transform.position = Vector3.Lerp(_targetPos, _startPos, ((Time.time - _startTimeThrow) / throwDuration));
        }
        else if (_onTheWayBack == true && ((Time.time - _startTimeThrow) / throwDuration) >= 1f)
        {
            print("p4");
            _onTheWayBack = false;
            _boomerang.transform.SetParent(transform);
            _boomerang.transform.localPosition = new Vector3(0f, 0f, 0f);
            _boomerang.SetActive(false);
            _ammo++;
        }

        if (_inProgress && ((Time.time - _startTimeThrow) / throwDuration) > 0f)
        {
            
            _wolfMeleeColliders[_rnd].gameObject.transform.position = Vector3.Lerp(_startPos, _targetPos, ((Time.time - _startTimeThrow) / throwDuration));
        }
    }
    
    public void ThrowBoomerang(GameObject player)
    {
        _ammo--;
        _boomerang.SetActive(true);
        _boomerang.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        _boomerang.transform.SetParent(null);
        _startPos = _boomerang.transform.position;
        //_targetPos = player.transform.position;
        _targetPos = _target.transform.position;
        //_targetPos.ro = Quaternion.LookRotation((_boomerang.transform.position - player.transform.position).normalized);
        print(_targetPos);
        _startTimeThrow = Time.time;
        _onTheWay = true;
    }

    public void ThrowWolf(GameObject player)
    {
        _rnd = Random.Range(0, _wolfMeleeColliders.Count);
        _startPos = _wolfMeleeColliders[_rnd].gameObject.transform.position;
        _targetPos = player.transform.position;
        _startTimeThrow = Time.time;
        _inProgress = true;
        //Turn off the wolf's ai when it is in the air
    }

    public bool CheckNearbyWolfs()
    {
        _wolfMeleeColliders.Clear();
        //Creates a sphere and takes data's of everything in there
        _hitColliders = Physics.OverlapSphere(transform.position, _range);

        //Looks at everything physics catched and does the ability to those who has the enemy script.
        foreach (var item in _hitColliders)
        {
            if (item.transform != null && TagManager.HasTag(item.gameObject, "wolfMelee"))
            {
                _wolfMeleeColliders.Add(item);
            }
        }

        if (_wolfMeleeColliders.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
