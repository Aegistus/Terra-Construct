using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ForestPlacer))]
public class ForestPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ForestPlacer constructor = (ForestPlacer)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Forests"))
        {
            constructor.PlaceForests();
        }

    }
}
