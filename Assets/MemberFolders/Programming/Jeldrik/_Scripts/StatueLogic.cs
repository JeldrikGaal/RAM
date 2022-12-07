using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueLogic : MonoBehaviour
{
    [SerializeField] GameObject enemiesToSpawn;
    [SerializeField] GameObject spikeCluster;

    [SerializeField] WallStuffA1L4 leftDoor;
    [SerializeField] WallStuffA1L4 rightDoor;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftDoor.completed && rightDoor.completed)
        {
            enemiesToSpawn.SetActive(true);
            spikeCluster.SetActive(false);
        }
    }
}
