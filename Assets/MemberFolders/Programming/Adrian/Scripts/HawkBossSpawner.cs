using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkBossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEnemy()
    {
        Instantiate(_enemies[Random.Range(0, _enemies.Length)], transform.position + (new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.x) * 4f), Quaternion.identity);
    }

}
