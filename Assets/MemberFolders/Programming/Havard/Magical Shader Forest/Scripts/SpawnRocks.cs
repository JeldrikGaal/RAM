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

    [SerializeField] private RuntimeAnimatorController _rockAnim;

    private GameObject _rockParent;

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
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            _moving = true;
            InvokeRepeating("SpawnRock", _timeBetweenSpawn, _timeBetweenSpawn);
            _rockParent.transform.position = this.transform.position;
            _rockParent.transform.rotation = this.transform.rotation;
        }

        if(_moving && _timer <= _maxTime)
        {
            _rockParent.transform.parent = this.transform;
            _currentPos = Vector3.Lerp(_rockParent.transform.localPosition, new Vector3(_length + _rockParent.transform.localPosition.x, _rockParent.transform.localPosition.y, _rockParent.transform.localPosition.z), _timer/ _maxTime);
            _rockParent.transform.parent = null;
            _timer += Time.deltaTime;
        } else if(_moving && _timer > _maxTime)
        {
            _moving = false;
            _timer = 0;
            CancelInvoke();
        }
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public void SpawnRock()
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
        cubeParent.transform.localScale = new Vector3(1, height, 1);
        // Spawn cube and set its transform to be what we want,
        GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newCube.transform.parent = cubeParent.transform;
        newCube.transform.localPosition = new Vector3(0, 0, 0);
        // Add animation to cube
        var cubeAnim = newCube.AddComponent<Animator>();
        cubeAnim.runtimeAnimatorController = _rockAnim;
        // Remove the cube collider
        Destroy(newCube.GetComponent<BoxCollider>());
        // Delete cube after animation is done
        Destroy(cubeParent, _rockAnim.animationClips[0].length);
    }

}
