using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ForestGenerator))]
public class ForestPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ForestGenerator constructor = (ForestGenerator)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Forests"))
        {
            constructor.Generate();
            EditorUtility.SetDirty(constructor);
        }
        if (GUILayout.Button("Clear Forests"))
        {
            constructor.Clear();
            EditorUtility.SetDirty(constructor);
        }

    }
}
