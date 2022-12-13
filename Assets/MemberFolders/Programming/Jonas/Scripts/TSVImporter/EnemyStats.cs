using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EnemyStats
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

            for (int i = 4; i < l.Count; i++)
            {
                stats.Add(float.Parse(l[i].Replace(',', '.'), culture));
            }

            Attacks[l[2]] = new EnemyAttackStats();
            Attacks[l[2]].SetVariables(stats);
        }
    }

    public void SetHealth(List<string> health)
    {
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

        Health = new List<float>();

        for(int i = 1; i < 4; i++)
        {
            Health.Add(float.Parse(health[i].Replace(',', '.'), culture));
        }
    }

    public EnemyAttackStats GetStats(string name)
    {
        if (Attacks == null)
            return new EnemyAttackStats(true);

        return Attacks[name];
    }

    public float GetHealth(int area)
    {
        return Health[area - 1];
    }

    [Button]
    public void PrintStats()
    {
        if (Attacks == null)
        {
            Debug.Log("Attack list is null");
            return;
        }

        foreach (KeyValuePair<string, EnemyAttackStats> k in Attacks)
        {
            Debug.Log(k.Key);
        }
    }

    [Button]
    public void PrintStat(string name)
    {
        if (Attacks == null)
        {
            Debug.Log("Attack list is null");
            return;
        }

        if (!Attacks.ContainsKey(name))
        {
            Debug.Log($"No attack with name: {name}");
            return;
        }
        Debug.Log(Attacks[name].Damage1);
    }

    [Button]
    public void PrintHealth()
    {
        if (Health == null)
        {
            Debug.Log("Health list is null");
            return;
        }

        foreach (float f in Health)
        {
            Debug.Log(f);
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Sirenix.OdinInspector;

//[System.Serializable]
//public class EnemyStats : ScriptableObject
//{
//    public Dictionary<string, EnemyAttackStats> Attacks;
//    public List<float> Health;

//    public void SetAttacks(List<List<string>> variables)
//    {
//        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

//        Attacks = new Dictionary<string, EnemyAttackStats>();

//        foreach (List<string> l in variables)
//        {
//            List<float> stats = new List<float>();

//            for (int i = 2; i < l.Count; i++)
//            {
//                stats.Add(float.Parse(l[i], culture));
//            }

//            Attacks[l[0]] = new EnemyAttackStats();
//            Attacks[l[0]].SetVariables(stats);
//        }
//    }

//    public void SetHealth(List<string> health)
//    {
//        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

//        Health = new List<float>();

//        foreach (string s in health)
//        {
//            Health.Add(float.Parse(s, culture));
//        }
//    }

//    public EnemyAttackStats GetStats(string name)
//    {
//        if (Attacks == null)
//            return new EnemyAttackStats(true);

//        return Attacks[name];
//    }

//    public float GetHealth(int area)
//    {
//        return Health[area - 1];
//    }

//    [Button]
//    public void PrintStats()
//    {
//        if (Attacks == null)
//        {
//            Debug.Log("Attack list is null");
//            return;
//        }

//        foreach (KeyValuePair<string, EnemyAttackStats> k in Attacks)
//        {
//            Debug.Log(k.Key);
//        }
//    }

//    [Button]
//    public void PrintStat(string name)
//    {
//        if (Attacks == null)
//        {
//            Debug.Log("Attack list is null");
//            return;
//        }

//        if (!Attacks.ContainsKey(name))
//        {
//            Debug.Log($"No attack with name: {name}");
//            return;
//        }
//        Debug.Log(Attacks[name].Damage1);
//    }

//    [Button]
//    public void PrintHealth()
//    {
//        if (Health == null)
//        {
//            Debug.Log("Health list is null");
//            return;
//        }

//        foreach (float f in Health)
//        {
//            Debug.Log(f);
//        }
//    }
//}
