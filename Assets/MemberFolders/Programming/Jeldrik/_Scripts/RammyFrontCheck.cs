using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammyFrontCheck : MonoBehaviour
{
    private BoxCollider _boxCollider;
    public List<GameObject> _objectsInCollider;
    private RammyController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>(); 
        _controller = transform.parent.GetComponent<RammyController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> remove = new List<GameObject> ();
        foreach (GameObject g in _objectsInCollider)
        {
            if (g)
            {
                if (Vector3.Distance(g.transform.position, _controller.transform.position) > 5)
                {
                    remove.Add(g);
                }
            }
               
        }

        foreach (GameObject g in remove)
        {
            _objectsInCollider.Remove(g);
        }
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
