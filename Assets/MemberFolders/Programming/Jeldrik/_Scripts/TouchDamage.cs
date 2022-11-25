using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDamage : MonoBehaviour
{

    [SerializeField] private float _damage;
    [SerializeField] private float _damageCoolDown;
    private float _lastDamageTime;

    // Start is called before the first frame update
    void Start()
    {
        _lastDamageTime = Time.time;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (TagManager.HasTag(collision.gameObject, "player"))
        {
            if (Time.time - _lastDamageTime > _damageCoolDown)
            {
                RammyController _controller = collision.transform.GetComponent<RammyController>();
                if (!_controller.GetAttacking()) _controller.TakeDamageRammy(_damage);
                _lastDamageTime = Time.time;
            }
           
        }
    }
}
