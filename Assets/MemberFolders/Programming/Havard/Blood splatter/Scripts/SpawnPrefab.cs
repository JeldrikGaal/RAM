using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float destroyTime = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.FindGameObjectsWithTag("Player");
        if (Input.GetKeyDown(KeyCode.Return)){
            var temp = Instantiate(prefab, transform.position, transform.rotation);
            Destroy(temp, destroyTime);
        }
    }
}
