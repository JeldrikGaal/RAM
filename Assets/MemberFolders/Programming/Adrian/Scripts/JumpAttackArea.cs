using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackArea : MonoBehaviour
{
    // [HideInInspector]
    public float Damage;
    // [HideInInspector]
    public float StunDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            other.GetComponent<EnemyTesting>().TakeDamage(Damage, transform.up);
            StartCoroutine(other.GetComponent<EnemyTesting>().Stun(StunDuration));
        }
    }
}
