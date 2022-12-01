using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HawkBossManager : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;

    // [SerializeField] private HawkBossAttackPhaseOne _state;


    [SerializeField] private GameObject _egg;


    [SerializeField] private Transform _shootPoint;

    [SerializeField] private float _shootSpeed;

    [SerializeField] private float _sprayRotationSpeed;
    [SerializeField] private float _finalRotation;
    private float _realFinalRotation;
    private float _sprayTimer;
    [SerializeField] private float _sprayShotDelay;
    [SerializeField] private bool _spraying;

    [SerializeField] private GameObject[] _spawnPoints;
    [SerializeField] private int _enemyWaveAmount;

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
        _phaseOne = true;
        _stageOne = true;

        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
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
                egg.GetComponent<Rigidbody>().AddForce(transform.forward * _shootSpeed);
                Destroy(egg, 6);

                _sprayTimer += _sprayShotDelay;
            }

        }
    }

    private void ChangeToStageOne()
    {
        if (_controller.Health < 10)
        {
            _controller.Health = _controller.MaxHealth;
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
        if (_controller.Health < 90)
        {
            _stageOne = false;
            _stageTwo = true;
        }
    }

    private void ChangeToStageThree()
    {
        if (_controller.Health < 40)
        {
            _stageTwo = false;
            _stageThree = true;
        }
    }

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
        if (_flee)
        {
            _controller.MoveInput = -(_player.transform.position - transform.position).normalized;
        }
    }

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
        print("Horizontal Spray");
        yield return new WaitForSeconds(_realFinalRotation / _sprayRotationSpeed);
        _spraying = false;
        _attacking = false;

    }

    private IEnumerator MinionSwarm()
    {
        print("Minion Swarm");
        for (int i = 0; i < _enemyWaveAmount; i++)
        {
            for (int j = 0; j < _spawnPoints.Length; j++)
            {
                _spawnPoints[j].GetComponent<HawkBossSpawner>().SpawnEnemy();
            }
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
        _attacking = false;
    }

    private IEnumerator SuperClawMelee()
    {
        print("Super Claw Melee");
        yield return new WaitForSeconds(1);

    }

    private IEnumerator ToggleFlee()
    {
        if (!_phaseOne && !_stageOne)
        {
            yield return new WaitForSeconds(1);
        }
    }
}
