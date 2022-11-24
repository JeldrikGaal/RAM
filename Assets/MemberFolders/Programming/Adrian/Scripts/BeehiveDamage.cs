using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehiveDamage : MonoBehaviour
{
    [SerializeField] private float _damageFrequency;
    [SerializeField] private float _damage;
    private float _localTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (_localTimer <= 0)
            {
                other.GetComponent<RammyController>().TakeDamageRammy(_damage);
                _localTimer = _damageFrequency;
            }
            else
            {
                _localTimer -= Time.deltaTime;
            }
        }
    }
}
