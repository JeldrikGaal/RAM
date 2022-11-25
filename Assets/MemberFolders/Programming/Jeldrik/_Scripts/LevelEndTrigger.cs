using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
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
        if (TagManager.HasTag(other.gameObject, "player"))
        {
            SceneManager.LoadScene(0);
            //Application.Quit();
            Debug.Log("LEVEL ENDED");
        }
    }
}
