using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel5Area1 : MonoBehaviour
{

    [SerializeField] GameObject bossHolder;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHolder.transform.childCount == 0)
        {
            Debug.Log("END LEVEL");
        }
    }
}
