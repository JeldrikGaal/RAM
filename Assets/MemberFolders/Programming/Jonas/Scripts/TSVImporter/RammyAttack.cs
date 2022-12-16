using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RammyAttack
{
    public float Dmg;
    public float Cooldown;
    public float Range;
    public float FreeVariable;
    public float AttackTime;
    public float SplashRadius;
    public float AttackDegrees;

    public float UDmg;
    public float UCooldown;
    public float URange;
    public float UFreeVariable;
    public float UAttackTime;
    public float USplashRadius;
    public float UAttackDegrees;

    public void SetVariables(float dmg, float cooldown, float range, float freeVariable, float attackTime, float splashRadius, float attackDegrees)
    {
        Dmg = dmg/10;
        Cooldown = cooldown;
        Range = range;
        FreeVariable = freeVariable;
        AttackTime = attackTime;
        SplashRadius = splashRadius;
        AttackDegrees = attackDegrees;
    }

    public void SetUVariables(float uDmg, float uCooldown, float uRange, float uFreeVariable, float uAttackTime, float uSplashRadius, float uAttackDegrees)
    {
        UDmg = uDmg/10;
        UCooldown = uCooldown;
        URange = uRange;
        UFreeVariable = uFreeVariable;
        UAttackTime = uAttackTime;
        USplashRadius = uSplashRadius;
        UAttackDegrees = uAttackDegrees;
    }
}
