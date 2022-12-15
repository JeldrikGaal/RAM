using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBonkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _bonkEffect;
    [SerializeField] private float _effectOffset;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            RaycastHit hit;
            var layer = 1 << 10;
            if (Physics.Raycast(transform.position + new Vector3(0, 100, 0), -Vector3.up, out hit, Mathf.Infinity, layer))
            {
                var bonkEffect = Instantiate(_bonkEffect, hit.point + _effectOffset * Vector3.up, Quaternion.Euler(0, 0, 0));
                Destroy(bonkEffect, 20f);
            }
        }

    }
}
