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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   
    void OnCollisionEnter(Collision other)
    {
        if (!_hasSmudge)
        {

            foreach (var item in other.contacts)
            {

                // Figure out the rotation for the splat:

                if (other.gameObject.layer == 10)
                    {
                    Quaternion splatRotation = Quaternion.Euler(new Vector3(0, 0, 0));

                    // Spawn the splat:
                    _smudge = Instantiate(SplatObject, item.point + item.normal * 0.6f, splatRotation);
                    _startPos = item.point + item.normal * 0.6f;
                    _hasSmudge = true;
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


        }

        if(rb.velocity.magnitude <= 0.01f)
        {
            rb.isKinematic = true;
            this.GetComponent<Collider>().enabled = false;
        }

    }
}
