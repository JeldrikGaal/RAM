using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoSillyTree : MonoBehaviour, IRammable
{
    public bool Hit(GameObject g)
    {
        var fromPlayer = Quaternion.LookRotation(-(g.transform.position - transform.position));


        return true;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
