

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Splinter : MonoBehaviour
{
    [SerializeField] Rigidbody _rigid;
    Vector3 _initialVelocity;
#if UNITY_EDITOR
    [SerializeField] bool _interactIrammableEditorOnly = false;
#endif

    private void OnEnable()
    {
        _initialVelocity = _rigid.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.HasTag("enemy"))
        {
            // damaging the enemy
            collision.gameObject.GetComponent<EnemyTesting>().TakeDamage(0.1f, transform.up);
            // adds knockback
            collision.rigidbody.velocity =( _initialVelocity + _rigid.velocity ) /2;
        }

#if UNITY_EDITOR
        // Editor only functionality for stresstesting.
        if (collision.gameObject.TryGetComponent(out IRammable rammable) && _interactIrammableEditorOnly )
        {
            if (rammable.Hit(gameObject))
            {
                Destroy(collision.gameObject);
            }
            
            
        }
#endif



    }




}
