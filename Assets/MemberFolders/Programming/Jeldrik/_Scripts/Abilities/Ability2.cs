using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability2 : Abilities
{
    Rigidbody _rb;
    Ray _ray;
    [SerializeField] Collider[] hitColliders = new Collider[50];
    [SerializeField] float range = 5;
    [SerializeField] float damage = 15;
    public override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
    }
    override public void Update()
    {
        base.Update();
    }
    override public void Activate()
    {
        //Creates a sphere and takes data's of everything in there
        hitColliders = Physics.OverlapSphere(transform.position, range);

        //To use until we get the animation
        foreach (var item in hitColliders)
        {
            if (item.transform != null && item.transform.gameObject.GetComponent<EnemyTesting>())
            {
                item.transform.gameObject.GetComponent<EnemyTesting>().TakeDamage(damage, transform.up);
            }
        }
    }
    void OnDrawGizmos()
    {
        // Draws a yellow sphere at the transform's position
        Gizmos.color = new Color(1, 1, 1, 0.3f);
        Gizmos.DrawSphere(transform.position, range);
    }
}
