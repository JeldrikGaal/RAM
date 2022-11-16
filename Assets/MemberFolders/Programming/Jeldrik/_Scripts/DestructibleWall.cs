using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{

    public float Direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Hit(GameObject g)
    {
        switch (Direction)
        {
            // X and Z Bigger 
            case 0:
                if (transform.position.x > g.transform.position.x && transform.position.z > g.transform.position.z)
                {
                    return true;
                }
                break;
            // X Bigger Z Smaller
            case 1:
                if (transform.position.x > g.transform.position.x && transform.position.z < g.transform.position.z)
                {
                    return true;
                }
                break;
            // X Smaller Z Bigger
            case 2:
                if (transform.position.x < g.transform.position.x && transform.position.z > g.transform.position.z)
                {
                    return true;
                }
                break;
            // X Smaller Z Smaller 
            case 3:
                if (transform.position.x < g.transform.position.x && transform.position.z < g.transform.position.z)
                {
                    return true;
                }
                break;
         
        }
        return false;
    }
}
