using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackArea : MonoBehaviour
{
    [HideInInspector]
    public float Damage;
    [HideInInspector]
    public float StunDuration;

    [HideInInspector] public RammyController PlayerController;

    // VFX:
    public RammyVFX VFXScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            // If the damage kills the enemy, do cool stuff
            if (other.gameObject.GetComponent<EnemyController>().TakeDamage(Damage, transform.up))
            {
                PlayerController.Kill(other.gameObject);
            }

            // Sets the stun duration in the enemy script
            other.GetComponent<EnemyController>().StunDuration = StunDuration;

            // Sets the stund variable to true so the enemy is stunned
            other.GetComponent<EnemyController>().Stun();

            // Tells the VFX script to do VFX things:
            VFXScript.Ab1Attack(other.gameObject);
        }
    }
}
