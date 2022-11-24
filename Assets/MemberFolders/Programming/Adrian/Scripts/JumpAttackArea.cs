using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackArea : MonoBehaviour
{
    [HideInInspector]
    public float Damage;
    [HideInInspector]
    public float StunDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            // Makes the enemy take damage
            other.GetComponent<EnemyTesting>().TakeDamage(Damage, transform.up);

            // Sets the stun duration in the enemy script
            other.GetComponent<EnemyTesting>().StunDuration = StunDuration;

            // Sets the stund variable to true so the enemy is stunned
            other.GetComponent<EnemyTesting>().Stunned = true;
        }
    }
}
