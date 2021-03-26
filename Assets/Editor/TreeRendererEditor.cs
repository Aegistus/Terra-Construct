using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TreeRenderer))]
public class TreeRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TreeRenderer constructor = (TreeRenderer)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Create Tree Pool"))
        {
            constructor.CreateTreePool();
        }
        if (GUILayout.Button("Clear Tree Pool"))
        {
            constructor.ClearTreePool();
        }
        if (GUILayout.Button("Update Trees"))
        {
            constructor.UpdateTreesAroundEditorCamera();
        }
    }
}
