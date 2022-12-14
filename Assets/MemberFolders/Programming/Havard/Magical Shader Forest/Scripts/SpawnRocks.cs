using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRocks : MonoBehaviour
{

    [SerializeField] private float _length = 5;
    [SerializeField] private Vector3 _currentPos;

    [SerializeField] private float _minWidth = 0.5f;
    [SerializeField] private float _maxWidth = 0.5f;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight = 3;
    [SerializeField] private float _heightRandom = 0.2f;

    private bool _moving = false;

    private float _timer = 0;
    [SerializeField] private float _maxTime = 1f;
    [SerializeField] private float _timeBetweenSpawn = 0.1f;
    [SerializeField] private int _amountPerSpawn = 1;

    [SerializeField] private RuntimeAnimatorController _rockAnim;

    private GameObject _rockParent;

    [SerializeField] private bool _testCube = false;
    [SerializeField] private GameObject _rock;
    [SerializeField] private Vector3 _rockBaseSize = new Vector3(1, 1, 1);

    [SerializeField] private GameObject _splatEffects;
    [SerializeField] private Vector3 _splatSizes = new Vector3(1, 1, 1);

    [SerializeField] private GameObject _particle;
    [SerializeField] private Vector3 _particleSize = new Vector3(1, 1, 1);

    [SerializeField] private GameObject _dust;
    [SerializeField] private GameObject _dustKeeper;
    [SerializeField] private float _dustSize = 1;

    [SerializeField] private bool _360 = false;

    // Start is called before the first frame update
    void Start()
    {
        _rockParent = new GameObject("Rock Keeper");
    }

    public void InitiateRocks()
    {
        _moving = true;
        InvokeRepeating("SpawnRock", _timeBetweenSpawn, _timeBetweenSpawn);
        _rockParent.transform.position = this.transform.position;
        _rockParent.transform.rotation = this.transform.rotation;
        if (_360 && _dust && _dustKeeper)
        {
            _dustKeeper.transform.localScale = new Vector3(_dustSize, 1, _dustSize);
            _dust.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // When starting the rocks:
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    InitiateRocks();
        //}
        // During the rock effect:
        if (_moving && _timer <= _maxTime)
        {
            _rockParent.transform.parent = this.transform;
            _currentPos = Vector3.Lerp(_rockParent.transform.localPosition, new Vector3(_length + _rockParent.transform.localPosition.x, _rockParent.transform.localPosition.y, _rockParent.transform.localPosition.z), _timer / _maxTime);
            _rockParent.transform.parent = null;
            _timer += Time.deltaTime;
        }
        // After the rock effect:
        else if (_moving && _timer > _maxTime)
        {
            _moving = false;
            _timer = 0;
            CancelInvoke();
            Invoke("DelayEnd", 1f);
        }
    }

    public void DelayEnd()
    {
        if (_dust)
        {
            _dust.SetActive(false);
        }
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public void SpawnRock()
    {
        for (int i = 0; i < _amountPerSpawn; i++)
        {

            // Set a height within height that gets the time difference:
            float height = map(_timer, 0, _maxTime, _minHeight, _maxHeight);
            height = Random.Range(height - _heightRandom, height + _heightRandom);

            // Set a width within height that gets the time difference:
            float width = map(_timer, 0, _maxTime, _minWidth, _maxWidth);

            // Spawn cube parent and sets its transform to be what we want,
            GameObject cubeParent = new GameObject();
            cubeParent.transform.parent = _rockParent.transform;
            cubeParent.transform.rotation = _rockParent.transform.rotation;
            cubeParent.transform.localPosition = new Vector3(_currentPos.x, _currentPos.y, Random.Range(-width, width) + _currentPos.z);
            cubeParent.transform.localScale = new Vector3(1 * _rockBaseSize.x, height * _rockBaseSize.y, 1 * _rockBaseSize.z);

            if (_testCube)
            {
                // Spawn cube and set its transform to be what we want,
                GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newCube.transform.parent = cubeParent.transform;
                newCube.transform.localPosition = new Vector3(0, 0, 0);
                // Add animation to cube
                var cubeAnim = newCube.AddComponent<Animator>();
                cubeAnim.runtimeAnimatorController = _rockAnim;
            }
            else
            {
                // Spawn rock and set its transform to be what we want,
                GameObject newRock = Instantiate(_rock, cubeParent.transform);
                newRock.transform.localPosition = new Vector3(0, 0, 0);
                newRock.transform.localRotation = Quaternion.Euler(0, 0, 0);
                // Add animation to cube
                var rockAnim = newRock.AddComponent<Animator>();
                rockAnim.runtimeAnimatorController = _rockAnim;
            }
            if (_splatEffects)
            {
                var splatty = Instantiate(_splatEffects, cubeParent.transform);
                splatty.transform.localPosition = new Vector3(0, 0.01f, 0);
                splatty.transform.localRotation = Quaternion.Euler(90, 0, Random.Range(0, 360));
                splatty.transform.localScale = _splatSizes * width;
                splatty.transform.position += new Vector3(0, 0.5f, 0);
            }
            if (_particle)
            {
                var party = Instantiate(_particle, cubeParent.transform);
                party.transform.localPosition = new Vector3(0, 0, 0);
                party.transform.localScale = _particleSize * width;
                party.transform.position += new Vector3(0, 0.5f, 0);
            }
            if (_360)
            {
                foreach (Transform child in _rockParent.transform)
                {
                    child.parent = null;
                }
                _rockParent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            }
            // Delete cube after animation is done
            Destroy(cubeParent, _rockAnim.animationClips[0].length);
        }
    }
}
