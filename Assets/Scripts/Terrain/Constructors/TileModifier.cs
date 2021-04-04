using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileModifier
{
    public static void VaryTileElevation(GameObject tileGameObject, NoiseMap noise)
    {
        MeshFilter[] meshFilters = tileGameObject.GetComponentsInChildren<MeshFilter>();
        Mesh[] allMeshes = new Mesh[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            allMeshes[i] = meshFilters[i].sharedMesh;
        }
        for (int i = 0; i < allMeshes.Length; i++)
        {
            Vector3[] vertices = allMeshes[i].vertices;
            for (int j = 0; j < vertices.Length; j++)
            {
                vertices[i] = new Vector3(vertices[i].x, noise.GetLayeredPerlinValueAtPosition(vertices[i].x, vertices[i].z), vertices[i].z);
            }
            allMeshes[i].vertices = vertices;
        }
    }
}
