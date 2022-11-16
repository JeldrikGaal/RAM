using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Text;


public class SaveNloadTest : MonoBehaviour
{
    SaveData _data = new();
    string _path;
    byte[] bytes = { 0x05, 0x26 };
    // Start is called before the first frame update
    void Start()
    {
        _path = Application.persistentDataPath + "/SaveData.ejson";
        Load();
        PossibleAbilityData abilityData = new();
        abilityData.level = 2;

        _data.possibleAbilities.Add(abilityData);
        print(_data.GetType().IsSerializable);
        print(JsonConvert.SerializeObject(_data));
        Save();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Load()
    {
        try
        {
            _data = JsonConvert.DeserializeObject<SaveData>(Decrypt( File.ReadAllText(_path)));
        }
        catch (Exception e)
        {
            Debug.LogError(e);

        }

       

    }

    void Save()
    {
        File.WriteAllText(_path,Encrypt( JsonConvert.SerializeObject(_data)));
    }

    string Encrypt(string data)
    {
        UnicodeEncoding encoding = new();
        

        return data;
    }

    string Decrypt(string digest)
    {
        UnicodeEncoding encoding = new();

        return digest;
    }

}

[Serializable]
public class SaveData 
{
    public Stats.StatsData Stats = new Stats.StatsData();
    public List<PossibleAbilityData> possibleAbilities = new();
    public int CurrentLevel;
}


[Serializable]

public class PossibleAbilityData 
{
    public int abilityID;
    public float level;
    public Stats.StatsData stats;
}