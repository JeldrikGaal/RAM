using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitVelocity : MonoBehaviour
{
    // Variables to control the velocity:
    private Rigidbody rb;
    [SerializeField] private Vector2 velocity = new Vector2(0,2);
    [SerializeField] private Vector2 angularVelocity = new Vector2(0,360);
    [SerializeField] private float randomOffsetValue = 1;

    // Settings for moving the objects away from the player
    [SerializeField] private bool awayFromPlayer = false;

    // I anticipate 2 ways to get the directions depending on the way the player attacks:
    // If the player attacks in front of it, the direction should be the way the player is facing
    // If the player is dealing AoE damage, the directrion should be calculated with the direction of the enemy from the player
    /// This script desires two rotation inputs if the bloodsplatter is to be in a cone output

    void Start()
    {
        /// Temporary:
        var player = GameObject.FindGameObjectsWithTag("Player");
        /// End of temporary. Please delete.

        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = new Vector3(Random.Range(angularVelocity.x, angularVelocity.y), Random.Range(angularVelocity.x, angularVelocity.y), Random.Range(angularVelocity.x, angularVelocity.y));

        if (awayFromPlayer == false)
        {
            rb.velocity = new Vector3(Random.Range(velocity.x- randomOffsetValue, velocity.x+ randomOffsetValue), Random.Range(velocity.y - randomOffsetValue, velocity.y + randomOffsetValue), Random.Range(velocity.x - randomOffsetValue, velocity.x + randomOffsetValue));
        } else if (awayFromPlayer == true)
        {
            // These two next lines needs to be swapped out with the two directions the blood should fall between
            Vector3 calcDirLeft = player[0].transform.GetChild(2).transform.position - player[0].transform.GetChild(0).transform.position;
            Vector3 calcDirRight = player[0].transform.GetChild(3).transform.position - player[0].transform.GetChild(0).transform.position;

            ApplyVelocity(RandomVector3(calcDirLeft, calcDirRight), Random.Range(velocity.y - randomOffsetValue, velocity.y + randomOffsetValue));
        }
    }
    // Simple function that applies force with the direction. Can easily be modified.
    public void ApplyVelocity(Vector3 dir, float force)
    {
        rb.velocity = new Vector3(dir.x, dir.y, dir.z) * force;
    }

    // Gives a random rotation within 2 rotations
    public static Vector3 RandomVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
}
