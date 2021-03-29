using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class TerrainSaver
{
    public static void SaveTerrain(TerrainData terrain)
    {
        string json = JsonUtility.ToJson(terrain);
        File.WriteAllText(Application.dataPath + "/saves.txt", json);
    }

    public static TerrainData LoadTerrain()
    {
        if (File.Exists(Application.dataPath + "/saves.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/saves.txt");
            TerrainData data = JsonUtility.FromJson<TerrainData>(saveString);
            return data;
        }
        return null;
    }
}
