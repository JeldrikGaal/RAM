using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "enemy")
        {
            other.gameObject.GetComponent<EnemyTesting>().TakeDamage(5, Vector3.up);
        }
    }
}