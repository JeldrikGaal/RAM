using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyStats : ScriptableObject
{
    public Dictionary<string, EnemyAttackStats> Attacks;
    public Dictionary<int, float> Health;

    public void SetAttacks(List<List<string>> variables)
    {
        Attacks = new Dictionary<string, EnemyAttackStats>();

        foreach (List<string> l in variables)
        {
            Debug.Log(l[0]);

            List<float> stats = new List<float>();

            for(int i = 2; i < l.Count; i++)
            {
                stats.Add(float.Parse(l[i]));
            }

            Attacks[l[0]] = new EnemyAttackStats();
            Attacks[l[0]].SetVariables(stats);
        }
    }

    public void SetHealth()
    {

    }

    public EnemyAttackStats GetStats(string name)
    {
        return Attacks[name];
    }

    [Button]
    public void PrintStats(string name)
    {
        foreach (KeyValuePair<string, EnemyAttackStats> k in Attacks)
        {
            Debug.Log(k.Key);
        }
    }

    public float GetHealth(int area)
    {
        return Health[area];
    }
}
