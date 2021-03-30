using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainConstructor))]
public class TerrainConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainConstructor terrain = (TerrainConstructor)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Terrain"))
        {
            terrain.GenerateTerrain();
            EditorUtility.SetDirty(terrain);
        }
        if (GUILayout.Button("Construct Terrain"))
        {
            terrain.ClearTerrain();
            terrain.ConstructTerrain();
            EditorUtility.SetDirty(terrain);
        }
        if (GUILayout.Button("Clear All"))
        {
            terrain.ClearTerrain();
            EditorUtility.SetDirty(terrain);
        }
        if (GUILayout.Button("Save Terrain"))
        {
            TerrainSaver.SaveTerrain(terrain.terrainData, terrain.terrainFileName);
        }
        if (GUILayout.Button("Load Terrain"))
        {
            TerrainData data = TerrainSaver.LoadTerrain(terrain.terrainFileName);
            terrain.terrainData = data;
            terrain.ConstructTerrain();
        }
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}
