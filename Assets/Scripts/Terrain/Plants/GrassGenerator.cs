using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGenerator : MonoBehaviour, IGenerator
{
    public GrassSet grassSet;

    public int maxGrassPerTile = 50;

    [HideInInspector]
    public List<TerrainObjectData> placedGrass;
    private TerrainConstructor terrain;
    private TerrainData Data => terrain.terrainData;

    public void Clear()
    {
        throw new System.NotImplementedException();
    }

    public void Generate()
    {
        terrain = FindObjectOfType<TerrainConstructor>();
        Clear();
        placedGrass = new List<TerrainObjectData>();
        for (int x = 0; x < Data.xSize; x++)
        {
            for (int z = 0; z < Data.zSize; z++)
            {
                TileData tile = Data.GetTileAtCoordinates(x, z);
                if (tile.type == TileType.FlatLand)
                {
                    for (int i = 0; i < maxGrassPerTile; i++)
                    {
                        //if (Random.value > treeClumping)
                        //{
                        //    break;
                        //}
                        // create a common grass spawnPoint
                        Vector3 randomPosition = new Vector3(Random.Range(0, terrain.tileSize), 0, Random.Range(0, terrain.tileSize));
                        randomPosition += tile.Transform.position;
                        int typeIndex = Random.Range(0, grassSet.common.Length);
                        Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
                        placedGrass.Add(new TerrainObjectData(typeIndex, randomPosition, randomRotation, Vector3.one * 2));
                    }
                }
            }
        }
    }
}
