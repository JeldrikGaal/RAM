using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [HideInInspector] public GameObject Wolf;
    public float damage = 1.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Wolf == null)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddTorque(Vector3.up * -100);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RammyController>())
        {
            collision.gameObject.GetComponent<RammyController>().TakeDamageRammy(damage);
        }
        else
        {

        }
    }
}
