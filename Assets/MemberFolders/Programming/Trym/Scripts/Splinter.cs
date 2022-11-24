

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Splinter : MonoBehaviour
{
    
    [SerializeField] Rigidbody _rigid;
    SplinterProperties _properties;
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
            var enemy = collision.gameObject.GetComponent<EnemyTesting>();

            // damaging the enemy
            enemy.TakeDamage(_properties.Damage, transform.forward);
            StartCoroutine(enemy.Stun(_properties.StunTime));

           // _rigid.velocity = _initialVelocity;
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
    /*
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.HasTag("enemy"))
        {
            collision.rigidbody.velocity = _initialVelocity;
        }

    }*/


    public void SetProperties(SplinterProperties properties)
    {
        _properties = properties;
    }
    // Enables conversion between Rigidbody and Splinter
    public static implicit operator Rigidbody(Splinter splinter)=>splinter._rigid;
    public static implicit operator Splinter( Rigidbody body)=>body.gameObject.GetComponent<Splinter>();

}
[System.Serializable]
public class SplinterProperties
{
    public float Damage;
    public float StunTime;
}