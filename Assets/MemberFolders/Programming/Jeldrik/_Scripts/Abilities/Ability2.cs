using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability2 : Abilities
{
    public int level = 0;
    Rigidbody _rb;
    Ray _ray;
    [SerializeField] Collider[] hitColliders = new Collider[50];
    [SerializeField] float range = 5f;
    [SerializeField] float damage = 15f;
    [SerializeField] float pushBackForce = 7f;
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

        //Looks at everything physics catched and does the ability to those who has the enemy script.
        foreach (var item in hitColliders)
        {
            if (item.transform != null && item.transform.gameObject.GetComponent<EnemyTesting>())
            {
                item.transform.gameObject.GetComponent<EnemyTesting>().TakeDamage(damage, transform.up);
                item.transform.rotation = Quaternion.LookRotation(transform.position - item.transform.position);
                item.transform.gameObject.GetComponent<Rigidbody>().AddForce(-item.transform.forward * pushBackForce, ForceMode.Impulse);
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
