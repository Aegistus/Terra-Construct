using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    public List<TileData> tiles = new List<TileData>();
    public List<GameObject> objectsInsideChunk = new List<GameObject>();
    public readonly GameObject gameObject;

    public TerrainChunk(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public void CombineMeshes()
    {
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        filter.mesh = MeshCombiner.CombineMeshesOfChildren(gameObject);
    }

    public void ClearTiles()
    {
        while (gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Object.DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
            }
        }
    }

}
