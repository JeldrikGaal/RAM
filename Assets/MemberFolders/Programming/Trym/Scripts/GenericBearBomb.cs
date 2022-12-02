using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBearBomb : MonoBehaviour
{
    [SerializeField] GameObject _effect;
    [SerializeField] Rigidbody _rigid;
    [SerializeField] float _fuse;
    [SerializeField] Vector3 _effectPosMod;
    
    public Rigidbody Rb { get { return _rigid; } }
    public bool HitCheck { get; private set; } = false;
    private void OnCollisionEnter(Collision collision)
    {
        // activates the timer after hitting the ground.
        if (collision.collider.attachedRigidbody != null)
        {
            if (collision.collider.attachedRigidbody.isKinematic)
            {
                LightFuse();
            }
        }
        else
        {
            LightFuse();
        }
        
    }

    private void LightFuse()
    {
        StartCoroutine(Counting());
        HitCheck = true;
        _rigid.isKinematic = true;
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
    public void SetProperties(float fuseTime)
    {
        
        _fuse = fuseTime;
    }

}
