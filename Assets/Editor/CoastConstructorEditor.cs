using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CoastConstructor))]
public class CoastConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CoastConstructor constructor = (CoastConstructor)target;
        TerrainConstructor terrainConst = constructor.GetComponent<TerrainConstructor>();

        DrawDefaultInspector();

        if (GUILayout.Button("Construct"))
        {
            constructor.ReplaceCoastalTiles(terrainConst.tileSize);
        }
    }
}
