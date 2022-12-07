using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndDoorLvL1 : MonoBehaviour
{
    public List<GameObject> AllEnemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player"))
        {
            if (GameObject.FindGameObjectsWithTag("wolf").Length == 0)
            {
                Debug.Log("level completed");
            }
        }
    }
}
