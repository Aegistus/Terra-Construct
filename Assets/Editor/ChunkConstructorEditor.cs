using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChunkConstructor))]
public class ChunkConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChunkConstructor constructor = (ChunkConstructor)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Arrange Chunks"))
        {
            constructor.ArrangeChunks();
        }
        if (GUILayout.Button("Separate LODs"))
        {
            constructor.CombineTerrainMeshes();
        }
        if (GUILayout.Button("Generate Chunks"))
        {
            constructor.GenerateChunks();
        }

    }
}
