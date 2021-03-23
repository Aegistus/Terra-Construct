using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RiverConstructor))]
public class RiverConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RiverConstructor constructor = (RiverConstructor)target;
        TerrainConstructor terrain = constructor.GetComponent<TerrainConstructor>();

        DrawDefaultInspector();

        if (GUILayout.Button("Construct"))
        {
            constructor.ConstructRivers(terrain.tileSize);
        }

    }
}
