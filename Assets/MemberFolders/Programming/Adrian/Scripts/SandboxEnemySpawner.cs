using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Vector3 worldPosition = Vector3.zero;
            Plane plane = new Plane(Vector3.up, 0);

            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance);
            }

            Instantiate(_enemyPrefab, new Vector3(worldPosition.x, worldPosition.y + 1, worldPosition.z), Quaternion.identity);
        }
    }
}
