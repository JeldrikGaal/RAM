using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TSVImporter
{
    public static List<List<string>> ReadTSV(TextAsset data, int xLen, int yLen, int xStart = 0, int yStart = 0)
    {
        List<List<string>> readData = new List<List<string>>();

        string[] dataRows = data.text.Split(new string[] { "\n" }, StringSplitOptions.None);
        int yCounter = 0;

        try
        {
            for (int y = yStart; y < yStart + yLen; y++)
            {
                readData.Add(new List<string>());
                string[] dataColumn = dataRows[y].Split(new string[] { "\t" }, StringSplitOptions.None);

                for (int x = xStart; x < xStart + xLen; x++)
                {
                    readData[yCounter].Add(dataColumn[x]);
                }
                yCounter++;
            }
        }
        catch (Exception)
        {
            Debug.LogError("Invalid parameters in TSV Import");
            throw;
        }

        return readData;
    }
}
