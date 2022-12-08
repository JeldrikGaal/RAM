using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackStats
{
    public float Damage1;
    public float Damage2;
    public float Damage3;
    public float ProjectileSpeed;
    public float Range;
    public float ReloadTime;
    public float AnticipationTime;
    public float AttackTime;
    public float RecoveryTime;
    public float SplashRadius;
    public float ConeAttackDegree;
    public float ProjectileRadius;
    public float OnGroundEffectTime;
    public float PlayerHitEffectTime;

    public float Damage(int area)
    {
        switch (area)
        {
            case 1: return Damage1;
            case 2: return Damage2;
            case 3: return Damage3;
            default: return 0;
        }
    }

    public void SetVariables(List<float> values)
    {
        Damage1 = values[0];
        Damage2 = values[1];
        Damage3 = values[2];
        ProjectileSpeed = values[4];
        Range = values[5];
        ReloadTime = values[6];
        AnticipationTime = values[7];
        AttackTime = values[8];
        RecoveryTime = values[9];
        SplashRadius = values[11];
        ConeAttackDegree = values[12];
        ProjectileRadius = values[13];
        OnGroundEffectTime = values[14];
        PlayerHitEffectTime = values[15];
    }
}
