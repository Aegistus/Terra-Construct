using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverConstructor : MonoBehaviour
{
    public TerrainTileSet tileSet;

    GameObject[,] modifiedTiles;
    float tileSize;

    public TerrainData ConstructRivers(TerrainData terrainData, float tileSize)
    {
        modifiedTiles = terrainData.Tiles;
        this.tileSize = tileSize;
        for (int x = 0; x < modifiedTiles.GetLength(0); x++)
        {
            for (int z = 0; z < modifiedTiles.GetLength(1); z++)
            {
                if (!terrainData.IsOceanTile(x,z) && terrainData.AdjacentOceanTilesCount(x,z) == 1)
                {
                    PlaceRiveMouthTile(x, z);
                }
            }
        }
        return terrainData;
    }

    public void PlaceRiveMouthTile(int x, int z)
    {
        DestroyImmediate(modifiedTiles[x, z]);
        int randomTileIndex = Random.Range(0, tileSet.riverMouth.Length);
        GameObject newTile = Instantiate(tileSet.riverMouth[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
        modifiedTiles[x, z] = newTile;
    }
}
