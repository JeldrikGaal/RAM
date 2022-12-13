using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public float damage = 1.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RammyController>() != null)
        {
            collision.gameObject.GetComponent<RammyController>().TakeDamageRammy(damage);
            gameObject.SetActive(false);
        }
        else
        {
            //Destroy(gameObject, 5);
        }
    }
}
