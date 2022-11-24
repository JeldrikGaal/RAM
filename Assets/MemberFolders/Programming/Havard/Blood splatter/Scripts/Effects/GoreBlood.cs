using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoreBlood : MonoBehaviour
{
    public GameObject SplatObject;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other)
    {
        foreach (var item in other.contacts)
        {

            // Figure out the rotation for the splat:

            Quaternion splatRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            if (other.gameObject.layer == 10)
            {
                // Calculates the direction for laying flat
                var flatLook = -item.normal;
                var lookDir = rb.velocity;
                lookDir.y = 0; // keep only the horizontal direction
                var velocityRotationEdit = Quaternion.LookRotation(lookDir);
                velocityRotationEdit *= Quaternion.Euler(90, flatLook.y, 0);
                splatRotation = velocityRotationEdit;
            }
            else if (other.gameObject.layer == 12)
            {
                splatRotation = Quaternion.LookRotation(-item.normal);
            }


            // Spawn the splat:
            var prefab = Instantiate(SplatObject, item.point + item.normal * 0.6f, splatRotation);


        }
    }
}
