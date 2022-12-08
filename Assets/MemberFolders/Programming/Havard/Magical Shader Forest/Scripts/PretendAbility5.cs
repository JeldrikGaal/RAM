using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PretendAbility5 : MonoBehaviour
{
    [SerializeField] private SpawnRocks _rockSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Spawns rocks!
            if (_rockSpawner)
            {
                //_rockSpawner.gameObject.transform.parent = null;
                //_rockSpawner.transform.rotation = Quaternion.LookRotation(this.transform.rotation.eulerAngles, Vector3.forward);
                _rockSpawner.InitiateRocks();
                //_rockSpawner.gameObject.transform.parent = this.transform;
            }
        }
    }
}
