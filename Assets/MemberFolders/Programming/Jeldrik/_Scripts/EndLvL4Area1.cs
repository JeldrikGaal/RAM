using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLvL4Area1 : MonoBehaviour
{
    public GameObject enemies;

    private bool done;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.transform.childCount == 0)
        {
            done = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player") && done)
        {
            SceneManager.LoadScene(5);
        }
    }
}
