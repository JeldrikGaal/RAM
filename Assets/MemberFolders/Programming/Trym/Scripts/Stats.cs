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

    #region Time Played
    /// <summary>
    /// Gets the TimePlayed stat.
    /// </summary>
    /// <returns></returns>
    public static float GetTimePlayed() => _stats.TimePlayed;

    /// <summary>
    /// Sets te time played
    /// </summary>
    /// <param name="time"></param>
    public static void SetTimePlayed(float time) => _stats.TimePlayed = time;
    /// <summary>
    /// adds the time played
    /// </summary>
    /// <param name="time"></param>
    public static void AddTimePlayed(float time) => _stats.TimePlayed += time;
    #endregion

    #region     Enemies Killed
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
    #endregion

    #region Damage Dealth
    /// <summary>
    /// Gets the DamageDealth stat.
    /// </summary>
    /// <returns></returns>
    public static float GetDamageDealth() => _stats.DamageDealth;
    /// <summary>
    /// Sets the DamageDealth stat.
    /// </summary>
    /// <param name="damage"></param>
    public static void SetDamageDealth( float damage) => _stats.DamageDealth = damage;
    /// <summary>
    /// adds damage to th DamageDealth stat
    /// </summary>
    /// <param name="damage"></param>
    public static void AddDamageDealth( float damage) => _stats.DamageDealth = damage;

    #endregion

    #region Damage Taken
    /// <summary>
    /// Gets the DamageTaken stat
    /// </summary>
    /// <returns></returns>
    public static float GetDamageTaken() => _stats.DamageTaken;

    /// <summary>
    /// Sets the DamageTaken stat
    /// </summary>
    /// <param name="damage"></param>
    public static void SetDamageTaken(float damage) => _stats.DamageTaken = damage;
    /// <summary>
    /// adds the spesified damage to the DamageTaken stat.
    /// </summary>
    /// <param name="damage"></param>
    public static void AddDamageTaken(float damage) => _stats.DamageTaken += damage;

    #endregion
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

