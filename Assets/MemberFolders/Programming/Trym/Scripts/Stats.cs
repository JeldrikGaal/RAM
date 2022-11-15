using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Stats
{
    private static StatsData _stats = new StatsData();

    /// <summary>
    /// Gets a data class intended for saving.
    /// </summary>
    /// <returns></returns>
    public static  StatsData GetData() => _stats;

    /// <summary>
    /// Sets the stats, Intended for loading.
    /// </summary>
    /// <param name="stats"></param>
    /// <returns></returns>
    public static  StatsData SetData( StatsData stats ) => _stats = stats;

    //      -- Time Played --
    public static void SetTimePlayed(float time) => _stats.TimePlayed = time;
    public static void AddTimePlayed(float time) => _stats.TimePlayed += time;

    //      -- Enemies Killed --

    /// <summary>
    /// adds 1 to the KilledEnemies Stat 
    /// </summary>
    public static void AddEnemyKilled() => _stats.KilledEnemies++;
    /// <summary>
    /// gets the KilledEnemies Stat
    /// </summary>
    /// <returns></returns>
    public static int GetKilledEnemies() => _stats.KilledEnemies;
    /// <summary>
    /// adds the spesified number of enemies to the KilledEnemies stat
    /// </summary>
    /// <param name="enemies"></param>
    public static void AddKilledEnemies(int enemies) => _stats.KilledEnemies += enemies;
    /// <summary>
    /// Sets the killed enemies stat.
    /// </summary>
    /// <param name="enemies"></param>
    public static void SetKilledEnemies(int enemies) => _stats.KilledEnemies = enemies;

    /// <summary>
    /// Serializable Container for stats.
    /// </summary>
    [System.Serializable]
    public class StatsData
    {
        public float TimePlayed;
        public int KilledEnemies;
        public float DamageDealth;
        public float DamageTaken;
        


    }
    
}

