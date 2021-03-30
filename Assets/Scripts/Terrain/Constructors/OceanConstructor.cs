using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanConstructor
{

    //private GameObject[,] oceanTiles;

    //public void GenerateOcean(TerrainData data, TerrainSettings settings)
    //{
    //    if (oceanTiles != null)
    //    {
    //        ClearOcean();
    //    }
    //    int xTileTotal = (int)(settings.xSize / settings.tileSize);
    //    int zTileTotal = (int)(settings.zSize / settings.tileSize);
    //    oceanTiles = new GameObject[xTileTotal, zTileTotal];
    //    for (int x = 0; x < xTileTotal; x++)
    //    {
    //        for (int z = 0; z < zTileTotal; z++)
    //        {
    //            int randIndex = Random.Range(0, tileSet.waterTiles.Length);
    //            oceanTiles[x, z] = Instantiate(tileSet.waterTiles[randIndex], new Vector3(x * tileSize, seaLevel, z * tileSize), Quaternion.identity, transform);
    //        }
    //    }
    //}

    //public void ClearOcean()
    //{
    //    if (oceanTiles == null)
    //    {
    //        return;
    //    }
    //    for (int x = 0; x < oceanTiles.GetLength(0); x++)
    //    {
    //        for (int z = 0; z < oceanTiles.GetLength(1); z++)
    //        {
    //            DestroyImmediate(oceanTiles[x, z]);
    //            oceanTiles[x, z] = null;
    //        }
    //    }
    //    oceanTiles = null;
    //}
}
