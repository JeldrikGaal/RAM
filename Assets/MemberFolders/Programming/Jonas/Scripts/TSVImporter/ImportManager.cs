using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ImportManager : MonoBehaviour
{
    public static Dictionary<string, EnemyStats> EnemyStats;
    public static Dictionary<string, RammyAttack> RammyAttacks;

    private void Awake()
    {
        string json = File.ReadAllText(Application.dataPath + "/EnemyStats.json");
        EnemyStats = JsonConvert.DeserializeObject<Dictionary<string, EnemyStats>>(json);

        json = File.ReadAllText(Application.dataPath + "/RammyAttacks.json");
        RammyAttacks = JsonConvert.DeserializeObject<Dictionary<string, RammyAttack>>(json);
    }

    public static EnemyStats GetEnemyStats(string enemyName)
    {
        return EnemyStats[enemyName];
    }

    public static RammyAttack GetRammyAttack(string attackName)
    {
        return RammyAttacks[attackName];
    }
}
