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
        Debug.Log("ASDASD");
        //string json = File.ReadAllText(Application.dataPath + "/EnemyStats.json");
        //string json = File.ReadAllText(Path.Combine(Application.dataPath, "Resources/EnemyStats.json"));
        TextAsset data = Resources.Load<TextAsset>("EnemyStats");
        string json = data.text;
        Debug.Log(json);
        EnemyStats = JsonConvert.DeserializeObject<Dictionary<string, EnemyStats>>(json);
        Debug.LogError(EnemyStats);

        //  json = File.ReadAllText(Path.Combine(Application.dataPath, "Resources/RammyAttacks.json"));
        data = Resources.Load("RammyAttacks") as TextAsset;
        json = data.text;
        RammyAttacks = JsonConvert.DeserializeObject<Dictionary<string, RammyAttack>>(json);
        Debug.LogError(RammyAttacks);
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
