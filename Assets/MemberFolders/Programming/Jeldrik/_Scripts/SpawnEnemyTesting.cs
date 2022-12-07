using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyTesting : MonoBehaviour
{
    public bool first;
    public bool special;

    public GameObject enemy1;
    public GameObject enemy2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8) && first && !special)
        {
            GameObject temp = Instantiate(enemy1,this.transform);
            temp.transform.localPosition = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) && !first && !special)
        {
            GameObject temp = Instantiate(enemy2,this.transform);
            temp.transform.localPosition = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && !first && special)
        {
            GameObject temp = Instantiate(enemy1, this.transform);
            temp.transform.localPosition = Vector3.zero;
        }
    }
}
