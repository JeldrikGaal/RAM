

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Splinter : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] bool _interactIrammableEditorOnly = false;
#endif
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.HasTag("enemy"))
        {
            collision.gameObject.GetComponent<EnemyTesting>().TakeDamage(0.1f, transform.up);
        }

#if UNITY_EDITOR
        
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
