using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBearBomb : MonoBehaviour
{
    [SerializeField] GameObject _effect;
    [SerializeField] Rigidbody _rigid;
    [SerializeField] float _fuse;
    [SerializeField] Vector3 _effectPosMod;
    [SerializeField] Collider _collider;
    private bool _allowHit = false;
    public Rigidbody Rb { get { return _rigid; } }
    public bool HitCheck { get; private set; } = false;

    private Vector3 _effectLocation;
    [SerializeField] private float _effectOffset;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _toonExplosion;

    //private void Start() => StartCoroutine(WaitTil());

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.rigidbody != null && _allowHit)
        {
            if (!collision.rigidbody.isKinematic)
            {
                _rigid.isKinematic = false;
                HitCheck = true;
            }
        }


#if false
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
#endif
        if (collision.gameObject.layer == 10)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            var layer = 1 << 10;
            if (Physics.Raycast(transform.position + new Vector3(0, 100, 0), transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, layer))
            {
                _effectLocation = hit.point + _effectOffset * Vector3.up;
                print(hit.point);
            }

            // _effectLocation = collision.contacts[0].point + collision.contacts[0].normal * _effectOffset;
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
        _effect.transform.parent = null;

        _toonExplosion.transform.LookAt(Camera.main.transform);
        _toonExplosion.SetActive(true);
        _toonExplosion.transform.parent = null;

        _explosion.SetActive(true);
        _explosion.transform.parent = null;

        // _effect.transform.position = transform.position + _effectPosMod;
        _effect.transform.position = _effectLocation;
        // _effect.transform.LookAt(_effect.transform.position + Vector3.forward, Vector3.up);
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

    private IEnumerator WaitTil()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        _collider.enabled = true;
        _allowHit = true;
    }



}
