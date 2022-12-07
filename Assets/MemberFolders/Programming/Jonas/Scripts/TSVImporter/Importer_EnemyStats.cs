using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class Importer_EnemyStats : ScriptableObject
{
    public TextAsset rawDataAttacks;
    public TextAsset rawDataHealth;

    public List<EnemyStats> EnemyStats;

    private List<List<string>> _dataAttacks;
    private List<List<string>> _dataAttacksBosses;
    private List<List<string>> _dataHealth;

    [Button]
    public void ImportData()
    {
        _dataAttacks = TSVImporter.ReadTSV(rawDataAttacks, 20, 23, 2, 2);
        _dataAttacksBosses = TSVImporter.ReadTSV(rawDataAttacks, 20, 37, 0, 25);
        _dataHealth = TSVImporter.ReadTSV(rawDataHealth, 1, 20, 1, 1);
    }

    [Button]
    public void UpdateData()
    {
        UnityEditor.Undo.RecordObjects(EnemyStats.ToArray(), "Update Enemy Stats");

        List<List<string>> statLists = new List<List<string>>(); ;
        int enemyCounter = 0;

        foreach (List<string> l in _dataAttacks)
        {
            if (string.IsNullOrEmpty(l[0]))
            {
                EnemyStats[enemyCounter].SetAttacks(statLists);
                statLists = new List<List<string>>();
            }
            else
            {
                statLists.Add(l);
            }
        }
    }
}
