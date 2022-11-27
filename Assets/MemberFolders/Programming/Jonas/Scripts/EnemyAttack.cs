using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _damage;

    //private void OnEnable()
    //{

    //}

    public void Init(float damage)
    {
        if(damage != -1) _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{_damage} dealt to {other.name}");
    }


}
