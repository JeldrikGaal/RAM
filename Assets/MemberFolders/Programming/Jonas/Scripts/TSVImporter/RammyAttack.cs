using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammyAttack : ScriptableObject
{
    public float Dmg;
    public float Cooldown;
    public float Range;
    public float AttackTime;
    public float SplashRadius;
    public float AttackDegrees;

    public float UDmg;
    public float UCooldown;
    public float URange;
    public float UAttackTime;
    public float USplashRadius;
    public float UAttackDegrees;

    public void SetVariables(float dmg, float cooldown, float range, float attackTime, float splashRadius, float attackDegrees)
    {
        Dmg = dmg/10;
        Cooldown = cooldown;
        Range = range;
        AttackTime = attackTime;
        SplashRadius = splashRadius;
        AttackDegrees = attackDegrees;
    }

    public void SetUVariables(float uDmg, float uCooldown, float uRange, float uAttackTime, float uSplashRadius, float uAttackDegrees)
    {
        UDmg = uDmg/10;
        UCooldown = uCooldown;
        URange = uRange;
        UAttackTime = uAttackTime;
        USplashRadius = uSplashRadius;
        UAttackDegrees = uAttackDegrees;
    }
}
