using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TerrainConstructor))]
public class OceanConstructor : MonoBehaviour
{
    public TerrainTileSet tileSet;
    public float seaLevel = -1;
    public float tileSize = 100;

    private GameObject[,] oceanTiles;

    public void ConstructOcean(float xSize, float zSize)
    {
        if (oceanTiles != null)
        {
            ClearOcean();
        }
        int xTileTotal = (int)(xSize / tileSize);
        int zTileTotal = (int)(zSize / tileSize);
        oceanTiles = new GameObject[xTileTotal, zTileTotal];
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                int randIndex = Random.Range(0, tileSet.waterTiles.Length);
                oceanTiles[x, z] = Instantiate(tileSet.waterTiles[randIndex], new Vector3(x * tileSize, seaLevel, z * tileSize), Quaternion.identity, transform);
            }
        }
    }

    public void ClearOcean()
    {
        if (oceanTiles == null)
        {
            return;
        }
        for (int x = 0; x < oceanTiles.GetLength(0); x++)
        {
            for (int z = 0; z < oceanTiles.GetLength(1); z++)
            {
                DestroyImmediate(oceanTiles[x, z]);
                oceanTiles[x, z] = null;
            }
        }
        oceanTiles = null;
    }
}
