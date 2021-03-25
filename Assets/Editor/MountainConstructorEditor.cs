﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MountainConstructor))]
public class MountainConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MountainConstructor constructor = (MountainConstructor)target;
        TerrainConstructor terrain = constructor.GetComponent<TerrainConstructor>();

        DrawDefaultInspector();

        if (GUILayout.Button("Construct"))
        {
            constructor.PlaceMountains();
        }
        if (GUILayout.Button("Clear"))
        {
            constructor.ClearMountains();
        }

    }

}