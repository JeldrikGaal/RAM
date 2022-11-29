using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoreBlood : MonoBehaviour
{
    public GameObject SplatObject;
    private Rigidbody rb;

    private bool _hasSmudge = false;
    [SerializeField] private GameObject _smudge;
    private Vector3 _startPos;
    private Vector3 _currentVel;

    public DoubleArrayPooling DoubleArrayScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // InvokeRepeating("ResetSmudge", 0.3f, 0.3f);
    }

    private void SpawnSplat(Vector3 position)
    {
        Quaternion splatRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        if (!DoubleArrayScript.FullArray2)
        {
            // Spawn the splat:
            _smudge = Instantiate(SplatObject, position, splatRotation);
            _startPos = position;
            _hasSmudge = true;
            DoubleArrayScript.AddPoint(_smudge);
        }
        else if (DoubleArrayScript.FullArray2)
        {
            _smudge = DoubleArrayScript.NextToTake;
            _startPos = position;
            _smudge.transform.position = position;
            _smudge.transform.rotation = splatRotation;
            if (_smudge.GetComponent<FadeOnTrigger>())
            {
                _smudge.GetComponent<FadeOnTrigger>().StopFade();
            }
            _hasSmudge = true;
            DoubleArrayScript.AddPoint(_smudge);
        }
    }
   
    void OnCollisionEnter(Collision other)
    {
        foreach (var item in other.contacts)
        {
            if (!_hasSmudge)
            {
                    // Figure out the rotation for the splat:

                    if (other.gameObject.layer == 10)
                    {
                        SpawnSplat(item.point + item.normal * 0.6f);
                    }
            } else if (_hasSmudge)
            {
                if(other.gameObject.layer != 10)
                {
                    SpawnSplat(new Vector3(transform.position.x,1.1f,transform.position.z));
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer == 10)
        {
            if (_hasSmudge)
            {
                _hasSmudge = false;
            }
        }
    }
    private void Update()
    {
        if (_hasSmudge)
        {
            _smudge.transform.position = Vector3.Lerp(_startPos, new Vector3(this.transform.position.x, _startPos.y, this.transform.position.z), 0.5f);
            _smudge.transform.localScale = new Vector3(Vector3.Distance(_startPos, _smudge.transform.position)*2, 1, 1);
            var rot = ((_startPos - new Vector3(this.transform.position.x, _startPos.y, this.transform.position.z)).normalized);
            _smudge.transform.rotation = Quaternion.LookRotation(rot) * Quaternion.Euler(90,0,90);
            
            /*
            if (_currentVel != rb.velocity.normalized)
            {
                print(_currentVel);
                print(rb.velocity.normalized);
                print(gameObject.name + " changed direction!");
            }
            _currentVel = rb.velocity.normalized;
            */
        }

        if(rb.velocity.magnitude <= 0.01f)
        {
            rb.isKinematic = true;
            this.GetComponent<Collider>().enabled = false;
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this);
        }
    }

    /*private void ResetSmudge()
    {
        if (_hasSmudge && _currentVel != rb.velocity.normalized)
        {
            _hasSmudge = false;
        }
    }*/

}
