using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HawkBossManager : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;
    [SerializeField] private EnemyController _testingScript;

    [SerializeField] private GameObject _model;

    // [SerializeField] private HawkBossAttackPhaseOne _state;

    [Header("Basic Attack")]
    [SerializeField] private GameObject _egg;


    [SerializeField] private Transform _shootPoint;

    [SerializeField] private float _shootSpeed;

    [Header("Horizontal Spray")]
    [SerializeField] private float _sprayRotationSpeed;
    [SerializeField] private float _finalRotation;
    private float _realFinalRotation;
    private float _sprayTimer;
    [SerializeField] private float _sprayShotDelay;
    [SerializeField] private bool _spraying;

    [Header("Minion Swarm")]
    [SerializeField] private GameObject[] _spawnPoints;
    [SerializeField] private int _enemyWaveAmount;

    [Header("Super Claw Melee")]
    [SerializeField] private bool _meleeAttack;
    private bool _rising;
    private bool _crashing;
    private Vector3 _modelStartPos;
    [SerializeField] private AnimationCurve _modelPosCurve;
    [SerializeField] private float _flightTime;
    [SerializeField] private float _riseTime;
    [SerializeField] private float _flightHeight;
    private float _flightTimer;
    private Vector3 _crashPos;
    private float _crashTimer;
    [SerializeField] private GameObject _crashPath;
    [SerializeField] private float _slowDownDistance;
    private bool _slowedDown;
    private bool _insideSlowRange;
    [SerializeField] private GameObject _damageArea;

    private Dictionary<string, int> _weightedAttacks = new Dictionary<string, int>()
    {
        {"Basic Attack", 90},
        {"Horizontal Spray", 10},
        {"Minion Swarm", 0},
        {"Super Claw Melee", 0}
    };

    private GameObject _player;

    private Rigidbody _rb;

    private Vector3 _moveInput;

    private bool _canAttack;

    // [SerializeField] private List<WeightedAttacks> _weightedAttacks;

    // [System.Serializable]
    // public struct WeightedAttacks
    // {
    //     public string Attack;
    //     public int Weight;
    // }

    [Header("Flee")]
    [SerializeField] private GameObject[] _fleePoints;
    [SerializeField] private Vector3 _selectedFleePoint;
    public float DamageTakenRecently;
    [SerializeField] private bool _risingFlee;



    [Header("States")]
    [SerializeField] private bool _phaseOne;
    [SerializeField] private bool _phaseTwo;
    [SerializeField] private bool _phaseThree;

    [SerializeField] private bool _stageOne;
    [SerializeField] private bool _stageTwo;
    [SerializeField] private bool _stageThree;

    [SerializeField] private bool _attacking;
    [SerializeField] private bool _flee;
    [SerializeField] private bool _chase;

    [SerializeField] private float _fleeTimer;



    // Start is called before the first frame update
    void Start()
    {
        _phaseThree = true;
        _stageThree = true;

        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();

        GenerateCurve();
        // _state.ShootSpeed = _shootSpeed;
        // _state.ShootPoint = _shootPoint;
        // _state.Egg = _egg;
    }

    // Update is called once per frame
    void Update()
    {
        if (_phaseOne)
        {
            #region PhaseOne
            if (_stageOne)
            {
                _weightedAttacks["Basic Attack"] = 90;
                _weightedAttacks["Horizontal Spray"] = 10;

                ChangeToStageTwo();
            }
            else if (_stageTwo)
            {
                _weightedAttacks["Basic Attack"] = 65;
                _weightedAttacks["Horizontal Spray"] = 35;

                ChangeToStageThree();
            }
            else if (_stageThree)
            {
                _weightedAttacks["Basic Attack"] = 35;
                _weightedAttacks["Horizontal Spray"] = 65;

                ChangeToStageOne();
            }
            #endregion
        }
        else if (_phaseTwo)
        {
            #region PhaseTwo
            if (_stageOne)
            {
                _weightedAttacks["Basic Attack"] = 70;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 20;

                ChangeToStageTwo();
            }
            else if (_stageTwo)
            {
                _weightedAttacks["Basic Attack"] = 45;
                _weightedAttacks["Horizontal Spray"] = 15;
                _weightedAttacks["Minion Swarm"] = 40;

                ChangeToStageThree();
            }
            else if (_stageThree)
            {
                _weightedAttacks["Basic Attack"] = 25;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 65;

                ChangeToStageOne();
            }
            #endregion
        }
        else if (_phaseThree)
        {
            #region PhaseThree
            if (_stageOne)
            {
                _weightedAttacks["Basic Attack"] = 55;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 10;
                _weightedAttacks["Super Claw Melee"] = 25;

                ChangeToStageTwo();
            }
            else if (_stageTwo)
            {
                _weightedAttacks["Basic Attack"] = 25;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 30;
                _weightedAttacks["Super Claw Melee"] = 35;

                ChangeToStageThree();
            }
            else if (_stageThree)
            {
                _weightedAttacks["Basic Attack"] = 10;
                _weightedAttacks["Horizontal Spray"] = 10;
                _weightedAttacks["Minion Swarm"] = 25;
                _weightedAttacks["Super Claw Melee"] = 65;
            }
            #endregion
        }

        if (!_flee && !_spraying)
        {
            transform.LookAt(_player.transform);
        }

        Chase();
        Flee();

        if (_chase || _flee)
        {
            _canAttack = false;
        }
        else
        {
            _canAttack = true;
        }

        if (!_chase && !_flee)
        {
            _controller.MoveInput = Vector3.zero;
        }

        #region UpdateAttacks
        Attack();

        if (_spraying)
        {
            // Rotates the pivot point
            transform.Rotate(new Vector3(0, _sprayRotationSpeed * Time.deltaTime, 0));

            if (_sprayTimer > 0)
            {
                _sprayTimer -= Time.deltaTime;
            }

            if (_sprayTimer < 0)
            {
                var egg = Instantiate(_egg, _shootPoint.position, Quaternion.identity);
                egg.GetComponent<Rigidbody>().AddForce(transform.forward * (_shootSpeed * 1.5f));
                Destroy(egg, 6);

                _sprayTimer += _sprayShotDelay;
            }
        }

        if (_rising || _risingFlee)
        {
            // Increased the jump timer
            _flightTimer += Time.deltaTime;

            // Sets the y position based on the animation curve
            _model.transform.position = new Vector3(transform.position.x, _modelStartPos.y + _modelPosCurve.Evaluate(_flightTimer), transform.position.z);

            // Sets the layer so it doesn't collide with the player
            gameObject.layer = 23;
        }

        if (_crashing)
        {
            float timeToReachTarget = 0.3f;
            if (_crashTimer < 1)
            {
                _crashTimer += timeToReachTarget * Time.deltaTime;
            }

            if (_crashTimer > 0.13f)
            {
                _crashing = false;
                gameObject.layer = 0;
                var damageArea = Instantiate(_damageArea, transform.position, Quaternion.identity);
                Destroy(damageArea, 0.5f);
                StartCoroutine(WaitAfterMeleeAttack());
            }

            if (Vector3.Distance(transform.position, _crashPos) < _slowDownDistance && !_slowedDown)
            {
                _insideSlowRange = true;
            }

            if (_insideSlowRange)
            {
                _crashTimer = 0;
                _crashPos = _player.transform.position;
                StartCoroutine(SlowDownWait());
            }

            transform.position = Vector3.Lerp(transform.position, _crashPos, _crashTimer);
            _model.transform.localPosition = Vector3.Lerp(_model.transform.localPosition, new Vector3(0, 0, 0), _crashTimer);
        }

        if (_flightTimer > _modelPosCurve[_modelPosCurve.length - 1].time)
        {
            if (!_flee)
            {
                _meleeAttack = false;
                _flightTimer = 0;
                _crashing = true;
                _crashPos = _player.transform.position;
                _rising = false;
                print("This should not be happening");
            }
        }

        #endregion
    }

    #region StageChanges
    private void ChangeToStageOne()
    {
        if (_testingScript.Health < 10)
        {
            transform.GetChild(0).GetComponent<HealthBar>().UpdateHealthBar((_controller.MaxHealth - _testingScript.Health) / 100);
            print(_controller.MaxHealth - _testingScript.Health);
            _testingScript.Health = _controller.MaxHealth;
            _stageThree = false;
            _stageOne = true;

            if (_phaseOne)
            {
                _phaseOne = false;
                _phaseTwo = true;
            }
            else if (_phaseTwo)
            {
                _phaseTwo = false;
                _phaseThree = true;
            }
        }
    }

    private void ChangeToStageTwo()
    {
        if (_testingScript.Health < 90)
        {
            _stageOne = false;
            _stageTwo = true;
        }
    }

    private void ChangeToStageThree()
    {
        if (_testingScript.Health < 40)
        {
            _stageTwo = false;
            _stageThree = true;
        }
    }

    #endregion

    private int GetWeightedValue()
    {
        // Gets all the different weights and adds them to a sepparate array
        int[] weights = _weightedAttacks.Values.ToArray();

        // Gets a random weight
        int randomWeight = Random.Range(0, weights.Sum());

        // Makes an int for the accumulated weights
        int accumulation = 0;

        // Makes a new array for the running total of the weights
        int[] runningTotal = new int[weights.Length];

        // Loops through and adds together all the weights, adding the current running total to the runningtotal array
        for (int i = 0; i < weights.Length; i++)
        {
            // Adds the current weight to the total
            accumulation += weights[i];

            // Adds the current running total to the array
            runningTotal[i] = accumulation;
        }

        // Makes new values for the min and max value
        var low = 0;
        var high = weights.Length;

        // As long as low is less than high
        while (low < high)
        {
            // Mid is the halfway point between low and high
            var mid = (int)((low + high) / 2);

            // Sets distance to the halfway point
            var distance = runningTotal[mid];

            // If the mid point value is lower than the weight we want
            if (distance < randomWeight)
            {
                // Set low to a point over mid
                low = mid + 1;
            }
            else if (distance > randomWeight)
            {
                // If the midpoint value is higher than the point we want
                high = mid;
            }
            else
            {
                // Return mid if it is neither higher nor lower than the mid point
                return mid;
            }
        }
        // Return low when low no longer is less than high
        return low;
    }

    #region  States
    private void Attack()
    {
        if (!_attacking && _canAttack)
        {
            _attacking = true;
            string[] attacks = _weightedAttacks.Keys.ToArray();

            var attack = attacks[GetWeightedValue()];

            switch (attack)
            {
                case "Basic Attack":
                    StartCoroutine(BasicAttack());
                    break;
                case "Horizontal Spray":
                    StartCoroutine(HorizontalSpray());
                    break;
                case "Minion Swarm":
                    StartCoroutine(MinionSwarm());
                    break;
                case "Super Claw Melee":
                    StartCoroutine(SuperClawMelee());
                    break;
                default:
                    break;
            }
        }
    }

    private void Chase()
    {
        int distance;
        if (_phaseOne)
        {
            distance = 15;
        }
        else
        {
            distance = 10;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) > distance && !_attacking && !_flee)
        {
            _controller.MoveInput = (_player.transform.position - transform.position).normalized;
            // print(_rb.velocity);
            _chase = true;
        }
        else
        {
            _chase = false;
        }
    }

    private void Flee()
    {
        if (DamageTakenRecently > 30 && !_flee && !_risingFlee)
        {
            StartCoroutine(ToggleFlee());
            DamageTakenRecently = 0;
            _risingFlee = true;
        }

        if (_flightTimer > _modelPosCurve[_modelPosCurve.length - 1].time && _risingFlee)
        {
            _risingFlee = false;
            _flee = true;
        }

        if (_flee)
        {
            float timeToDestination = 0.2f;
            if (_fleeTimer < 1)
            {
                _fleeTimer += timeToDestination * Time.deltaTime;

                transform.position = Vector3.Lerp(transform.position, new Vector3(_selectedFleePoint.x, transform.position.y, _selectedFleePoint.z), _fleeTimer);
                _model.transform.position = Vector3.Lerp(_model.transform.position, new Vector3(_model.transform.position.x, _selectedFleePoint.y, _model.transform.position.z), _fleeTimer);
                Instantiate(_crashPath, _model.transform.position, Quaternion.identity);
                Instantiate(_crashPath, transform.position, Quaternion.identity);
            }
        }
    }

    #endregion

    #region  Attacks
    private IEnumerator BasicAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            var egg = Instantiate(_egg, _shootPoint.position, Quaternion.identity);
            egg.GetComponent<Rigidbody>().AddForce(transform.forward * _shootSpeed);
            Destroy(egg, 6);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2);
        _attacking = false;
    }

    private IEnumerator HorizontalSpray()
    {
        _realFinalRotation = transform.localEulerAngles.y + _finalRotation;
        _spraying = true;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - 90, transform.localEulerAngles.z);

        _sprayTimer += _sprayShotDelay;
        // print("Horizontal Spray");
        yield return new WaitForSeconds(_realFinalRotation / _sprayRotationSpeed);
        _spraying = false;

        yield return new WaitForSeconds(2);
        _attacking = false;

    }

    private IEnumerator MinionSwarm()
    {
        // print("Minion Swarm");
        for (int i = 0; i < _enemyWaveAmount; i++)
        {
            for (int j = 0; j < _spawnPoints.Length; j++)
            {
                // _spawnPoints[j].GetComponent<HawkBossSpawner>().SpawnEnemy();
            }
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
        _attacking = false;
    }

    private IEnumerator SuperClawMelee()
    {
        _modelStartPos = _model.transform.position;

        _meleeAttack = true;

        _rising = true;

        _slowedDown = false;

        _crashTimer = 0;

        GenerateCurve();

        yield return new WaitForSeconds(1);
    }

    private IEnumerator SlowDownWait()
    {
        yield return new WaitForSeconds(1);
        _slowedDown = true;
        _insideSlowRange = false;
    }

    private IEnumerator WaitAfterMeleeAttack()
    {
        yield return new WaitForSeconds(2);
        _attacking = false;
    }

    #endregion
    private IEnumerator ToggleFlee()
    {
        if (!_phaseOne && !_stageOne)
        {
            _selectedFleePoint = _fleePoints[Random.Range(0, _fleePoints.Length)].transform.position;
            _modelStartPos = _model.transform.position;
            yield return new WaitForSeconds(1);
        }
    }

    private void GenerateCurve()
    {
        // Creates a local keyframe array with 3 values
        Keyframe[] keyframes = new Keyframe[3];

        // Sets the first keyfram at 0 seconds and 0 value
        keyframes[0] = new Keyframe(0, 0);

        // Sets the second keyframe to be at half the duration of the jump and at max height
        keyframes[1] = new Keyframe(_riseTime, _flightHeight);

        // Sets the last keyframe at the full duration of the jump and the value to 0
        keyframes[2] = new Keyframe(_riseTime + _flightTime, _flightHeight);

        // keyframes[3].outWeight = 0f;

        // Sets the keyframe values to the animation curve
        _modelPosCurve = new AnimationCurve(keyframes);
    }
}
