using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator
{
    public TerrainData Generate(TerrainData data, TerrainSettings settings)
    {
        List<TerrainObjectData> placedTrees = new List<TerrainObjectData>();
        float minValue;
        float maxValue;
        if (settings.placeAboveWaterLevel)
        {
            minValue = settings.oceanPercent + .01f;
        }
        else
        {
            minValue = settings.elevationMin;
        }
        if (settings.placeBelowMountainLevel)
        {
            maxValue = settings.mountainLevel - .01f;
        }
        else
        {
            maxValue = settings.elevationMax;
        }
        for (int x = 0; x < data.xSize; x++)
        {
            for (int z = 0; z < data.zSize; z++)
            {
                TileData tile = data.GetTileAtCoordinates(x, z);
                if (tile.noiseValue > minValue && tile.noiseValue < maxValue && tile.type == TileType.FlatLand)
                {
                    for (int i = 0; i < settings.maxTreesPerForestTile; i++)
                    {
                        //if (Random.value > treeClumping)
                        //{
                        //    break;
                        //}
                        // create a common tree spawnPoint
                        Vector3 randomPosition = new Vector3(Random.Range(0, settings.tileSize), 0, Random.Range(0, settings.tileSize));
                        randomPosition += tile.position;
                        int typeIndex = Random.Range(0, settings.treeSet.commonTrees.Count);
                        Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
                        placedTrees.Add(new TerrainObjectData(typeIndex, randomPosition, randomRotation, Vector3.one * 2));
                    }
                }
            }
        }
        data.trees = placedTrees;
        return data;
    }

    //private void OnDrawGizmos()
    //{
    //    if (placedTrees != null)
    //    {
    //        Gizmos.color = Color.red;
    //        foreach (var tree in placedTrees)
    //        {
    //            Gizmos.DrawSphere(tree.position, 1f);
    //        }
    //    }
    //}
}
