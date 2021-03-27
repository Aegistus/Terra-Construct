using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestPlacer : MonoBehaviour
{
    public TreeSet trees;
    public bool placeAboveWaterLevel = true;
    public bool placeBelowMountainLevel = true;
    [Range(0f, 1f)]
    public float elevationMin = 0f;
    [Range(0f, 1f)]
    public float elevationMax = 0f;
    public int maxTreesPerForestTile = 5;
    [Range(0f, 1f)]
    public float treeClumping = .5f;
    private TerrainConstructor terrain;
    private TerrainData Data => terrain.terrainData;
    private MountainConstructor mountains;

    public void PlaceForests()
    {
        terrain = FindObjectOfType<TerrainConstructor>();
        mountains = FindObjectOfType<MountainConstructor>();
        ClearForests();
        Data.Trees = new List<TreeData>();
        float minValue;
        float maxValue;
        if (placeAboveWaterLevel)
        {
            minValue = terrain.oceanPercent + .05f;
        }
        else
        {
            minValue = elevationMin;
        }
        if (placeBelowMountainLevel)
        {
            maxValue = mountains.mountainLevel - .05f;
        }
        else
        {
            maxValue = elevationMax;
        }
        for (int x = 0; x < Data.Tiles.GetLength(0); x++)
        {
            for (int z = 0; z < Data.Tiles.GetLength(1); z++)
            {
                TileData tile = Data.Tiles[x, z];
                if (tile.noiseValue > minValue && tile.noiseValue < maxValue && tile.type == TileType.FlatLand)
                {
                    for (int i = 0; i < maxTreesPerForestTile; i++)
                    {
                        //if (Random.value > treeClumping)
                        //{
                        //    break;
                        //}
                        // create a common tree spawnPoint
                        Vector3 randomPosition = new Vector3(Random.Range(0, terrain.tileSize), 0, Random.Range(0, terrain.tileSize));
                        randomPosition += tile.Transform.position;
                        int typeIndex = Random.Range(0, trees.commonTrees.Count);
                        Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
                        Data.Trees.Add(new TreeData(typeIndex, randomPosition, randomRotation, Vector3.one * 2));
                    }
                }
            }
        }
    }

    public void ClearForests()
    {
        terrain = FindObjectOfType<TerrainConstructor>();
        if (Data.Trees != null)
        {
            Data.Trees.Clear();
        }
    }

    private void OnValidate()
    {
        if (placeAboveWaterLevel)
        {
            elevationMin = 0f;
        }
        else if (elevationMin >= elevationMax)
        {
            elevationMin = elevationMax - .01f;
        }
        if (placeBelowMountainLevel)
        {
            elevationMax = 1f;
        }
        else if (elevationMax <= elevationMin)
        {
            elevationMax = elevationMin + .01f;
        }
    }
}
