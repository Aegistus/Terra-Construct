using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GrassRenderer))]
public class GrassRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GrassRenderer constructor = (GrassRenderer)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Create Grass Pool"))
        {
            constructor.CreateGrassPool();
            EditorUtility.SetDirty(constructor);
        }
        if (GUILayout.Button("Clear Grass Pool"))
        {
            constructor.ClearGrassPool();
            EditorUtility.SetDirty(constructor);
        }
        if (GUILayout.Button("Update Grass"))
        {
            constructor.UpdateGrassAroundEditorCamera();
        }
    }
}
