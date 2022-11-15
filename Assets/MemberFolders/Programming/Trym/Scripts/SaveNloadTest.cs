using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;


public class SaveNloadTest : MonoBehaviour
{
    SaveData _data = new SaveData();
    string _path;
    // Start is called before the first frame update
    void Start()
    {
        _path = Application.persistentDataPath + "/SaveData.json";
        LoadData();
        FakeAbilityData abilityData = new FakeAbilityData();
        abilityData.DamageDealth = 5; 
        abilityData.Kills = 2;

        _data.AbilityDatas.Add(new FakeAbilityData().GetType(),abilityData);
        print(_data.GetType().IsSerializable);
        print(JsonConvert.SerializeObject(_data));
        File.WriteAllText(_path, JsonConvert.SerializeObject(_data));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void LoadData()
    {
        try
        {
            _data = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(_path));
        }
        catch (Exception e)
        {
            Debug.LogError(e);

        }

        foreach (var item in _data.AbilityDatas)
        {
            switch (item.Key.Name)
            {
                case "FakeAbilityData": print(JsonConvert.SerializeObject(item.Value as FakeAbilityData));
                        break;
                default:
                    break;
            }
        }

    }


}

[Serializable]
public class SaveData 
{
    public Stats.StatsData Stats = new Stats.StatsData();
    public Dictionary<Type,object> AbilityDatas = new Dictionary<Type,object>();
    public int CurrentLevel;
}


[Serializable]
public class FakeAbilityData 
{
    public int Kills;
    public float DamageDealth;
}