using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBlood : MonoBehaviour
{
    // Splat for the initial quad that spawned:
    [SerializeField] private Material bloodSplatMat;
    [SerializeField] private float splatSize = 1;
    [SerializeField] private float splatOffset = 0.1f;

    [SerializeField] private GameObject splatObject;

    public BloodySteps BloodStepScript;
    public float BloodSize;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Rotates the proectile with the direction it is going.
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    void OnCollisionEnter(Collision other)
    {
        foreach (var item in other.contacts)
        {
            Quaternion splatRotation = Quaternion.Euler(new Vector3(0,0,0));
            // Calculates the direction for laying flat
            if(other.gameObject.layer == 10)
            {
                var flatLook = -item.normal;
                var lookDir = rb.velocity;
                lookDir.y = 0; // keep only the horizontal direction
                var velocityRotationEdit = Quaternion.LookRotation(lookDir);
                velocityRotationEdit *= Quaternion.Euler(90, flatLook.y, 0);
                splatRotation = velocityRotationEdit;
            } else if(other.gameObject.layer != 10)
            {
                splatRotation = Quaternion.LookRotation(-item.normal);
            }

            var prefab = Instantiate(splatObject, item.point + item.normal * 0.6f, splatRotation);
            prefab.transform.localScale = new Vector3(BloodSize, BloodSize, 1);
            BloodStepScript.AddPoint(new Vector2(item.point.x, item.point.z), prefab.gameObject);

            /// All of this was for a quad which spawned on the item normal. DONT DELETE, we might need to use it later if the decal doesn't work that well
            /*
            // Creates a quad,
            GameObject bloodQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // sets the quad the be rotated with the collision polygon normal,
            bloodQuad.transform.rotation = Quaternion.LookRotation(-item.normal);
            bloodQuad.transform.position = item.point;
            // offsets it a bit so it doesn't z-plane,
            bloodQuad.transform.localPosition = bloodQuad.transform.localPosition + item.normal * splatOffset;
            bloodQuad.transform.localScale = new Vector3(splatSize, splatSize);
            // and then removes the mesh collider
            Destroy(bloodQuad.GetComponent<MeshCollider>());
            // and gives it the blood material
            bloodQuad.GetComponent<Renderer>().material = bloodSplatMat;
            */

            // Destroys the projectile
            Destroy(this.gameObject);
        }
    }
}
