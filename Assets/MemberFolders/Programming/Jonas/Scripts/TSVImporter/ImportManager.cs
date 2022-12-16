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
        TextAsset data = Resources.Load<TextAsset>("EnemyStats");
        string json = data.text;
        EnemyStats = JsonConvert.DeserializeObject<Dictionary<string, EnemyStats>>(json);

        TextAsset data2 = Resources.Load<TextAsset>("RammyAttacks");
        string json2 = data2.text;
        RammyAttacks = JsonConvert.DeserializeObject<Dictionary<string, RammyAttack>>(json2);
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
