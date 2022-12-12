using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour, IRammable
{

    public GameObject Complete;
    public GameObject Broken;
    private BoxCollider _collider;
    [SerializeField] private GameObject _destroyParticle;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Hit(GameObject g)
    {
        if (_destroyParticle)
        {
            Instantiate(_destroyParticle, transform.position, transform.rotation);
        }
        _collider.enabled = false;
        Broken.SetActive(true);
        Complete.SetActive(false);
        return false;
    }
}
