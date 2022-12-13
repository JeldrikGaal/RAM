using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private bool _spawnMultipleTimes;
    public int AmountOfEnemiesToSpawn;

    public float WaitDurationBetweenEnemies;

    public GameObject Enemy;

    public Transform SpawnPoint;

    private StatManager _statManager;


    private void Start()
    {
        _statManager = FindObjectOfType<StatManager>();
    }

    private IEnumerator SpawnEnemies(int amount, float waitDuration)
    {
        for (int i = 0; i < amount; i++)
        {
            // _statManager.MaxKills++;

            var enemy = Instantiate(Enemy, SpawnPoint.position + (new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.x) * 6f), Quaternion.identity);

            AmountOfEnemiesToSpawn--;

            yield return new WaitForSeconds(waitDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(SpawnEnemies(5, WaitDurationBetweenEnemies));
            if (!_spawnMultipleTimes)
            {
                Destroy(GetComponent<Collider>());
            }
        }
    }
}
