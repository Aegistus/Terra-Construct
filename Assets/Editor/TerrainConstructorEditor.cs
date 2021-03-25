using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainConstructor))]
public class TerrainConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainConstructor constructor = (TerrainConstructor)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Construct"))
        {
            constructor.ConstructTerrain();
        }
        if (GUILayout.Button("Chunk Terrain"))
        {
            constructor.ChunkTerrain();
        }
        if (GUILayout.Button("Clear Tiles"))
        {
            constructor.ClearTiles();
        }
        if (GUILayout.Button("Clear All (Including Chunks)"))
        {
            constructor.ClearTerrain();
        }
    }
}
