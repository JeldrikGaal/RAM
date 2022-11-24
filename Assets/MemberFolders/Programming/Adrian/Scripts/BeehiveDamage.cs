using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehiveDamage : MonoBehaviour
{
    [SerializeField] private float _damageFrequency;
    [SerializeField] private float _damage;
    private float _localTimer;


    private void OnTriggerStay(Collider other)
    {
        // Checks to see if the player is inside
        if (other.tag == "Player")
        {
            // If the timer has reached zero
            if (_localTimer <= 0)
            {
                // Rammy takes damage
                other.GetComponent<RammyController>().TakeDamageRammy(_damage);

                // Restarts the timer
                _localTimer = _damageFrequency;
            }
            else
            {
                // The timer counts down each second
                _localTimer -= Time.deltaTime;
            }
        }
    }
}
