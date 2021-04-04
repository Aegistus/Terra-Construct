using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainData
{
    public int xSize;
    public int zSize;
    [SerializeField] public List<TileData> tiles;
    public List<TerrainObjectData> grass;
    public List<TerrainObjectData> trees;
    public List<TerrainObjectData> rareTrees;
    public List<TerrainObjectData> mountains;
    public List<TerrainObjectData> foothills;
    public List<TerrainObjectData> boulders;

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
            if (tiles[i].xCoordinate == x && tiles[i].zCoordinate == z)
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

    public List<TileData> GetAllEdgeAdjacentTiles(int x, int z)
    {
        List<TileData> tiles = new List<TileData>();
        if (x - 1 >= 0)
        {
            tiles.Add(GetTileAtCoordinates(x - 1, z));
        }
        if (x + 1 < xSize)
        {
            tiles.Add(GetTileAtCoordinates(x + 1, z));
        }
        if (z - 1 >= 0)
        {
            tiles.Add(GetTileAtCoordinates(x, z - 1));
        }
        if (z + 1 < zSize)
        {
            tiles.Add(GetTileAtCoordinates(x, z + 1));
        }
        return tiles;
    }

    public List<TileData> GetEdgeAdjacentTilesOfType(int x, int z, TileType type)
    {
        List<TileData> tilesOfType = new List<TileData>();
        if (x - 1 >= 0 && GetTileAtCoordinates(x - 1, z).type == type)
        {
            tilesOfType.Add(GetTileAtCoordinates(x - 1, z));
        }
        if (x + 1 < xSize && GetTileAtCoordinates(x + 1, z).type == type)
        {
            tilesOfType.Add(GetTileAtCoordinates(x + 1, z));
        }
        if (z - 1 >= 0 && GetTileAtCoordinates(x, z - 1).type == type)
        {
            tilesOfType.Add(GetTileAtCoordinates(x, z - 1));
        }
        if (z + 1 < zSize && GetTileAtCoordinates(x, z + 1).type == type)
        {
            tilesOfType.Add(GetTileAtCoordinates(x, z + 1));
        }
        return tilesOfType;
    }

    public List<TileData> GetCornerAdjacentTilesOfType(int x, int z, TileType type)
    {
        List<TileData> cornerTilesOfType = new List<TileData>();
        if (x - 1 >= 0 && z - 1 >= 0 && GetTileAtCoordinates(x - 1, z - 1).type == type)
        {
            cornerTilesOfType.Add(GetTileAtCoordinates(x - 1, z - 1));
        }
        if (x + 1 < xSize && z + 1 < zSize && GetTileAtCoordinates(x + 1, z + 1).type == type)
        {
            cornerTilesOfType.Add(GetTileAtCoordinates(x + 1, z + 1));
        }
        if (x - 1 >= 0 && z + 1 < zSize && GetTileAtCoordinates(x - 1, z + 1).type == type)
        {
            cornerTilesOfType.Add(GetTileAtCoordinates(x - 1, z + 1));
        }
        if (x + 1 < zSize && z - 1 >= 0 && GetTileAtCoordinates(x + 1, z - 1).type == type)
        {
            cornerTilesOfType.Add(GetTileAtCoordinates(x + 1, z - 1));
        }
        return cornerTilesOfType;
    }

    public bool IsOceanTile(int x, int z)
    {
        return GetTileAtCoordinates(x, z).type == TileType.OceanFloor;
    }

    public bool IsLandTile(int x, int z)
    {
        return GetTileAtCoordinates(x, z).type == TileType.Plains;
    }

    public bool IsCoastalTile(int x, int z)
    {
        return GetTileAtCoordinates(x, z).type == TileType.CoastStraight || GetTileAtCoordinates(x, z).type == TileType.CoastOuterCorner || GetTileAtCoordinates(x, z).type == TileType.CoastInnerCorner;
    }
}
