using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomObject : MonoBehaviour
{
    [SerializeField] private GameObject[] _arrayOfRandomItems;
    [SerializeField] private int _amountSpawned;

    public void SpawnRandomItem(Vector3 direction)
    {
        for (int i = 0; i < _amountSpawned; i++)
        {
            var rand = Random.Range(0, _arrayOfRandomItems.Length);
            var item = Instantiate(_arrayOfRandomItems[rand], transform.position + new Vector3(Random.insideUnitCircle.x, 1f, Random.insideUnitCircle.x), Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

            var randDir = new Vector3(Random.Range(0f, 2f), Random.Range(0f, 2f), Random.Range(0f, 2f));

            item.GetComponent<Rigidbody>().AddForce((direction) * 13f, ForceMode.Impulse);
        }
        // Destroy(gameObject);
    }
}
