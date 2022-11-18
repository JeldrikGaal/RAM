using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammyFrontCheck : MonoBehaviour
{
    private BoxCollider _boxCollider;
    public List<GameObject> _objectsInCollider;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _objectsInCollider.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        _objectsInCollider.Remove(other.gameObject);
    }
}
