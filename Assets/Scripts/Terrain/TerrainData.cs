using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewTerrainData", menuName = "Terrain Data", order = 5)]
public class TerrainData : ScriptableObject
{
    public int xSize;
    public int zSize;
    public NoiseMap noiseMap;
    [SerializeField] private List<TileData> tiles;
    public List<TerrainObjectData> trees;
    public List<TerrainObjectData> mountains;
    public List<TerrainObjectData> foothills;

    public void CreateTiles(int xSize, int zSize)
    {
        this.xSize = xSize;
        this.zSize = zSize;
        tiles = new List<TileData>();
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                tiles.Add(new TileData(x,z));
            }
        }
    }

    public TileData GetTileAtCoordinates(int x, int z)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].xPos == x && tiles[i].zPos == z)
            {
                return tiles[i];
            }
        }
        return null;
    }

    public TileData GetTileAtCoordinates(Coordinates coords)
    {
        return GetTileAtCoordinates(coords.x, coords.z);
    }


    public int AdjacentOceanTilesCount(int x, int z)
    {
        int landTilesCount = 0;
        if (x - 1 >= 0 && IsOceanTile(x - 1, z))
        {
            landTilesCount++;
        }
        if (x + 1 < xSize && IsOceanTile(x + 1, z))
        {
            landTilesCount++;
        }
        if (z - 1 >= 0 && IsOceanTile(x, z - 1))
        {
            landTilesCount++;
        }
        if (z + 1 < zSize && IsOceanTile(x, z + 1))
        {
            landTilesCount++;
        }
        return landTilesCount;
    }

    public List<TileData> GetEdgeAdjacentOceanTiles(int x, int z)
    {
        List<TileData> oceanTiles = new List<TileData>();
        if (x - 1 >= 0 && IsOceanTile(x - 1, z))
        {
            oceanTiles.Add(GetTileAtCoordinates(x - 1, z));
        }
        if (x + 1 < xSize && IsOceanTile(x + 1, z))
        {
            oceanTiles.Add(GetTileAtCoordinates(x + 1, z));
        }
        if (z - 1 >= 0 && IsOceanTile(x, z - 1))
        {
            oceanTiles.Add(GetTileAtCoordinates(x, z - 1));
        }
        if (z + 1 < zSize && IsOceanTile(x, z + 1))
        {
            oceanTiles.Add(GetTileAtCoordinates(x, z + 1));
        }
        return oceanTiles;
    }

    public List<TileData> GetCornerAdjacentOceanTiles(int x, int z)
    {
        List<TileData> oceanTiles = new List<TileData>();
        if (x - 1 >= 0 && z - 1 >= 0 && IsOceanTile(x - 1, z - 1))
        {
            oceanTiles.Add(GetTileAtCoordinates(x - 1, z - 1));
        }
        if (x + 1 < xSize && z + 1 < zSize && IsOceanTile(x + 1, z + 1))
        {
            oceanTiles.Add(GetTileAtCoordinates(x + 1, z + 1));
        }
        if (x - 1 >= 0 && z + 1 < zSize && IsOceanTile(x - 1, z + 1))
        {
            oceanTiles.Add(GetTileAtCoordinates(x - 1, z + 1));
        }
        if (x + 1 < zSize && z - 1 >= 0 && IsOceanTile(x + 1, z - 1))
        {
            oceanTiles.Add(GetTileAtCoordinates(x + 1, z - 1));
        }
        return oceanTiles;
    }

    public List<TileData> GetEdgeAdjacentLandTiles(int x, int z)
    {
        List<TileData> landTiles = new List<TileData>();
        if (x - 1 >= 0 && IsLandTile(x - 1, z))
        {
            landTiles.Add(GetTileAtCoordinates(x - 1, z));
        }
        if (x + 1 < xSize && IsLandTile(x + 1, z))
        {
            landTiles.Add(GetTileAtCoordinates(x + 1, z));
        }
        if (z + 1 < zSize && IsLandTile(x, z + 1))
        {
            landTiles.Add(GetTileAtCoordinates(x, z + 1));
        }
        if (z - 1 >= 0 && IsLandTile(x, z - 1))
        {
            landTiles.Add(GetTileAtCoordinates(x, z - 1));
        }
        return landTiles;
    }

    public Coordinates GetTileCoordinates(TileData tile)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                if (GetTileAtCoordinates(x,z).Equals(tile))
                {
                    return new Coordinates(x, z);
                }
            }
        }
        return new Coordinates(-1, -1);
    }

    public bool IsOceanTile(int x, int z)
    {
        return GetTileAtCoordinates(x, z).type == TileType.OceanFloor;
    }

    public bool IsLandTile(int x, int z)
    {
        return GetTileAtCoordinates(x, z).type == TileType.FlatLand;
    }

    public bool IsCoastalTile(int x, int z)
    {
        return GetTileAtCoordinates(x, z).type == TileType.CoastStraight || GetTileAtCoordinates(x, z).type == TileType.CoastOuterCorner || GetTileAtCoordinates(x, z).type == TileType.CoastInnerCorner;
    }


}
