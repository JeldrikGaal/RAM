using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkChargeAttack : MonoBehaviour
{
    [SerializeField] GameObject[] _egg = new GameObject[3];
    [SerializeField] int _ammo = 3;
    [SerializeField] float _shootSpeed = 50f;
    public float damage = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HawkCharge()
    {
        if (_ammo == 0)
        {
            _ammo = 3;
            foreach (var item in _egg)
            {
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.transform.parent = transform;
                item.transform.localPosition = new Vector3(0f, 0f, 1.07f);
                item.SetActive(false);
            }
        }
        _ammo--;
        _egg[_ammo].SetActive(true);
        _egg[_ammo].transform.parent = null;
        _egg[_ammo].GetComponent<Rigidbody>().AddForce(transform.forward * _shootSpeed);
    }
}
