using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MountainConstructor))]
public class MountainConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MountainConstructor constructor = (MountainConstructor)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Place Mountains"))
        {
            constructor.PlaceMountains();
            EditorUtility.SetDirty(FindObjectOfType<TerrainConstructor>());
        }
        if (GUILayout.Button("Place Foothills"))
        {
            constructor.PlaceFootHills();
            EditorUtility.SetDirty(FindObjectOfType<TerrainConstructor>());
        }
        if (GUILayout.Button("Clear"))
        {
            constructor.ClearMountains();
            EditorUtility.SetDirty(FindObjectOfType<TerrainConstructor>());
        }
    }

}
