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
            constructor.Construct();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Clear All"))
        {
            constructor.ClearTerrain();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Construct All"))
        {
            IConstructor[] constructors = constructor.GetComponents<IConstructor>();
            for (int i = 0; i < constructors.Length; i++)
            {
                constructors[i].Construct();
            }
        }
    }
}
