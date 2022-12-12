using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player"))
        {
            RammyController controller = other.GetComponent<RammyController>();
            controller.TakeDamageRammy(1000);
        }
        if (TagManager.HasTag(other.gameObject, "enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(1000,Vector3.zero);
        }
    }
}
