using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoreBlood : MonoBehaviour
{
    public GameObject SplatObject;
    private Rigidbody rb;

    public bool HasSmudge = false;
    public GameObject Smudge;
    public Vector3 StartPos;
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
            Smudge = Instantiate(SplatObject, position, splatRotation);
            StartPos = position;
            StartPos = new Vector3(StartPos.x, position.y, StartPos.z);
            HasSmudge = true;
            DoubleArrayScript.AddPoint(Smudge);
        }
        else if (DoubleArrayScript.FullArray2)
        {
            Smudge = DoubleArrayScript.NextToTake;
            StartPos = position;
            StartPos = new Vector3(StartPos.x, position.y, StartPos.z);
            Smudge.transform.position = position;
            Smudge.transform.rotation = splatRotation;
            if (Smudge.GetComponent<FadeOnTrigger>())
            {
                Smudge.GetComponent<FadeOnTrigger>().StopFade();
            }
            HasSmudge = true;
            DoubleArrayScript.AddPoint(Smudge);
        }
    }
   
    void OnCollisionEnter(Collision other)
    {
        foreach (var item in other.contacts)
        {
            if (!HasSmudge)
            {
                    // Figure out the rotation for the splat:

                    if (other.gameObject.layer == 10)
                    {
                        SpawnSplat(item.point + item.normal * 0.6f);
                    }
            } else if (HasSmudge)
            {
                if(other.gameObject.layer != 10)
                {
                    SpawnSplat(new Vector3(transform.position.x, item.point.y + item.normal.y * 0.6f, transform.position.z));
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer == 10)
        {
            if (HasSmudge)
            {
                HasSmudge = false;
            }
        }
    }
    private void Update()
    {
        if (HasSmudge)
        {
            Smudge.transform.position = Vector3.Lerp(StartPos, new Vector3(this.transform.position.x, StartPos.y, this.transform.position.z), 0.5f);
            Smudge.transform.localScale = new Vector3(Vector3.Distance(StartPos, Smudge.transform.position)*2, 1, 1);
            var rot = ((StartPos - new Vector3(this.transform.position.x, StartPos.y, this.transform.position.z)).normalized);
            Smudge.transform.rotation = Quaternion.LookRotation(rot) * Quaternion.Euler(90,0,90);
            
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
