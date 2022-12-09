using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyStats : ScriptableObject
{
    public Dictionary<string, EnemyAttackStats> Attacks;
    public List<float> Health;

    public void SetAttacks(List<List<string>> variables)
    {
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

        Attacks = new Dictionary<string, EnemyAttackStats>();

        foreach (List<string> l in variables)
        {
            List<float> stats = new List<float>();

            for(int i = 2; i < l.Count; i++)
            {
                stats.Add(float.Parse(l[i], culture));
            }

            Attacks[l[0]] = new EnemyAttackStats();
            Attacks[l[0]].SetVariables(stats);
        }
    }

    public void SetHealth(List<string> health)
    {
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

        Health = new List<float>();

        foreach (string s in health)
        {
            Health.Add(float.Parse(s, culture));
        }
    }

    public EnemyAttackStats GetStats(string name)
    {
        return Attacks[name];
    }

    public float GetHealth(int area)
    {
        return Health[area-1];
    }

    [Button]
    public void PrintStats()
    {
        foreach (KeyValuePair<string, EnemyAttackStats> k in Attacks)
        {
            Debug.Log(k.Key);
        }
    }

    [Button]
    public void PrintStat(string name)
    {
        Debug.Log(Attacks[name].Damage1);
    }

    [Button]
    public void PrintHealth()
    {
        foreach (float f in Health)
        {
            Debug.Log(f);
        }
    }
}
