using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner
{
    public static Mesh CombineMeshesOfChildren(GameObject parent)
    {
        MeshFilter[] filters = parent.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[filters.Length];
        for (int i = 1; i < filters.Length; i++) // start at one because filters[0] is the parent's filter
        {
            combine[i].mesh = filters[i].sharedMesh;
            combine[i].transform = filters[i].transform.localToWorldMatrix;
            filters[i].gameObject.SetActive(false);
        }
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);
        return combinedMesh;
    }
}
