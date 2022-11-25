using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int AmountOfEnemiesToSpawn;

    public float WaitDurationBetweenEnemies;

    public GameObject Enemy;

    public Transform SpawnPoint;


    private IEnumerator SpawnEnemies(int amount, float waitDuration)
    {
        for (int i = 0; i < amount; i++)
        {
            var enemy = Instantiate(Enemy, SpawnPoint.position, Quaternion.identity);

            AmountOfEnemiesToSpawn--;

            yield return new WaitForSeconds(waitDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(SpawnEnemies(AmountOfEnemiesToSpawn, WaitDurationBetweenEnemies));
            Destroy(GetComponent<Collider>());
        }
    }
}
