using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFalldamage : MonoBehaviour
{
    [SerializeField] private int _fallDamage;

    [SerializeField] private bool _die;

    private EnemyController _enemyController;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<StateMachine>().enabled = false;
        _enemyController = GetComponent<EnemyController>();
        _enemyController.SetInviciblity(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 10)
        {
            GetComponent<EnemyController>().enabled = true;
            _enemyController.SetInviciblity(false);

            if (_die)
            {
                // If the enemy is supposed to die, take maxhealth as damage
                GetComponent<EnemyController>().TakeDamage(GetComponent<EnemyController>().Stats.GetHealth(1), Vector3.up);
            }
            else
            {
                // Else it will take a set amount of damage
                GetComponent<StateMachine>().enabled = true;
                Debug.Log(("Health", GetComponent<EnemyController>().Health));
                GetComponent<EnemyController>().TakeDamage(_fallDamage, Vector3.up);
                
            }
        }
    }
}
