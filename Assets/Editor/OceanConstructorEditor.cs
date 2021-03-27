using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OceanConstructor))]
public class OceanConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        OceanConstructor constructor = (OceanConstructor)target;
        TerrainConstructor terrain = constructor.GetComponent<TerrainConstructor>();

        DrawDefaultInspector();

        if (GUILayout.Button("Construct"))
        {
            constructor.ConstructOcean(terrain.settings.xSize, terrain.settings.zSize);
            EditorUtility.SetDirty(terrain);
        }
        if (GUILayout.Button("Clear"))
        {
            constructor.ClearOcean();
            EditorUtility.SetDirty(terrain);
        }
    }
}
