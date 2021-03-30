using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class TerrainSaver
{
    public static void SaveTerrain(TerrainData terrain, string fileName)
    {
        string json = JsonUtility.ToJson(terrain, true);
        File.WriteAllText(Application.dataPath + "/" + fileName, json);
    }

    public static TerrainData LoadTerrain(string fileName)
    {
        if (File.Exists(Application.dataPath + "/" + fileName))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/" + fileName);
            TerrainData data = JsonUtility.FromJson<TerrainData>(saveString);
            return data;
        }
        return null;
    }
}
