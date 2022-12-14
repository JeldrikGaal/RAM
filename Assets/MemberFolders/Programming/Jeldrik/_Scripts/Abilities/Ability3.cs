using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability3 : Abilities
{
    [SerializeField] private float _stunDuration;
    [SerializeField] private float _pullForce;
    [SerializeField] private int _points;
    [SerializeField] private float _distanceToPoints;
    [SerializeField] private float _restrictedTime;

    public bool Upgraded;

    private float _baseSpeed;

    [SerializeField] private GameObject _testObject;
    [SerializeField] private GameObject _smokeParticle;

    // public Dictionary<Vector3, bool> PointList = new Dictionary<Vector3, bool>();

    public Dictionary<Vector3, bool> Points = new Dictionary<Vector3, bool>();

    public List<GameObject> EnemyList = new List<GameObject>();

    public override void Start()
    {
        base.Start();
    }
    override public void Update()
    {
        base.Update();
    }
    override public void Activate()
    {
        // Sets particle to happen
        _smokeParticle.SetActive(true);

        // Clear the list of enemies
        EnemyList.Clear();

        // Clear the list of points
        Points.Clear();

        // Gets an array of all the colliders within a radius around a point
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _upgraded ? Stats.USplashRadius : Stats.SplashRadius);

        print(Stats.SplashRadius);

        // Local transform list
        List<Transform> enemyTransforms = new List<Transform>();


        _audio.Play();

        // Checks all the colliders and adds the ones with the enemy tag to the list
        foreach (Collider col in hitColliders)
        {
            if (col.tag == "enemy" || col.tag == "wolf")
            {
                if (!enemyTransforms.Contains(col.transform))
                {
                    enemyTransforms.Add(col.transform);
                }

                if (col.GetComponent<HawkBossManager>() != null)
                {
                    if (col.GetComponent<HawkBossManager>().Fleeing || col.GetComponent<HawkBossManager>().RisingFlee || col.GetComponent<HawkBossManager>().LoweringFlee || col.GetComponent<HawkBossManager>().Rising || col.GetComponent<HawkBossManager>().Crashing || col.GetComponent<HawkBossManager>().MeleeAttack)
                    {
                        enemyTransforms.Remove(col.transform);
                    }
                }
            }
        }

        // Gets the 8 closes enemies to the player and adds them to a list
        EnemyList = GetClosestEnemies(enemyTransforms);

        _points = EnemyList.Count;


        // Finds an amount of points equally spaced around a circle
        for (int i = 0; i < _points; i++)
        {
            #region IStoleThis
            // Distance around the circle
            var radians = 2 * Mathf.PI / _points * i;

            // Get the vector direction
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            // Get the spawn position
            var spawnPos = transform.position + spawnDir * _distanceToPoints;
            #endregion

            // Spawns in objects to show the different points (Only for testing purposes)
            // var marker = Instantiate(_testObject, spawnPos, Quaternion.identity);

            // Destroys the marker after one second
            // Destroy(marker, 1);

            // Adds the point and a true value to the dictionary
            Points.Add(spawnPos, true);
        }



        // Makes an array of all the keys in the dictionary
        Vector3[] keys = Points.Keys.ToArray();

        // For each closest enemy
        foreach (GameObject enemy in EnemyList)
        {
            // Set default values
            float dist = Mathf.Infinity;
            Vector3 closestPoint = Vector3.positiveInfinity;

            // Loops over all the keys (points on the circle)
            foreach (Vector3 key in keys)
            {
                // Check if distance is closer than last point checked and that the point is not already occupied
                if (Vector3.Distance(enemy.transform.position, key) < dist && Points[key])
                {
                    // If it is save the distance and point
                    dist = Vector3.Distance(enemy.transform.position, key);
                    closestPoint = key;
                }
            }

            // Sets the point to unavailable
            Points[closestPoint] = false;


            if (enemy.GetComponent<EnemyController>() != null)
            {
                if (enemy.GetComponent<EnemyController>().Health > 0)
                {
                    // Makes the enemy take damage
                    if (enemy.GetComponent<EnemyController>().TakeDamage((_upgraded ? Stats.UDmg : Stats.Dmg) * _controller.Damage, Vector3.up))
                    {
                        _controller.Kill(enemy);
                    }
                    print("Dealt damage");
                }

                // enemy.GetComponent<EnemyController>().StunDuration = _stunDuration;
                // enemy.GetComponent<EnemyController>().Stun();

                enemy.GetComponent<EnemyController>().PullPoint = closestPoint;
                enemy.GetComponent<EnemyController>().Pulled = true;
                StartCoroutine(enemy.GetComponent<EnemyController>().Stun(2f));

                // Tells the VFX script to do something
                GetComponent<RammyVFX>().Ab3Attack(enemy, closestPoint);

                // If the ability is upgraded
                if (Upgraded)
                {
                    // Pass the stun duration and the stun bool to the enemy
                    enemy.GetComponent<EnemyController>().StunDuration = _stunDuration;
                    enemy.GetComponent<EnemyController>().Stun();
                }
            }
        }

        // Start the coroutine that stops the player from moving
        StartCoroutine(RestrictPlayerMovement(_restrictedTime));
        // Debug.Log("Ability 3");
    }

    // Returns a list of the closest transforms from a list
    List<GameObject> GetClosestEnemies(List<Transform> enemies)
    {
        // Local list for the closest objects
        List<GameObject> closestList = new List<GameObject>();

        var enemyCount = Mathf.Clamp(enemies.Count, 0, 8);

        for (int i = 0; i < enemyCount; i++)
        {
            // Sets base values
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            // Loops through all the transforms in the given list
            foreach (Transform potentialTarget in enemies)
            {
                // Gets the direction between the target and player
                Vector3 directionToTarget = potentialTarget.position - currentPosition;

                // Gets the squared length of the vector (Distance) Apparently this is better than Vector3.distance because it does the same thing
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                // If the distance is less than the current saved closes distance
                if (dSqrToTarget < closestDistanceSqr)
                {
                    // Save the new closest distance and point
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }

            // Add the closest target to the list
            closestList.Add(bestTarget.gameObject);

            // Removes the target from the given list so it can't be chosen again
            enemies.Remove(bestTarget);
        }

        // Returns the list of the closes enemies
        return closestList;
    }

    private IEnumerator Windup(float duration)
    {
        yield return new WaitForSeconds(duration);

    }

    private IEnumerator RestrictPlayerMovement(float duration)
    {
        _controller.BlockPlayerMovment();

        // Wait a bit
        yield return new WaitForSeconds(duration);

        // Sets the movespeed to the default
        _controller.UnBlockPlayerMovement();
    }
}
