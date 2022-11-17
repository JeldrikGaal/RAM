using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour
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
        if (other.tag == "Player")
        {
            // Halves the movespeed of the player when they enter the slow zone
            other.GetComponent<RammyController>().MovementSpeed /= 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Multiplies the speed with 2 again to return it to the default value (might cause problems later, Risk of Rain 1)
            other.GetComponent<RammyController>().MovementSpeed *= 2;
        }
    }
}
