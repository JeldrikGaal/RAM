using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollVelocity : MonoBehaviour
{
    public Vector3 CalcDirLeft;
    public Vector3 CalcDirRight;
    public float BloodForceMin;
    public float BloodForceMax;

    private bool _static = false;

    void Start()
    {
        // Calculates a random direction and force, and multiplies them together:
        Vector3 randomDir = RandomVector3(CalcDirLeft, CalcDirRight);
        float randomForce = Random.Range(BloodForceMin, BloodForceMax);
        Vector3 finalVelocity = randomDir * randomForce;
        
        // Goes through every child, including childs of childs, checks if they are rigidbodies and gives them the velocity that we calculated.
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.GetComponent<Rigidbody>())
            {
                child.GetComponent<Rigidbody>().velocity = finalVelocity;
            }
        }
    }

    // Simple function that randomizes Vector3's.
    public static Vector3 RandomVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }

    private void Update()
    {
        // Here we freeze everything when it stops.

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.GetComponent<Rigidbody>())
            {
                if (_static)
                {
                    child.GetComponent<Rigidbody>().isKinematic = true;

                    if (GetComponent<CapsuleCollider>())
                    {
                        child.GetComponent<CapsuleCollider>().enabled = false;
                    }
                }

                if (child.GetComponent<Rigidbody>().velocity.magnitude <= 0.1)
                {
                    print(this.name);
                    _static = true;
                }
            }
        }
    }
}
