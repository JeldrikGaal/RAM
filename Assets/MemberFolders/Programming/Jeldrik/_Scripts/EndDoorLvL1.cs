using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoorLvL1 : MonoBehaviour
{
    public List<GameObject> AllEnemies = new List<GameObject>();

    public bool level2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (level2 && Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(3);
        }
        if (!level2 && Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player"))
        {
            if (GameObject.FindGameObjectsWithTag("wolf").Length == 0  )
            {
                if (!level2)
                {
                    SceneManager.LoadScene(2);
                    Debug.Log("level completed");
                }
                else
                {
                    SceneManager.LoadScene(3);
                    Debug.Log("level completed");
                }
               
            }

        }
    }
}
