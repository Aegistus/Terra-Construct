using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainConstructor : MonoBehaviour
{
    public TerrainData terrainData;
    public TerrainSettings settings;
    public TerrainTileSet tileSet;
    public NoiseMap elevationNoiseMap;

    public void GenerateTerrainData()
    {
        terrainData.CreateTiles(settings.xSize / settings.tileSize, settings.zSize / settings.tileSize);
    }

    public void Construct()
    {
        for (int x = 0; x < terrainData.xSize; x++)
        {
            for (int z = 0; z < terrainData.zSize; z++)
            {
                TileData tile = terrainData.GetTileAtCoordinates(x, z);
                GameObject tileGameObject = null;
                switch(tile.type)
                {
                    case TileType.CoastInnerCorner: tileGameObject = tileSet.coastalInnerCorner[0];break;
                    case TileType.CoastOuterCorner: tileGameObject = tileSet.coastalOuterCorner[0];break;
                    case TileType.CoastStraight: tileGameObject = tileSet.coastalStraight[0]; break;
                    case TileType.FlatLand: tileGameObject = tileSet.landTiles[0]; break;
                    case TileType.OceanFloor: tileGameObject = tileSet.oceanFloorTiles[0]; break;
                }
                Instantiate(tileGameObject, tile.position, Quaternion.Euler(tile.rotation), transform);
            }
        }
    }

    public void ClearTerrain()
    {
        while (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        elevationNoiseMap.ResetNoiseRange();
    }
}
