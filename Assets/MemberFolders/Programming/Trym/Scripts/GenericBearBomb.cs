using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBearBomb : MonoBehaviour
{
    [SerializeField] GameObject _effect;
    [SerializeField] float _fuse;
    [SerializeField] Vector3 _effectPosMod;
    
    

    private void OnCollisionEnter(Collision collision)
    {
        // activates the timer after hitting the ground.
        if (collision.collider.attachedRigidbody != null)
        {
            if (collision.collider.attachedRigidbody.isKinematic)
            {
                StartCoroutine(Counting());
            }
        }
        else
        {
            StartCoroutine(Counting());
        }
        
    }

    //activate when the fuse is finished.
    IEnumerator Counting()
    {
        yield return new WaitForSeconds(_fuse);
        _effect.SetActive(true);
        _effect.transform.LookAt(transform.position + Vector3.forward, Vector3.up);
        _effect.transform.parent = null;
        _effect.transform.position = transform.position + _effectPosMod;
        Destroy(gameObject);
    }

    /// <summary>
    /// Overrides the properties of the bomb
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="fuseTime"></param>
    public void SetProperties(GameObject effect,float fuseTime)
    {
        _effect = effect;
        _fuse = fuseTime;
    }

}
