﻿using System.Collections;
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
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Clear All"))
        {
            constructor.ClearTerrain();
            EditorUtility.SetDirty(target);
        }
    }
}
