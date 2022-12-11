using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class Importer_RammyAttacks : ScriptableObject
{
    public TextAsset rawData;

    public List<RammyAttack> Attacks;

    private List<List<string>> _data;
    private List<List<string>> _upgradeData;

    [Button]
    public void ImportData()
    {
        _data = TSVImporter.ReadTSV(rawData, 10, 8, 1, 1);
        _upgradeData = TSVImporter.ReadTSV(rawData, 10, 6, 1, 11);
    }

    [Button]
    public void UpdateData()
    {
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

#if (UNITY_EDITOR)
        UnityEditor.Undo.RecordObjects(Attacks.ToArray(), "Update Rammy Stats");
#endif
        for (int i = 0; i < 8; i++)
        {
            Attacks[i].SetVariables(float.Parse(_data[i][0], culture), float.Parse(_data[i][1], culture), float.Parse(_data[i][3], culture), float.Parse(_data[i][4], culture), float.Parse(_data[i][5], culture), float.Parse(_data[i][7], culture), float.Parse(_data[i][8], culture));
        }

        for (int i = 0; i < 6; i++)
        {
            Attacks[i + 2].SetUVariables(float.Parse(_upgradeData[i][0], culture), float.Parse(_upgradeData[i][1], culture), float.Parse(_upgradeData[i][3], culture), float.Parse(_upgradeData[i][4], culture), float.Parse(_upgradeData[i][5], culture), float.Parse(_upgradeData[i][7], culture), float.Parse(_upgradeData[i][8], culture));
        }
    }

    [Button]
    public void PrintData()
    {
        if (_data == null)
        {
            Debug.Log("Data is null");
            return;
        }

        if (_data.Count == 0)
        {
            Debug.Log("No entries in data");
            return;
        }

        foreach (List<string> l in _data)
        {
            string stringLine = "";
            foreach (string s in l)
            {
                stringLine += s + " , ";
            }

            Debug.Log(stringLine);
        }
    }
}
