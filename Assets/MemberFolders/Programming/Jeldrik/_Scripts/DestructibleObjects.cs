using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleObjects : MonoBehaviour, IRammable
{
    private BoxCollider _collider;
    [SerializeField] private GameObject _destroyParticle;

    [SerializeField] private bool _dropPowerup;
    [SerializeField] private GameObject _powerUp;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public bool Hit(GameObject g)
    {
        if (_destroyParticle)
        {
            Instantiate(_destroyParticle, transform.position, transform.rotation);
        }
        if (_dropPowerup)
        {
            Instantiate(_powerUp, transform.position, transform.rotation);
        }
        _collider.enabled = false;
        Destroy(this.gameObject);
        return false;
        
    }


}
