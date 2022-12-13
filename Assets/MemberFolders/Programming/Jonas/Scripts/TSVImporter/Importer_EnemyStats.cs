using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

public class Importer_EnemyStats : ScriptableObject
{
    public TextAsset rawDataAttacks;
    public TextAsset rawDataHealth;

    //public List<EnemyStats> EnemyStats;
    public Dictionary<string, EnemyStats> EnemyStats;

    private List<List<string>> _dataAttacks;
    private List<List<string>> _dataHealth;

    #region Temp variables(?)

    string json;

    #endregion

    [Button]
    public void ImportData()
    {
        //_dataAttacks = TSVImporter.ReadTSV(rawDataAttacks, 20, 52, 2, 2);
        _dataAttacks = TSVImporter.ReadTSV(rawDataAttacks, 22, 52, 0, 2);
        _dataHealth = TSVImporter.ReadTSV(rawDataHealth, 4, 10, 0, 2);
    }

    [Button]
    public void UpdateData()
    {
        #region Enemy Attacks

        string lastName = "Wolf_Melee";

        EnemyStats = new Dictionary<string, EnemyStats>();

        List<List<string>> statLists = new List<List<string>>(); ;

        foreach (List<string> l in _dataAttacks)
        {
            if (string.IsNullOrEmpty(l[2]))
            {
                EnemyStats[lastName] = new EnemyStats();
                EnemyStats[lastName].SetAttacks(statLists);
                lastName = l[0];
                statLists = new List<List<string>>();
            }
            else
            {
                statLists.Add(l);
            }
        }

        EnemyStats["Bear_Boss"] = new EnemyStats();
        EnemyStats["Bear_Boss"].SetAttacks(statLists);

        //foreach (KeyValuePair<string, EnemyStats> k in EnemyStats)
        //    Debug.Log(k.Key);

        #endregion

        #region Health

        foreach (List<string> l in _dataHealth)
        {
            EnemyStats[l[0]].SetHealth(l);
        }

        #endregion

        json = JsonConvert.SerializeObject(EnemyStats, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/EnemyStats.json", json);
    }

    //using System.Collections;
    //using System.Collections.Generic;
    //using UnityEngine;
    //using Sirenix.OdinInspector;

    //public class Importer_EnemyStats : ScriptableObject
    //{
    //    public TextAsset rawDataAttacks;
    //    public TextAsset rawDataHealth;

    //    public List<EnemyStats> EnemyStats;

    //    private List<List<string>> _dataAttacks;
    //    private List<List<string>> _dataHealth;

    //    [Button]
    //    public void UpdateData()
    //    {
    //#if (UNITY_EDITOR)
    //        UnityEditor.Undo.RecordObjects(EnemyStats.ToArray(), "Update Enemy Stats");
    //#endif

    //        #region Enemy Attacks

    //        List<List<string>> statLists = new List<List<string>>(); ;
    //        int enemyCounter = 0;

    //        foreach (List<string> l in _dataAttacks)
    //        {
    //            if (string.IsNullOrEmpty(l[0]))
    //            {
    //                EnemyStats[enemyCounter++].SetAttacks(statLists);
    //                statLists = new List<List<string>>();
    //            }
    //            else
    //            {
    //                statLists.Add(l);
    //            }
    //        }

    //        EnemyStats[enemyCounter++].SetAttacks(statLists);

    //        #endregion

    //        #region Health

    //        enemyCounter = 0;

    //        foreach (List<string> l in _dataHealth)
    //        {
    //            EnemyStats[enemyCounter++].SetHealth(l);
    //        }

    //        #endregion
    //    }
}
