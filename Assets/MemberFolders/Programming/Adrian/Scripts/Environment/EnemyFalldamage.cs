using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFalldamage : MonoBehaviour
{
    [SerializeField] private int _fallDamage;

    [SerializeField] private bool _die;
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
        if (other.gameObject.tag == "Ground")
        {
            GetComponent<EnemyController>().enabled = true;

            if (_die)
            {
                // If the enemy is supposed to die, take maxhealth as damage
                GetComponent<EnemyController>().TakeDamage(GetComponent<EnemyController>().MaxHealth, Vector3.up);
            }
            else
            {
                // Else it will take a set amount of damage
                GetComponent<EnemyController>().TakeDamage(_fallDamage, Vector3.up);
                GetComponent<StateMachine>().enabled = true;
            }
        }
    }
}
