using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData
{
    public GameObject[,] Tiles { get; set; }

    public TerrainData(int xTiles, int zTiles)
    {
        Tiles = new GameObject[xTiles, zTiles];
    }

    public int AdjacentOceanTilesCount(int x, int z)
    {
        int landTilesCount = 0;
        if (x - 1 >= 0 && IsOceanTile(x - 1, z))
        {
            landTilesCount++;
        }
        if (x + 1 < Tiles.GetLength(0) && IsOceanTile(x + 1, z))
        {
            landTilesCount++;
        }
        if (z - 1 >= 0 && IsOceanTile(x, z - 1))
        {
            landTilesCount++;
        }
        if (z + 1 < Tiles.GetLength(1) && IsOceanTile(x, z + 1))
        {
            landTilesCount++;
        }
        return landTilesCount;
    }

    public List<GameObject> GetEdgeAdjacentOceanTiles(int x, int z)
    {
        List<GameObject> oceanTiles = new List<GameObject>();
        if (x - 1 >= 0 && IsOceanTile(x - 1, z))
        {
            oceanTiles.Add(Tiles[x - 1, z]);
        }
        if (x + 1 < Tiles.GetLength(0) && IsOceanTile(x + 1, z))
        {
            oceanTiles.Add(Tiles[x + 1, z]);
        }
        if (z - 1 >= 0 && IsOceanTile(x, z - 1))
        {
            oceanTiles.Add(Tiles[x, z - 1]);
        }
        if (z + 1 < Tiles.GetLength(1) && IsOceanTile(x, z + 1))
        {
            oceanTiles.Add(Tiles[x, z + 1]);
        }
        return oceanTiles;
    }

    public List<GameObject> GetCornerAdjacentOceanTiles(int x, int z)
    {
        List<GameObject> oceanTiles = new List<GameObject>();
        if (x - 1 >= 0 && z - 1 >= 0 && IsOceanTile(x - 1, z - 1))
        {
            oceanTiles.Add(Tiles[x - 1, z - 1]);
        }
        if (x + 1 < Tiles.GetLength(0) && z + 1 < Tiles.GetLength(1) && IsOceanTile(x + 1, z + 1))
        {
            oceanTiles.Add(Tiles[x + 1, z + 1]);
        }
        if (x - 1 >= 0 && z + 1 < Tiles.GetLength(1) && IsOceanTile(x - 1, z + 1))
        {
            oceanTiles.Add(Tiles[x - 1, z + 1]);
        }
        if (x + 1 < Tiles.GetLength(1) && z - 1 >= 0 && IsOceanTile(x + 1, z - 1))
        {
            oceanTiles.Add(Tiles[x + 1, z - 1]);
        }
        return oceanTiles;
    }

    public bool IsOceanTile(int x, int z)
    {
        return Tiles[x, z].CompareTag("Terrain/OceanFloor");
    }
}
