using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability2 : Abilities
{
    public int level = 0;
    Rigidbody _rb;
    Ray _ray;
    [SerializeField] Collider[] _hitColliders = new Collider[50];
    [SerializeField] float _pushBackForce = 7f;
    [SerializeField] GameObject _groundSmokeVFX;
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
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        //Waiting time for the initial phase
        yield return new WaitForSeconds(0.25f);

        //Plays the ground smoke and screen shake
        _groundSmokeVFX.SetActive(true);
        GetComponent<RammyController>().AddScreenShake(1.2f);

        //Creates a sphere and takes data's of everything in there
        _hitColliders = Physics.OverlapSphere(transform.position, _upgraded ? Stats.USplashRadius : Stats.SplashRadius);

        //Looks at everything physics catched and does the ability to those who has the enemy script.
        foreach (var item in _hitColliders)
        {
            if (item.transform != null && item.transform.gameObject.GetComponent<EnemyController>())
            {
                item.transform.gameObject.GetComponent<EnemyController>().TakeDamage((_upgraded ? Stats.UDmg : Stats.Dmg) * _controller.Damage, transform.up);
                item.transform.rotation = Quaternion.LookRotation(transform.position - item.transform.position);
                item.transform.gameObject.GetComponent<Rigidbody>().AddForce(-item.transform.forward * _pushBackForce, ForceMode.Impulse);
            }
        }
    }
    void OnDrawGizmos()
    {
        // Draws a yellow sphere at the transform's position
        Gizmos.color = new Color(1, 1, 1, 0.3f);
        Gizmos.DrawSphere(transform.position, _upgraded ? Stats.USplashRadius : Stats.SplashRadius);
    }
}
