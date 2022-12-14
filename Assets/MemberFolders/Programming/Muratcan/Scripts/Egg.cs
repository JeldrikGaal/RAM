using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public float damage = 1.5f;

    [SerializeField] private GameObject _eggsplosion;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        _eggsplosion.transform.parent = this.transform;
        _eggsplosion.transform.localPosition = new Vector3(0, 0, 0);
        _eggsplosion.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RammyController>() != null)
        {
            collision.gameObject.GetComponent<RammyController>().TakeDamageRammy(damage);

            // VFX:

            _eggsplosion.SetActive(true);
            _eggsplosion.transform.parent = null;


            gameObject.SetActive(false);
        }
        else
        {
            //Destroy(gameObject, 5);
        }
    }
}
