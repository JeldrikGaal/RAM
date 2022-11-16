using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

using System.IO;
using Newtonsoft.Json;
using System.Text;


public static class SaveNload
{
    
    static string _defaultPath = Application.persistentDataPath + "/SaveData.data";

    /// <summary>
    /// Loads data from the default paht: ...AppData\LocalLow\DefaultCompany\RAM/SaveData.data
    /// </summary>
    /// <returns></returns>
    public static SaveData Load() => Load(_defaultPath);

    /// <summary>
    /// loads data from a custom path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static SaveData Load(string path)
    {
        
        try
        {
            var text = File.ReadAllText(path);
            Debug.Log(text);
            return JsonConvert.DeserializeObject<SaveData>(text);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            
        }

        return new();

    }
    /// <summary>
    /// Saves data to the default paht: ...AppData\LocalLow\DefaultCompany\RAM/SaveData.data
    /// </summary>
    /// <param name="data"></param>
    public static void Save(SaveData data) => Save(data, _defaultPath);

    /// <summary>
    /// Saves the data to a custom path.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="path"></param>
    public static void Save(SaveData data,string path)
    {
        
        File.WriteAllText(path,JsonConvert.SerializeObject(data));
    }

    

}
/// <summary>
/// the class to save the data in
/// </summary>
[Serializable]
public class SaveData 
{
    public Stats.StatsData Stats = new Stats.StatsData();
    public List<PossibleAbilityData> possibleAbilities = new();
    public int CurrentLevel;
}

/// <summary>
/// standin ability class for testing saving.
/// </summary>
[Serializable]
public class PossibleAbilityData 
{
    public int abilityID;
    public float level;
    public Stats.StatsData stats;
}