using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _bearPrefab;
    [SerializeField] private GameObject _wolfPrefab;
    [SerializeField] private GameObject _hawkPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SpawnEnemy(_bearPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            SpawnEnemy(_wolfPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            SpawnEnemy(_hawkPrefab);
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        Vector3 worldPosition = Vector3.zero;
        Plane plane = new Plane(Vector3.up, -20);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }

        Instantiate(enemy, new Vector3(worldPosition.x, worldPosition.y + 1, worldPosition.z), Quaternion.identity);
    }
}
