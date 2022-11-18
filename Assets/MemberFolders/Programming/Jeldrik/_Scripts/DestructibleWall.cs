using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour, IRammable
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
        Vector3 direction = transform.position - g.transform.position;
        direction = direction.normalized;
        Debug.Log(direction);
        switch (Direction)
        {
            // Front
            case 0:
                if (direction.z > 0)
                {
                    return true;
                }
                break;
            // Back
            case 1:
                if (direction.z < 0)
                {
                    return true;
                }
                break;
            
         
        }
        return false;
    }
}
