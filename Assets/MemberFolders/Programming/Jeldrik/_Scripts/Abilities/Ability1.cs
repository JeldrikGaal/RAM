using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability1 : Abilities
{
    public override void Start()
    {
        base.Start();
    }
    override public void Update()
    {
        base.Update();
    }
    override public void Activate()
    {
        Debug.Log(_controller.transform.position);
        Debug.Log("Ability 1");
    }
}
