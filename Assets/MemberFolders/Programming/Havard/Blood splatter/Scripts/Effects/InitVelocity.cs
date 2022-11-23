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
    [SerializeField] private bool awayFromPlayer = true;
    public Vector3 CalcDirLeft;
    public Vector3 CalcDirRight;
    public float BloodForceMin;
    public float BloodForceMax;


    // I anticipate 2 ways to get the directions depending on the way the player attacks:
    // If the player attacks in front of it, the direction should be the way the player is facing
    // If the player is dealing AoE damage, the directrion should be calculated with the direction of the enemy from the player
    /// This script desires two rotation inputs if the bloodsplatter is to be in a cone output

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = new Vector3(Random.Range(angularVelocity.x, angularVelocity.y), Random.Range(angularVelocity.x, angularVelocity.y), Random.Range(angularVelocity.x, angularVelocity.y));

        if (awayFromPlayer == false)
        {
            rb.velocity = new Vector3(Random.Range(velocity.x- randomOffsetValue, velocity.x+ randomOffsetValue), Random.Range(velocity.y - randomOffsetValue, velocity.y + randomOffsetValue), Random.Range(velocity.x - randomOffsetValue, velocity.x + randomOffsetValue));
        } else if (awayFromPlayer == true)
        {
            ApplyVelocity(RandomVector3(CalcDirLeft, CalcDirRight), Random.Range(BloodForceMin, BloodForceMax));
        }
    }
    // Simple function that applies force with the direction. Can easily be modified.
    public void ApplyVelocity(Vector3 dir, float force)
    {
        rb.velocity = new Vector3(dir.x, dir.y, dir.z) * force;
        print(transform.name);
    }

    // Gives a random rotation within 2 rotations
    public static Vector3 RandomVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
}
