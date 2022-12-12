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
    private List<List<string>> _dataHealth;

    [Button]
    public void ImportData()
    {
        _dataAttacks = TSVImporter.ReadTSV(rawDataAttacks, 20, 52, 2, 2);
        _dataHealth = TSVImporter.ReadTSV(rawDataHealth, 3, 10, 1, 2);
    }

    [Button]
    public void UpdateData()
    {
#if (UNITY_EDITOR)
        UnityEditor.Undo.RecordObjects(EnemyStats.ToArray(), "Update Enemy Stats");
#endif

        #region Enemy Attacks

        List<List<string>> statLists = new List<List<string>>(); ;
        int enemyCounter = 0;

        foreach (List<string> l in _dataAttacks)
        {
            if (string.IsNullOrEmpty(l[0]))
            {
                EnemyStats[enemyCounter++].SetAttacks(statLists);
                statLists = new List<List<string>>();
            }
            else
            {
                statLists.Add(l);
            }
        }

        EnemyStats[enemyCounter++].SetAttacks(statLists);

        #endregion

        #region Health

        enemyCounter = 0;

        foreach (List<string> l in _dataHealth)
        {
            EnemyStats[enemyCounter++].SetHealth(l);
        }

        #endregion
    }
}
